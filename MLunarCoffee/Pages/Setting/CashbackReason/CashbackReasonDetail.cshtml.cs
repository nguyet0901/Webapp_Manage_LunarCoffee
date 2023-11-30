using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.CashbackReason
{
    public class CashbackReasonDetailModel : PageModel
    {
        public string _CashbackReasonDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _CashbackReasonDetailID = curr != null ? curr.ToString() : "0";
        }


        public async Task<IActionResult> OnPostLoadDetail(string CurrentID)
        {
            var user = Session.GetUserSession(HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_CashbackReason_LoadDetail]", CommandType.StoredProcedure,
                    "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID));
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
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                CashbackReason DataMain = JsonConvert.DeserializeObject<CashbackReason>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_CashbackReason_Insert]", CommandType.StoredProcedure,
                              "@Name ", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                              "@UserID", SqlDbType.Int, user.sys_userid
                          );

                        if (dt.Rows[0][0].ToString() != "1")
                        {
                            return Content("-1");
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_CashbackReason_Update]", CommandType.StoredProcedure,
                              "@Name ", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                              "@UserID", SqlDbType.Int, user.sys_userid,
                              "@currentID ", SqlDbType.Int, CurrentID
                        );

                        if (dt.Rows[0][0].ToString() != "1")
                        {
                            return Content("-1");
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class CashbackReason
        {
            public string Name { get; set; }

        }
    }
}
