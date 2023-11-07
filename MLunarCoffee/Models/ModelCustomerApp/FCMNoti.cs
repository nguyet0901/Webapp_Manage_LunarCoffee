namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class FCMNoti
    {
        public string to { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string icon { get; set; }
        public string data { get; set; }

    }
    public class AppNotiIns
    {
        public string token_id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string ID { get; set; }
        public string Image { get; set; }
        public string Platform { get; set; }
    }
    public class AppNotiTopic
    {
        public string topic { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string ID { get; set; }
        public string Image { get; set; }
        public string Platform { get; set; }
    }
}
