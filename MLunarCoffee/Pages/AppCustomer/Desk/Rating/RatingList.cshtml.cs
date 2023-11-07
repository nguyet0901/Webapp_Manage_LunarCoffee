using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.AppCustomer.Desk.Rating
{
    public class RatingListModel : PageModel
    {
        public string branchID { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public void OnGet()
        {
            string _branchID = Request.Query["branchID"];
            branchID = _branchID != null ? _branchID.ToString() : "0";

            string _dateFrom = Request.Query["dateFrom"];
            dateFrom = _dateFrom != null ? _dateFrom.ToString() : "";

            string _dateTo = Request.Query["dateTo"];
            dateTo = _dateTo != null ? _dateTo.ToString() : "";
        }
        public async Task<IActionResult> OnPostInit()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_Rating_LoadType]" ,CommandType.StoredProcedure);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadTotalData(int BranchID ,string DateFrom ,string DateTo)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_Rating_Total]" ,CommandType.StoredProcedure ,
                        "@BranchID" ,SqlDbType.Int ,BranchID ,
                        "@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateFrom) ,
                        "@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateTo));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadRating(int BranchID ,string DateFrom ,string DateTo ,string BeginID ,int Limit ,int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_Rating_LoadList]" ,CommandType.StoredProcedure ,
                        "@Limit" ,SqlDbType.Int ,Limit ,
                        "@BranchID" ,SqlDbType.Int ,BranchID ,
                        "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                        "@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateFrom) ,
                        "@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DateTo) ,
                        "@BeginID" ,SqlDbType.Int ,Convert.ToInt32(BeginID));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
