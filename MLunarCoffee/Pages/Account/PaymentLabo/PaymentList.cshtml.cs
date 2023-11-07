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
 
namespace MLunarCoffee.Pages.Account.PaymentLabo
{
    public class PaymentList : PageModel
    {
        public string _versionofWebApplication { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            _versionofWebApplication = Comon.Global.sys_DBVersion;
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";

        }
         public async Task<IActionResult> OnPostLoadFunction()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Account_LaboList]", CommandType.StoredProcedure);

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
        public async Task<IActionResult> OnPostChart(int supid)
        {
            try
            {
                DataSet ds = new DataSet();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    ds=await connFunc.ExecuteDataSet("[YYY_sp_Account_LaboDetail_Chart]",CommandType.StoredProcedure
                        ,"@SupID",SqlDbType.Int,Convert.ToInt32(supid)
                        );

                }
                if(ds!=null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
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
        public async Task<IActionResult> OnPostDataInvoice(int supid,int currentID,int beginID,string dateFrom,string dateTo,string search)
        {
            try
            {
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    dt=await connFunc.ExecuteDataTable("[YYY_sp_Account_LaboDetail]",CommandType.StoredProcedure
                        ,"@SupID",SqlDbType.Int,Convert.ToInt32(supid)
                        ,"@CurrentID",SqlDbType.Int,Convert.ToInt32(currentID)
                        ,"@beginID",SqlDbType.Int,Convert.ToInt32(beginID)
                        ,"@datefrom",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@dateto",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        ,"@search",SqlDbType.NVarChar,Comon.Comon.ConvertToUnsign((search!=null) ? search  :"").ToLower()
                        );

                }
                if(dt!=null)
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
        public async Task<IActionResult> OnPostDataForm(int supid,int beginID,string dateFrom,string dateTo,string search)
        {
            try
            {
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    dt=await connFunc.ExecuteDataTable("[YYY_sp_Account_LaboForm]",CommandType.StoredProcedure
                        ,"@SupID",SqlDbType.Int,Convert.ToInt32(supid)
                        ,"@beginID",SqlDbType.Int,Convert.ToInt32(beginID)
                        ,"@datefrom",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@dateto",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        ,"@search",SqlDbType.NVarChar,Comon.Comon.ConvertToUnsign((search!=null) ? search : "").ToLower()
                        );

                }
                if(dt!=null)
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
    }
}
