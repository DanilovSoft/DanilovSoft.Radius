using System;
using System.Text;

namespace DanilovSoft.Radius
{
    //[DebuggerDisplay("{}")]
    internal struct SharedSecret
    {
        /// <summary>
        /// Секрет в кодировке UTF-8.
        /// </summary>
        public readonly byte[] Raw;

        public SharedSecret(string sharedSecret)
        {
            Raw = string.IsNullOrEmpty(sharedSecret) ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(sharedSecret);
        }

        public override string ToString()
        {
            return "\"" + Encoding.UTF8.GetString(Raw) + "\"";
        }

        //public static explicit operator byte[](SharedSecret sharedSecret)
        //{
        //    return sharedSecret.Raw;
        //}

        //public static explicit operator ReadOnlySpan<byte>(SharedSecret sharedSecret)
        //{
        //    return sharedSecret.Raw;
        //}
    }
}
