using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Ticket
{
    public class Ticket : PageModel
    {
 
        public void OnGet()
        {
 

        }
 
        public async Task<IActionResult> OnPostLoadTicket(string WareID, string ProductID, string ser_preID, string DateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_Norm_Ticket]", CommandType.StoredProcedure,
                      "@WareID", SqlDbType.Int, WareID,
                      "@ProductID", SqlDbType.Int, ProductID,
                      "@ser_preID",SqlDbType.Int,ser_preID,
                      "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateTo)
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadTicketEdit(string lockID,string ProductID )
        {
            try
            {
                DataSet ds = new DataSet();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds=await connFunc.ExecuteDataSet("[YYY_sp_v2_WareHouse_Norm_TicketEdit]",CommandType.StoredProcedure,
                      "@LockID",SqlDbType.Int,lockID,
                      "@ProductID",SqlDbType.Int,ProductID 
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
         

    }
}
