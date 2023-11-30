using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Stock
{
    public class StockList  : PageModel
    {

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadData(string curid,string type,string WareID,string dateFrom,string dateTo
            ,string beginex, string beginexts, string beginim,string limit)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds=await confunc.ExecuteDataSet("[MLU_sp_WareHouse_StockList]",CommandType.StoredProcedure
                        ,"@curid",SqlDbType.Int,curid
                        ,"@type",SqlDbType.Int,type
                        ,"@WareID",SqlDbType.Int,WareID
                        ,"@limit",SqlDbType.Int,Convert.ToInt32(limit)
                        ,"@beginex",SqlDbType.Int,Comon.Comon.DateTimeDMY_To_YMDHHMM(beginex)
                        ,"@beginexts",SqlDbType.Int,Comon.Comon.DateTimeDMY_To_YMDHHMM(beginexts)
                        ,"@beginim",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMDHHMM(beginim)
                        ,"@datefrom",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@dateto",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                }
                if(ds!=null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("0");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostUpdateSign(int id,string sign,string typeid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Stock_Update_Sign]",CommandType.StoredProcedure,
                        "@CurrentID",SqlDbType.Int,Convert.ToInt32(id),
                        "@userID",SqlDbType.Int,user.sys_userid,
                         "@Sign",SqlDbType.NVarChar,sign,
                         "@Type",SqlDbType.Int,Convert.ToInt32(typeid)
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
    }
}
