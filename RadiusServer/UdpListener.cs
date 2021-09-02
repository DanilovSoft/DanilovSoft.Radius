using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DanilovSoft.Radius
{
    internal sealed class UdpListener : IDisposable
    {
        private readonly Socket _socket;
        private bool _disposed;

        public UdpListener(int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Не генерировать исключения при закрытии сокета.
            SocketHelper.SetSioUdpConnectionReset(_socket);

            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public async Task<ReceiveResult> ReceiveAsync(ArraySegment<byte> buffer)
        {
            ReceiveResult result = await _socket.ReceiveFromTaskAsync(buffer).ConfigureAwait(false);
            return result;
        }

        public async Task SendAsync(byte[] buffer, int offset, int size, EndPoint endPoint)
        {
            int sendedCount = await _socket.SendToTaskAsync(buffer, offset, size, endPoint).ConfigureAwait(false);
        }

        public void Dispose()
        {
            if(!_disposed)
            {
                _disposed = true;
            }
        }
    }
}
