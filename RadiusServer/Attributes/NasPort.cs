using System;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут указывает физический порт сервера NAS, через который обратился идентифицируемый пользователь.
    /// Этот атрибут используется только в пакетах Access-Request. Отметим, что номер порта NAS, не имеет 
    /// никакого отношения к номерам используемых портов TCP или UDP. Если сервер NAS способен различать свои порты, 
    /// в пакеты Access-Request следует включать атрибут NAS-Port или NAS-Port-Type (61), допускается одновременное включение обоих атрибутов.
    /// </summary>
    [RadiusAttribute(AttributeType.NAS_Port)]
    public class NasPort : RadiusAttribute
    {
        /// <summary>
        /// Четырехоктетное значение идентификатора порта NAS.
        /// </summary>
        public ReadOnlyMemory<byte> Value { get; }

        public NasPort(ReadOnlySpan<byte> span) : base(AttributeType.NAS_Port)
        {
            if(span.Length != 4)
            {
                IsValid = false;
                return;
            }

            Value = new ReadOnlyMemory<byte>(span.ToArray());
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{FriendlyType} = 0x{Converter.ToHex(Value.Span)}";
        }
    }
}
