using System;

namespace DanilovSoft.Radius
{
    internal delegate RadiusAttribute CreateAttributeDelegate(ReadOnlySpan<byte> span);
}
