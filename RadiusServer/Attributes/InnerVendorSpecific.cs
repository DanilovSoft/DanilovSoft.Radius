using System;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут позволяет разработчикам поддерживать фирменные атрибуты, недоступные или неподходящие для общего пользования. 
    /// Недопустимо влияние таких атрибутов на работу протокола RADIUS.
    /// Серверы, не понимающие полученные от клиента фирменные атрибуты, должны 
    /// игнорировать их(допускается генерация отчетов о получении таких атрибутов). Клиентам, не получившим желаемого отклика 
    /// на фирменный атрибут, следует предпринять попытку работы без такого атрибута, хотя бы в усеченном режиме(генерируя соответствующий отчет).
    /// </summary>
    [RadiusAttribute(AttributeType.Vendor_Specific)]
    internal class InnerVendorSpecific : RadiusAttribute
    {
        /// <summary>
        /// Код SMI Network Management Private Enterprise для производителя в соответствии с "Assigned Numbers".
        /// Может принимать значения от 1 до 16777216 (24 bit)
        /// </summary>
        public int VendorId { get; }

        /// <summary>
        /// Содержит по крайней мере один октет.
        /// </summary>
        public ReadOnlyMemory<byte> String { get; }

        /// <summary>
        /// Ленивое свойство. Используется только для отладки.
        /// </summary>
        private PenVendorInfo VendorInfo => PenDictionary.TryGetVendorInfo(VendorId);

        public InnerVendorSpecific(ReadOnlySpan<byte> span) : base(AttributeType.Vendor_Specific)
        {
            if(span.Length < 5)
            {
                IsValid = false;
                return;
            }

            VendorId = BitConverterReversed.ToInt32(span);
            String = new ReadOnlyMemory<byte>(span.Slice(4).ToArray());
            IsValid = true;

            //server.
        }

        public override string ToString()
        {
            return $"{FriendlyType} = 0x{Converter.ToHex(String.Span)}";
        }
    }
}
