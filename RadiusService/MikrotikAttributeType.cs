using System.ComponentModel;

namespace RadiusService
{
    internal enum MikrotikAttributeType : byte
    {
        /// <summary>
        /// Mikrotik-Recv-Limit.
        /// </summary>
        [Description("Mikrotik-Recv-Limit")]
        Mikrotik_Recv_Limit = 1,

        /// <summary>
        /// Mikrotik-Xmit-Limit.
        /// </summary>
        [Description("Mikrotik-Xmit-Limit")]
        Mikrotik_Xmit_Limit = 2,

        /// <summary>
        /// Mikrotik-Group.
        /// </summary>
        [Description("Mikrotik-Group")]
        Mikrotik_Group = 3,

        /// <summary>
        /// Mikrotik-Wireless-Forward.
        /// </summary>
        [Description("Mikrotik-Wireless-Forward")]
        Mikrotik_Wireless_Forward = 4,

        /// <summary>
        /// Mikrotik-Wireless-Skip-Dot1x.
        /// </summary>
        [Description("Mikrotik-Wireless-Skip-Dot1x")]
        Mikrotik_Wireless_Skip_Dot1x = 5,

        /// <summary>
        /// Mikrotik-Wireless-Enc-Algo.
        /// </summary>
        [Description("Mikrotik-Wireless-Enc-Algo")]
        Mikrotik_Wireless_Enc_Algo = 6,

        /// <summary>
        /// Mikrotik-Wireless-Enc-Key.
        /// </summary>
        [Description("Mikrotik-Wireless-Enc-Key")]
        Mikrotik_Wireless_Enc_Key = 7,

        /// <summary>
        /// Mikrotik-Rate-Limit.
        /// </summary>
        [Description("Mikrotik-Rate-Limit")]
        Mikrotik_Rate_Limit = 8,

        /// <summary>
        /// Mikrotik-Realm.
        /// </summary>
        [Description("Mikrotik-Realm")]
        Mikrotik_Realm = 9,

        /// <summary>
        /// Mikrotik-Host-IP.
        /// </summary>
        [Description("Mikrotik-Host-IP")]
        Mikrotik_Host_IP = 10,

        /// <summary>
        /// Mikrotik-Mark-Id.
        /// </summary>
        [Description("Mikrotik-Mark-Id")]
        Mikrotik_Mark_Id = 11,

        /// <summary>
        /// Mikrotik-Advertise-URL.
        /// </summary>
        [Description("Mikrotik-Advertise-URL")]
        Mikrotik_Advertise_URL = 12,

        /// <summary>
        /// Mikrotik-Advertise-Interval.
        /// </summary>
        [Description("Mikrotik-Advertise-Interval")]
        Mikrotik_Advertise_Interval = 13,

        /// <summary>
        /// Mikrotik-Recv-Limit-Gigawords.
        /// </summary>
        [Description("Mikrotik-Recv-Limit-Gigawords")]
        Mikrotik_Recv_Limit_Gigawords = 14,

        /// <summary>
        /// Mikrotik-Xmit-Limit-Gigawords.
        /// </summary>
        [Description("Mikrotik-Xmit-Limit-Gigawords")]
        Mikrotik_Xmit_Limit_Gigawords = 15,

        /// <summary>
        /// Mikrotik-Wireless-PSK.
        /// </summary>
        [Description("Mikrotik-Wireless-PSK")]
        Mikrotik_Wireless_PSK = 16,

        /// <summary>
        /// Mikrotik-Total-Limit.
        /// </summary>
        [Description("Mikrotik-Total-Limit")]
        Mikrotik_Total_Limit = 17,

        /// <summary>
        /// Mikrotik-Total-Limit-Gigawords.
        /// </summary>
        [Description("Mikrotik-Total-Limit-Gigawords")]
        Mikrotik_Total_Limit_Gigawords = 18,

        /// <summary>
        /// Mikrotik-Address-List.
        /// </summary>
        [Description("Mikrotik-Address-List")]
        Mikrotik_Address_List = 19,

        /// <summary>
        /// Mikrotik-Wireless-MPKey.
        /// </summary>
        [Description("Mikrotik-Wireless-MPKey")]
        Mikrotik_Wireless_MPKey = 20,

        /// <summary>
        /// Mikrotik-Wireless-Comment.
        /// </summary>
        [Description("Mikrotik-Wireless-Comment ")]
        Mikrotik_Wireless_Comment = 21,

        /// <summary>
        /// Mikrotik-Delegated-IPv6-Pool.
        /// </summary>
        [Description("Mikrotik-Delegated-IPv6-Pool")]
        Mikrotik_Delegated_IPv6_Pool = 22,

        /// <summary>
        /// Mikrotik_DHCP_Option_Set.
        /// </summary>
        [Description("Mikrotik_DHCP_Option_Set")]
        Mikrotik_DHCP_Option_Set = 23,

        /// <summary>
        /// Mikrotik_DHCP_Option_Param_STR1.
        /// </summary>
        [Description("Mikrotik_DHCP_Option_Param_STR1")]
        Mikrotik_DHCP_Option_Param_STR1 = 24,

        /// <summary>
        /// Mikortik_DHCP_Option_Param_STR2.
        /// </summary>
        [Description("Mikortik_DHCP_Option_Param_STR2")]
        Mikortik_DHCP_Option_Param_STR2 = 25,

        /// <summary>
        /// Mikrotik_Wireless_VLANID.
        /// </summary>
        [Description("Mikrotik_Wireless_VLANID")]
        Mikrotik_Wireless_VLANID = 26,

        /// <summary>
        /// Mikrotik_Wireless_VLANIDtype.
        /// </summary>
        [Description("Mikrotik_Wireless_VLANIDtype")]
        Mikrotik_Wireless_VLANIDtype = 27,

        /// <summary>
        /// Mikrotik_Wireless_Minsignal.
        /// </summary>
        [Description("Mikrotik_Wireless_Minsignal")]
        Mikrotik_Wireless_Minsignal = 28,

        /// <summary>
        /// Mikrotik_Wireless_Maxsignal.
        /// </summary>
        [Description("Mikrotik_Wireless_Maxsignal")]
        Mikrotik_Wireless_Maxsignal = 29,
    }
}
