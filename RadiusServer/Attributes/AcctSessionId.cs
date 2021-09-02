using System;
using System.Text;
using System.Threading;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Unique Accounting ID to make it easy to match
    /// start and stop records in a log file.The start and stop records
    /// for a given session MUST have the same Acct-Session-Id.An
    /// Accounting-Request packet MUST have an Acct-Session-Id.An
    /// Access-Request packet MAY have an Acct-Session-Id; if it does,
    /// then the NAS MUST use the same Acct-Session-Id in the Accounting-
    /// Request packets for that session.
    /// </summary>
    [RadiusAttribute(AttributeType.Acct_Session_Id)]
    public class AcctSessionId : RadiusAttribute
    {
        private string _asUtf8;

        public AcctSessionId(ReadOnlySpan<byte> span) : base(AttributeType.Acct_Session_Id)
        {
            if(span.Length < 1)
            {
                IsValid = false;
                return;
            }

            Text = new ReadOnlyMemory<byte>(span.ToArray());
            IsValid = true;
        }

        public string AsUTF8 => LazyInitializer.EnsureInitialized(ref _asUtf8, () => Encoding.UTF8.GetString(Text.Span));

        /// <summary>
        /// SHOULD contain UTF-8 encoded characters.
        /// </summary>
        public ReadOnlyMemory<byte> Text { get; }

        public override string ToString()
        {
            return $"{FriendlyType} = \"{AsUTF8}\"";
        }
    }
}
