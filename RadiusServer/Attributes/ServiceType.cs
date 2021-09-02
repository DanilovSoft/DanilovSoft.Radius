/*
https://rfc2.ru/2865.rfc/20
 0                   1                   2                   3
 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
|     Type      |    Length     |             Value
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
           Value (cont)         |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
 
*/

using System;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут показывает тип сервиса, запрошенного пользователем или тип обеспечиваемого пользователю сервиса. 
    /// Атрибут может использоваться в пакетах Access-Request и Access-Accept.От серверов NAS не требуется реализация
    /// всех типов сервиса, они просто должны трактовать неизвестные типы как неподдерживаемые значения Service-Type(как при получении ответа Access-Reject).
    /// </summary>
    [RadiusAttribute(AttributeType.Service_Type)]
    public class ServiceType : RadiusAttribute
    {
        public ServiceTypeValue Value { get; }

        public ServiceType(ReadOnlySpan<byte> span) : base(AttributeType.Service_Type)
        {
            if(span.Length != 4)
            {
                IsValid = false;
                return;
            }

            int intValue = BitConverterReversed.ToInt32(span); // (array[offset] << 24) | (array[offset + 1] << 16) | (array[offset + 2] << 8) | (array[offset + 3]);
            if(!EnumHelper.TryParse(intValue, out ServiceTypeValue value))
            {
                IsValid = false;
                return;
            }
            Value = value;
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{FriendlyType} = {Value.FriendlyName()}";
        }
    }
}
