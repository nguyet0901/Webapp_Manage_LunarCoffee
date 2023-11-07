using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Student.Library
{

    public class AttachDetail : PageModel
    {
        public string _FolderID { get; set; }
        public string _ParentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _FolderID = curr.ToString();
            else _FolderID = "0";

        }


        public async Task<IActionResult> OnPostInsert(string id, string url, string filetype, string name)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                filetype = filetype.ToLower().Trim();
                if (filetype.Contains("doc")) filetype = "word";
                if (filetype.Contains("xl")) filetype = "xcel";

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_MediaDetail_Insert]", CommandType.StoredProcedure
                       , "@FolderID", SqlDbType.Int, id
                       , "@url", SqlDbType.NVarChar, Global.sys_HTTPImageRoot + "Sys_Student/" + url
                       , "@filetype", SqlDbType.NVarChar, filetype
                       , "@name", SqlDbType.NVarChar, Path.GetFileNameWithoutExtension(name)
                       , "@Created_By", SqlDbType.Int, user.sys_userid
                    );
                }
                if (dt != null && dt.Rows.Count == 1)
                {
                    return Content("1");
                }
                else return Content("0");


            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }

}
