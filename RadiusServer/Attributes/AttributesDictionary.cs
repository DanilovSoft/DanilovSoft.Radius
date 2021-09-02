using DanilovSoft.Radius.Attributes;
using DanilovSoft.Radius.Emit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DanilovSoft.Radius
{
    internal static class AttributesDictionary
    {
        private static readonly Dictionary<AttributeType, CreateAttributeDelegate> _attributesActivators;
        private static readonly Type[] _attributeCtorTypes = new[] { typeof(ReadOnlySpan<byte>) };

        static AttributesDictionary()
        {
            IEnumerable<Type> attributeTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(RadiusAttribute)) 
                && Attribute.IsDefined(x, typeof(RadiusAttributeAttribute)) /*&& x.GetConstructor(_attributeCtorTypes) != null*/);

            _attributesActivators = new Dictionary<AttributeType, CreateAttributeDelegate>(32 /* всего атрибутов 44 но сервер обрабатывает меньшее количество */);

            foreach (Type type in attributeTypes)
            {
                RadiusAttributeAttribute attrib = type.GetCustomAttribute<RadiusAttributeAttribute>(inherit: false);
                CreateAttributeDelegate activatorDelegate = DynamicMethodFactory.CreateConstructor<CreateAttributeDelegate>(type, _attributeCtorTypes);
                _attributesActivators.Add(attrib.Type, activatorDelegate);
            }

            Trace.WriteLine($"Создано {_attributesActivators.Count} атрибутов");
            Trace.WriteLine("Атрибуты прогреты");
        }

        public static bool GetAttribute(AttributeType attributeType, ReadOnlySpan<byte> span, out RadiusAttribute radiusAttribute)
        {
            if (_attributesActivators.TryGetValue(attributeType, out CreateAttributeDelegate attributeFactory))
            {
                RadiusAttribute attribute = attributeFactory(span);
                if (attribute.IsValid)
                {
                    radiusAttribute = attribute;
                    return true;
                }
            }
            DebugOnly.Break();
            radiusAttribute = null;
            return false;
        }
    }
}
