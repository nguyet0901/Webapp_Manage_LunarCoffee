using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Account
{
    public class InvoicePaymentShowDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _Type { get; set; }
        public string _Code { get; set; }
        public void OnGet()
        {
            string CurrentID = Request.Query["CurrentID"];
            string Type = Request.Query["Type"];
            string Code = Request.Query["Code"];
            _CurrentID = (CurrentID != null) ? CurrentID : "0";
            _Type = (Type != null) ? Type : "0";
            _Code = (Code != null) ? Code : "";
        }
        public async Task<IActionResult> OnPostLoadata(string TypeID ,string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Accountant_ReceiptShowDetail]" ,CommandType.StoredProcedure
                        ,"@TypeID" ,SqlDbType.Int ,Convert.ToInt32(TypeID)
                        ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
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
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
    }
}
