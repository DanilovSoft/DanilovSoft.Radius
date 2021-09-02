using DanilovSoft.Radius.Attributes;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DanilovSoft.Radius
{
    [RadiusPackage(Code.Access_Request)]
    public class AccessRequest : RadiusPackage
    {
        private static readonly byte[] Empty16Bytes = new byte[16];

        /// <summary>
        /// Имя пользователя. Этот атрибут должен присутствовать но может быть <see langword="null"/>.
        /// </summary>
        public UserName UserName { get; }

        /// <summary>
        /// Пароль пользователя. Если этот атрибут равен <see langword="null"/> то <see cref="ChapPassword"/> или <see cref="State"/> не равен <see langword="null"/>.
        /// </summary>
        public string UserPassword { get; }

        /// <summary>
        /// Пароль пользователя. Если этот атрибут равен <see langword="null"/> то <see cref="UserPassword"/> или <see cref="State"/> не равен <see langword="null"/>.
        /// </summary>
        public ChapPassword ChapPassword { get; }

        /// <summary>
        /// Если этот атрибут равен <see langword="null"/> то <see cref="UserPassword"/> или <see cref="ChapPassword"/> не равен <see langword="null"/>.
        /// </summary>
        public State State { get; }

        /// <summary>
        /// Если этот атрибут равен <see langword="null"/> то <see cref="NasIdentifier"/> не равен <see langword="null"/>.
        /// </summary>
        public NasIpAddress NasIpAddress { get; }

        /// <summary>
        /// Если этот атрибут равен <see langword="null"/> то <see cref="NasIpAddress"/> не равен <see langword="null"/>.
        /// </summary>
        public NasIdentifier NasIdentifier { get; }

        /// <summary>
        /// Этот атрибут может быть <see langword="null"/>.
        /// </summary>
        public NasPort NASPort { get; }

        /// <summary>
        /// Этот атрибут может быть <see langword="null"/>.
        /// </summary>
        public NasPortType NASPortType { get; }

        public AccessRequest(RadiusServer _server, ReadOnlySpan<byte> span, EndPoint endPoint) : base(_server, span, Code.Access_Request, endPoint)
        {
            // Если базовый клас не прошел валидацию то нет смысла продолжать.
            if (!IsValid)
                return;

            // SHOULD contain a User-Name.
            UserName = Attributes.OfType<UserName>().FirstOrDefault();
            NasIpAddress = Attributes.OfType<NasIpAddress>().FirstOrDefault();
            NasIdentifier = Attributes.OfType<NasIdentifier>().FirstOrDefault();

            // MUST contain either a NAS-IP - Address attribute or a NAS-Identifier attribute(or both).
            if(NasIpAddress == null && NasIdentifier == null)
            {
                IsValid = false;
                return;
            }

            var userPassword = Attributes.OfType<UserPassword>().FirstOrDefault();
            ChapPassword =  Attributes.OfType<ChapPassword>().FirstOrDefault();
            State = Attributes.OfType<State>().FirstOrDefault();

            // MUST contain either a User-Password or a CHAP-Password or a State.
            if (userPassword == null && ChapPassword == null && State == null)
            {
                IsValid = false;
                return;
            }

            // Недопустимо помещать в пакет оба атрибута User-Password и CHAP-Password.
            if(userPassword != null && ChapPassword != null)
            {
                IsValid = false;
                return;
            }

            // В пакеты Access-Request следует включать атрибут NAS-Port или NAS-Port-Type (возможно включение обоих атрибутов), 
            // за исключениям тех случаев, когда запрашиваемый тип доступа включает порт или NAS не различает портов.
            NASPort = Attributes.OfType<NasPort>().FirstOrDefault();
            NASPortType = Attributes.OfType<NasPortType>().FirstOrDefault();

            // Декодировать пароль.
            if (userPassword != null)
            {
                UserPassword = DecodePapPassword(userPassword.String.Span, Authenticator.Span, _server.SharedSecret.Raw);
            }
            else if(ChapPassword != null)
            {
                bool passIsValid = ValidatePassword("123");
            }

            MessageAuthenticator messageAuthenticator = Attributes.OfType<MessageAuthenticator>().FirstOrDefault();
            if (messageAuthenticator != null)
            {
                using (var hmacMd5 = new HMACMD5(_server.SharedSecret.Raw))
                {
                    byte[] buffer = MessageHandler.SharedPool.Rent(span.Length);
                    try
                    {
                        span.CopyTo(buffer);

                        //int startPos = messageAuthenticator.String.Offset - Offset;
                        int left = span.Length - 4 - 16;

                        // 4 байта перед "Request Authenticator".
                        hmacMd5.TransformBlock(buffer, 0, 4, null, 0);

                        // Пустой "Request Authenticator".
                        hmacMd5.TransformBlock(Empty16Bytes, 0, 16, null, 0);

                        // Весь оставщийся фрейм.
                        hmacMd5.TransformBlock(buffer, 16, left, null, 0);

                        hmacMd5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                        //hmacMd5.ComputeHash(Array, Offset, Length);
                        byte[] hash = hmacMd5.Hash;
                        if (!messageAuthenticator.String.Span.SequenceEqual(hash))
                        {
                            IsValid = false;
                            return;
                        }
                    }
                    finally
                    {
                        MessageHandler.SharedPool.Return(buffer);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password">Пароль длиной до 128 символов.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <returns></returns>
        public bool ValidatePassword(string password)
        {
            int passLen = Encoding.UTF8.GetByteCount(password);
            if (passLen > 128)
                throw new ArgumentOutOfRangeException(nameof(password));

            if (UserPassword != null)
                return UserPassword == password;

            if (ChapPassword != null)
            {
                Span<byte> pass = stackalloc byte[passLen];
                Encoding.UTF8.GetBytes(password, pass);

                // Если атрибут отсутствует то используется Request Authenticator как CHAP challenge.
                ReadOnlySpan<byte> chapChallengeSpan = Authenticator.Span;
                var chapChallenge = Attributes.OfType<ChapChallenge>().FirstOrDefault();
                if (chapChallenge != null)
                    chapChallengeSpan = chapChallenge.String.Span;

                Span<byte> hash = stackalloc byte[16];
                Hash(ChapPassword.CHAPIdent, pass, chapChallengeSpan, hash);
                bool passwordIsValid = hash.SequenceEqual(ChapPassword.CHAPResponse.Span);
                return passwordIsValid;
            }
            throw new InvalidOperationException("Отсутствуют атрибуты UserPassword и ChapPassword");
        }

        /// <summary>
        /// MD5(ChapIdent + Password + ChapChallenge)
        /// </summary>
        /// <param name="chapIdent"></param>
        /// <param name="password"></param>
        /// <param name="chapChallenge"></param>
        /// <returns></returns>
        private void Hash(byte chapIdent, ReadOnlySpan<byte> password, ReadOnlySpan<byte> chapChallenge, Span<byte> hash)
        {
            Span<byte> buf = stackalloc byte[1 + password.Length + chapChallenge.Length];
            buf[0] = chapIdent;
            password.CopyTo(buf.Slice(1));
            chapChallenge.CopyTo(buf.Slice(1 + password.Length));

            using (var md5 = MD5.Create())
                md5.TryComputeHash(buf, hash, out int _);
        }

        /// <summary>
        /// Decodes the passed encrypted password and returns the clear-text form.
        /// </summary>
        /// <param name="encryptedPass">Зашифрованный пароль. Длина от 16 до 128.</param>
        /// <param name="sharedSecret">Shared secret.</param>
        /// <param name="authenticator">"Request Authenticator". Длина 16 байт.</param>
        /// <returns>decrypted password</returns>
        private string DecodePapPassword(ReadOnlySpan<byte> encryptedPass, ReadOnlySpan<byte> authenticator, byte[] sharedSecret)
        {
            // Пароль уже проверен на кратность 16.
            // Максимальный размер 128 байт.
            Span<byte> encryptedPassCopy = stackalloc byte[encryptedPass.Length];

            // Копируем зашифрованный пароль что-бы его можно было модифицировать.
            encryptedPass.CopyTo(encryptedPassCopy);

            // Хеш MD5 всегда 16 байт.
            Span<byte> hash = stackalloc byte[16];

            // Пароль декодируется по 16 байт.
            Span<byte> lastBlock = stackalloc byte[16];

            // Декодируем пароль по 16 байт.
            for (int i = 0; i < encryptedPassCopy.Length; i += 16)
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    // Сначала хешируем секрет.
                    md5.TransformBlock(sharedSecret, 0, sharedSecret.Length, null, 0);

                    // В первую итерацию хешируем "Request Authenticator".
                    // Последующие итерации хешируем оригинальный пароль.
                    md5.TryComputeHash(i == 0 ? authenticator : lastBlock, hash, out int _);

                    // Скопировать 16 байт в первозданном виде перед их ксором.
                    encryptedPassCopy.Slice(i, 16).CopyTo(lastBlock);

                    // perform the XOR as specified by RFC 2865.
                    for (int j = 0; j < 16; j++)
                        encryptedPassCopy[i + j] = (byte)(hash[j] ^ encryptedPassCopy[i + j]);
                }
            }

            // Нужно пропустить нуль-терминаторы.
            int len = encryptedPassCopy.Length;

            // Считаем реальную длину пароля в байтах.
            while (len > 0 && encryptedPassCopy[len - 1] == 0)
            {
                len--;
            }

            string decodedPassword = Encoding.UTF8.GetString(encryptedPassCopy.Slice(0, len));
            return decodedPassword;
        }

        private byte[] DecodePapPassword2(ReadOnlySpan<byte> encryptedPass, ReadOnlySpan<byte> authenticator, byte[] sharedSecret)
        {
            // Пароль уже проверен на кратность 16.
            // Максимальный размер 128 байт.
            Span<byte> encryptedPassCopy = stackalloc byte[encryptedPass.Length];

            // Копируем зашифрованный пароль что-бы его можно было модифицировать.
            encryptedPass.CopyTo(encryptedPassCopy);

            // Хеш MD5 всегда 16 байт.
            Span<byte> hash = stackalloc byte[16];

            // Пароль декодируется по 16 байт.
            Span<byte> lastBlock = stackalloc byte[16];

            // Декодируем пароль по 16 байт.
            for (int i = 0; i < encryptedPassCopy.Length; i += 16)
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    // Сначала хешируем секрет.
                    md5.TransformBlock(sharedSecret, 0, sharedSecret.Length, null, 0);

                    // В первую итерацию хешируем "Request Authenticator".
                    // Последующие итерации хешируем оригинальный пароль.
                    md5.TryComputeHash(i == 0 ? authenticator : lastBlock, hash, out int _);

                    // Скопировать 16 байт в первозданном виде перед их ксором.
                    encryptedPassCopy.Slice(i, 16).CopyTo(lastBlock);

                    // perform the XOR as specified by RFC 2865.
                    for (int j = 0; j < 16; j++)
                        encryptedPassCopy[i + j] = (byte)(hash[j] ^ encryptedPassCopy[i + j]);
                }
            }
            return encryptedPassCopy.ToArray();
        }

        /**
	 * This method encodes the plaintext user password according to RFC 2865.
	 * 
	 * @param userPass
	 *            the password to encrypt
	 * @param sharedSecret
	 *            shared secret
	 * @return the byte array containing the encrypted password
	 */
        private byte[] EncodePapPassword(byte[] userPass, ReadOnlySpan<byte> authenticator, byte[] sharedSecret)
        {
            // the password must be a multiple of 16 bytes and less than or equal
            // to 128 bytes. If it isn't a multiple of 16 bytes fill it out with zeroes
            // to make it a multiple of 16 bytes. If it is greater than 128 bytes
            // truncate it at 128.
            byte[] userPassBytes = null;
            if (userPass.Length > 128)
            {
                userPassBytes = new byte[128];
                Array.Copy(userPass, 0, userPassBytes, 0, 128);
            }
            else
            {
                userPassBytes = userPass;
            }

            // declare the byte array to hold the final product
            byte[] encryptedPass = null;
            if (userPassBytes.Length < 128)
            {
                if (userPassBytes.Length % 16 == 0)
                {
                    // tt is already a multiple of 16 bytes
                    encryptedPass = new byte[userPassBytes.Length];
                }
                else
                {
                    // make it a multiple of 16 bytes
                    encryptedPass = new byte[((userPassBytes.Length / 16) * 16) + 16];
                }
            }
            else
            {
                // the encrypted password must be between 16 and 128 bytes
                encryptedPass = new byte[128];
            }

            // copy the userPass into the encrypted pass and then fill it out with zeroes by default.
            Array.Copy(userPassBytes, 0, encryptedPass, 0, userPassBytes.Length);

            // digest shared secret and authenticator
            //MessageDigest md5 = getMd5Digest();

            // According to section-5.2 in RFC 2865, when the password is longer than 16
            // characters: c(i) = pi xor (MD5(S + c(i-1)))
            for (int i = 0; i < encryptedPass.Length; i += 16)
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    //md5.reset();
                    // Сначала хешируем секрет.
                    md5.TransformBlock(sharedSecret, 0, sharedSecret.Length, null, 0);

                    if (i == 0)
                    {
                        md5.TransformFinalBlock(authenticator.ToArray(), 0, 16);
                        //md5.update(getAuthenticator());
                    }
                    else
                    {
                        md5.TransformFinalBlock(encryptedPass, i - 16, 16);
                    }

                    byte[] bn = md5.Hash;

                    // perform the XOR as specified by RFC 2865.
                    for (int j = 0; j < 16; j++)
                        encryptedPass[i + j] = (byte)(bn[j] ^ encryptedPass[i + j]);
                }
            }
            return encryptedPass;
        }

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    //sb.AppendLine($"UserPassword = \"{UserPassword}\"");
        //    sb.AppendLine(base.ToString());
        //    return sb.ToString();
        //}
    }
}
