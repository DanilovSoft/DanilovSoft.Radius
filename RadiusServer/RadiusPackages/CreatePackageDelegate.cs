using System;
using System.Net;

namespace DanilovSoft.Radius.Packages
{
    delegate RadiusPackage CreatePackageDelegate(RadiusServer radiusServer, ReadOnlySpan<byte> span, EndPoint endPoint);
}
