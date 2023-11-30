namespace MLunarCoffee.Models.VTTECH.Option
{
    public class DBOption
    {
        public int DentistCosmetic { get; set; } = 0;
        public int IsTreatmentPlan { get; set; } = 0;
        public int IsPatientRecord { get;set; } = 0;
        public int IsShowUserGuide { get; set; } = 0;
        public ImageOption ImageServer { get;set;}
    }


    public class ImageOption
    {
        public string ImageFolder { get; set; } = "";
        public string ImageUserName { get; set; } = "";
        public string ImagePassword { get; set; } = "";
    }
}
