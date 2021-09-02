using System;

namespace DanilovSoft.Radius
{
    internal static class BitConverterReversed
    {
        public static int ToInt32(ReadOnlySpan<byte> buffer)
        {
            // Обратный порядок.
            int value = (buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | (buffer[3]);
            return value;
        }

        public static ushort ToUInt16(ReadOnlySpan<byte> buffer)
        {
            return (ushort)((buffer[0] << 8) | buffer[1]);
        }
    }
}
