namespace DanilovSoft.Radius
{
    public enum AttributeType : byte
    {
        // RFC 2865
        /// <summary>
        /// User-Name.
        /// </summary>
        [EnumDisplayValue("User-Name")]
        User_Name = 1,

        /// <summary>
        /// User-Password
        /// </summary>
        [EnumDisplayValue("User-Password")]
        User_Password = 2,

        /// <summary>
        /// CHAP-Password.
        /// </summary>
        [EnumDisplayValue("CHAP-Password")]
        CHAP_Password = 3,

        /// <summary>
        /// NAS-IP-Address
        /// </summary>
        [EnumDisplayValue("NAS-IP-Address")]
        NAS_IP_Address = 4,

        /// <summary>
        /// NAS-Port.
        /// </summary>
        [EnumDisplayValue("NAS-Port")]
        NAS_Port = 5,
        /// <summary>
        /// Service-Type.
        /// </summary>
        [EnumDisplayValue("Service-Type")]
        Service_Type = 6,

        /// <summary>
        /// Framed-Protocol.
        /// </summary>
        [EnumDisplayValue("Framed-Protocol")]
        Framed_Protocol = 7,

        /// <summary>
        /// Framed-IP-Address.
        /// </summary>
        [EnumDisplayValue("Framed-IP-Address")]
        Framed_IP_Address = 8,

        /// <summary>
        /// Framed-IP-Netmask.
        /// </summary>
        [EnumDisplayValue("Framed-IP-Netmask")]
        Framed_IP_Netmask = 9,

        /// <summary>
        /// Framed-Routing.
        /// </summary>
        [EnumDisplayValue("Framed-Routing")]
        Framed_Routing = 10,

        /// <summary>
        /// Filter-Id.
        /// </summary>
        [EnumDisplayValue("Filter-Id")]
        Filter_Id = 11,

        /// <summary>
        /// Framed-MTU.
        /// </summary>
        [EnumDisplayValue("Framed-MTU")]
        Framed_MTU = 12,

        /// <summary>
        /// Framed-Compression.
        /// </summary>
        [EnumDisplayValue("Framed-Compression")]
        Framed_Compression = 13,

        /// <summary>
        /// Login-IP-Host.
        /// </summary>
        [EnumDisplayValue("Login-IP-Host")]
        Login_IP_Host = 14,

        /// <summary>
        /// Login-Service.
        /// </summary>
        [EnumDisplayValue("Login-Service")]
        Login_Service = 15,

        /// <summary>
        /// Login-TCP-Port.
        /// </summary>
        [EnumDisplayValue("Login-TCP-Port")]
        Login_TCP_Port = 16,

        /// <summary>
        /// Reply-Message.
        /// </summary>
        [EnumDisplayValue("Reply-Message")]
        Reply_Message = 18,

        /// <summary>
        /// Callback-Number.
        /// </summary>
        [EnumDisplayValue("Callback-Number")]
        Callback_Number = 19,

        /// <summary>
        /// Callback-Id.
        /// </summary>
        [EnumDisplayValue("Callback-Id")]
        Callback_Id = 20,

        /// <summary>
        /// Framed-Route.
        /// </summary>
        [EnumDisplayValue("Framed-Route")]
        Framed_Route = 22,

        /// <summary>
        /// Framed-IPX-Network.
        /// </summary>
        [EnumDisplayValue("Framed-IPX-Network")]
        Framed_IPX_Network = 23,

        /// <summary>
        /// State.
        /// </summary>
        [EnumDisplayValue("State")]
        State = 24,

        /// <summary>
        /// Class.
        /// </summary>
        [EnumDisplayValue("Class")]
        Class = 25,

        /// <summary>
        /// Vendor-Specific.
        /// </summary>
        [EnumDisplayValue("Vendor-Specific")]
        Vendor_Specific = 26,

        /// <summary>
        /// Session-Timeout.
        /// </summary>
        [EnumDisplayValue("Session-Timeout")]
        Session_Timeout = 27,

        /// <summary>
        /// Idle-Timeout.
        /// </summary>
        [EnumDisplayValue("Idle-Timeout")]
        Idle_Timeout = 28,

        /// <summary>
        /// Termination-Action.
        /// </summary>
        [EnumDisplayValue("Termination-Action")]
        Termination_Action = 29,

        /// <summary>
        /// Called-Station-Id.
        /// </summary>
        [EnumDisplayValue("Called-Station-Id")]
        Called_Station_Id = 30,

        /// <summary>
        /// Calling-Station-Id
        /// </summary>
        [EnumDisplayValue("Calling-Station-Id")]
        Calling_Station_Id = 31,

        /// <summary>
        /// NAS-Identifier.
        /// </summary>
        [EnumDisplayValue("NAS-Identifier")]
        NAS_Identifier = 32,

        /// <summary>
        /// Proxy-State.
        /// </summary>
        [EnumDisplayValue("Proxy-State")]
        Proxy_State = 33,

        /// <summary>
        /// Login-LAT-Service.
        /// </summary>
        [EnumDisplayValue("Login-LAT-Service")]
        Login_LAT_Service = 34,

        /// <summary>
        /// Login-LAT-Node.
        /// </summary>
        [EnumDisplayValue("Login-LAT-Node")]
        Login_LAT_Node = 35,

        /// <summary>
        /// Login-LAT-Group.
        /// </summary>
        [EnumDisplayValue("Login-LAT-Group")]
        Login_LAT_Group = 36,

        /// <summary>
        /// Framed-AppleTalk-Link.
        /// </summary>
        [EnumDisplayValue("Framed-AppleTalk-Link")]
        Framed_AppleTalk_Link = 37,

        /// <summary>
        /// Framed-AppleTalk-Network.
        /// </summary>
        [EnumDisplayValue("Framed-AppleTalk-Network")]
        Framed_AppleTalk_Network = 38,

        /// <summary>
        /// Framed-AppleTalk-Zone.
        /// </summary>
        [EnumDisplayValue("Framed-AppleTalk-Zone")]
        Framed_AppleTalk_Zone = 39,

        // из RFC 2866
        /// <summary>
        /// Acct-Session-Id.
        /// </summary>
        [EnumDisplayValue("Acct-Session-Id")]
        Acct_Session_Id = 44,

        /// <summary>
        /// CHAP-Challenge.
        /// </summary>
        [EnumDisplayValue("CHAP-Challenge")]
        CHAP_Challenge = 60,

        /// <summary>
        /// NAS-Port-Type.
        /// </summary>
        [EnumDisplayValue("NAS-Port-Type")]
        NAS_Port_Type = 61,

        /// <summary>
        /// Port-Limit.
        /// </summary>
        [EnumDisplayValue("Port-Limit")]
        Port_Limit = 62,

        /// <summary>
        /// Login-LAT-Port.
        /// </summary>
        [EnumDisplayValue("Login-LAT-Port")]
        Login_LAT_Port = 63,

        // дополнительные из RFC 2869
        /// <summary>
        /// Nas-Port-Id.
        /// </summary>
        [EnumDisplayValue("Nas-Port-Id")]
        Nas_Port_Id = 87,

        /// <summary>
        /// Connect-Info.
        /// </summary>
        [EnumDisplayValue("Connect-Info")]
        Connect_Info = 77,

        /// <summary>
        /// EAP-Message.
        /// </summary>
        [EnumDisplayValue("EAP-Message")]
        EAP_Message = 79,

        /// <summary>
        /// Message-Authenticator.
        /// </summary>
        [EnumDisplayValue("Message-Authenticator")]
        Message_Authenticator = 80,
    }
}
