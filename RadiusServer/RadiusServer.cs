using DanilovSoft.Radius.Packages;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DanilovSoft.Radius
{
    public class RadiusServer : IDisposable
    {
        public delegate VendorSpecific VendorSpecificAttributeFactory(ReadOnlySpan<byte> span);
        internal const int AuthenticationPort = 1812;
        internal readonly SharedSecret SharedSecret;
        private readonly MessageHandler _messageHandler;
        private int _disposed;
        private Dictionary<int, Dictionary<byte, VendorSpecificAttributeFactory>> _vendorSpecDict = new Dictionary<int, Dictionary<byte, VendorSpecificAttributeFactory>>();
        public event EventHandler<AccessRequestEventArgs> AccessRequest;

        static RadiusServer()
        {
            // Прогрев атрибутов через статический конструктор.
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(AttributesDictionary).TypeHandle);

            // Прогрев атрибутов через статический конструктор
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(PackageDictionary).TypeHandle);
        }

        internal BaseRadiusPackage OnAccessRequest(AccessRequest accessRequest)
        {
            var args = new AccessRequestEventArgs(accessRequest);
            AccessRequest?.Invoke(this, args);
            return args.Response;
        }

        public RadiusServer(string sharedSecret)
        {
            SharedSecret = new SharedSecret(sharedSecret);
            _messageHandler = new MessageHandler(this);
        }

        public void RegisterVendorSpecificAttribute(int vendorId, byte vendorType, VendorSpecificAttributeFactory factory)
        {
            // Первый октет vendorId не используется. можно записать туда vendorType.
            

            if (!_vendorSpecDict.TryGetValue(vendorId, out Dictionary<byte, VendorSpecificAttributeFactory> dict))
            {
                dict = new Dictionary<byte, VendorSpecificAttributeFactory>();
                _vendorSpecDict.Add(vendorId, dict);
            }

            dict.Add(vendorType, factory);
        }

        public void RegisterCustomVendorSpecificAttribute(int vendorId, VendorSpecificAttributeFactory factory)
        {

        }

        /// <summary>
        /// Потокобезопасно запускает сервер.
        /// </summary>
        /// <exception cref="InvalidOperationException">Возникает при попытке повторного запуска.</exception>
        public void Start()
        {
            _messageHandler.Start();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                _messageHandler.Dispose();
            }
        }
    }
}
