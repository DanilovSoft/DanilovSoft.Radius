using System;
using System.Text;

namespace DanilovSoft.Radius.Attributes
{
    [RadiusAttribute(AttributeType.User_Name)]
    public class UserName : RadiusAttribute
    {
        public UserName(ReadOnlySpan<byte> span) : base(AttributeType.User_Name)
        {
            if (span.Length < 1)
            {
                IsValid = false;
                return;
            }

            Utf8String = Encoding.UTF8.GetString(span);
            IsValid = true;
        }

        public string? Utf8String { get; }

        public override string ToString()
        {
            return $"{FriendlyType} = \"{Utf8String}\"";
        }
    }
}
