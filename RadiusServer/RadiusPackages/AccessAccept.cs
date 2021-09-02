using System;
using System.Diagnostics;

namespace DanilovSoft.Radius
{
    /// <summary>
    /// Пакет Access-Accept передаётся сервером RADIUS и содержит конфигурационные параметры, 
    /// необходимые для начала предоставления услуг пользователю. Если все значения атрибутов, 
    /// полученные в пакете Access-Request, приемлемы, реализация RADIUS должна передать пакет с Code = 2 (Access-Accept).
    /// </summary>
    [DebuggerDisplay("{DebugDisplay,nq}")]
    public class AccessAccept : BaseRadiusPackage
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebugDisplay => "{" + $"{Code}, Identifier = {Identifier}, Attributes = {Attributes.Length}" + "}";
        private readonly AccessRequest _accessRequest;
        public override RadiusAttribute[] Attributes { get; private protected set; } = Array.Empty<RadiusAttribute>();

        /// <summary>
        /// Создает разрешающий ответ для начала предоставления услуг пользователю.
        /// </summary>
        /// <param name="accessRequest">Основание — запрос пользователя.</param>
        public AccessAccept(AccessRequest accessRequest) : base(accessRequest)
        {
            _accessRequest = accessRequest;
            Code = Code.Access_Accept;

            // Копируем "Response Authenticator" из запроса.
            Authenticator = accessRequest.Authenticator;

            IsValid = true;
        }

        //internal RadiusPackage.Builder CreateBuilder(Memory<byte> memory)
        //{
        //    var builder = _accessRequest.GetResponseBuilder(Code.Access_Accept, memory);

        //    EapMessage eap = _accessRequest.Attributes.OfType<EapMessage>().FirstOrDefault();
        //    if (eap != null)
        //    {
        //        builder.AddAttribute(new EapMessage(new EapSucess(eap.EapPacket)));
        //        //_requiredMessageAuthenticator = true;
        //    }

        //    return builder;
        //}

        //private RadiusPackage ParseFromRequest()
        //{
        //    return new RadiusPackage(_accessRequest.Server, Memory.Span, Code, EndPoint);
        //}

        //internal int Build()
        //{
            //if (_requiredMessageAuthenticator)
            //{
            //    _builder.SetHasMessageAuthenticator();
            //}

            //Memory = _builder.Build();
            //_builder.WriteLength();

            //if (_requiredMessageAuthenticator)
            //{
            //    using (var hmacMd5 = new HMACMD5(_radiusPackage.RadiusServer.SharedSecret.Raw))
            //    {
            //        //hmacMd5.ComputeHash(Array, Offset, Length);
            //        //hmacMd5.Hash.CopyTo(Array, messageAuthenticatorOffset);
            //    }

                
            //}
            //Validate();
            //return Memory.Length;

            //_builder.WriteResponseAuthentificator();
        //}

        //[Conditional("DEBUG")]
        //private void Validate()
        //{
        //    var accessRequest = new AccessRequest(_accessRequest.Server, Memory.Span, EndPoint);
        //    if (!accessRequest.IsValid)
        //        DebugOnly.Break();
        //}
    }
}
