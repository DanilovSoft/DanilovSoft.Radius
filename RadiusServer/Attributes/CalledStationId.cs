using System;
using System.Text;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут позволяет серверу NAS передать в пакете Access-Request номер телефона,
    /// набранный пользователем и определенный с помощью DNIS (Dialed Number Identification — идентификация вызывающего номера)
    /// или иной технологии. Отметим, что этот номер может отличаться от реального номера, с которым было 
    /// организовано соединение. Атрибут может использоваться только в пакетах Access-Request.
    /// </summary>
    [RadiusAttribute(AttributeType.Called_Station_Id)]
    public class CalledStationId : RadiusAttribute
    {
        /// <summary>
        /// Содержит по крайней мере один октет.
        /// Рекомендуется использовать текст в кодировке UTF-8.
        /// </summary>
        public ReadOnlyMemory<byte> String { get; }
        public string AsUTF8 => Encoding.UTF8.GetString(String.Span);

        public CalledStationId(ReadOnlySpan<byte> span) : base(AttributeType.Called_Station_Id)
        {
            if(span.Length < 1)
            {
                IsValid = false;
                return;
            }

            String = new ReadOnlyMemory<byte>(span.ToArray());
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{FriendlyType} = \"{AsUTF8}\"";
        }
    }
}
