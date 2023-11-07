using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MLunarCoffee.Comon
{
    public static class SendNoti
    {
        public static void SendNoti_OneToken_Noti_NewAppointment(string platform, string token, string header, string content, string ArticleID, string userid)
        {
            try
            {
                if (Global.sys_isNotiNewAppointment == 1)
                {
                   // Execute_Send_Noti(platform, token, header, content, ArticleID, userid);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static void SendNoti_OneToken_Noti_EditAppointment(string platform, string token, string header, string content, string ArticleID, string userid)
        {
            try
            {
                if (Global.sys_isNotiChangeAppointment == 1)
                {
                    //Execute_Send_Noti(platform, token, header, content, ArticleID, userid);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static void SendNoti_OneToken_Noti_ChangeStatusAppointment(string platform, string token, string header, string content, string ArticleID, string userid)
        {
            try
            {
                if (Global.sys_isNotiChangeStatusAppointment == 1)
                {
                    //Execute_Send_Noti(platform, token, header, content, ArticleID, userid);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static void SendNoti_OneToken_Noti_AfterPayment(string platform, string token, string header, string content, string ArticleID, string userid)
        {
            try
            {
                if (Global.sys_isNotiAfterPayment == 1)
                {
                    //Execute_Send_Noti(platform, token, header, content, ArticleID, userid);
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        //private static async void  Execute_Send_Noti(string platform, string token, string header, string content, string ArticleID, string userid)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await connFunc.ExecuteDataTable("[YYY_sp_AppCustomer_Notification_Insert_Push]", CommandType.StoredProcedure,
        //             "@ArticleID", SqlDbType.NVarChar, ArticleID,
        //             "@Range", SqlDbType.NVarChar, userid,
        //             "@Title", SqlDbType.NVarChar, header,
        //             "@Body", SqlDbType.NVarChar, content
        //       );
        //            if (dt.Rows[0][0].ToString() == "1")
        //            {
        //                if (platform.ToLower() == "ios")
        //                    NotificationApp.NotificationIOS(ArticleID, header, content, "\"" + token + "\"");
        //                else
        //                    NotificationApp.NotificationAND(ArticleID, header, content, "\"" + token + "\"");
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
            
        //}
        public static void SendSMS_OneToken(string CustomerID, string Amount)
        {
            try
            {
                if (Global.sys_isSMSAfterPayment == 1)
                {
                    //GlobalUser user = (GlobalUser)HttpContext.Current.Session["CurrentUserInfo"];
                    //DataTable dt = new DataTable();
                    //using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    //{
                    //    dt =await connFunc.ExecuteDataTable("YYY_sp_SMS_GetPhoneCustomer", CommandType.StoredProcedure,
                    //         "@CustID", SqlDbType.Int, CustomerID);
                    //    if (dt != null && dt.Rows.Count == 1)
                    //    {
                    //        string phone = dt.Rows[0]["Phone"].ToString();
                    //        string name = dt.Rows[0]["Name"].ToString();
                    //        string code = dt.Rows[0]["CustCode"].ToString();
                    //        string brandnameSms = Global.sys_SMSBrandNameShow;
                    //        string hotline = user.sys_hotline;
                    //        string content = Global.sys_SMSAfterPaymentContent;
                    //        string amount = Convert.ToDecimal(Amount).ToString("N0");
                    //        if (content != "")
                    //        {
                    //            content = content.Replace("#CustName#", name);
                    //            content = content.Replace("#CustCode#", code);
                    //            content = content.Replace("#BrandName#", brandnameSms);
                    //            content = content.Replace("#HotLine#", hotline);
                    //            content = content.Replace("#Amount#", amount);
                    //            Comon.SendSMS(content, phone);
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}