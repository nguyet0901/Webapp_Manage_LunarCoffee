using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MLunarCoffee.Pages.AppCustomer.Noti
{
    public class NotiListModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {

            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
        }


        public async Task<IActionResult> OnPostLoadNoti(string dateFrom ,string dateTo ,int Limit ,int Sender ,string Indivi ,string BeginID ,int CurrentID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_AppCustomer_NotiLoad]" ,CommandType.StoredProcedure ,
                  "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                   "@dateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                   ,"@Limit" ,SqlDbType.Int ,Limit
                   ,"@IsIndi" ,SqlDbType.Int ,Indivi
                   ,"@sender" ,SqlDbType.Int ,Sender
                   ,"@BeginID" ,SqlDbType.BigInt ,Convert.ToInt64(BeginID)
                   ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt64(CurrentID)
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
    }
}
