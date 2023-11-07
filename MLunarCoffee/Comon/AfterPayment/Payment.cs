using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SourceMain.Models;

namespace SourceMain.Comon.AfterPayment
{
    public static class Payment
    {
        public async static Task<SMSTemplateResult> SMS(string CustomerID ,string amount ,string paymentcode ,string servicename ,string paymentdate ,string appdate ,string userid ,string branchid
            ,Dictionary<string ,string> mainCust_SMS ,Dictionary<string ,string> mainCust_Key)
        {
            try
            {
                SMSTemplateResult dataResult = new SMSTemplateResult()
                {
                    Result = "0" ,
                    Content = "" ,
                    Phone = "" ,
                };
                await Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("YYY_sp_v2_Customer_LoadPaymentInfo" ,CommandType.StoredProcedure
                           ,"@CurrentID" ,SqlDbType.Int ,CustomerID
                           ,"@DentistCosmetic" ,SqlDbType.Int ,Convert.ToInt32(Global.sys_DentistCosmetic));
                        if (dt != null && dt.Rows.Count != 0)
                        {
                            var dr = dt.Rows[0];
                            decimal PRICE_DISCOUNTED = Convert.ToDecimal(dr["PRICE_DISCOUNTED"]);
                            decimal PAID = Convert.ToDecimal(dr["PAID"]);
                            decimal PRICE_TREAT = Convert.ToDecimal(dr["PRICE_TREAT"]);
                            string LEFTTREAT = (PRICE_TREAT - PAID > 0 ? PRICE_TREAT - PAID : 0).ToString("#,##0.00");
                            string LEFTTAB = (PRICE_DISCOUNTED - PAID).ToString("#,##0.00");
                            mainCust_Key["LeftTreat"] = LEFTTREAT;
                            mainCust_Key["LeftTab"] = LEFTTAB;
                            mainCust_Key["Amount"] = amount; // SET Amount Payment
                            mainCust_Key["PaymentCode"] = paymentcode != null ? paymentcode : ""; // SET Code Payment
                            mainCust_Key["ServiceName"] = servicename != null ? servicename : "";
                            mainCust_Key["PaymentDate"] = paymentdate != null ? paymentdate : "";
                            mainCust_Key["AppNextDate"] = appdate != null ? (appdate != "" ? appdate : "None") : "None";

                            string contenttemplate = mainCust_SMS["ContentSMSPayment"];
                            string dataSend = Comon.GetRepTemp(mainCust_Key ,contenttemplate);
                            string branchname = mainCust_SMS["SMSBranchName"];
                            if (dataSend != "")
                            {
                                dataResult.Content = dataSend;
                                dataResult.Phone = dr["PHONE"].ToString();
                                if (mainCust_SMS["AllowSendPay"] == "1" && dataResult.Phone != "" && dataResult.Phone.Length != 0)
                                {
                                    dataResult.Result = await SourceMain.Comon.SMS.SMSAction.SMS_Pending(CustomerID ,userid ,dataResult.Phone ,amount ,dataSend ,branchid);
                                }
                            }
                        }
                    }

                });
                return dataResult;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async static Task<SMSTemplateResult> ChangeCard(string CustomerID ,string amount ,string cardid ,string userid ,string branchid
           ,Dictionary<string ,string> datasms ,Dictionary<string ,string> keymess)
        {
            try
            {
                string result = "0";
                SMSTemplateResult dataResult = new SMSTemplateResult()
                {
                    Result = "0" ,
                    Content = "" ,
                    Phone = "" ,
                };
                await Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("YYY_sp_v2_Customer_LoadCardInfo" ,CommandType.StoredProcedure
                           ,"@CustomerID" ,SqlDbType.Int ,CustomerID
                           ,"@CardID" ,SqlDbType.Int ,cardid);
                        if (dt != null && dt.Rows.Count != 0)
                        {
                            var dr = dt.Rows[0];
                            string CardCode = dr["CardCode"].ToString();
                            decimal CardLeft = Convert.ToDecimal(dr["CardLeft"].ToString());
                            keymess["CardCode"] = CardCode;
                            keymess["CardLeft"] = CardLeft.ToString("#,##0");
                            keymess["Amount"] = amount;
                            string contenttemplate = datasms["ContentSMSPayment"];
                            string dataSend = Comon.GetRepTemp(keymess ,contenttemplate);
                            string branchname = datasms["SMSBranchName"];
                            if (dataSend != "")
                            {
                                dataResult.Content = dataSend;
                                dataResult.Phone = dr["PHONE"].ToString();
                                if (datasms["AllowSendPay"] == "1" && dataResult.Phone != "" && dataResult.Phone.Length != 0)
                                {
                                    dataResult.Result = await SourceMain.Comon.SMS.SMSAction.SMS_Pending(CustomerID ,userid ,dataResult.Phone ,amount ,dataSend ,branchid);
                                }
                            }
                        }
                    }

                });
                return dataResult;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
