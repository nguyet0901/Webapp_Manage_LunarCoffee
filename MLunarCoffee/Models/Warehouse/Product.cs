namespace MLunarCoffee.Models.Warehouse
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
        public string TokenID { get; set; } = "";
        public string TokenTypeID { get; set; } = "";
        public int Limit { get; set; } = 1000;
    }

    public class ProductParaExec
    {
        public string data { get; set; } = "";
        public string dataUnit { get; set; } = "";
        public int CurrentID { get; set; } = 0;
    }

    public class Product
    {
        public string Image { get; set; } = "";
        public int TypeID { get; set; } = 0;
        public string Media { get; set; } = "[]";
        public string Name { get; set; } = "";
        public string NameNosign { get; set; } = "";
        public string Code { get; set; } = "";
        public int UnitID { get; set; } = 0;
        public int N1 { get; set; } = 0;
        public int N2 { get; set; } = 0;
        public int N3 { get; set; } = 0;
        public int IsMaterial { get; set; } = 0;
        public string Property { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal CostPrice { get; set; } = 0;
        public decimal PriceToSale { get; set; } = 0;
        public string Note { get; set; } = "";
    }
}
