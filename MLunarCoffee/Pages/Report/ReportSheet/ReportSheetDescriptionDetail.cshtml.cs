using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.Report.ReportSheet
{
    public class ReportSheetDescriptionDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _Type { get; set; }

        public void OnGet()
        {
            string curr = Request.Query["SheetID"];
            string type = Request.Query["Type"];
            if (curr != null)
            {
                _Type = type;
                _CurrentID = curr;
            }
            else
            {
                _CurrentID = "0";
                _Type = "0";
            }
        }
         public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await  confunc.ExecuteDataTable("ZZZ_sp_ReportSheet_LoadDetail", CommandType.StoredProcedure,
                  "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            if (dt != null)
            {
                return Content(JsonConvert.SerializeObject(dt));
            }
            else
            {
                return Content("");
            }

        }
        
         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, int Type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                ReportSheet DataMain = JsonConvert.DeserializeObject<ReportSheet>(data);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    if (Type == 0)
                    {


                        DataTable dt =await connFunc.ExecuteDataTable("ZZZ_sp_ReportSheet_Description_Update", CommandType.StoredProcedure,
                            "@Description", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim()
                            , "@CurrentID", SqlDbType.Int, CurrentID
                            , "@Modified_by", SqlDbType.Int, user.sys_userid

                        ); ;
                    }
                    else
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("ZZZ_sp_ReportSheet_Description_Code_Update", CommandType.StoredProcedure,
                            "@Description_code", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim()
                            , "@CurrentID", SqlDbType.Int, CurrentID
                            , "@Modified_by", SqlDbType.Int, user.sys_userid

                        ); ;
                    }
                    return Content("1");
                }

            }


            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
    public class ReportSheet
    {
        public string Name { get; set; }
    }
}
