namespace DanilovSoft.Radius.Attributes
{
    public enum ServiceTypeValue
    {
        /// <summary>
        /// Login.
        /// </summary>
        [EnumDisplayValue("Login")]
        Login = 1,
        /// <summary>
        /// Framed.
        /// </summary>
        [EnumDisplayValue("Framed")]
        Framed = 2,
        /// <summary>
        /// Callback Login.
        /// </summary>
        [EnumDisplayValue("Callback Login")]
        Callback_Login = 3,
        /// <summary>
        /// Callback Framed.
        /// </summary>
        [EnumDisplayValue("Callback Framed")]
        Callback_Framed = 4,
        /// <summary>
        /// Outbound.
        /// </summary>
        [EnumDisplayValue("Outbound")]
        Outbound = 5,
        /// <summary>
        /// Administrative.
        /// </summary>
        [EnumDisplayValue("Administrative")]
        Administrative = 6,
        /// <summary>
        /// NAS Prompt.
        /// </summary>
        [EnumDisplayValue("NAS Prompt")]
        NAS_Prompt = 7,
        /// <summary>
        /// Authenticate Only.
        /// </summary>
        [EnumDisplayValue("Authenticate Only")]
        Authenticate_Only = 8,
        /// <summary>
        /// Callback NAS Prompt.
        /// </summary>
        [EnumDisplayValue("Callback NAS Prompt")]
        Callback_NAS_Prompt = 9,
        /// <summary>
        /// Call Check.
        /// </summary>
        [EnumDisplayValue("Call Check")]
        Call_Check = 10,
        /// <summary>
        /// Callback Administrative.
        /// </summary>
        [EnumDisplayValue("Callback Administrative")]
        Callback_Administrative = 11,
    }
}
