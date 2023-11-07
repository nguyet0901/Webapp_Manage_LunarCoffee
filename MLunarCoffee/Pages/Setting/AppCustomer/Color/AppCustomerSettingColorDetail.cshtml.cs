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


namespace MLunarCoffee.Pages.Setting.AppCustomer.Color
{
    public class AppCustomerSettingColorDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            _CurrentID = Request.Query["CurrentID"].ToString();
        }
        public async Task<IActionResult> OnPostLoadDetail(int CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    ds = await conFunc.ExecuteDataSet("[YYY_sp_AC_ColorCollect_LoadDetail]", CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data)
        {
            try
            {
                Setting_Detail DataMain = JsonConvert.DeserializeObject<Setting_Detail>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_ColorCollect_Update]", CommandType.StoredProcedure
                        , "@NameD", SqlDbType.NVarChar, DataMain.NameD
                        , "@TrueToneD", SqlDbType.NVarChar, DataMain.TrueToneD
                        , "@ColorToneD", SqlDbType.NVarChar, DataMain.ColorToneD
                        , "@ColorTone2D", SqlDbType.NVarChar, DataMain.ColorTone2D
                        , "@ColorTone3D", SqlDbType.NVarChar, DataMain.ColorTone3D
                        , "@HeavyTone1D", SqlDbType.NVarChar, DataMain.HeavyTone1D
                        , "@HeavyTone2D", SqlDbType.NVarChar, DataMain.HeavyTone2D
                        , "@HeavyTone3D", SqlDbType.NVarChar, DataMain.HeavyTone3D
                        , "@ColorTabBkD", SqlDbType.NVarChar, DataMain.ColorTabBkD
                        , "@ColorTabIconD" ,SqlDbType.NVarChar ,DataMain.ColorTabIconD
                        , "@ColorTabIconSelectD" ,SqlDbType.NVarChar ,DataMain.ColorTabIconSelectD
                        , "@ColorTabIconSelectHeaderD" ,SqlDbType.NVarChar ,DataMain.ColorTabIconSelectHeaderD

                        , "@NameN", SqlDbType.NVarChar, DataMain.NameN
                        , "@TrueToneN", SqlDbType.NVarChar, DataMain.TrueToneN
                        , "@ColorToneN", SqlDbType.NVarChar, DataMain.ColorToneN
                        , "@ColorTone2N", SqlDbType.NVarChar, DataMain.ColorTone2N
                        , "@ColorTone3N", SqlDbType.NVarChar, DataMain.ColorTone3N
                        , "@HeavyTone1N", SqlDbType.NVarChar, DataMain.HeavyTone1N
                        , "@HeavyTone2N", SqlDbType.NVarChar, DataMain.HeavyTone2N
                        , "@HeavyTone3N", SqlDbType.NVarChar, DataMain.HeavyTone3N
                        , "@ColorTabBkN" ,SqlDbType.NVarChar ,DataMain.ColorTabBkN
                        , "@ColorTabIconN" ,SqlDbType.NVarChar ,DataMain.ColorTabIconN
                        , "@ColorTabIconSelectN" ,SqlDbType.NVarChar ,DataMain.ColorTabIconSelectN
                        , "@ColorTabIconSelectHeaderN" ,SqlDbType.NVarChar ,DataMain.ColorTabIconSelectHeaderN
                        , "@Modified_By", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class Setting_Detail
        {
            public string NameD { get; set; }
            public string TrueToneD { get; set; }
            public string ColorToneD { get; set; }
            public string ColorTone2D { get; set; }
            public string ColorTone3D { get; set; }
            public string HeavyTone1D { get; set; }
            public string HeavyTone2D { get; set; }
            public string HeavyTone3D { get; set; }
            public string ColorTabBkD { get; set; }
            public string ColorTabIconD { get; set; }
            public string ColorTabIconSelectD { get; set; }
            public string ColorTabIconSelectHeaderD { get; set; }

            public string NameN { get; set; }
            public string TrueToneN { get; set; }
            public string ColorToneN { get; set; }
            public string ColorTone2N { get; set; }
            public string ColorTone3N { get; set; }
            public string HeavyTone1N { get; set; }
            public string HeavyTone2N { get; set; }
            public string HeavyTone3N { get; set; }
            public string ColorTabBkN { get; set; }
            public string ColorTabIconN { get; set; }
            public string ColorTabIconSelectN { get; set; }
            public string ColorTabIconSelectHeaderN { get; set; }
        }
    }
}
