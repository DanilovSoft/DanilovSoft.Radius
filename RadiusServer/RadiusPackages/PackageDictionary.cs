using DanilovSoft.Radius.Emit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;

namespace DanilovSoft.Radius.Packages
{
    internal class PackageDictionary
    {
        /// <summary>
        /// Словарь содержащий активароты для разных типов пакетов.
        /// </summary>
        private static readonly Dictionary<Code, CreatePackageDelegate> _packageActivators;
        private static readonly Type[] _сtorTypes = new[] { typeof(RadiusServer), typeof(ReadOnlySpan<byte>), typeof(EndPoint) };
        private readonly RadiusServer _radiusServer;

        static PackageDictionary()
        {
            var attributeTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(RadiusPackage)) && Attribute.IsDefined(x, typeof(RadiusPackageAttribute)));

            _packageActivators = new Dictionary<Code, CreatePackageDelegate>(6 /* всего 8 типов пакетов но сервер обрабатывает меньшее количество */);
            foreach (var type in attributeTypes)
            {
                var attrib = type.GetCustomAttribute<RadiusPackageAttribute>(inherit: false);
                var activatorDelegate = DynamicMethodFactory.CreateConstructor<CreatePackageDelegate>(type, _сtorTypes);
                _packageActivators.Add(attrib.Code, activatorDelegate);
            }
        }

        public PackageDictionary(RadiusServer radiusServer)
        {
            _radiusServer = radiusServer;
        }

        public bool TryParse(ReadOnlySpan<byte> span, EndPoint endPoint, out RadiusPackage radiusPackage)
        {
            // Фрейм должен быть валидного размера иначе такой пакет отбрасывается без уведомления.
            if (span.Length >= MessageHandler.MinimumPackageSize && span.Length <= MessageHandler.MaximumPackageSize)
            {
                // Если идентификатор не определен то такой пакет отбрасывается без уведомления.
                // В первом байте содержится идентификатор.
                if (EnumHelper.TryParse(span[0], out Code code))
                {
                    if (_packageActivators.TryGetValue(code, out CreatePackageDelegate ctorDelegate))
                    {
                        RadiusPackage package = ctorDelegate(_radiusServer, span, endPoint);
                        if (package.IsValid)
                        {
                            radiusPackage = package;
                            return true;
                        }
                    }
                }
            }

            DebugOnly.Break();
            radiusPackage = null;
            return false;
        }
    }
}
