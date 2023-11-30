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

using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Student.Library
{
    public class ClassLibrary : PageModel
    {

         public string _Class { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["Class"];
            if (curr != null) _Class = curr.ToString();
            else _Class = "0";
        }

        public async Task<IActionResult> OnPostLoadLibrary(string ParentID, string begin, string limit,string loadmore)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_ST_ClassLibrary_Load]", CommandType.StoredProcedure,
                        "@ParentID", SqlDbType.Int, ParentID
                        , "@begin", SqlDbType.Int, begin
                        , "@limit", SqlDbType.Int, limit
                        , "@loadmore", SqlDbType.Int, loadmore
                         , "@CreatedID", SqlDbType.Int, user.sys_userid);
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
            catch (Exception)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostSearch(string name)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_ST_ClassLibrary_Search]", CommandType.StoredProcedure,
                        "@name", SqlDbType.NVarChar, name != null ? name : ""
                         , "@CreatedID", SqlDbType.Int, user.sys_userid);
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
            catch (Exception)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostActionpublish(string currentid, string publish)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                     await confunc.ExecuteDataSet("[MLU_sp_ST_Attachmedia_Publish]", CommandType.StoredProcedure,
                        "@Status", SqlDbType.Int, publish
                        , "@CurrentID", SqlDbType.Int, currentid
                        , "@CreatedID", SqlDbType.Int, user.sys_userid
                        );
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeletefile(string currentid, string url)
        {
            try
            {
                string filename = System.IO.Path.GetFileName(url);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                     await confunc.ExecuteDataSet("[MLU_sp_ST_Attachmedia_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, currentid
                        , "@CreatedID", SqlDbType.Int, user.sys_userid
                        );
                }
                Comon.Comon.DeleteFile(Global.sys_ServerImageFolder + "Sys_Student/" + filename);
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
