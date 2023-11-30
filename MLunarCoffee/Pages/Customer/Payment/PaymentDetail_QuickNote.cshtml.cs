using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;


namespace MLunarCoffee.Pages.Customer.Payment
{
    public class PaymentDetail_QuickNote : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }
        public string _currentID { get; set; }
        public string _Type { get; set; }
        public string _isRefund { get; set; } 
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string currType = Request.Query["Type"];
            string isRefund = Request.Query["IsRefund"];            
            _isRefund = isRefund != null ? isRefund.ToString() : "";
            if (curr != null)
            {
                _currentID = curr.ToString();
                _Type = currType.ToString();
            }
            else
            {
                _currentID = "0";
                _Type = "0";
            }
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoadInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMethod = new DataTable();
                        dtMethod = await confunc.LoadPara("Method");
                        dtMethod.TableName = "Method";
                        return dtMethod;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMethodType = new DataTable();
                        dtMethodType = await confunc.ExecuteDataTable("[MLU_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                        dtMethodType.TableName = "MethodType";
                        return dtMethodType;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id ,int type)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    var user = Session.GetUserSession(HttpContext);
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Payment_QuickNote_LoadDetail]" ,CommandType.StoredProcedure ,
                      "@ID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id)
                      ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate
                       ,"@Type" ,SqlDbType.Int ,Convert.ToInt32(type == 0 ? 0 : type)
                      );
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
        public async Task<IActionResult> OnPostExcuteData(int id ,string data ,string type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                PaymentDetail DataMain = JsonConvert.DeserializeObject<PaymentDetail>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Payment_QuickNote]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid ,
                        "@Type" ,SqlDbType.Int ,type ,
                        "@Content" ,SqlDbType.NVarChar ,DataMain.Content ,
                        "@MethodID" ,SqlDbType.NVarChar ,DataMain.MethodID ,
                        "@MedthodDetail" ,SqlDbType.NVarChar ,DataMain.MedthodDetail ,
                        "@PosType" ,SqlDbType.NVarChar ,DataMain.PosType ,
                        "@TransferType" ,SqlDbType.NVarChar ,DataMain.TransferType
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

    }
}
