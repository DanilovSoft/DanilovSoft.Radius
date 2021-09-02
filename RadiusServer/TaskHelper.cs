using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DanilovSoft.Radius
{
    internal class TaskHelper
    {
        private readonly static IPEndPoint DefaultSender = new IPEndPoint(IPAddress.Any, 0);

        public static async Task<ReceiveResult> ReceiveFromTaskAsync(Socket socket, ArraySegment<byte> buffer)
        {
            var tcs = new TaskCompletionSource<ReceiveResult>();
            EndPoint ep = DefaultSender;
            IAsyncResult asyncResult = socket.BeginReceiveFrom(buffer.Array, buffer.Offset, buffer.Count, SocketFlags.None, ref ep, OnReceiveFromComplete, (socket, tcs));
            var result = await tcs.Task.ConfigureAwait(false);
            return result;
        }

        private static void OnReceiveFromComplete(IAsyncResult asyncResult)
        {
            var (socket, tcs) = ((Socket socket, TaskCompletionSource<ReceiveResult> tcs))asyncResult.AsyncState;

            int count;
            EndPoint ep = DefaultSender;

            try
            {
                count = socket.EndReceiveFrom(asyncResult, ref ep);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                return;
            }

            tcs.SetResult(new ReceiveResult(ep, count));
        }

        public static async Task<int> SendToTaskAsync(Socket socket, byte[] buffer, int offset, int size, EndPoint endPoint)
        {
            IAsyncResult asyncState = socket.BeginSendTo(buffer, offset, size, SocketFlags.None, endPoint, null, null);
            int count = await Task.Factory.FromAsync(asyncState, socket.EndSendTo).ConfigureAwait(false);
            return count;
        }
    }
}
