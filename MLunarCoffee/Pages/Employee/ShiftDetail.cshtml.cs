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

namespace MLunarCoffee.Pages.Employee
{
    public class ShiftDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _dataShift { get; set; }

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

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Shift_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                Shift DataMain = JsonConvert.DeserializeObject<Shift>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Shift_Insert]", CommandType.StoredProcedure,
                              "@Code ", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                              "@ColorCode", SqlDbType.NVarChar, DataMain.ColorCode.Replace("'", "").Trim(),
                              "@HourFrom", SqlDbType.NVarChar, DataMain.Hour_from.Replace("'", "").Trim(),
                              "@HourTo", SqlDbType.NVarChar, DataMain.Hour_to.Replace("'", "").Trim(),
                              "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@OverNight", SqlDbType.Int, DataMain.OverNight

                          );

                        if (dt.Rows[0][0].ToString() != "-1")
                        {
                            return Content("1");
                        }
                        else
                        {
                            return Content("-1");
                        }

                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("MLU_sp_Shift_Update", CommandType.StoredProcedure,
                            "@Code", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                            "@ColorCode", SqlDbType.NVarChar, DataMain.ColorCode.Replace("'", "").Trim(),
                            "@HourFrom", SqlDbType.NVarChar, DataMain.Hour_from.Replace("'", "").Trim(),
                            "@HourTo", SqlDbType.NVarChar, DataMain.Hour_to.Replace("'", "").Trim(),
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@CurrentID ", SqlDbType.Int, CurrentID,
                            "@OverNight", SqlDbType.Int, DataMain.OverNight
                        );

                        if (dt.Rows[0][0].ToString() != "-1")
                        {
                            return Content("1");
                        }
                        else
                        {
                            return Content("-1");
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
    public class Shift
    {
        public string Code { get; set; }
        public string ColorCode { get; set; }
        public string Hour_from { get; set; }
        public string Hour_to { get; set; }
        public string Note { get; set; }
        public int OverNight { get; set; }
    }
}
