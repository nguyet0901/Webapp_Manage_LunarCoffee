using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Myinfo.MyReport
{
    public class CommissionDoctorByPaidModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branchID { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branchID = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

    }
}
