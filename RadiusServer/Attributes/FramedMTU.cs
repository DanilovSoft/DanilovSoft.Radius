using System;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут показывает устанавливаемое для пользователя значение MTU, если это значение не согласуется 
    /// иным способом (например, на уровне PPP). Атрибут может использоваться в пакетах Access-Accept. 
    /// Возможно включение этого атрибута в пакеты Access-Request в качестве рекомендации серверу NAS, 
    /// но сервер не обязан следовать этим рекомендациям.
    /// </summary>
    [RadiusAttribute(AttributeType.Framed_MTU)]
    public class FramedMTU : RadiusAttribute
    {
        public ushort Value { get; }

        public FramedMTU(ReadOnlySpan<byte> span) : base(AttributeType.Framed_MTU)
        {
            if(span.Length != 4)
            {
                IsValid = false;
                return;
            }

            int value = BitConverterReversed.ToInt32(span);
            if(value < 64 || value > ushort.MaxValue)
            {
                IsValid = false;
                return;
            }

            Value = (ushort)value;
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{FriendlyType} = {Value}";
        }
    }
}
