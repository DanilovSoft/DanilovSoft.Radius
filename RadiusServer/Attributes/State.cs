using System;
using System.Diagnostics;

namespace DanilovSoft.Radius.Attributes
{
    /// <summary>
    /// Атрибут доступен для передачи от сервера к клиентам в пакетах Access-Challenge и должен передаваться 
    /// без изменения от клиента к серверу в новом пакете Access-Request, являющемся откликом на запрос (challenge), если таковой отклик передается.
    /// Этот атрибут доступен для передачи от сервера к клиентам в пакетах Access-Accept, которые включают также 
    /// атрибут Termination-Action Attribute со значением RADIUS-Request.Если сервер NAS выполняет операцию Termination-Action путем передачи 
    /// нового пакета Access-Request при разрыве текущего сеанса, он должен включить атрибут State в этот пакет Access-Request без изменения атрибута.
    /// В любом случае для клиента недопустима локальная интерпретация атрибута. Пакет не может включать более 
    /// одного атрибута State. Использование атрибутов State зависит от реализации.
    /// </summary>
    [RadiusAttribute(AttributeType.State)]
    public class State : RadiusAttribute
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebugDisplay => "{" + $"{Type}, String.Length = {String.Length}" + "}";

        public ReadOnlyMemory<byte> String { get; }

        public State(ReadOnlySpan<byte> span) : base(AttributeType.State)
        {
            if(span.Length < 1)
            {
                IsValid = false;
                return;
            }

            String = new ReadOnlyMemory<byte>(span.ToArray());
            IsValid = true;
        }

        //public State(ReadOnlySpan<byte> span) : base(AttributeType.State)
        //{
        //    if (data.Count == 0)
        //        throw new ArgumentOutOfRangeException("Lenght should be greater than 0");

        //    String = data;
        //    IsValid = true;
        //}

        //byte IOutputAttribute.Write(Span<byte> span)
        //{
            //byte attributeLength = (byte)(String.Length + 2);
            //buffer[offset] = (byte)Type;
            //buffer[offset + 1] = attributeLength;
            //Array.Copy(String.Array, String.Offset, buffer, (offset + 2), String.Length);
            //return attributeLength;
        //}

        public override string ToString()
        {
            return $"{FriendlyType} = 0x{Converter.ToHex(String.Span)}";
        }
    }
}
