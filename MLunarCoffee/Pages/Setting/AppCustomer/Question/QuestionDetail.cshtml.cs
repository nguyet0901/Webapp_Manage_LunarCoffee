using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.AppCustomer.Question
{
    public class QuestionDetailModel : PageModel
    {
        public string _QuestionCurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _QuestionCurrentID = curr.ToString();
            }
            else
            {
                _QuestionCurrentID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_AC_Question_LoadDetail]", CommandType.StoredProcedure,
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
        public async Task<IActionResult> OnPostExcuteData(string Question, string content, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID.Trim() == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                       dt = await connFunc.ExecuteDataTable("[MLU_sp_AC_Question_Insert]", CommandType.StoredProcedure,
                        "@Question", SqlDbType.Int, Question.Replace("'", "").Trim(),
                        "@Content", SqlDbType.Int, content != null ? content.Replace("'", "").Trim() : "",
                        "@UserID", SqlDbType.Int, user.sys_userid);
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                       dt = await connFunc.ExecuteDataTable("[MLU_sp_AC_Question_Update]", CommandType.StoredProcedure,
                           "@Question", SqlDbType.Int, Question.Replace("'", "").Trim(),
                            "@Content", SqlDbType.Int, content != null ? content.Replace("'", "").Trim() : "",
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
