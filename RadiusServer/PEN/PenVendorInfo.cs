using System.Diagnostics;

namespace DanilovSoft.Radius
{
    [DebuggerDisplay("{Organization}")]
    public class PenVendorInfo
    {
        public string Organization { get; }
        public string Contact { get; }
        public string Email { get; }

        internal PenVendorInfo(string organization, string contact, string email)
        {
            Organization = organization;
            Contact = contact;
            Email = email;
        }
    }
}
