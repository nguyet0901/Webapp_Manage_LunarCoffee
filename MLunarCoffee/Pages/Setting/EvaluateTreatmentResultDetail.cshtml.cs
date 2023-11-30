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


namespace MLunarCoffee.Pages.Setting
{
    public class EvaluateTreatmentResultDetailModel : PageModel
    {
        public string _EvaluateDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _EvaluateDetailID = curr.ToString();
            }
            else
            {
                _EvaluateDetailID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_Evaluate_Treatment_Result_LoadDetail]", CommandType.StoredProcedure,
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
            catch
            {
                return Content("");
            }

        }


        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                Evaluate_Treatment_Result DataMain = JsonConvert.DeserializeObject<Evaluate_Treatment_Result>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_Evaluate_Treatment_Result_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@UserID", SqlDbType.Int, user.sys_userid,
                            "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_Evaluate_Treatment_Result_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@User_ID", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
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
public class Evaluate_Treatment_Result
{
    public string Name { get; set; }
    public string Note { get; set; }
}
