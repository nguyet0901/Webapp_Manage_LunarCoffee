using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.AppCustomer.PolicyApp
{
    public class PolicyAppDetailModel : PageModel
    {
        public string _PolicyAppCurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _PolicyAppCurrentID = curr.ToString();
            }
            else
            {
                _PolicyAppCurrentID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_AC_PolicyApp_LoadDetail]", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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
            catch (Exception ex)
            {
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostExcuteData(string policy, string shortDescription, string content, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID.Trim() == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_PolicyApp_Insert]", CommandType.StoredProcedure,
                         "@Policy", SqlDbType.NVarChar, policy != null ? policy.Replace("'", "").Trim() : "",
                         "@ShortDesciption", SqlDbType.NVarChar, shortDescription != null ? shortDescription.Replace("'", "").Trim() : "",
                         "@Content", SqlDbType.NVarChar, content != null ? content.Replace("'", "").Trim() : "",
                         "@UserID", SqlDbType.Int, user.sys_userid);
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_AC_PolicyApp_Update]", CommandType.StoredProcedure,
                             "@Policy", SqlDbType.NVarChar, policy != null ? policy.Replace("'", "").Trim() : "",
                             "@ShortDesciption", SqlDbType.NVarChar, shortDescription != null ? shortDescription.Replace("'", "").Trim() : "",
                             "@Content", SqlDbType.NVarChar, content != null ? content.Replace("'", "").Trim() : "",
                             "@UserID", SqlDbType.Int, user.sys_userid,
                             "@CurrentID", SqlDbType.Int, CurrentID);
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
