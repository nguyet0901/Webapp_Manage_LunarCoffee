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
using MLunarCoffee.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace MLunarCoffee.Pages.Labo.Status
{
    public class LaboChangeStatusModel : PageModel
    {
        public string _LaboDetailID { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public LaboChangeStatusModel ( IHubContext<NotiHub> hubContext ) {
            this.hubContext = hubContext;
        }
        public void OnGet()
        { 
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _LaboDetailID = curr.ToString();
            }
            else
            {
                _LaboDetailID = "0";
            } 
        }
       
         public async Task<IActionResult> OnPostInitialize(string currentid)
        {

            try
            {
                DataSet ds = new DataSet(); 
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_Labo_LoadNextStatus", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, currentid);

                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        
        public async Task<IActionResult> OnPostExcuteData (string data, string CurrentID)
        {

            try
            {
                DataTable dt = new DataTable();
                LaboStatus DataMain = JsonConvert.DeserializeObject<LaboStatus>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    return Content("0");
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("[YYY_sp_Labo_ChangeStatus]", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Status", SqlDbType.Int, DataMain.StatusID,
                            "@QuickNote", SqlDbType.Int, DataMain.QuickNote,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@Created_By", SqlDbType.Int, user.sys_userid);
                    }
                    int result = Convert.ToInt32(dt.Rows[0]["RESULT"]);
                    string code=  dt.Rows[0]["Code"].ToString();
                    if (result != 0)
                    {
                        int userto = Convert.ToInt32(dt.Rows[0]["USER_TO"]);
                        int userfrom = Convert.ToInt32(dt.Rows[0]["USER_FROM"]);
                       // if (userto != 0 && userto != user.sys_userid) await NotiLocal.Noti_Labo_User(hubContext,userto, Convert.ToInt32(CurrentID), result, 0, DataMain.StatusID, "");
                       // if (userfrom != 0 && userfrom != user.sys_userid) await NotiLocal.Noti_Labo_User(hubContext,userfrom, Convert.ToInt32(CurrentID), result, 0, DataMain.StatusID, "");
                    }
                    return Content(code.ToString());

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Content("0");
            }
        }
    }
    public class LaboStatus
    {
        public int StatusID { get; set; }
        public string Content { get; set; }
        public int QuickNote { get; set; }

    }

}
