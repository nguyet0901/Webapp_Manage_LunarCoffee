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

namespace MLunarCoffee.Pages.Setting.CustomerProperties
{
    public class CustomerPropertiesDetailModel : PageModel
    {
        public string _DataDetailID { get; set; }
        public string _dataUnit { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _DataDetailID = curr.ToString();
            }
            else
            {
                _DataDetailID = "0";
            }
        }
         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[YYY_sp_Setting_CustPropeties_LoadDetail]", CommandType.StoredProcedure,
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
            catch (Exception ex)
            {
                return Content("");
            }


        }
         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                CustProp DataMain = JsonConvert.DeserializeObject<CustProp>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Setting_CustPropeties_Insert]", CommandType.StoredProcedure,
                              "@Name ", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                              "@UserID", SqlDbType.Int, user.sys_userid,                              
                              "@Color ", SqlDbType.NVarChar, DataMain.Color.Replace("'", "").Trim(),
                              "@Col", SqlDbType.Int, DataMain.Col
                          );
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Setting_CustPropeties_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@currentID ", SqlDbType.Int, CurrentID,                            
                            "@Color ", SqlDbType.NVarChar, DataMain.Color.Replace("'", "").Trim(),
                            "@Col" ,SqlDbType.Int ,DataMain.Col

                        );
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class CustProp
    {
        public string Name { get; set; }        
        public string Color { get; set; }
        public string Col { get; set; }
    }

}
