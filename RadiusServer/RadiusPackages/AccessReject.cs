using System;

namespace DanilovSoft.Radius.RadiusPackages
{
    internal class AccessReject : BaseRadiusPackage, IOutRadiusPackage
    {
        public AccessReject(AccessRequest accessRequest, Memory<byte> memory) : base(accessRequest)
        {

        }

        public int Build()
        {
            throw new NotImplementedException();
        }
    }
}
