using System;

namespace DanilovSoft.Radius.Attributes
{
    [RadiusAttribute(AttributeType.Message_Authenticator)]
    public class MessageAuthenticator : RadiusAttribute
    {
        /// <summary>
        /// HMAC-MD5 (Type, Identifier, Length, Request Authenticator, Attributes)
        /// </summary>
        public ReadOnlyMemory<byte> String { get; }

        public MessageAuthenticator(ReadOnlySpan<byte> span) : base(AttributeType.Message_Authenticator)
        {
            if(span.Length != 16)
            {
                IsValid = false;
                return;
            }

            String = new ReadOnlyMemory<byte>(span.ToArray());
            IsValid = true;
        }

        public MessageAuthenticator() : base(AttributeType.Message_Authenticator)
        {

        }

        public override string ToString()
        {
            return $"{FriendlyType} = 0x{Converter.ToHex(String.Span)}";
        }

        //byte IOutputAttribute.Write(Span<byte> span)
        //{
            //buffer[offset] = (byte)Type;
            //buffer[offset + 1] = 18;

            //for (int i = (offset + 2); i < offset + 18; i++)
            //{
            //    buffer[i] = 0;
            //}

            //return 18;
        //}
    }
}
