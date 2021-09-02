using System;

namespace DanilovSoft.Radius.Attributes
{
    [RadiusAttribute(AttributeType.User_Password)]
    public class UserPassword : RadiusAttribute
    {
        public UserPassword(ReadOnlySpan<byte> span) : base(AttributeType.User_Password)
        {
            // От 16 до 128.
            if (span.Length < 16 || span.Length > 128)
            {
                IsValid = false;
                return;
            }

            // Длина должна быть кратна 16.
            if (span.Length % 16 != 0)
            {
                IsValid = false;
                return;
            }

            String = new ReadOnlyMemory<byte>(span.ToArray());
            IsValid = true;
        }

        /// <summary>
        /// Зашифрованный пароль длиной от 16 до 128 байт и кратностью 16.
        /// </summary>
        public ReadOnlyMemory<byte> String { get; }

        public override string ToString()
        {
            return $"{FriendlyType} = 0x{Converter.ToHex(String.Span)}";
        }
    }
}
