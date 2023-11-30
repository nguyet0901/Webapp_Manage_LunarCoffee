using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Report.Labo.Material
{
    public class MaterialLaboGeneral : PageModel
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
        public async Task<IActionResult> OnPostLoadata(string branchID, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Report_Labo_Material_General]", CommandType.StoredProcedure
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@branchID", SqlDbType.Int, Convert.ToInt32(branchID)
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch);

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
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadataDetail(string branchID, string MaterialID, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Report_Labo_Material_Detail]", CommandType.StoredProcedure
                       , "@branchID", SqlDbType.Int, Convert.ToInt32(branchID)
                       , "@MaterialID", SqlDbType.Int, Convert.ToInt32(MaterialID)
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                       , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch);
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
                return Content("[]");
            }
        }
    }
}

