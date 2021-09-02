using System;

/*
  https://rfc2.ru/2865.rfc/39#p5.41
 */

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Показывает тип физического порта сервера NAS, через который подключается пользователь.
    /// Атрибут может передаваться вместо атрибута NAS-Port (5) или в дополнение к нему. 
    /// Атрибут передается только в пакетах Access-Request. В каждый пакет Access-Request 
    /// следует включать атрибут NAS-Port (5) или NAS-Port-Type (или оба атрибута), если сервер NAS
    /// может различать свои порты.
    /// </summary>
    [RadiusAttribute(AttributeType.NAS_Port_Type)]
    public class NasPortType : RadiusAttribute
    {
        public NasPortTypeValue Value { get; }

        public NasPortType(ReadOnlySpan<byte> span) : base(AttributeType.NAS_Port_Type)
        {
            if(span.Length != 4)
            {
                IsValid = false;
                return;
            }

            int value = BitConverterReversed.ToInt32(span);
            if (!EnumHelper.TryParse(value, out NasPortTypeValue nasPortTypeValue))
            {
                IsValid = false;
                return;
            }
            Value = nasPortTypeValue;
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{FriendlyType} = \"{Value.FriendlyName()}\"";
        }
    }
}
