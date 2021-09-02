namespace DanilovSoft.Radius.Attributes
{
    public enum NasPortTypeValue
    {
        /// <summary>
        /// Async.
        /// </summary>
        [EnumDisplayValue("Async")]
        Async = 0,

        /// <summary>
        /// Sync.
        /// </summary>
        [EnumDisplayValue("Sync")]
        Sync = 1,

        /// <summary>
        /// ISDN Sync.
        /// </summary>
        [EnumDisplayValue("ISDN Sync")]
        ISDN_Sync = 2,

        /// <summary>
        /// ISDN Async V.120.
        /// </summary>
        [EnumDisplayValue("ISDN Async V.120")]
        ISDN_Async_V_120 = 3,

        /// <summary>
        /// ISDN Async V.110.
        /// </summary>
        [EnumDisplayValue("ISDN Async V.110")]
        ISDN_Async_V_110 = 4,

        /// <summary>
        /// Virtual.
        /// </summary>
        [EnumDisplayValue("Virtual")]
        Virtual = 5,

        /// <summary>
        /// PIAFS.
        /// </summary>
        [EnumDisplayValue("PIAFS")]
        PIAFS = 6,

        /// <summary>
        /// HDLC Clear Channel.
        /// </summary>
        [EnumDisplayValue("HDLC Clear Channel")]
        HDLC_Clear_Channel = 7,

        /// <summary>
        /// X.25.
        /// </summary>
        [EnumDisplayValue("X.25")]
        X_25 = 8,

        /// <summary>
        /// X.75.
        /// </summary>
        [EnumDisplayValue("X.75")]
        X_75 = 9,

        /// <summary>
        /// G.3 Fax.
        /// </summary>
        [EnumDisplayValue("G.3 Fax")]
        G_3_Fax = 10,

        /// <summary>
        /// SDSL.
        /// </summary>
        [EnumDisplayValue("SDSL")]
        SDSL = 11,

        /// <summary>
        /// ADSL-CAP.
        /// </summary>
        [EnumDisplayValue("ADSL-CAP")]
        ADSL_CAP = 12,

        /// <summary>
        /// ADSL-DMT.
        /// </summary>
        [EnumDisplayValue("ADSL-DMT")]
        ADSL_DMT = 13,

        /// <summary>
        /// IDSL.
        /// </summary>
        [EnumDisplayValue("IDSL")]
        IDSL = 14,

        /// <summary>
        /// Ethernet.
        /// </summary>
        [EnumDisplayValue("Ethernet")]
        Ethernet = 15,

        /// <summary>
        /// xDSL.
        /// </summary>
        [EnumDisplayValue("xDSL")]
        xDSL = 16,

        /// <summary>
        /// Cable.
        /// </summary>
        [EnumDisplayValue("Cable")]
        Cable = 17,

        /// <summary>
        /// Wireless - Other.
        /// </summary>
        [EnumDisplayValue("Wireless - Other")]
        Wireless_Other = 18,

        /// <summary>
        /// Wireless - IEEE 802.11.
        /// </summary>
        [EnumDisplayValue("Wireless - IEEE 802.11")]
        Wireless_IEEE_802_11 = 19
    }
}
