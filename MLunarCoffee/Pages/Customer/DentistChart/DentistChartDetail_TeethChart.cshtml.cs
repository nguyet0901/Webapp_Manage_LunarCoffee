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
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Customer.DentistChart
{

    public class DentistChartDetail_TeethChartModel : PageModel {
        public string _imageFeature_Disease { get; set; }
        public void OnGet ( ) {
            _imageFeature_Disease = Comon.Global.sys_HTTPImageRoot + "Sys_Disease_Feature/";
        }

         public async Task<IActionResult> OnPostLoad_Layout_Teeth ( ) {
            DataSet ds = new DataSet ();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                ds = await confunc.ExecuteDataSet ("[MLU_sp_Dentist_Chart_LoadTeeth]", CommandType.StoredProcedure
                     , "@Type", SqlDbType.Int, 1);
            }
            if (ds != null) {
                return Content (Comon.DataJson.Dataset(ds));
            }
            else {
                return Content ("");
            }
        }
         public async Task<IActionResult> OnPostLoad_Chart_Teeth_Detail ( string currentid ) {
            DataTable dt = new DataTable ();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                dt =await  confunc.ExecuteDataTable ("[MLU_sp_Customer_Chart_Teeth_LoadDetail]", CommandType.StoredProcedure
                    , "@currentid", SqlDbType.Int, Convert.ToInt32 (currentid));
            }
            if (dt != null) {
                return Content (Comon.DataJson.Datatable(dt));
            }
            else {
                return Content ("");
            }
        }
    }
}
