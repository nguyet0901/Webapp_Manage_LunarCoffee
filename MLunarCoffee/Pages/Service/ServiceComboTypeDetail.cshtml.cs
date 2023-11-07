using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Service
{
    public class ServiceComboTypeDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
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
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_setting_SerComboType_LoadDetail]" ,CommandType.StoredProcedure ,
                  "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id));
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
        public async Task<IActionResult> OnPostDeleteComboType(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_setting_SerComboType_Delete]" ,CommandType.StoredProcedure ,
                         "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                         "@CurrentID" ,SqlDbType.Int ,id);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }

            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostExecuteData(string data ,string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                ServiceComboType DataMain = JsonConvert.DeserializeObject<ServiceComboType>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_setting_SerComboType_Insert]" ,CommandType.StoredProcedure ,
                          "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                          "@UserID" ,SqlDbType.Int ,user.sys_userid
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
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_setting_SerComboType_Update]" ,CommandType.StoredProcedure ,
                            "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                            "@CurrentID" ,SqlDbType.Int ,CurrentID
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

    public class ServiceComboType
    {
        public string Name { get; set; }
    }
}
