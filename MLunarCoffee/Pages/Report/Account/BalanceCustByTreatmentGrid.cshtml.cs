using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Account
{
    public class BalanceCustByTreatmentGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branch { get; set; }
        public string _SheetID { get; set; }
        public string _Debt_By_Treatment { get; set; }
        public string sys_TreatManualAmount { get; set; }

        public void OnGet()
        {
            _Debt_By_Treatment = Global.sys_Debt_By_Treatment != null ? Global.sys_Debt_By_Treatment.ToString() : "0";
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branch = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
            sys_TreatManualAmount = Comon.Global.sys_TreatManualamount.ToString();
        }

        public async Task<IActionResult> OnPostLoadata(int branchID ,string dateFrom ,string dateTo ,string sersourceID ,string TreatManualAmount)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[sp_Report_Accountant_Liability_Dentist_V2]" ,CommandType.StoredProcedure ,
                        "@dateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       ,"@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       ,"@DentistCosmetic" ,SqlDbType.Int ,Global.sys_DentistCosmetic != null ? Global.sys_DentistCosmetic : 0
                       ,"@branchID" ,SqlDbType.Int ,branchID
                       ,"@sys_TreatManualAmount" ,SqlDbType.Int ,Convert.ToInt32(TreatManualAmount)
                       ,"@sersourceID" ,SqlDbType.Int ,sersourceID
                       );
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
