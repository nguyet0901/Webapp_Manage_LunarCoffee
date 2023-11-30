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

namespace MLunarCoffee.Pages.Discount
{
    public class DiscountVoucherDetailModel : PageModel
    {
        public string _DataDetailID { get; set; }
        public string _DataBranch { get; set; }
        public string _DataAllService { get; set; }
        public string _DataAllServiceType { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _DataDetailID = !String.IsNullOrEmpty(curr) ? curr.ToString() : "0";
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }

         public async Task<IActionResult> OnPostLoadInitialize(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                //load combo
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                      , "@UserID", SqlDbType.Int, user.sys_userid
                      , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "cbbBranch";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("MLU_sp_Discount_Service_Type_LoadList", CommandType.StoredProcedure);
                    dt.TableName = "ServiceType";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("MLU_sp_Discount_Service_LoadList", CommandType.StoredProcedure);
                    dt.TableName = "Service";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Discount_Voucher_LoadToDetail]", CommandType.StoredProcedure,
                     "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                    dt.TableName = "dataDetail";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

         public async Task<IActionResult> OnPostExcuteDataVoucher(string data, string CurrentID)
        {
            try
            {
                VCDiscount DataMain = JsonConvert.DeserializeObject<VCDiscount>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("MLU_sp_Discount_Voucher_Insert", CommandType.StoredProcedure,
                         "@TimesUsed", SqlDbType.Int, Convert.ToInt32(DataMain.TimesUsed) ,
                         "@IssueQty", SqlDbType.Int, DataMain.IssueQty,
                         "@Name", SqlDbType.NVarChar, DataMain.Name,
                         "@Code", SqlDbType.NVarChar, DataMain.Code,
                         "@Amount" , SqlDbType.Decimal, DataMain.Amount,
                         "@Percent", SqlDbType.Int, DataMain.Percent,
                         "@Rule", SqlDbType.NVarChar, DataMain.Rule,
                         "@IsAllBranch", SqlDbType.Int, DataMain.IsAllBranch,
                         "@Date_from", SqlDbType.DateTime, Comon.Comon.StringDMY_To_DateTime(DataMain.DateFrom),
                         "@Date_to", SqlDbType.DateTime, Comon.Comon.StringDMY_To_DateTime(DataMain.DateTo),
                         "@TokenBranch", SqlDbType.DateTime, DataMain.TokenBranch,
                         "@Created_By", SqlDbType.Int, user.sys_userid
                         );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("MLU_sp_Discount_Voucher_Update", CommandType.StoredProcedure,
                         "@TimesUsed", SqlDbType.Int, Convert.ToInt32(DataMain.TimesUsed),
                         "@IssueQty", SqlDbType.Int, DataMain.IssueQty,
                         "@Amount", SqlDbType.Decimal, DataMain.Amount,
                         "@Percent", SqlDbType.Int, DataMain.Percent,
                         "@Name", SqlDbType.NVarChar, DataMain.Name,
                         "@Code" ,SqlDbType.NVarChar ,DataMain.Code ,
                         "@Rule" , SqlDbType.NVarChar, DataMain.Rule,
                         "@IsAllBranch", SqlDbType.Int, DataMain.IsAllBranch,
                         "@Date_from", SqlDbType.DateTime, Comon.Comon.StringDMY_To_DateTime(DataMain.DateFrom),
                         "@Date_to", SqlDbType.DateTime, Comon.Comon.StringDMY_To_DateTime(DataMain.DateTo),
                         "@TokenBranch", SqlDbType.DateTime, DataMain.TokenBranch,
                         "@currentID", SqlDbType.Int, CurrentID,
                         "@Modified_By", SqlDbType.Int, user.sys_userid
                         );
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("Error");
            }


        }
    }
    public class VCDiscount
    {
        public string TimesUsed { get; set; }
        public string IssueQty { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public string Percent { get; set; }
        public string Rule { get; set; }
        public int IsAllBranch { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string TokenBranch { get; set; }
    }
}
