using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing
{
    public class TicketSourceCatDetailModel : PageModel
    {
        public string _SourceCatID { get; set; }
        public void OnGet()
        {
            var curr = Request.Query["CurrentID"];
            if (curr.ToString() != "")
            {
                _SourceCatID = curr.ToString();
            }
            else
            {
                _SourceCatID = "0";

            }

        }

         public async Task<IActionResult> OnPostLoadDataUpdate(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[MLU_sp_CustomerType_Cat_LoadDetail]", CommandType.StoredProcedure,
                        "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception e)
            {
                return Content("");
            }
        }

         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                Typecat DataMain = JsonConvert.DeserializeObject<Typecat>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_CustomerType_Cat_Insert]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Content", SqlDbType.Int, DataMain.Content.Replace("'", "").Trim()
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
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("MLU_sp_CustomerType_Cat_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@currentID ", SqlDbType.Int, CurrentID,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim()
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

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class Typecat
    {
        public string Name { get; set; }
        public string Content { get; set; }

    }
}
