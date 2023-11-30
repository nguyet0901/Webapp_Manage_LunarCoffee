using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.ListReport
{
    public class ListReportShowDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _CurrentID = curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }

        }

        public async Task<IActionResult> OnPostLoad_Initialize(string currentid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Report_List_Show_LoadDetail]" ,CommandType.StoredProcedure ,
                      "@ID" ,SqlDbType.Int ,Convert.ToInt32(currentid));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataTable dt = new DataTable();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Report_Teamplate_Group_LoadCombo" ,CommandType.StoredProcedure);

                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        //XỬ LÍ THÊM SỬA DETAIL

        public async Task<IActionResult> OnPostExcuteDataDetail(string ReportDetail ,string CurrentID)
        {
            try
            {

                ReportDetail DataMain = JsonConvert.DeserializeObject<ReportDetail>(ReportDetail);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Report_List_Show_Update]" ,CommandType.StoredProcedure ,
                        "@ID" ,SqlDbType.Int ,CurrentID ,
                        "@GroupID" ,SqlDbType.Int ,DataMain.GroupID ,
                        "@Title" ,SqlDbType.NVarChar ,DataMain.Title ,
                        "@Teamplate" ,SqlDbType.NVarChar ,DataMain.Teamplate ,
                        "@Index" ,SqlDbType.Int ,DataMain.Index ,
                        "@UseBranch" ,SqlDbType.Int ,DataMain.UseBranch ,
                        "@UseDate" ,SqlDbType.Int ,DataMain.UseDate ,
                        "@UseTypeDate" ,SqlDbType.Int ,DataMain.UseTypeDate ,
                        "@Description" ,SqlDbType.NVarChar ,DataMain.Description ,
                        "@UseWare" ,SqlDbType.Int ,DataMain.UseWare ,
                        "@UseAllWare" ,SqlDbType.Int ,DataMain.UseAllWare ,
                        "@UseMultiBranch" ,SqlDbType.Int ,DataMain.UseMultiBranch ,
                        "@UseAllBranch" ,SqlDbType.Int ,DataMain.UseAllBranch ,
                        "@AllowRangeDate" ,SqlDbType.Int ,DataMain.AllowRangeDate ,
                        "@UseSource" ,SqlDbType.Int ,DataMain.UseSource ,
                        "@UseStaff" ,SqlDbType.Int ,DataMain.UseStaff ,
                        "@UseMultiGroupStaff" ,SqlDbType.Int ,DataMain.UseMultiGroupStaff ,
                        "@UseGroupStaff" ,SqlDbType.Int ,DataMain.UseGroupStaff ,
                        "@UsePage" ,SqlDbType.Int ,DataMain.UsePage ,
                        //"@State", SqlDbType.Int, DataMain.State,
                        "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                        "@Modified" ,SqlDbType.Int ,DateTime.Now ,
                        "@IsActive" ,SqlDbType.Int ,DataMain.IsActive
                    );

                    return Content("1");

                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class ReportDetail
        {
            public int GroupID { get; set; }
            public int Index { get; set; }
            public int UseBranch { get; set; }
            public int UseDate { get; set; }
            public int UseTypeDate { get; set; }
            public int UseWare { get; set; }
            public int UseAllWare { get; set; }
            public int UseMultiBranch { get; set; }
            public int UseAllBranch { get; set; }
            public int AllowRangeDate { get; set; }
            public int UseSource { get; set; }
            public int UseStaff { get; set; }
            public int UseGroupStaff { get; set; }
            public int UseMultiGroupStaff { get; set; }
            public int UsePage { get; set; }
            public int State { get; set; }
            public int IsActive { get; set; }
            public string Title { get; set; }
            public string Teamplate { get; set; }
            public string Description { get; set; }
        }
    }
}
