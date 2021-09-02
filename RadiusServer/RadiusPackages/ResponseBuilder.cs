using System;
using System.Security.Cryptography;

namespace DanilovSoft.Radius
{
    internal class Builder
    {
        private readonly BaseRadiusPackage _requestPackage;
        private readonly Memory<byte> _memory;
        private readonly RadiusServer _server;
        private ushort _totalSize = 20;
        private bool _hasMessageAuthenticator;

        public Builder(BaseRadiusPackage responsePackage, RadiusServer server, Memory<byte> memory)
        {
            _server = server;
            _memory = memory;
            _requestPackage = responsePackage;

            // Записываем тип пакета.
            memory.Span[0] = (byte)responsePackage.Code;

            // Записываем идентификатор.
            memory.Span[1] = responsePackage.Identifier;

            // Записываем "Response Authenticator".
            responsePackage.Authenticator.CopyTo(memory.Slice(4));
        }

        //public Builder AddAttribute(IOutputAttribute radiusAttribute)
        //{
        //    if (_hasMessageAuthenticator)
        //        throw new InvalidOperationException("Уже установлен Message Authenticator");

        //    byte attributeLength = radiusAttribute.Write(_memory.Slice(_totalSize).Span);
        //    _totalSize += attributeLength;
        //    return this;
        //}

        private int SetHasMessageAuthenticator()
        {
            throw new NotImplementedException();
            //if (_hasMessageAuthenticator)
            //    throw new InvalidOperationException("MessageAuthenticator");

            //_hasMessageAuthenticator = true;

            //byte attributeLength = ((IOutputAttribute)new MessageAuthenticator()).Write(_buffer, _offset);
            //int messageAuthenticatorOffset = (_offset + 2);
            //_offset += attributeLength;
            //_totalSize += attributeLength;

            //return messageAuthenticatorOffset;
        }

        /// <summary>
        /// Расчитывает "Response Authenticator". Этот метод должен вызываться в последнюю очередь.
        /// MD5(Code+ID+Length+RequestAuth+Attributes+Secret)
        /// </summary>
        private void WriteResponseAuthenticator()
        {
            using (var md5 = MD5.Create())
            {
                byte[] buffer = MessageHandler.SharedPool.Rent(_totalSize);
                try
                {
                    _memory.Slice(0, _totalSize).CopyTo(buffer);
                    md5.TransformBlock(buffer, 0, _totalSize, null, 0);
                    md5.TryComputeHash(_server.SharedSecret.Raw, _memory.Slice(4, 16).Span, out int _);
                }
                finally
                {
                    MessageHandler.SharedPool.Return(buffer);
                }
            }
            _hasMessageAuthenticator = true;
        }

        //private byte[] CalcResponseAuthenticator2()
        //{
        //    byte[] sharedSecret = _requestPackage.Server.SharedSecret.Raw;

        //    using (var md5 = MD5.Create())
        //    {
        //        byte[] buffer = MessageHandler.SharedPool.Rent(_totalSize);
        //        try
        //        {
        //            _memory.Slice(0, _totalSize).CopyTo(buffer);
        //            md5.TransformBlock(buffer, 0, _totalSize, null, 0);
        //            md5.TransformFinalBlock(sharedSecret, 0, sharedSecret.Length);
        //            return md5.Hash;
        //        }
        //        finally
        //        {
        //            MessageHandler.SharedPool.Return(buffer);
        //        }
        //    }
        //}

        /// <summary>
        /// Записывает Length в 3-й и 4-й октет.
        /// </summary>
        private void WriteLength()
        {
            _memory.Span[2] = (byte)((_totalSize >> 8) & 0xFF);
            _memory.Span[3] = (byte)(_totalSize & 0xFF);
        }

        public ReadOnlyMemory<byte> Build()
        {
            WriteLength();
            WriteResponseAuthenticator();
            return _memory.Slice(0, _totalSize);
        }
    }
}
