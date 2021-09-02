using System;
using System.Text;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// This attribute is sent from the NAS to indicate the nature of the
    /// user's connection.
    /// The NAS MAY send this attribute in an Access-Request or
    /// Accounting-Request to indicate the nature of the user's
    /// connection.
    /// </summary>
    [RadiusAttribute(AttributeType.Connect_Info)]
    public class ConnectInfo : RadiusAttribute
    {
        /// <summary>
        /// Не может быть <see langword="null"/>.
        /// </summary>
        public string Text { get; }

        public ConnectInfo(ReadOnlySpan<byte> span) : base(AttributeType.Connect_Info)
        {
            if(span.Length < 1)
            {
                IsValid = false;
                return;
            }

            Text = Encoding.UTF8.GetString(span);
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{Type} = \"{Text}\"";
        }
    }
}
