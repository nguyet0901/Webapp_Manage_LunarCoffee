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
    public class TypeCustomerStatusDetailModel : PageModel
    {
        public string _TelePageTypeCustomerStatusDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _TelePageTypeCustomerStatusDetailID = curr.ToString();
            }
            else
            {
                _TelePageTypeCustomerStatusDetailID = "0";
            }
        }

        public  async Task<IActionResult> OnPostLoadata(int id)
        {
            try {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_TypeCustomerStatus_LoadDetails]", CommandType.StoredProcedure,
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
            catch { return Content(""); }
        }

        public  async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                TypeCustomerStatus DataMain = JsonConvert.DeserializeObject<TypeCustomerStatus>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_TypeCustomerStatus_Insert]", CommandType.StoredProcedure,
                          "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                          "@UserID", SqlDbType.Int, user.sys_userid,
                          "@Created", SqlDbType.Int, DateTime.Now
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Trùng dữ liệu");
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
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_TypeCustomerStatus_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@User_ID", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Modified", SqlDbType.Int, DateTime.Now
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Trùng dữ liệu");
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

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
public class TypeCustomerStatus
{
    public string Name { get; set; }
    public string Note { get; set; }
}