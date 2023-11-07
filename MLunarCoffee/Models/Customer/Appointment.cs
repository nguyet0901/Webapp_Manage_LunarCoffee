namespace MLunarCoffee.Models.Customer
{
    public class AppStatus
    {
        public string CurrentID { get; set; }
        public string AppToken { get; set; }
        public string AppPlatform { get; set; }
        public string AppUser { get; set; }
        public AppStatusDetail AppDetail { get; set; }
    }

    public class AppStatusDetail
    {
        public int StatusID { get; set; }
        public string Content { get; set; }
        public int ForectTime { get; set; }
        public int RoomID { get; set; }
        public int ChairID { get; set; }
        
        public int DoctorID { get; set; }
        public int DoctorID2 { get; set; }
        public int DoctorID3 { get; set; }
        public int DoctorID4 { get; set; }
        public int AssistID { get; set; }
        public int AssistID2 { get; set; }
        public int AssistID3 { get; set; }
        public int AssistID4 { get; set; }
        public int IsCheckout { get; set; }

    }
}
