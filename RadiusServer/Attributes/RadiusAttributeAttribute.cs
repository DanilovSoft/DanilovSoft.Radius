using System;

namespace DanilovSoft.Radius.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    internal class RadiusAttributeAttribute : Attribute
    {
        public AttributeType Type { get; }

        public RadiusAttributeAttribute(AttributeType type)
        {
            Type = type;
        }
    }
}
