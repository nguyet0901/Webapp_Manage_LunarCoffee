using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Mail
{
    public class MailDetailModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _CustomerCardID { get; set; }
        public string _AppID { get; set; }
        public string _PaymentID { get; set; }
        public string _Link { get; set; }
        public string _Type { get; set; }
        public void OnGet()
        {
            var cust = Request.Query["CustomerID"];
            var custcard = Request.Query["CustomerCardID"];
            var appid = Request.Query["AppID"];
            var payid = Request.Query["PaymentID"];
            var link = Request.Query["Link"];
            var type = Request.Query["Type"];
            _CustomerID = cust.ToString() != null ? cust.ToString() : "0";
            _CustomerCardID = custcard.ToString() != null ? custcard.ToString() : "0";
            _AppID = appid.ToString() != null ? appid.ToString() : "0";
            _PaymentID = payid.ToString() != null ? payid.ToString() : "0";
            _Link = link.ToString() != null ? link.ToString() : "0";
            _Type = type.ToString() != null ? type.ToString() : "";
        }

        public async Task<IActionResult> OnPostInitialize(string CustomerID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Mail_LoadOption", CommandType.StoredProcedure);
                    dt.TableName = "Option";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Mail_CustomerInfo", CommandType.StoredProcedure
                        , "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID));
                    dt.TableName = "Customer";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Mail_Branch_LoadCombo", CommandType.StoredProcedure
                        , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                        , "@currentBranch", SqlDbType.Int, user.sys_branchID
                        , "@areaBranch", SqlDbType.NVarChar, user.sys_AreaBranch);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostLoadData(string CustomerID, string CustomerCardID, string AppID, string PaymentID, string BranchID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Mail_LoadContentSend", CommandType.StoredProcedure
                        , "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                        , "@CustomerCardID", SqlDbType.Int, Convert.ToInt32(CustomerCardID)
                        , "@AppID", SqlDbType.Int, Convert.ToInt32(AppID)
                        , "@PaymentID", SqlDbType.Int, Convert.ToInt32(PaymentID)
                        , "@BranchID", SqlDbType.Int, Convert.ToInt32(BranchID));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostExecuteData(string data)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                MailDetail mailDetail = JsonConvert.DeserializeObject<MailDetail>(data);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Email_Insert", CommandType.StoredProcedure
                        , "@CustomerID", SqlDbType.Int, mailDetail.CustomerID
                        , "@BranchID", SqlDbType.Int, mailDetail.BranchID
                        , "@Subject", SqlDbType.NVarChar, mailDetail.Subject
                        , "@EmailFrom", SqlDbType.NVarChar, mailDetail.EmailFrom
                        , "@EmailTo", SqlDbType.NVarChar, mailDetail.EmailTo
                        , "@EmailCC", SqlDbType.NVarChar, mailDetail.EmailCC
                        , "@EmailBCC", SqlDbType.NVarChar, mailDetail.EmailBCC
                        , "@Type", SqlDbType.Int, mailDetail.Type
                        , "@CreatedBy", SqlDbType.Int,user.sys_userid);
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

    public class MailDetail
    {
        public int CustomerID { get; set; }
        public int BranchID { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string EmailBCC { get; set; }
        public string Subject { get; set; }
        public int Type { get; set; }
    }
}
