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
using Microsoft.AspNetCore.Http.Extensions;

namespace SourceMain.Pages.Disease
{

    public class DentistChartDiseaseDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            var curr = Request.Query["CurrentID"];
            if (curr.ToString() != String.Empty)
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
                dt = await confunc.ExecuteDataTable("[YYY_sp_Dentist_Chart_Disease_LoadDetail]", CommandType.StoredProcedure,
                  "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostExcuteData(string Name, string Description, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Dentist_Chart_Disease_Update]", CommandType.StoredProcedure,
                        "@Name", SqlDbType.NVarChar, Name.Replace("'", "").Trim(),
                        "@Description", SqlDbType.NVarChar, Description,
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@CurrentID", SqlDbType.Int, CurrentID
                    );
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            return Content("0");
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                    else
                    {
                        return Content("1");
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
