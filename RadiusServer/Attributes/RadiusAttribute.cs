using System.Diagnostics;

namespace DanilovSoft.Radius
{
    public abstract class RadiusAttribute
    {
        public AttributeType Type { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected string FriendlyType => Type.FriendlyName();
        public bool IsValid { get; protected set; }

        public RadiusAttribute(AttributeType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return FriendlyType;
        }
    }
}
