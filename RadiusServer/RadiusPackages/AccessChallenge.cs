using System;
using System.Diagnostics;

namespace DanilovSoft.Radius
{
    public class AccessChallenge : BaseRadiusPackage
    {
        //private readonly RadiusPackage.Builder _builder;
        private readonly AccessRequest _accessRequest;
        public override RadiusAttribute[] Attributes => ParseFromRequest().Attributes;
        public ReadOnlyMemory<byte> Memory { get; private set; }

        public AccessChallenge(AccessRequest accessRequest, Memory<byte> memory) : base(accessRequest)
        {
            _accessRequest = accessRequest;
            Code = Code.Access_Challenge;
            //_builder = accessRequest.GetResponseBuilder(Code.Access_Challenge, memory);
        }

        private RadiusPackage ParseFromRequest()
        {
            return new RadiusPackage(_accessRequest.Server, Memory.Span, Code, EndPoint);
        }

        //public AccessChallenge AddState(ArraySegment<byte> array)
        //{
        //    _builder.AddAttribute(new State(array));
        //    return this;
        //}

        //public AccessChallenge AddReplyMessage(string message)
        //{
        //    _builder.AddAttribute(new ReplyMessage(message));
        //    return this;
        //}

        //public int Build()
        //{
        //    Memory = _builder.Build();
        //    Validate();
        //    return Memory.Length;
        //}

        [Conditional("DEBUG")]
        private void Validate()
        {
            var accessRequest = new AccessRequest(_accessRequest.Server, Memory.Span, EndPoint);
            //var radiusPackage = new RadiusPackage(_accessRequest.RadiusServer, Memory.Span, Code, EndPoint);
            if (!accessRequest.IsValid)
                DebugOnly.Break();
        }
    }
}
