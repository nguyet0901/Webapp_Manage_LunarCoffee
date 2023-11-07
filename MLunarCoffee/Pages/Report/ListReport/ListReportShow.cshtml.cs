using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.ListReport
{
    public class ListReportShowModel : PageModel
    {
        public string layout { get; set; }
        public string _AllowGroup { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = (_layout != null) ? _layout : "";
            _AllowGroup = Comon.Global.sys_groupreport;
        }

        public async Task<IActionResult> OnPostLoadata(int currentID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Report_List_Show]" ,CommandType.StoredProcedure);

                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
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

        public async Task<IActionResult> OnPostExcuteData(string IsActive ,string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    DataTable dt = await connFunc.ExecuteDataTable("ZZZ_sp_EditDescriptionReportGroup_Active" ,CommandType.StoredProcedure ,
                         "@IsActive" ,SqlDbType.NVarChar ,IsActive
                        ,"@CurrentID" ,SqlDbType.Int ,CurrentID
                        ,"@Modified_by" ,SqlDbType.Int ,user.sys_userid

                    ); ;
                    return Content("1");
                }

            }


            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteDataDetail(string IsActive ,string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    DataTable dt = await connFunc.ExecuteDataTable("ZZZ_sp_EditDescriptionReport_Active" ,CommandType.StoredProcedure ,
                         "@IsActive" ,SqlDbType.NVarChar ,IsActive
                        ,"@CurrentID" ,SqlDbType.Int ,CurrentID
                        ,"@Modified_by" ,SqlDbType.Int ,user.sys_userid

                    ); ;
                    return Content("1");
                }

            }


            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
