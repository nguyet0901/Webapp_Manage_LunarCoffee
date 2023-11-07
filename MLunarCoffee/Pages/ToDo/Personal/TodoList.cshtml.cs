using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.ToDo.Personal
{
    public class TodoListModel : PageModel
    {
        public int _TodoID { get; set; }
        public void OnGet()
        {
            string todoid = Request.Query["detail"];
            _TodoID = Convert.ToInt32((todoid != null ? todoid : "0"));
        }

        public async Task<IActionResult> OnPostLoadData(string CurrentID, int limit, string TimeMessbegin)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await confunc.ExecuteDataTable("[YYY_sp_Todo_LoadList_ByUser]", CommandType.StoredProcedure
                        , "@limit", SqlDbType.Int, limit
                        , "@TimeMessbegin", SqlDbType.Int, TimeMessbegin
                        , "@User_ID", SqlDbType.Int, Convert.ToInt32(user.sys_userid)
                        , "@DetailID", SqlDbType.Int, CurrentID
                        );

                }
                if (dt != null) return Content(JsonConvert.SerializeObject(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
    }
}
