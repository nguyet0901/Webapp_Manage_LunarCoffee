using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Setting
{
    public class UnitCountDetailModel : PageModel
    {
        public string _UnitCountID { get; set; }

        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)  _UnitCountID = curr.ToString();
            else _UnitCountID = "0";
        }

        public async Task<IActionResult> OnPostLoadData(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await confunc.ExecuteDataTable("[MLU_sp_Product_Unit_LoadDetail]", CommandType.StoredProcedure,
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

        public async Task<IActionResult> OnPostExcuteData(string Name, string Note, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                Note = Note != null ? Note : "";
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Unit_Insert]", CommandType.StoredProcedure,
                              "@Name ", SqlDbType.Int, Name.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                              "@Note ", SqlDbType.Int, Note.Replace("'", "").Trim()
                          );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("MLU_sp_Product_Unit_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, Name.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                             "@currentID ", SqlDbType.Int, CurrentID,
                              "@Note ", SqlDbType.NVarChar, Note.Replace("'", "").Trim()

                        );
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
