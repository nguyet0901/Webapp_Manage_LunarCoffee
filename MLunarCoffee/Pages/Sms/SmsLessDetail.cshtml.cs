using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Sms
{
    public class SmsLessDetailModel : PageModel
    {

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoadInitiallize()
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
                        dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                        dt.TableName = "cbbBranch";
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





        //public async Task<IActionResult> OnPostExcuteData(string content, string data, string typeCare)
        //{
        //    try
        //    {
        //        DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);

        //        DataTable dtMain_sms = new DataTable(); // data insert table sms
        //        dtMain_sms.Columns.Add("Index", typeof(Int32));
        //        dtMain_sms.Columns.Add("Customer_ID", typeof(Int32));
        //        dtMain_sms.Columns.Add("TicketID", typeof(Int32));
        //        dtMain_sms.Columns.Add("Phone", typeof(String));
        //        dtMain_sms.Columns.Add("AppID", typeof(String));
        //        dtMain_sms.Columns.Add("Type", typeof(Int32));
        //        dtMain_sms.Columns.Add("MessageContent", typeof(String));


        //        if (typeCare == "1")
        //        {
        //            for (int i = 0; i < DataMain.Rows.Count; i++)
        //            {
        //                DataRow dr = dtMain_sms.NewRow();
        //                String contentmess = DectectContentSMS(DataMain, content, i);
        //                String phone = DataMain.Rows[i]["Phone"].ToString();
        //                dr["Index"] = i + 1;
        //                dr["Customer_ID"] = Convert.ToInt32(DataMain.Rows[i]["CustID"]);
        //                dr["TicketID"] = Convert.ToInt32(DataMain.Rows[i]["TicketID"]);
        //                dr["Phone"] = phone;
        //                dr["AppID"] = Convert.ToInt32(DataMain.Rows[i]["AppID"]);
        //                dr["Type"] = Convert.ToInt32(typeCare);
        //                dr["MessageContent"] = contentmess;

        //                dtMain_sms.Rows.Add(dr);
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < DataMain.Rows.Count; i++)
        //            {
        //                DataRow dr = dtMain_sms.NewRow();
        //                String contentmess = DectectContentSMS(DataMain, content, i);
        //                String phone = DataMain.Rows[i]["Phone"].ToString();
        //                dr["Index"] = i + 1;
        //                dr["Customer_ID"] = Convert.ToInt32(DataMain.Rows[i]["CustID"]);
        //                dr["TicketID"] = 0;
        //                dr["Phone"] = phone;
        //                dr["AppID"] = 0;
        //                dr["Type"] = Convert.ToInt32(typeCare);
        //                dr["MessageContent"] = contentmess;
        //                dtMain_sms.Rows.Add(dr);
        //            }
        //        }


        //        var user = Session.GetUserSession(HttpContext);

        //        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //        {
        //            await connFunc.ExecuteDataTable("MLU_sp_CustomerCare_SMS_Insert", CommandType.StoredProcedure,
        //                "@Created_By", SqlDbType.Int, user.sys_userid,
        //                "@Branch", SqlDbType.Int, user.sys_branchID,
        //                "@table_data", SqlDbType.Structured, (dtMain_sms.Rows.Count > 0) ? dtMain_sms : null
        //            );
        //        }

        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("-1");
        //    }
        //}

        //private static string DectectContentSMS(DataTable dt, string content, int i)
        //{
        //    try
        //    {
        //        content = content.Replace("#CustCode#", dt.Rows[i]["CustCode"].ToString());
        //        content = content.Replace("#CustName#", dt.Rows[i]["CustName"].ToString());
        //        content = content.Replace("#AppDay#", dt.Rows[i]["AppDay"].ToString());
        //        content = content.Replace("#AppHour#", dt.Rows[i]["AppHour"].ToString());
        //        content = content.Replace("#BranchAddress#", dt.Rows[i]["BranchAddress"].ToString());
        //        content = content.Replace("#CompanyName#", dt.Rows[i]["CompanyName"].ToString());
        //        content = content.Replace("#BranchHotline#", dt.Rows[i]["BranchHotline"].ToString());
        //        return content;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }

        //}


        //public IActionResult OnPostSendData(string content, string phone)
        //{
        //    try
        //    {
        //        return Content("0"); // Content(Comon.Comon.SendSMS(content, phone).ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("-1");
        //    }
        //}
    }
}
