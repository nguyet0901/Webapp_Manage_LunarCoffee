using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Dash
{
    public class FindTicket : PageModel
    {

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_LoadCombo]",CommandType.StoredProcedure
                    ,"@UserID",SqlDbType.Int,user.sys_userid
                    ,"@LoadNormal",SqlDbType.Int,1);
                    dt.TableName="Warehouse";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch(Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string code,string WareID,string dateFrom,string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds=await connFunc.ExecuteDataSet("[YYY_sp_v2_WareHouse_FindTicket]",CommandType.StoredProcedure,
                      "@WareID",SqlDbType.Int,WareID,
                      "@ProductCode",SqlDbType.NVarChar,code,
                      "@DateFrom",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                      "@DateTo",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostSeachProduct(string TextSeach)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_WareHouse_FindProduct]", CommandType.StoredProcedure
                        , "@TextSeach", SqlDbType.NVarChar, TextSeach
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
