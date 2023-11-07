namespace MLunarCoffee.Models.API.APIClient
{
    public class APIClient
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Url { get; set; } = "";
        public int IsPortal { get; set; } = 0;
        public int IsMobileApp { get; set; } = 0;
    }

    public class APIAuthen
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
    }
}
