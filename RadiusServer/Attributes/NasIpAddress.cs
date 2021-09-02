using System;
using System.Net;

namespace DanilovSoft.Radius.Attributes
{
    [RadiusAttribute(AttributeType.NAS_IP_Address)]
    public class NasIpAddress : RadiusAttribute
    {
        public NasIpAddress(ReadOnlySpan<byte> address) : base(AttributeType.NAS_IP_Address)
        {
            if(address.Length != 4)
            {
                IsValid = false;
                return;
            }

            Address = new IPAddress(address);
            IsValid = true;
        }

        public IPAddress Address { get; }

        public override string ToString()
        {
            return $"{FriendlyType} = {Address}";
        }
    }
}
