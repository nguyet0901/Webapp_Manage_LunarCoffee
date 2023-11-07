using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace SourceMain.Pages.Customer.Treatment
{
    public class NK_ProfileTreatmentModel : PageModel {
        public string _CustomerID { get; set; }
        public string PermissionTable_TabControl { get; set; }
        public void OnGet ( ) {
            var v = Request.Query["id"];
            _CustomerID = (v.ToString () == String.Empty ? "0" : v.ToString ());
            var user = Session.GetUserSession (HttpContext);
            PermissionTable_TabControl = JsonConvert.SerializeObject (user.sys_PermissionTabControl);
        }
         public async Task<IActionResult> OnPostLoadataProfileTreat ( int CustomerID ) {
            DataSet ds = new DataSet ();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                DataTable dt = new DataTable ();
                dt =await  confunc.ExecuteDataTable ("[YYY_sp_Customer_LoadProfilePrint]", CommandType.StoredProcedure,
                  "@CustomerID", SqlDbType.Int, Convert.ToInt32 (CustomerID == 0 ? 0 : CustomerID));
                dt.TableName = "dtProfile";
                ds.Tables.Add (dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                DataTable dt = new DataTable ();
                dt =await  confunc.ExecuteDataTable ("[YYY_sp_Customer_Treatment_LoadPrint]", CommandType.StoredProcedure,
                      "@Customer_ID", SqlDbType.Int, Convert.ToInt32 (CustomerID == 0 ? 0 : CustomerID),
                      "@PatientRecordID", SqlDbType.Int, 0);
                dt.TableName = "dtTreatment";
                ds.Tables.Add (dt);
            }
            if (ds != null) {
                return Content (JsonConvert.SerializeObject (ds));
            }
            else {
                return Content ("[]");
            }

        }
    }
}
