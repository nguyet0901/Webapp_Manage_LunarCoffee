namespace MLunarCoffee.Models.Product
{
    public class ProductPara
    {
        public int TypeID { get; set; } = 0;
        public int ProductID { get; set; } = 0;
        public int HasDisable { get; set; } = 0;
        public int PagingNumber { get; set; } = 1;
        public string TextSearch { get; set; } = "";
        public int Limit { get; set; } = 1000;
    }

    public class ProductParaExec
    {
        public string data { get; set; } = "";
        public string dataUnit { get; set; } = "";
        public string dataPackage { get; set; } = "";
        public int CurrentID { get; set; } = 0;
    }
}
