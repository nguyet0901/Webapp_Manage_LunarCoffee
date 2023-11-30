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

namespace MLunarCoffee.Pages.Labo.Status
{
    public class LaboDiscountModel : PageModel
    {
        public string _LaboID { get; set; }
        public void OnGet()
        {
            string laboID = Request.Query["LaboID"];
            if (laboID != null)
            {
                _LaboID = laboID.ToString();
            }
            else
            {
                _LaboID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadInit(string LaboID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Quick_Discount_Labo_Load]" ,CommandType.StoredProcedure
                        ,"@LaboID" ,SqlDbType.Int ,Convert.ToInt32(LaboID)
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExecuted(string LaboID, string DiscountAmount, string DiscountPercent, string Price)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Quick_Discount_Labo_Update]" ,CommandType.StoredProcedure
                        , "@LaboID", SqlDbType.Int, Convert.ToInt32(LaboID)
                        , "@DiscountAmount", SqlDbType.Decimal, Convert.ToDecimal(DiscountAmount)
                        , "@DiscountPercent", SqlDbType.Decimal, Convert.ToDecimal(DiscountPercent)
                        , "@Price", SqlDbType.Decimal, Convert.ToDecimal(Price)
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
