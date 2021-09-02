using DanilovSoft.Radius;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Net.Sockets
{
    [DebuggerNonUserCode]
    internal static class SocketExtensions
    {
        public static Task<ReceiveResult> ReceiveFromTaskAsync(this Socket socket, ArraySegment<byte> buffer)
        {
            return TaskHelper.ReceiveFromTaskAsync(socket, buffer);
        }

        public static Task<int> SendToTaskAsync(this Socket socket, byte[] buffer, int offset, int size, EndPoint endPoint)
        {
            return TaskHelper.SendToTaskAsync(socket, buffer, offset, size, endPoint);
        }
    }
}
