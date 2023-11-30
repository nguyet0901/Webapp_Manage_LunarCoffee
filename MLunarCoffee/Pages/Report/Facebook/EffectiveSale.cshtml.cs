using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Report.Facebook
{
    public class EffectiveSaleModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
        }

        public async Task<IActionResult> OnPostLoadata(string dateFrom ,string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_rp_Effective_SaleFacebook]" ,CommandType.StoredProcedure
                        ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        ,"@Type" ,SqlDbType.Int ,1
                        ,"@CreatedBy" ,SqlDbType.Int ,0
                        ,"@PID" ,SqlDbType.NVarChar ,""
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadTele(string dateFrom ,string dateTo ,string PID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_rp_Effective_SaleFacebook]" ,CommandType.StoredProcedure
                        ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        ,"@Type" ,SqlDbType.Int ,2
                        ,"@CreatedBy" ,SqlDbType.Int ,0
                        ,"@PID" ,SqlDbType.NVarChar ,PID
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(string dateFrom ,string dateTo ,string CreatedBy)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_rp_Effective_SaleFacebook]" ,CommandType.StoredProcedure
                        ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        ,"@Type" ,SqlDbType.Int ,3
                        ,"@CreatedBy" ,SqlDbType.Int ,CreatedBy
                        ,"@PID" ,SqlDbType.NVarChar ,""
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
