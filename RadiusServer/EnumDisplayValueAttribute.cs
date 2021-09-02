using System;

namespace DanilovSoft.Radius
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    internal class EnumDisplayValueAttribute : Attribute
    {
        public string Name { get; }

        public EnumDisplayValueAttribute(string name)
        {
            Name = name;
        }
    }
}
