using System;
using System.Text;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут позволяет серверу NAS передать в пакете Access-Request телефонный номер пользователя,
    /// определенный с помощью ANI (Automatic Number Identification — АОН) или иной технологии.
    /// Атрибут может использоваться только в пакетах Access-Request.
    /// </summary>
    [RadiusAttribute(AttributeType.Calling_Station_Id)]
    public class CallingStationId : RadiusAttribute
    {
        /// <summary>
        /// Содержит по крайней мере один октет.
        /// Рекомендуется использовать текст в кодировке UTF-8.
        /// </summary>
        public ReadOnlyMemory<byte> RawString { get; }
        public string AsString => Encoding.UTF8.GetString(RawString.Span);

        public CallingStationId(ReadOnlySpan<byte> span) : base(AttributeType.Calling_Station_Id)
        {
            if(span.Length < 1)
            {
                IsValid = false;
                return;
            }

            RawString = new ReadOnlyMemory<byte>(span.ToArray());
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{FriendlyType} = \"{AsString}\"";
        }
    }
}
