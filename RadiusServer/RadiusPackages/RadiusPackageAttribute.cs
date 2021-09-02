using System;

namespace DanilovSoft.Radius
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    internal class RadiusPackageAttribute : Attribute
    {
        public readonly Code Code;

        public RadiusPackageAttribute(Code code)
        {
            Code = code;
        }
    }
}
