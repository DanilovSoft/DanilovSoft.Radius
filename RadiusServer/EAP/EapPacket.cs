using System;
using System.Text;

namespace DanilovSoft.Radius.EAP
{
    public class EapPacket
    {
        public EapCode Code { get; }
        public byte Identifier { get; }
        public ushort Length { get; }
        public ReadOnlyMemory<byte> Buffer { get; }

        public EapPacket(ReadOnlyMemory<byte> buffer)
        {
            Buffer = buffer;
            Code = (EapCode)buffer.Span[0];
            Identifier = buffer.Span[1];
            Length = (ushort)((buffer.Span[2] << 8) | buffer.Span[3]);
            var length = BitConverterReversed.ToUInt16(buffer.Span.Slice(2));

            //if (Code == EapCode.Request || Code == EapCode.Response)
            //{
            //    var type = (EapType)buffer[4];
            //    ushort typeDataLength = (ushort)(Length - 5);
            //    string utf8Data = Encoding.UTF8.GetString(buffer.Array, buffer.Offset + 5, typeDataLength);
            //}
        }

        public override string ToString()
        {
            //var type = (EapType)Buffer[4];
            ushort typeDataLength = (ushort)(Length - 5);
            string utf8Str = Encoding.UTF8.GetString(Buffer.Span.Slice(5, typeDataLength));
            return "utf-8 = \"" + utf8Str + "\"";
        }
    }

    public enum EapCode : byte
    {
        Request = 1,
        Response = 2,
        Success = 3,
        Failure = 4,
    }

    internal enum EapType : byte
    {
        Identity = 1,
        Notification = 2,
        Nak = 3,
        MD5_Challenge = 4,
        One_Time_Password = 5,
        Generic_Token_Card = 6,
        Expanded_Type = 254,
        Experimental = 255,
    }
}
