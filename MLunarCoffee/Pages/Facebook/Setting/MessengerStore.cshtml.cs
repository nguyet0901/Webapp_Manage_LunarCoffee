using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Fanpage.Comon.Session;
using Fanpage.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;
namespace Fanpage.Pages.Facebook.Setting
{
    public class MessengerStoreModel : PageModel
    {
        private readonly IHubContext<NotiHub> hubContext;
        public MessengerStoreModel ( IHubContext<NotiHub> hubContext ) {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
        }
         public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("[YYY_sp_PageFacebookUser_MessengerStore_User]", CommandType.StoredProcedure);
                    DataTable dtEmployee = new DataTable();
                    dtEmployee.Columns.Add("ID", typeof(String));
                    dtEmployee.Columns.Add("Name", typeof(String));
                    dtEmployee.Columns.Add("Pages", typeof(String));
                    dtEmployee.Columns.Add("STT", typeof(String));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dtEmployee.NewRow();
                        dr["ID"] = dt.Rows[i]["ID"].ToString();
                        dr["Name"] = dt.Rows[i]["Name"].ToString();
                        dr["Pages"] = dt.Rows[i]["Pages"].ToString();
                        dr["STT"] = (i + 1).ToString();
                        dtEmployee.Rows.Add(dr);
                    }

                    dtEmployee.TableName = "dtEmployee";
                    ds.Tables.Add(dtEmployee);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("[YYY_sp_PageFacebook_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "Fanpage";
                    ds.Tables.Add(dt);
                }
                return Content(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
         
         public async Task<IActionResult> OnPostLoadInitializeData(string PageID, string Date)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (Date.Contains(" to "))
                {
                    Date = Date.Replace(" to ", "@");
                    dateFrom = Date.Split('@')[0];
                    dateTo = Date.Split('@')[1];
                }
                else
                {
                    dateFrom = Date;
                    dateTo = Date;
                }
                DataTable dt2 = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt2 =await  confunc.ExecuteDataTable("[YYY_sp_Messenger_No_Telesale]", CommandType.StoredProcedure
                         , "@pageID", SqlDbType.Int, Convert.ToInt32(PageID)
                         , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                         , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
                }
                return Content(JsonConvert.SerializeObject(dt2));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
         
        public async Task<IActionResult> OnPostExecute (string data)
        {
            try
            {
                //DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                //var user = Session.GetUserSession(HttpContext);
                //DataTable dt = new DataTable();
                //dt.Columns.Add("from_key", typeof(String));
                //dt.Columns.Add("page_key", typeof(String));
                //dt.Columns.Add("TeleID", typeof(String));
                //for (int i = 0; i < DataMain.Rows.Count; i++)
                //{
                //    DataRow dr = dt.NewRow();
                //    dr["from_key"] = DataMain.Rows[i]["fromid"].ToString();
                //    dr["page_key"] = DataMain.Rows[i]["pageid"].ToString();
                //    dr["TeleID"] = DataMain.Rows[i]["Tele"].ToString();
                //    dt.Rows.Add(dr);

                //}
                //using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //{
                //    DataTable result = new DataTable();
                //    result =await  confunc.ExecuteDataTable("[YYY_sp_Messenger_Store_Insert]", CommandType.StoredProcedure,
                //          "@Created_By", SqlDbType.Int, user.sys_userid,
                //          "@Table", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                //      );
                //    if (result != null && result.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < result.Rows.Count; i++)
                //        {
                //            await NotiLocal.Noti_Parti_Emp(hubContext,Convert.ToInt32(result.Rows[i]["TeleID"]), "Được Phân Từ Page: " + result.Rows[i]["NamePage"], "Số Lượng Inbox Được Chia: " + result.Rows[i]["CountInbox"]);
                //        }
                //    }

                //}
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
