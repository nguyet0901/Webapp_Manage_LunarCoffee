using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Labo.Status
{
    public class LaboChangeSupModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        { 
            var curr = Request.Query["CurrentID"];
            _CurrentID = curr.ToString();
        }
        public async Task<IActionResult> OnPostLaboSup()
        {
            try
            {
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("YYY_sp_LaboSupplier_Load",CommandType.StoredProcedure);
                }
                if(dt!=null)
                    return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");

            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadData(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await  confunc.ExecuteDataTable("[YYY_sp_Labo_LoadDetail_ChangeSub]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, CurrentID
                        );

                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
         
         public async Task<IActionResult> OnPostGetPrice(int CurrentID, int Sub)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await  confunc.ExecuteDataTable("[YYY_sp_Labo_ChangeSub_GetPrice]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, CurrentID
                         , "@SupID", SqlDbType.Int, Sub
                        );

                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
         
         public async Task<IActionResult> OnPostChangePrice(int CurrentID, string unitprice, string price, int sub,string content)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await  confunc.ExecuteDataTable("[YYY_sp_Labo_ChangeSub_ChanePrice]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, CurrentID
                         , "@unitprice", SqlDbType.Decimal, Convert.ToDecimal(unitprice)
                          , "@price", SqlDbType.Decimal, Convert.ToDecimal(price)
                         , "@SupID", SqlDbType.Int, sub
                         , "@Content", SqlDbType.NVarChar, content
                          , "@CreatedBy", SqlDbType.Int, user.sys_userid
                        );

                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
