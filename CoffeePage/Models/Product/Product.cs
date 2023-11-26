namespace CoffeePage.Models.Product
{
    public class ProductPara
    {
        public int TypeID { get; set; } = 0;
        public int ProductID { get; set; } = 0;
        public int HasDisable { get; set; } = 0;
        public int PagingNumber { get; set; } = 1;
		public int IsBestSeller { get; set; } = 0;
		public int IsMaterial { get; set; } = -1;
		public string TextSearch { get; set; } = "";
        public int Limit { get; set; } = 1000;
    }
}
