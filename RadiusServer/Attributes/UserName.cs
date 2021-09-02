using System;
using System.Text;

namespace DanilovSoft.Radius.Attributes
{
    [RadiusAttribute(AttributeType.User_Name)]
    public class UserName : RadiusAttribute
    {
        public string String { get; }

        public UserName(ReadOnlySpan<byte> span) : base(AttributeType.User_Name)
        {
            if (span.Length < 1)
            {
                IsValid = false;
                return;
            }

            String = Encoding.UTF8.GetString(span);
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{FriendlyType} = \"{String}\"";
        }
    }
}
