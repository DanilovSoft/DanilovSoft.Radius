using System;
using System.Text;

namespace DanilovSoft.Radius.Attributes
{
    [RadiusAttribute(AttributeType.Nas_Port_Id)]
    public class NasPortId : RadiusAttribute
    {
        /// <summary>
        /// The Text field contains the name of the port using UTF-8 encoded characters.
        /// Не может быть <see langword="null"/>.
        /// </summary>
        public string Text { get; }

        public NasPortId(ReadOnlySpan<byte> span) : base(AttributeType.Nas_Port_Id)
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
            return $"{FriendlyType} = \"{Text}\"";
        }
    }
}
