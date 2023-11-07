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
    public class ImportSupplier : PageModel
    {
        public string _Detail { get; set; }
        public string _DetailOrder { get; set; }
        public void OnGet()
        {
            string Detail = Request.Query["Detail"];
            _Detail=(Detail==null) ? "0" : Detail.ToString();
            string DetailOrder = Request.Query["OrderDetail"];
            _DetailOrder=(DetailOrder==null) ? "0" : DetailOrder.ToString();
        }
        public async Task<IActionResult> OnPostInitialize(int DetailID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds=await confunc.ExecuteDataSet("YYY_sp_WareHouse_ImportSup_Info", CommandType.StoredProcedure
                    ,"@DetailID", SqlDbType.Int,DetailID);
                }
                if (ds!= null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
    
     
        public async Task<IActionResult> OnPostUnImport(int DetailID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("[YYY_sp_WareHouse_Export_UnImportSup]",CommandType.StoredProcedure
                         ,"@MasterID",SqlDbType.Int,DetailID);
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
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
     
      
        public async Task<IActionResult> OnPostUpdateSign(int id,string sign)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_WareHouse_SignImportOrder]",CommandType.StoredProcedure,
                        "@CurrentID",SqlDbType.Int,Convert.ToInt32(id),
                        "@userID",SqlDbType.Int,user.sys_userid,
                        "@Sign",SqlDbType.NVarChar,sign
                    );
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
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
    }
}
