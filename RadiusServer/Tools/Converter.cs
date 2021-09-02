using System;
using System.Collections.Generic;
using System.Text;

namespace DanilovSoft.Radius
{
    internal static class Converter
    {
        public static string ToHex(IList<byte> bytes)
        {
            var hexString = new StringBuilder(bytes.Count * 2);
            foreach (byte b in bytes)
            {
                hexString.AppendFormat("{0:x2}", b);
            }
            return hexString.ToString();
        }

        public static string ToHex(ReadOnlySpan<byte> span)
        {
            var hexString = new StringBuilder(span.Length * 2);
            foreach (byte b in span)
            {
                hexString.AppendFormat("{0:x2}", b);
            }
            return hexString.ToString();
        }
    }
}
