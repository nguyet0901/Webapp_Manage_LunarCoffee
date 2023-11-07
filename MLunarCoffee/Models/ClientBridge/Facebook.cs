using System;

namespace MLunarCoffee.Models.ClientBridge
{
    public class FbFanpagePara
    {
        public string Keypage { get; set; }
        public int UserID { get; set; }
        public string LongAccess { get; set; }
        public string PageName { get; set; }
        public string PageLink { get; set; }
        public string Token { get; set; }
    }
    public class FbFanpage
    {
        public string ClientName { get; set; }
        public int ClientID { get; set; }
        public string Keypage { get; set; }
        public string LongAccess { get; set; }
        public int UserID { get; set; }
        public string PageName { get; set; }
        public string PageLink { get; set; }
        public string Token { get; set; }
    }
    public class FbConverItem
    {
        public string converid { get; set; }
        public string fromid { get; set; }
        public string lastsender { get; set; }
        public string fromname { get; set; }
        public string fromavatar { get; set; }
        public string keypage { get; set; }
        public string snippet { get; set; }
        public int unread_count { get; set; }
        public DateTime updated_time { get; set; }
    }
    public class FbConverPara
    {
        public string KeyPage { get; set; }
        public FbConverItem[] items { get; set; }
    }
    public class FbConver
    {
        public int ClientID { get; set; }
        public string KeyPage { get; set; }
        public FbConverItem[] items { get; set; }
    }

    public class ConverInfo
    {
        public string converid { get; set; } = "";
        public string pageID { get; set; } = "";
        public string search { get; set; } = "";
        public double numBeginDate { get; set; } = 0;
        public int limit { get; set; } = 10000;
        public int hide { get; set; } = 0;
        public int read { get; set; } = 0;
        public int star { get; set; } = 0;
        public int tagid { get; set; } = 0;
    }
    public class FbConverAction
    {
        public string converid { get; set; }
        public string keypage { get; set; }
        public string status { get; set; }
        public string content { get; set; }
    }

    public class FbConverMessAction
    {
        public string converid { get; set; }
        public string keypage { get; set; }
    }

    public class FbTag
    {
        public string converid { get; set; }
        public string keypage { get; set; }
        public string tagid { get; set; }
        public string tag { get; set; }
    }



}
