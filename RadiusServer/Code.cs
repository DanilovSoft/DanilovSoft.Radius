namespace DanilovSoft.Radius
{
    /// <summary>
    /// Идентификатор типа пакета. Имеет размер 1 октет.
    /// </summary>
    public enum Code : byte
    {
        /// <summary>
        /// Access-Request.
        /// </summary>
        Access_Request = 1,
        /// <summary>
        /// Access-Accept.
        /// </summary>
        Access_Accept = 2,
        /// <summary>
        /// Access-Reject.
        /// </summary>
        Access_Reject = 3,
        /// <summary>
        /// Accounting-Request.
        /// </summary>
        Accounting_Request = 4,
        /// <summary>
        /// Accounting-Response.
        /// </summary>
        Accounting_Response = 5,
        /// <summary>
        /// Access-Challenge.
        /// </summary>
        Access_Challenge = 11,
        /// <summary>
        /// Status-Server (экспериментальный).
        /// </summary>
        Status_Server = 12,
        /// <summary>
        /// Status-Client (экспериментальный).
        /// </summary>
        Status_Client = 13,
        /// <summary>
        /// Зарезервирован.
        /// </summary>
        Reserved = 255,
    }
}
