using DanilovSoft.Radius;
using DanilovSoft.Radius.Attributes;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace RadiusService
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TraceToConsole());

            var radiusServer = new RadiusServer(sharedSecret: "secret");
            radiusServer.RegisterVendorSpecificAttribute(14988, (byte)MikrotikAttributeType.Mikrotik_Host_IP, span => new MikrotikHostIP(span));
            radiusServer.AccessRequest += RadiusServer_AccessRequest;
            radiusServer.Start();
            Thread.Sleep(-1);
        }

        private static void RadiusServer_AccessRequest(object sender, AccessRequestEventArgs e)
        {
            CallingStationId callingStationId = e.AccessRequest.Attributes.OfType<CallingStationId>().FirstOrDefault();
            string mac = callingStationId?.AsString;
            string userName = e.AccessRequest.UserName?.Utf8String;
            bool passwordIsValid = e.AccessRequest.ValidatePassword("12345678");

            e.Response = new AccessAccept(e.AccessRequest);
        }
    }
}
