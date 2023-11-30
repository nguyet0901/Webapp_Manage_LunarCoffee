using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Require 
{
    public class RequireList : PageModel
    {
        public string layout { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            CurrentPath = HttpContext.Request.Path;
            if(_layout!=null)
            {
                layout=_layout;
            }
            else layout="";
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
                    dt=await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_LoadCombo]",CommandType.StoredProcedure
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
        public async Task<IActionResult> OnPostLoadData(string curid ,string WareID,string dateFrom,string dateTo
            ,string type
            ,string beginor,string begintr, string beginorw, string limit)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_WareHouse_FormOrder]",CommandType.StoredProcedure
                        ,"@curid",SqlDbType.Int,curid
                        ,"@WareID",SqlDbType.Int,WareID
                        ,"@limit",SqlDbType.Int,Convert.ToInt32(limit)
                        ,"@begintr", SqlDbType.Int,Convert.ToInt32(begintr)
                        ,"@beginor",SqlDbType.Int,Convert.ToInt32(beginor)
                        ,"@beginorw", SqlDbType.Int,Convert.ToInt32(beginorw)
                        ,"@type",SqlDbType.Int,Convert.ToInt32(type)
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
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Product_Stock_Update_Sign]",CommandType.StoredProcedure,
                        "@CurrentID",SqlDbType.Int,Convert.ToInt32(id),
                        "@userID",SqlDbType.Int,user.sys_userid,
                         "@Sign",SqlDbType.NVarChar,sign,
                         "@Type",SqlDbType.Int,Convert.ToInt32(typeid)
                    );
                }
                return Content("1");
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
    }
}
