using System.Diagnostics;
using System.Net;

namespace DanilovSoft.Radius
{
    /// <summary>
    /// Содержит количество принятых байт и обратный адрес отправителя.
    /// </summary>
    [DebuggerDisplay("{DebugDisplay,nq}")]
    internal struct ReceiveResult
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebugDisplay => "{" + $"Count = {Count}, EndPoint = {EndPoint}" + "}";

        /// <summary>
        /// Обратный адрес отправителя.
        /// </summary>
        public EndPoint EndPoint { get; }
        /// <summary>
        /// Размер полученного фрейма. От 20 до 4096 байт.
        /// </summary>
        public int Count { get; }

        public ReceiveResult(EndPoint endPoint, int count)
        {
            EndPoint = endPoint;
            Count = count;
        }
    }
}
