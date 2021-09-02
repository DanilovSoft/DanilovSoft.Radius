using DanilovSoft.Radius.Packages;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DanilovSoft.Radius
{
    internal class MessageHandler : IDisposable
    {
        /// <summary>
        /// Максимальный размер буффера который использует RADIUS.
        /// </summary>
        internal const int MaximumPackageSize = 4096;
        internal const int MinimumPackageSize = 20;
        /// <summary>
        /// Общий буфер памяти для входящих и исходящих сообщений.
        /// </summary>
        internal static readonly ArrayPool<byte> SharedPool = ArrayPool<byte>.Shared;
        private byte[] _sendBuffer;
        private byte[] _receiveBuffer;
        private readonly UdpListener _udpListener;
        private readonly PackageDictionary _packages;
        private readonly RadiusServer _server;
        private int _started;

        public MessageHandler(RadiusServer radiusServer)
        {
            _server = radiusServer;
            _packages = new PackageDictionary(radiusServer);
            _udpListener = new UdpListener(RadiusServer.AuthenticationPort);
        }

        /// <summary>
        /// Потокобезопасно запускает сервер.
        /// </summary>
        /// <exception cref="InvalidOperationException">Возникает при попытке повторного запуска.</exception>
        public void Start()
        {
            // Запустить сервер можно только один раз.
            if (Interlocked.CompareExchange(ref _started, 1, 0) == 0)
            {
                ThreadPool.UnsafeQueueUserWorkItem(BackgroundReceive, null);
            }
            else
                throw new InvalidOperationException("Сервер уже запущен.");
        }

        // Фоновый поток обрабатывающий входящие сообщения.
        private async void BackgroundReceive(object _)
        {
            // Арендуем из пула блок для отправки сообщений.
            _sendBuffer = SharedPool.Rent(MaximumPackageSize);

            // Арендуем из пула блок для входящего сообщения.
            _receiveBuffer = SharedPool.Rent(MaximumPackageSize);

            try
            {
                while (true)
                {
                    // Считываем из сокета в блок памяти.
                    ReceiveResult inputFrame = await _udpListener.ReceiveAsync(_receiveBuffer);
                    
                    // Если тип пакета не определен то такой пакет отбрасывается без уведомления.
                    if (_packages.TryParse(_receiveBuffer.AsSpan(0, inputFrame.Count), inputFrame.EndPoint, out RadiusPackage package))
                    {
                        Trace.WriteLine($"Received {package}");

                        // Обрабатываем фрейм. Функция не генерирует исключения.
                        await ProcessPackageAsync((dynamic)package);
                    }
                }
            }
            finally
            {
                // Обязательно вернуть буфер в пул. Блок finally предохранит от ThreadAbortException.
                SharedPool.Return(_receiveBuffer);

                // Вернуть буфер в пул.
                SharedPool.Return(_sendBuffer);
            }
        }

        private async Task ProcessPackageAsync(AccessRequest accessRequest)
        {
            BaseRadiusPackage outPackage = _server.OnAccessRequest(accessRequest);
            //outPackage.GetResponseBuilder()

            //var accessChallenge = new AccessChallenge(accessRequest, _sendBuffer);
            //accessChallenge
            //    .AddReplyMessage("Привет!")
            //    .AddReplyMessage("Проверка");

            //var accept = new AccessAccept(accessRequest, _sendBuffer);

            await SendPackageAsync(outPackage);
        }

        private async Task SendPackageAsync(BaseRadiusPackage package)
        {
            var builder = new Builder(package, _server, _sendBuffer);
            ReadOnlyMemory<byte> memory = builder.Build();

            Trace.WriteLine($"Sending {package}");

            try
            {
                await _udpListener.SendAsync(_sendBuffer, 0, memory.Length, package.EndPoint);
            }
            catch (Exception ex)
            {
                Trace.Write(ex.ToString());
                Debugger.Break();
            }
        }

        public void Dispose()
        {
            _udpListener.Dispose();
        }
    }
}
