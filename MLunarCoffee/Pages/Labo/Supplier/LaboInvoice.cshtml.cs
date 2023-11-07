using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.Labo.Supplier
{
    public class LaboInvoiceModel : PageModel
    {
        public int _BranchID { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _BranchID = user.sys_branchID;
        }

         public async Task<IActionResult> OnPostLoadata_Invoice(string SupplierID, int limit, string idbegin, string textsearch)
        {
            try
            {
                textsearch = (textsearch != null ? textsearch :"");
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[ZZZ_sp_Labo_Supplier_Payment_LoadList_v2]", CommandType.StoredProcedure,
                        "@SupplierID", SqlDbType.Int, SupplierID
                        , "@limit", SqlDbType.Int, limit
                        , "@idbegin", SqlDbType.Int, idbegin
                        , "@UserId", SqlDbType.Int, user.sys_userid
                        , "@textsearch", SqlDbType.NVarChar, textsearch
                        , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                    );
                }
                if (ds != null)
                {
                    return Content(JsonConvert.SerializeObject(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

        
         public async Task<IActionResult> OnPostDeletePayment(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Labo_Supplier_Payment_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(JsonConvert.SerializeObject(dt));
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
         
         public async Task<IActionResult> OnPostLoadata_Form(string SupplierID, int limit, string idbegin, string textsearch)
        { 
            try
            {
                textsearch = (textsearch != null ? textsearch : "");
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Labo_Supplier_CustomerLabo_LoadList]", CommandType.StoredProcedure,
                        "@SupplierID", SqlDbType.Int, SupplierID
                        , "@limit", SqlDbType.Int, limit
                        , "@idbegin", SqlDbType.Int, idbegin
                        , "@textsearch", SqlDbType.NVarChar, textsearch
                    );
                }
                if (ds != null)
                {
                    return Content(JsonConvert.SerializeObject(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
