using System;
using System.Net;
using System.Text;

namespace DanilovSoft.Radius
{
    public abstract class BaseRadiusPackage
    {
        internal Code Code { get; private protected set; }
        internal byte Identifier { get; private protected set; }
        internal ReadOnlyMemory<byte> Authenticator { get; private protected set; }
        internal EndPoint EndPoint { get; private protected set; }
        /// <summary>
        /// Коллекция валидных аттрибутов в пакете.
        /// </summary>
        public virtual RadiusAttribute[] Attributes { get; private protected set; }
        internal bool IsValid { get; private protected set; }

        internal BaseRadiusPackage()
        {

        }

        /// <summary>
        /// Копирует Identifier и EndPoint.
        /// </summary>
        public BaseRadiusPackage(BaseRadiusPackage radiusPackage)
        {
            Identifier = radiusPackage.Identifier;
            EndPoint = radiusPackage.EndPoint;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{Code} with id {Identifier}, EndPoint: {EndPoint}");
            foreach (var attrib in Attributes)
            {
                sb.Append("  ");
                sb.AppendLine(attrib.ToString());
            }
            return sb.ToString();
        }
    }
}
