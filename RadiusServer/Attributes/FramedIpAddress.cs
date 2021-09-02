using System;
using System.Net;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Показывает предоставленный пользователю адрес.
    /// Значение 0xFFFFFFFF показывает, что серверу NAS следует позволить пользователю выбрать адрес (например, Negotiated). 
    /// Значение 0xFFFFFFFE показывает, что серверу NAS следует выбрать адрес
    /// для пользователя(например, из пула адресов, выделенного для NAS). Остальные корректные значения
    /// указывают серверу NAS значение IP-адреса, предоставляемого пользователю.
    /// </summary>
    [RadiusAttribute(AttributeType.Framed_IP_Address)]
    public class FramedIpAddress : RadiusAttribute
    {
        public IPAddress Address { get; }

        public FramedIpAddress(ReadOnlySpan<byte> span) : base(AttributeType.Framed_IP_Address)
        {
            if (span.Length != 4)
            {
                IsValid = false;
                return;
            }
            Address = new IPAddress(span);
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{FriendlyType} = {Address}";
        }
    }
}
