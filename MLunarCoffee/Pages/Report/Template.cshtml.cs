using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.Report
{
    public class Template : PageModel
    {
        public string _DataSheet { get; set; }
        public string _UseBranch { get; set; }
        public string _UseDate { get; set; }
        public string _UseTypeDate { get; set; }
        public string _UseWare { get; set; }
        public string _UseAllWare { get; set; }
        public string _UseMultiBranch { get; set; }
        public string _UseAllBranch { get; set; }
        public string _AllowRangeDate { get; set; }
        public string _UseSource { get; set; }
        public string _UseStaff { get; set; }
        public string _UseGroupStaff { get; set; }
        public string _UseMultiGroupStaff { get; set; }
        public string _UsePage { get; set; }
        public string _UseServiceCare { get; set; }
        public string _UseTag { get; set; }
        public void OnGet()
        {
            _UseBranch = Request.Query["UseBranch"].ToString();
            _UseDate = Request.Query["UseDate"].ToString();
            _UseTypeDate = Request.Query["UseTypeDate"].ToString();
            _DataSheet = Request.Query["Data"].ToString();
            _UseWare = Request.Query["UseWare"].ToString();
            _UseAllWare = Request.Query["UseAllWare"].ToString();
            _UseMultiBranch = Request.Query["UseMultiBranch"].ToString();
            _UseAllBranch = Request.Query["UseAllBranch"].ToString();
            _AllowRangeDate = Request.Query["AllowRangeDate"].ToString();
            _UseSource = Request.Query["UseSource"].ToString();
            _UseStaff = Request.Query["UseStaff"].ToString();
            _UseGroupStaff = Request.Query["UseGroupStaff"].ToString();
            _UseMultiGroupStaff = Request.Query["UseMultiGroupStaff"].ToString();
            _UsePage = Request.Query["UsePage"].ToString();
            _UseServiceCare = Request.Query["UseServiceCare"].ToString();
            _UseTag = Request.Query["UseTag"].ToString();
        }
    }
}
