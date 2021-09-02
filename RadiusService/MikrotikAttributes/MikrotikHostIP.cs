using DanilovSoft.Radius;
using System;
using System.Net;

namespace RadiusService
{
    internal class MikrotikHostIP : VendorSpecific
    {
        public IPAddress Address { get; }
        public override bool IsValid { get; }

        public MikrotikHostIP(ReadOnlySpan<byte> span)
        {
            if (span.Length != 4)
            {
                IsValid = false;
                return;
            }
            Address = new IPAddress(span);
            IsValid = true;
        }
    }
}
