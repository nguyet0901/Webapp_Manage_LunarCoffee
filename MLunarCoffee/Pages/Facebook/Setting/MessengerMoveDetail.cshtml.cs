using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using MLunarCoffee.Comon.SignalR;
using MLunarCoffee.Comon.Session;

namespace Fanpage.Pages.Facebook.Setting
{
    public class MessengerMoveDetailModel : PageModel
    {
        private readonly IHubContext<NotiHub> hubContext;
        public MessengerMoveDetailModel ( IHubContext<NotiHub> hubContext ) {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
        }
         public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (MLunarCoffee.Models.ExecuteDataBase confunc = new MLunarCoffee.Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_PageFacebookUser_MessengerStore_User]", CommandType.StoredProcedure);
                    dt.TableName = "Invidual";
                    ds.Tables.Add(dt);
                }
                using (MLunarCoffee.Models.ExecuteDataBase confunc = new MLunarCoffee.Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_PageFacebook_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "Fanpage";
                    DataRow dr = dt.NewRow();
                    dr[0] = 0;
                    dr[1] = "Tất Cả";
                    dt.Rows.InsertAt(dr, 0);
                    ds.Tables.Add(dt);
                }
                return Content(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Content("0");
            }
        }

         
        public async Task<IActionResult> OnPostExcuteData (string userTo, string userFrom, string FanPage, string dateFrom, string dateTo)
        {
            try
            {
                //userTo = (userTo != null ? userTo : "");
                //userFrom = (userFrom != null ? userFrom : "");
                //FanPage = (FanPage != null ? FanPage : "");
                //dateFrom = (dateFrom != null ? dateFrom : "");
                //dateTo = (dateTo != null ? dateTo : "");

                //DataTable dt = new DataTable();
                //var user = Session.GetUserSession(HttpContext);
                //using (MLunarCoffee.Models.ExecuteDataBase connFunc = new MLunarCoffee.Models.ExecuteDataBase())
                //{
                //    dt =await connFunc.ExecuteDataTable("[MLU_sp_Facebook_Page_Inbox_Move_Insert]", CommandType.StoredProcedure,
                //    "@userTo", SqlDbType.NVarChar, Convert.ToInt32(userTo),
                //    "@userFrom", SqlDbType.NVarChar, Convert.ToInt32(userFrom),
                //    "@pageID", SqlDbType.NVarChar, Convert.ToInt32(FanPage),
                //    "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                //    "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                //    "@Created_By", SqlDbType.Int, user.sys_userid
                //  );
                //}
                //if (dt.Rows[0][0].ToString() != "0")
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        await NotiLocal.Noti_Parti_Emp(hubContext,Convert.ToInt32(dt.Rows[i]["TeleID"]), "Chuyển Inbox FanPage", "Số Lượng Inbox Nhận Được: " + dt.Rows[i]["CountInbox"]);
                //    }
                //    return Content("1");
                //}
                //else
                //{
                //    return Content("0");
                //}
                return Content("1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Content("0");
            }
        }
    }
}
