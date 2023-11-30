using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Sms
{
    public class SmsDetailModel : PageModel
    {
        public int SMSDetailCustomerID { get; set; }
        public int SMSDetailTicketID { get; set; }
        public int SMSDetailAppID { get; set; }
        public string SMSDetailType { get; set; }
        public int SMSDetailTypeCare { get; set; }
        public int SMSDetailMasterID { get; set; }
        public string SMSDetailNameTemp { get; set; }
        public string SMSDetailPhone { get; set; }
        public int SMSDetailCustCardID { get; set; }
        public string _ContentDemo { get; set; }
        public int SMSDetailPaymentStudentID { get; set; }
        public int SMSDetailTreatID { get; set; }
        public int SMSDetailPaymentID { get; set; }
        public string SMSDetailDate { get; set; } //yyyy-mm-dd
        public void OnGet()
        {
            string Curr = Request.Query["CustomerID"];
            string PayStudent = Request.Query["PaymentStudentID"];
            if (Curr != null || (Curr == null && PayStudent != null))
            {
                string cusID = Request.Query["CustomerID"];
                string typeid = Request.Query["Type"];
                string appid = Request.Query["AppID"];
                string ticketID = Request.Query["TicketID"];
                string masterid = Request.Query["MasterID"];
                string typecare = Request.Query["TypeCare"];
                string nametemp = Request.Query["NameTemp"];
                string phone = Request.Query["Phone"];
                string custCardID = Request.Query["CustCardID"];
                string date = Request.Query["Date"];
                string TreatID = Request.Query["TreatID"];
                string PaymentID = Request.Query["PaymentID"];

                SMSDetailCustomerID = Convert.ToInt32(cusID == null ? "0" : cusID.ToString());
                SMSDetailType = (typeid == null) ? "" : typeid.ToString();
                SMSDetailTypeCare = (typecare == null) ? 0 : Convert.ToInt32(typecare.ToString());
                SMSDetailTicketID = Convert.ToInt32(ticketID == null ? "0" : ticketID.ToString());
                SMSDetailAppID = Convert.ToInt32(appid == null ? "0" : appid.ToString());
                SMSDetailMasterID = Convert.ToInt32(masterid == null ? "0" : masterid.ToString());
                SMSDetailNameTemp = (nametemp == null) ? "" : Uri.EscapeUriString(nametemp.ToString());
                SMSDetailPhone = (phone == null) ? "" : phone.ToString();
                SMSDetailPaymentStudentID = Convert.ToInt32(PayStudent == null ? "0" : PayStudent.ToString());
                SMSDetailCustCardID = Convert.ToInt32(custCardID == null ? "0" : custCardID.ToString());
                SMSDetailTreatID = Convert.ToInt32(TreatID == null ? "0" : TreatID.ToString());
                SMSDetailPaymentID = Convert.ToInt32(PaymentID == null ? "0" : PaymentID.ToString());
                SMSDetailDate = Convert.ToString(date);

                // Kiểm tra nếu trong màn hình chăm sóc 
                if (PayStudent == null && custCardID == null)
                {
                    SMSDetailType = DectectFromCustomerCare(SMSDetailTypeCare);
                }
            }
        }
        private string DectectFromCustomerCare(int smsDetailTypeCare)
        {
            string resulf = "";
            switch (smsDetailTypeCare)
            {
                case 0:
                    resulf = "SMSContentGeneral";
                    break;
                case 1:
                    resulf = "SMSContentNLich";
                    break;
                case 2:
                    resulf = "SMSContentDKLDV";
                    break;
                case 3:
                    resulf = "SMSContentNLe";
                    break;
                case 4:
                    resulf = "SMSContentDLKD";
                    break;
                case 5:
                    resulf = "SMSContentCSSDT";
                    break;
                case 6:
                    resulf = "SMSContentGeneral";
                    break;
                case 7:
                    resulf = "SMSContentAfterPayment";
                    break;
                default:
                    resulf = "";
                    break;
            }
            return resulf;
        }


        public async Task<IActionResult> OnPostLoadInitiallize(int CustomerID ,int TicketID ,int PaymentStudentID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dt.TableName = "cbbBranch";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Customer_SMS_LoadByCustomerOrTicket" ,CommandType.StoredProcedure
                        ,"@CustomerID" ,SqlDbType.Int ,CustomerID
                        ,"@TicketID" ,SqlDbType.Int ,TicketID);
                        dt.TableName = "dataInfo";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_SMS_Detail_Option_LoadCombo" ,CommandType.StoredProcedure);
                        dt.TableName = "dataSmsType";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_ST_SMS_LoadByStudent" ,CommandType.StoredProcedure
                            ,"@PaymentStudentID" ,SqlDbType.Int ,PaymentStudentID
                            );
                        dt.TableName = "InfoStudent";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    DataTable dt = new DataTable();
                    dt = Comon.Global.sys_SMS_NotInBrandName.Copy();
                    dt.TableName = "dataSmsRules";
                    return dt;
                }));
                tasks.Add(Task.Run(async () =>
                {

                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("MLU_sp_v2_Customer_LoadPaymentInfo" ,CommandType.StoredProcedure
                            ,"@CurrentID" ,SqlDbType.Int ,CustomerID
                            ,"@DentistCosmetic" ,SqlDbType.Int ,Convert.ToInt32(Comon.Global.sys_DentistCosmetic));
                        dt.TableName = "PaymentInfo";
                        return dt;
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

                return Content("");
            }


        }


        public async Task<IActionResult> OnPostLoadData(int branchID ,string CustomerID ,string CustCardID ,string AppID ,string PayStudentID ,string type
            ,string contentid ,string treatid ,string paymentid ,string lefttreat ,string lefttab ,string date)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_SMS_LoadContentToSend" ,CommandType.StoredProcedure
                        ,"@CustID" ,SqlDbType.Int ,CustomerID
                        ,"@CustCardID" ,SqlDbType.Int ,CustCardID
                        ,"@Type" ,SqlDbType.NVarChar ,type != null ? type : ""
                        ,"@AppID" ,SqlDbType.Int ,AppID
                        ,"@PayStudentID" ,SqlDbType.Int ,Convert.ToInt32(PayStudentID)
                        ,"@contentid" ,SqlDbType.Int ,contentid != null ? contentid : "0"
                        ,"@treatid" ,SqlDbType.Int ,treatid != null ? treatid : "0"
                        ,"@PaymentID" ,SqlDbType.Int ,paymentid != null ? paymentid : "0"
                        ,"@BranchID" ,SqlDbType.Int ,branchID
                        ,"@LeftTreat" ,SqlDbType.NVarChar ,lefttreat
                        ,"@LeftTab" ,SqlDbType.NVarChar ,lefttab
                        ,"@Date" ,SqlDbType.DateTime ,date //yyyy-mm-dd
                        ,"@DentistCosmetic" ,SqlDbType.Int ,Convert.ToInt32(Global.sys_DentistCosmetic)
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        //public async Task<IActionResult> OnPostExcuteData(string content, string phone, int typeCare, string CustomerID
        //    , string TicketID, string appID, string masterID, string StudentID)
        //{
        //    try
        //    {
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //        {
        //            await connFunc.ExecuteDataTable("[MLU_sp_Customer_SMS_Insert]", CommandType.StoredProcedure,
        //                "@Customer_ID", SqlDbType.Int, CustomerID,
        //                "@Content", SqlDbType.NVarChar, content.Replace("'", "").Trim(),
        //                "@Created_By", SqlDbType.Int, user.sys_userid,
        //                "@Type", SqlDbType.NVarChar, typeCare.ToString(),
        //                "@TicketID", SqlDbType.Int, TicketID,
        //                "@Phone", SqlDbType.NVarChar, phone,
        //                "@AppID", SqlDbType.Int, appID,
        //                 "@Value", SqlDbType.NVarChar, "",
        //                "@Branch", SqlDbType.NVarChar, user.sys_branchID,
        //                "@StudentID", SqlDbType.Int, Convert.ToInt32(StudentID)
        //            );
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        public async Task<IActionResult> OnPostCheckContent(string content)
        {
            try
            {
                if (content == "" || content.Length < 5) return Content("-2");
                return Content(Comon.Comon.CheckContentSMS(content));
            }
            catch (Exception ex)
            {
                return Content("-1");
            }
        }
    }
}
