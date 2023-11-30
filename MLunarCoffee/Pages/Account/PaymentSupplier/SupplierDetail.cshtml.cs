using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Account.PaymentSupplier
{
    public class SupplierDetailModel : PageModel
    {
        public string supplierID { get; set; }
        public void OnGet()
        {
            string _supp = Request.Query["SuppID"];
            supplierID = !string.IsNullOrEmpty(_supp) ? _supp : "0";
        }

        public async Task<IActionResult> OnPostInit(string suppid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_Supplier_LoadInfo", CommandType.StoredProcedure
                        , "@SuppID", SqlDbType.Int, Convert.ToInt32(suppid));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostLoadInvoice(int suppid, int currentID, string data)
        {
            try
            {
                DataTable dt = new DataTable();
                SupplierInvoice dataMain = JsonConvert.DeserializeObject<SupplierInvoice>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Account_SupplierDetail]", CommandType.StoredProcedure
                        , "@SuppID", SqlDbType.Int, Convert.ToInt32(suppid)
                        , "@CurrentID", SqlDbType.Int, Convert.ToInt32(currentID)
                        , "@BeginPayID", SqlDbType.Int, Convert.ToInt32(dataMain.beginPayID)
                        , "@BeginDesID", SqlDbType.Int, Convert.ToInt32(dataMain.beginDesID)
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dataMain.dateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dataMain.dateTo)
                        , "@Type", SqlDbType.NVarChar, !String.IsNullOrEmpty(dataMain.type) ? dataMain.type.ToString() : ""
                        , "@Search", SqlDbType.NVarChar, Comon.Comon.ConvertToUnsign((dataMain.search != null) ? dataMain.search : "").ToLower());
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
            catch
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadForm(int supid, int beginID, string data)
        {
            try
            {
                DataTable dt = new DataTable();
                SupplierForm dataMain = JsonConvert.DeserializeObject<SupplierForm>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Account_SupplierForm]", CommandType.StoredProcedure
                        , "@SupID", SqlDbType.Int, Convert.ToInt32(supid)
                        , "@BeginID", SqlDbType.Int, Convert.ToInt32(beginID)
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dataMain.dateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dataMain.dateTo)
                        , "@Search", SqlDbType.NVarChar, Comon.Comon.ConvertToUnsign((dataMain.search != null) ? dataMain.search : "").ToLower());
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
            catch
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostDeleteDeposit(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Supplier_Deposit_Delete]", CommandType.StoredProcedure,
                          "@DepositID", SqlDbType.Int, id,
                          "@UserID", SqlDbType.Int, user.sys_userid);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

    }

    public class SupplierInvoice
    {
        public int beginPayID { get; set; }
        public int beginDesID { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string type { get; set; }
        public string search { get; set; }
    }

    public class SupplierForm
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string search { get; set; }
    }
}
