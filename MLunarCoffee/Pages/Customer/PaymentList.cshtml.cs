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


namespace MLunarCoffee.Pages.Customer
{

    public class PaymentListModel : PageModel
    {
        public string _PaymentCustomerID { get; set; }

        public int _PaymentType { get; set; }
        public int _PaymentIsLock { get; set; }

        public string _dtPermisstionControlMustHide { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public string _dtPermissionControl { get; set; }

        public async Task OnGet()
        {
            var v = Request.Query["CustomerID"];
            if (v.ToString() != String.Empty)
            {
                var user = Session.GetUserSession(HttpContext);
                _PaymentCustomerID = v.ToString();
                if (!await Comon.Option_Extension.Extension.CheckCashierTime(Convert.ToInt32(_PaymentCustomerID)
                    , Convert.ToInt32(user.sys_branchID),user.sys_Rule_OptionExtension.TimeInvoice.Value.ToString()))
                    _PaymentIsLock = 1;
                else _PaymentIsLock = 0;
            }
            _PaymentType = Comon.Global.sys_PrintPaymentType;
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }


        public async Task<IActionResult> OnPostCheckValidPayment(string CustomerID, string Type, string IsRefund, string data)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                DataTable dtData = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("ID", typeof(int));
                dtMain.Columns.Add("Amount", typeof(decimal));
                dtMain.Columns.Add("UsedDeposit", typeof(decimal));
                for (int i = 0; i < dtData.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["ID"] = Convert.ToInt32(dtData.Rows[i]["ID"]);
                    dr["Amount"] = Convert.ToDecimal(dtData.Rows[i]["Amount"]);
                    dr["UsedDeposit"] = Convert.ToDecimal(dtData.Rows[i]["UsedDeposit"]);
                    dtMain.Rows.Add(dr);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Payment_ValidTab]", CommandType.StoredProcedure,
                        "@CustomerID", SqlDbType.Int, CustomerID
                      , "@Type", SqlDbType.NVarChar, Type != null ? Type : "payment"
                      , "@IsRefund", SqlDbType.Int, IsRefund != null ? Convert.ToInt32(IsRefund) : 0
                      , "@TableMain", SqlDbType.Structured, dtMain.Rows.Count != 0 ? dtMain : null);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
