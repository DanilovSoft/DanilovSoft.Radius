using System;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут показывает значение отклика, представленное пользователем протокола CHAP в ответ на запрос (challenge).
    /// Атрибут может включаться только в пакеты Access-Request.
    /// </summary>
    [RadiusAttribute(AttributeType.CHAP_Password)]
    public class ChapPassword : RadiusAttribute
    {
        /// <summary>
        /// Идентификатор CHAP из пользовательского CHAP Response.
        /// </summary>
        public byte CHAPIdent { get; }
        /// <summary>
        /// 16-октетное поле содержит значение CHAP Response, принятое от пользователя.
        /// </summary>
        public ReadOnlyMemory<byte> CHAPResponse { get; }

        public ChapPassword(ReadOnlySpan<byte> span) : base(AttributeType.CHAP_Password)
        {
            if(span.Length != 17)
            {
                IsValid = false;
                return;
            }

            CHAPIdent = span[0];
            CHAPResponse = new ReadOnlyMemory<byte>(span.Slice(1).ToArray());
            IsValid = true;
        }
    }
}
