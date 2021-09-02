namespace DanilovSoft.Radius.EAP
{
    internal class EapSucess
    {
        private readonly EapPacket _eapPacket;
        private const ushort Length = 4;

        public EapSucess(EapPacket eapPacket)
        {
            _eapPacket = eapPacket;
        }

        public ushort Write(byte[] buffer, int offset)
        {
            buffer[offset] = (byte)EapCode.Success;
            buffer[offset + 1] = _eapPacket.Identifier;
            buffer[offset + 2] = (Length >> 8) & 0xFF;
            buffer[offset + 3] = Length & 0xFF;

            return Length;
        }
    }
}
