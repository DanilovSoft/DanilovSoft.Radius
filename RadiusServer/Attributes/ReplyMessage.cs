using System;
using System.Text;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут содержит текст, который может выводиться на консоль пользователя.
    /// В пакетах Access-Accept этот атрибут содержит информацию об успешной аутентификации пользователя.
    /// В пакетах Access-Reject этот атрибут служит для передачи информации об отказе.
    /// Атрибут может содержать приглашение пользователю для ввода дополнительной информации перед новой попыткой передачи пакета Access-Request.
    /// В пакетах Access-Challenge этот атрибут может содержать приглашение пользователя к диалогу для ввода дополнительной информации(отклика).
    /// Пакет может содержать несколько атрибутов данного типа. Если такие атрибуты отображаются пользователю, они должны выводиться в порядке их следования в пакете.
    /// </summary>
    [RadiusAttribute(AttributeType.Reply_Message)]
    public class ReplyMessage : RadiusAttribute
    {
        private const int MaximumBytesLength = byte.MaxValue - 2;
        /// <summary>
        /// Содержит по крайней мере один октет.
        /// Не может быть <see langword="null"/>.
        /// </summary>
        public string Text { get; }

        public ReplyMessage(ReadOnlySpan<byte> span) : base(AttributeType.Reply_Message)
        {
            if(span.Length < 1)
            {
                IsValid = false;
                return;
            }

            Text = Encoding.UTF8.GetString(span);
            IsValid = true;
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <param name="text"></param>
        public ReplyMessage(string text) : base(AttributeType.Reply_Message)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            int bytesLength = Encoding.UTF8.GetByteCount(text);

            if (bytesLength == 0)
                throw new ArgumentOutOfRangeException(nameof(text), "Сообщение не может быть пустой строкой.");

            if (bytesLength > MaximumBytesLength)
                throw new ArgumentOutOfRangeException(nameof(text), $"Text is too long. Maximum length is {MaximumBytesLength} bytes. Overflowed on {bytesLength - MaximumBytesLength} bytes.");

            Text = text;
            IsValid = true;
        }

        //byte IOutputAttribute.Write(Span<byte> span)
        //{
        //    span[0] = (byte)Type;
        //    int bytesCount = Encoding.UTF8.GetBytes(Text, span.Slice(2));
        //    byte attributeLength = (byte)(bytesCount + 2);
        //    span[1] = attributeLength;
        //    return attributeLength;
        //}

        public override string ToString()
        {
            return $"{FriendlyType} = \"{Text}\"";
        }
    }
}
