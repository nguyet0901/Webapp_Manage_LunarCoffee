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
namespace MLunarCoffee.Pages.Setting.Manufacture
{
    public class LevelDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _LevelID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string level = Request.Query["LevelID"];
            if (curr != null)
            {
                _CurrentID = curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
            if (level != null)
            {
                _LevelID = level.ToString();
            }
            else
            {
                _LevelID = "0";
            }
        }
       
         public async Task<IActionResult> OnPostLoadInitializeData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Product_Combo_Branch]", CommandType.StoredProcedure);
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
            catch (Exception ex)
            {
                return Content("");
            }
        }
       
         public async Task<IActionResult> OnPostLoadata(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Manufacture_Level_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
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
                LevelInBranch DataMain = JsonConvert.DeserializeObject<LevelInBranch>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Manufacture_Level_Insert]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                              "@IdBranch", SqlDbType.Int, DataMain.IdBranch
                          );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(JsonConvert.SerializeObject(dt));
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
                        DataTable dt =await connFunc.ExecuteDataTable("MLU_sp_Manufacture_Level_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@currentID ", SqlDbType.Int, CurrentID,
                            "@IdBranch ", SqlDbType.Int, DataMain.IdBranch

                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(JsonConvert.SerializeObject(dt));
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

       
         public async Task<IActionResult> OnPostDeleteItem(string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt =await connFunc.ExecuteDataTable("MLU_sp_Manufacture_Level_Delete", CommandType.StoredProcedure,
                        "@Modified_By", SqlDbType.Int, user.sys_userid,
                        "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                         "@currentID ", SqlDbType.Int, CurrentID
                    );
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
        public string IdBranch { get; set; }
    }
}
