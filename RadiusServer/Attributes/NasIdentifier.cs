using System;
using System.Text;
using System.Threading;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут содержит строку идентификации сервера NAS, передавшего пакет Access-Request. 
    /// Атрибут используется только в пакетах Access-Request. 
    /// Один из атрибутов NAS-IP-Address или NAS-Identifier должен присутствовать в каждом пакете Access-Request.
    /// </summary>
    [RadiusAttribute(AttributeType.NAS_Identifier)]
    public class NasIdentifier : RadiusAttribute
    {
        /// <summary>
        /// Содержит по крайней мере один октет.
        /// </summary>
        public ReadOnlyMemory<byte> String { get; }

        private string _AsUTF8;
        public string AsUTF8 => LazyInitializer.EnsureInitialized(ref _AsUTF8, () => Encoding.UTF8.GetString(String.Span));

        public NasIdentifier(ReadOnlySpan<byte> span) : base(AttributeType.NAS_Identifier)
        {
            if (span.Length < 1)
            {
                IsValid = false;
                return;
            }

            String = new ReadOnlyMemory<byte>(span.ToArray());
            IsValid = true;
        }

        public override string ToString()
        {
            return $"{FriendlyType} = \"{AsUTF8}\"";
        }
    }
}
