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
    public class RoomDetailModel : PageModel
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

         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Manufacture_Room_LoadDetail]", CommandType.StoredProcedure,
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
                RoomInLevel DataMain = JsonConvert.DeserializeObject<RoomInLevel>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Manufacture_Room_Insert]", CommandType.StoredProcedure,
                             "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                             "@IsTreatment" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsTreatment) ,
                             "@IsCashier" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsCashier) ,
                             "@IsXquang" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsXquang) ,
                             "@IsExamination" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsExamination) ,
                             "@Created_By" , SqlDbType.Int, user.sys_userid,
                             "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                             "@IdLevel", SqlDbType.Int, DataMain.IdLevel
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
                        DataTable dt =await connFunc.ExecuteDataTable("MLU_sp_Manufacture_Room_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@IsTreatment" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsTreatment) ,
                            "@IsCashier" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsCashier) ,
                            "@IsXquang" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsXquang) ,
                            "@IsExamination" ,SqlDbType.Int ,Convert.ToInt32(DataMain.IsExamination) ,
                            "@Modified_By" , SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@currentID ", SqlDbType.Int, CurrentID

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
                    DataTable dt =await connFunc.ExecuteDataTable("MLU_sp_Manufacture_Room_Delete", CommandType.StoredProcedure,
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
    public class RoomInLevel
    {
        public string Name { get; set; }
        public string IdLevel { get; set; }
        public string IsTreatment { get; set; }
        public string IsCashier { get; set; }
        public string IsXquang { get; set; }
        public string IsExamination { get; set; }
    }
}
