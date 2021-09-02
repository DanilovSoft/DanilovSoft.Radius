using System;

namespace DanilovSoft.Radius
{
    public class AccessRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Валидный запрос Access-Request.
        /// </summary>
        public AccessRequest AccessRequest { get; }
        public BaseRadiusPackage Response { get; set; }

        internal AccessRequestEventArgs(AccessRequest accessRequest)
        {
            AccessRequest = accessRequest;
        }
    }
}
