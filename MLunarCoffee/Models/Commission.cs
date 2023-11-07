

namespace MLunarCoffee.Models
{

    public class CommissionLoad
    {
        public string date { get; set; }
        public int maxdate { get; set; }
        public int branch { get; set; }
    }

    public class CommissionDetail
    {
        public string date { get; set; }
        public int maxdate { get; set; }
        public int limit { get; set; }
        public int empid { get; set; }
        public int branch { get; set; }
    }
}
