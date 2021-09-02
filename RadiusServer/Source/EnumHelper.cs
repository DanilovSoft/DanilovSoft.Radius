using System;

namespace DanilovSoft.Radius
{
    internal static class EnumHelper
    {
        public static bool TryParse<T>(object value, out T result)
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                result = (T)value;
                return true;
            }
            result = default;
            return false;
        }
    }
}
