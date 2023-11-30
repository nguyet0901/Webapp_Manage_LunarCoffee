using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.ListReport
{
    public class EditDescriptionReportGroupModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _dataDesDetail { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["SheetID"];
            if (curr != null)
            {
                _CurrentID = curr.ToString();

            }
            else
            {
                _CurrentID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_EditDescriptionReportGroup_LoadDetail", CommandType.StoredProcedure,
                  "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                LevelInBranch DataMain = JsonConvert.DeserializeObject<LevelInBranch>(data);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    DataTable dt = await connFunc.ExecuteDataTable("MLU_sp_EditDescriptionReportGroup_Update", CommandType.StoredProcedure,
                         "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim()
                        , "@Description", SqlDbType.NVarChar, DataMain.Description.Replace("'", "").Trim()
                        , "@CurrentID", SqlDbType.Int, CurrentID
                        , "@Modified_by", SqlDbType.Int, user.sys_userid

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

    public class LevelInBranch
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
