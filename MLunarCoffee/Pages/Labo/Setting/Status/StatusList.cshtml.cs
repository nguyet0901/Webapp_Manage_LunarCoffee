using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Labo.Setting.Status
{
    public class StatusListModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Setting_Status_LoadList]" ,CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostStatusFollowLoad(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Setting_Status_LoadStausFollow]" ,CommandType.StoredProcedure
                        ,"@CurrentID" ,SqlDbType.NVarChar ,CurrentID
                        );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostInfoDetailLoad(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Setting_Status_LoadDetail]" ,CommandType.StoredProcedure
                        ,"@CurrentID" ,SqlDbType.NVarChar ,CurrentID
                        );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostExecuteInfo(string CurrentID ,string Data)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(Data);
                var user = Session.GetUserSession(HttpContext);
                
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Setting_Status_Update]" ,CommandType.StoredProcedure
                        ,"@StatusName" ,SqlDbType.NVarChar ,DataMain.Rows[0]["StatusName"]
                        ,"@StatusContent" ,SqlDbType.NVarChar ,DataMain.Rows[0]["StatusContent"]
                        ,"@Color" ,SqlDbType.NVarChar ,DataMain.Rows[0]["Color"]
                        ,"@isBegin" ,SqlDbType.Int ,DataMain.Rows[0]["IsBegin"]
                        ,"@isCancel" ,SqlDbType.Int ,DataMain.Rows[0]["isCancel"]
                        ,"@isComplete" ,SqlDbType.Int ,DataMain.Rows[0]["isComplete"]
                        ,"@Modified_By" ,SqlDbType.Int ,user.sys_userid
                        ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)                        
                    );
                }
                
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostStatusFollowUpdate(string CurrentID ,string StatusFollow)
        {
            try
            {
                DataTable dt = new DataTable();                
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Setting_Status_StatusFollowUpdate]" ,CommandType.StoredProcedure
                        ,"@StatusFollow" ,SqlDbType.NVarChar ,StatusFollow
                        ,"@Modified_By" ,SqlDbType.Int ,user.sys_userid
                        ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
                    );
                }

                return Content("1");
            }
            catch
            {
                return Content("0");
            }
        }
    }
}
