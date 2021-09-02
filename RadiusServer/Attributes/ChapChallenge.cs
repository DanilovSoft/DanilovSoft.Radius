using System;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут содержит запрос CHAP Challenge, передаваемый сервером NAS 
    /// пользователю PPP CHAP (Challenge-Handshake Authentication Protocol). 
    /// Атрибут применяется только в пакетах Access-Request.
    /// Если значение CHAP challenge имеет размер 16 октетов, оно может быть 
    /// помещено в поле Request Authenticator вместо использования данного атрибута.
    /// </summary>
    [RadiusAttribute(AttributeType.CHAP_Challenge)]
    public class ChapChallenge : RadiusAttribute
    {
        /// <summary>
        /// CHAP Challenge.
        /// </summary>
        public ReadOnlyMemory<byte> String { get; }

        public ChapChallenge(ReadOnlySpan<byte> span) : base(AttributeType.CHAP_Challenge)
        {
            if(span.Length < 5)
            {
                IsValid = false;
                return;
            }
            String = new ReadOnlyMemory<byte>(span.ToArray());
            IsValid = true;
        }
    }
}
