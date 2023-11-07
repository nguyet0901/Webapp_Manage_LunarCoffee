using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Account
{
    public class InvoicePrintModel : PageModel
    {  
        public string _Invoice_Type { get; set; }
        public int _currentID { get; set; }
        public void OnGet()
        {
            string v = Request.Query["id"];
            string type = Request.Query["Type"];
            if (type != null && type == "1")
            {
                _Invoice_Type = "1";
            }
            else if (type != null && type == "0")
            {
                _Invoice_Type = "0";
            }
            _currentID = Convert.ToInt32(v == null ? "0" : v.ToString());
           
        }
         public async Task<IActionResult> OnPostLoadData(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await  confunc.ExecuteDataTable("[YYY_sp_InVoice_LoadDataToPrint]", CommandType.StoredProcedure,
                  "@currentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));

            }
            if (dt != null)
            {
                dt.Rows[0]["Word"] = Comon.Comon.ConvertNumToString(Convert.ToDouble(dt.Rows[0]["Word"]));
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("[]");
            }

        }
    }
}
