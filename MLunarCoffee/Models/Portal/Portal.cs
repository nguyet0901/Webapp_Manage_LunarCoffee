namespace MLunarCoffee.Models.Portal
{
    public class Portal
    {
        public string apiKey { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string key { get; set; }
        public string tokenFCM { get; set; }
        public string branch { get; set; }
    }
    public class PortalFunction
    {
        public string code { get; set; }
    }
    public class PortalResult
    {
        public string token { get; set; }
        public string key { get; set; }
        public PortalFunction[] func { get; set; }
    }
    public class Ini
    {
        public string apiKey { get; set; }
        public string branch { get; set; }
    }

    public class PortalProfile
    {
        public string apiKey { get; set; }
        public string email1 { get; set; }
        public string address { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string name { get; set; }
        public string gender_ID { get; set; }
        public string language_ID { get; set; }
        public string branch_ID { get; set; }
        public string birthday { get; set; }
        public string created_By { get; set; }
        public string cityID { get; set; }
        public string district { get; set; }
        public string career { get; set; }
        public string phonecode { get; set; }
        public string nationalID { get; set; }
        public string diafree { get; set; }
        public string diaimg { get; set; }
        public string diadataSVG { get; set; }
        public string diadataQues { get; set; }
        public string diaNote { get; set; }
        public string diaType { get; set; }
        public string preQuestion { get; set; }

        public string representname { get; set; } = "";
        public string representphone { get; set; } = "";
        public string relationship { get; set; } = "0";
        public string identitycard { get; set; } = "";

    }
    public class Ratingus
    {
        public string? apiKey { get; set; }
        public RatingusItem[] dtItem { get; set; }
        public string customer { get; set; }
        public string branchid { get; set; }

    }
    public class RatingusItem
    {
        public string type { get; set; }
        public string note { get; set; }
        public string star { get; set; }
        public string empid { get; set; }

    }
    public class Ratingtype
    {
        public string? apiKey { get; set; }
        public int type { get; set; } = 0;
        public string custid { get; set; }
    }
}
