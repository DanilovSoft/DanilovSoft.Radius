using DanilovSoft.Radius.EAP;
using System;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Extended Access Protocol.
    /// </summary>
    [RadiusAttribute(AttributeType.EAP_Message)]
    public class EapMessage : RadiusAttribute
    {
        public ReadOnlyMemory<byte> String { get; }
        public EapPacket EapPacket { get; }
        //private readonly IOutputEapPacket _eapPacket;

        private EapMessage(ReadOnlySpan<byte> span) : base(AttributeType.EAP_Message)
        {
            if(span.Length < 1)
            {
                IsValid = false;
                return;
            }

            String = new ReadOnlyMemory<byte>(span.ToArray());
            EapPacket = new EapPacket(String);
            IsValid = true;
        }

        //public EapMessage(IOutputEapPacket eapPacket) : base(AttributeType.EAP_Message)
        //{
        //    _eapPacket = eapPacket;
        //}

        public override string ToString()
        {
            return $"{FriendlyType} = {EapPacket}";
        }

        //byte IOutputAttribute.Write(Span<byte> span)
        //{
            //buffer[offset] = (byte)Type;
            //ushort eapLength = _eapPacket.Write(buffer, offset + 2);
            //byte radiusAttribLength = (byte)(eapLength + 2);
            //buffer[offset + 1] = radiusAttribLength;
            //return radiusAttribLength;
        //}
    }
}
