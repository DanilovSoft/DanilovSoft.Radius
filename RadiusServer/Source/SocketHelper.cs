using System.Net.Sockets;

namespace DanilovSoft.Radius
{
    internal static class SocketHelper
    {
        /// <summary>
        /// Подавляет исключения возникающие при закрытии сокета когда на другой стороне соединение уже закрыто.
        /// </summary>
        public static void SetSioUdpConnectionReset(Socket socket)
        {
            int sioUdpConnectionReset = -1744830452;
            var optionInValue = new byte[] { 0 };
            var optionOutValue = new byte[] { 0 };

            int outBytesCount = socket.IOControl(sioUdpConnectionReset, optionInValue, optionOutValue);
        }
    }
}
