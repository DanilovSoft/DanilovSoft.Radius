using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

/*
 0                   1                   2                   3
 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
|     Code      |  Identifier   |            Length             |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
|                                                               |
|                         Authenticator                         |
|                                                               |
|                                                               |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
|  Attributes ...
+-+-+-+-+-+-+-+-+-+-+-+-+-
 */

namespace DanilovSoft.Radius
{
    [DebuggerDisplay("{DebugDisplay,nq}")]
    public class RadiusPackage : BaseRadiusPackage
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebugDisplay => "{" + $"{Code}, Identifier = {Identifier}, Attributes = {Attributes.Length}" + "}";
        //internal ReadOnlyMemory<byte> Authenticator { get; }
        internal RadiusServer Server { get; }

        /// <summary>
        /// Извлекает Identifier, "Request Authenticator", атрибуты, 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="span"></param>
        /// <param name="code"></param>
        /// <param name="endPoint"></param>
        internal RadiusPackage(RadiusServer server, ReadOnlySpan<byte> span, Code code, EndPoint endPoint)
        {
            Server = server;
            EndPoint = endPoint;
            Code = code;
            Identifier = span[1];
            var length = BitConverterReversed.ToUInt16(span.Slice(2));

            // Если размер пакета меньше значения поля Length.
            if (span.Length < length)
            {
                IsValid = false;
                return;
            }

            // 16 байт "Request Authenticator".
            Authenticator = new ReadOnlyMemory<byte>(span.Slice(4, 16).ToArray());

            // Вытаскиваем атрибуты.
            Attributes = ExtractAttributes(span);

            IsValid = true;
        }

        private RadiusAttribute[] ExtractAttributes(ReadOnlySpan<byte> span)
        {
            if (span.Length == 20)
                return System.Array.Empty<RadiusAttribute>();

            var list = new List<RadiusAttribute>();
            int nextIndex = 0;
            ReadOnlySpan<byte> nextSpan = span.Slice(20);
            while (nextSpan.Length > 0)
            {
                if (TryParseAttribute(nextSpan, out RadiusAttribute attribute, out nextIndex))
                    list.Add(attribute);

                nextSpan = nextSpan.Slice(nextIndex);
            }
            return list.ToArray();
        }

        private bool TryParseAttribute(ReadOnlySpan<byte> span, out RadiusAttribute attribute, out int nextIndex)
        {
            byte length = span[1];
            nextIndex = length;

            if (EnumHelper.TryParse(span[0], out AttributeType attributeType))
            {   
                if (AttributesDictionary.GetAttribute(attributeType, span.Slice(2, length - 2), out attribute))
                {
                    return true;
                }
            }
            else
            // Игнорируем атрибуты неизвестных типов.
            {
                DebugOnly.Break();
            }
            attribute = null;
            return false;
        }

        private int CalcAttributesCount(ReadOnlySpan<byte> span)
        {
            int attributesLength = span.Length - 20;

            // Атрибуты начинаются с 20 байта.
            int index = 20;

            int count = 0;
            while (index < (attributesLength + 20))
            {
                // Найден атрибут.
                count++;

                byte length = span[index + 1];

                // Переходим к следующему атрибуту.
                index += length;
            }
            return count;
        }
    }
}
