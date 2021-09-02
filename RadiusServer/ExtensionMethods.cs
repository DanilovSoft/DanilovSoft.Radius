using DanilovSoft.Radius.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DanilovSoft.Radius
{
    internal static class ExtensionMethods
    {
        private readonly static Dictionary<AttributeType, string> _dict;

        static ExtensionMethods()
        {
            var values = Enum.GetValues(typeof(AttributeType));
            _dict = new Dictionary<AttributeType, string>(values.Length);
            foreach (AttributeType item in values)
            {
                string strName = item.ToString();
                MemberInfo[] memInfo = typeof(AttributeType).GetMember(strName);
                var attr = memInfo[0].GetCustomAttributes(false).OfType<EnumDisplayValueAttribute>().FirstOrDefault();
                string name = attr != null ? attr.Name : strName;
                _dict.Add(item, name);
            }
        }

        public static string FriendlyName(this AttributeType attributeType)
        {
            return _dict[attributeType];
        }
    }

    internal static class ExtensionMethods2
    {
        private readonly static Dictionary<ServiceTypeValue, string> _dict;

        static ExtensionMethods2()
        {
            var values = Enum.GetValues(typeof(ServiceTypeValue));
            _dict = new Dictionary<ServiceTypeValue, string>(values.Length);
            foreach (ServiceTypeValue item in values)
            {
                string strName = item.ToString();
                MemberInfo[] memInfo = typeof(ServiceTypeValue).GetMember(strName);
                var attr = memInfo[0].GetCustomAttributes(false).OfType<EnumDisplayValueAttribute>().FirstOrDefault();
                string name = attr != null ? attr.Name : strName;
                _dict.Add(item, name);
            }
        }

        public static string FriendlyName(this ServiceTypeValue serviceTypeValue)
        {
            return _dict[serviceTypeValue];
        }
    }

    internal static class ExtensionMethods3
    {
        private readonly static Dictionary<NasPortTypeValue, string> _dict;

        static ExtensionMethods3()
        {
            var values = Enum.GetValues(typeof(NasPortTypeValue));
            _dict = new Dictionary<NasPortTypeValue, string>(values.Length);
            foreach (NasPortTypeValue item in values)
            {
                string strName = item.ToString();
                MemberInfo[] memInfo = typeof(NasPortTypeValue).GetMember(strName);
                var attr = memInfo[0].GetCustomAttributes(false).OfType<EnumDisplayValueAttribute>().FirstOrDefault();
                string name = attr != null ? attr.Name : strName;
                _dict.Add(item, name);
            }
        }

        public static string FriendlyName(this NasPortTypeValue value)
        {
            return _dict[value];
        }
    }
}
