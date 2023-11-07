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

namespace MLunarCoffee.Pages.Setting.AppCustomer.Language
{
    public class AppCustomerEditLanguageDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _Type { get; set; }
        public void OnGet()
        {
            _CurrentID = Request.Query["CurrentID"].ToString();
            _Type = Request.Query["Type"].ToString();
        }
        public async Task<IActionResult> OnPostLoadDetail(int CurrentID, int Type)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    dt = await conFunc.ExecuteDataTable("[YYY_sp_AC_IniLanguage_LoadDetail]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, CurrentID
                        , "@Type",SqlDbType.Int, Type
                        );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string data ,int CurrentID, int Type)
        {
            try
            {
                EditLanguage dataMain = JsonConvert.DeserializeObject<EditLanguage>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                switch (Type)
                {
                    case 1:
                        using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                        {
                            dt = await conFunc.ExecuteDataTable("[YYY_sp_AC_IniLanguage_UpdateIcon]", CommandType.StoredProcedure
                                , "@textEN", SqlDbType.NVarChar, dataMain.TextEN
                                , "@textVi", SqlDbType.NVarChar, dataMain.TextVN
                                , "@Description", SqlDbType.NVarChar, dataMain.Description
                                , "@CurrentID", SqlDbType.Int, CurrentID
                                , "@Modified_by", SqlDbType.Int, user.sys_userid
                                );
                        }
                        break;
                    case 2:
                        using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                        {
                            dt = await conFunc.ExecuteDataTable("[YYY_sp_AC_IniLanguage_UpdateButton]", CommandType.StoredProcedure
                                , "@textEN", SqlDbType.NVarChar, dataMain.TextEN
                                , "@textVi", SqlDbType.NVarChar, dataMain.TextVN
                                , "@Description", SqlDbType.NVarChar, dataMain.Description
                                , "@CurrentID", SqlDbType.Int, CurrentID
                                , "@Modified_by", SqlDbType.Int, user.sys_userid
                                );
                        }
                        break;
                    case 3:
                        using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                        {
                            dt = await conFunc.ExecuteDataTable("[YYY_sp_AC_IniLanguage_UpdateLabel]", CommandType.StoredProcedure
                                , "@textEN", SqlDbType.NVarChar, dataMain.TextEN
                                , "@textVi", SqlDbType.NVarChar, dataMain.TextVN
                                , "@Description", SqlDbType.NVarChar, dataMain.Description
                                , "@CurrentID", SqlDbType.Int, CurrentID
                                , "@Modified_by", SqlDbType.Int, user.sys_userid
                                );
                        }
                        break;
                    default:
                        return Content("0");
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class EditLanguage
        {
            public string TextEN { get; set; }
            public string TextVN { get; set; }
            public string Description { get; set; }
        }
    }
}
