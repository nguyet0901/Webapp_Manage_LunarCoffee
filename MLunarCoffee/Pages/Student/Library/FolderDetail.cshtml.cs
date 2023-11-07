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

namespace MLunarCoffee.Pages.Student.Library
{

    public class FolderDetail : PageModel
    {
        public string _FolderID { get; set; }
        public string _ParentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _FolderID = curr.ToString();
            else _FolderID = "0";
            string par = Request.Query["ParentID"];
            if (par != null) _ParentID = par.ToString();
            else _ParentID = "0";
        }
        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_MediaComBo]", CommandType.StoredProcedure);

                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("0");


            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadData(string id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_ST_FolderDetail_Load]", CommandType.StoredProcedure,
                         "@id", SqlDbType.Int, id
                        , "@CreatedBy", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostExcuteData(string id, string parentid, string name,string type, string content)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                name = Comon.Comon.ConvertToUnsign(name).ToLower().Replace(" ", "").Trim();
                DataTable dtFolder = new DataTable();
                if (id == "0")
                {
                    using (Models.ExecuteDataBase func = new Models.ExecuteDataBase())
                    {
                        dt = await func.ExecuteDataTable("[YYY_sp_ST_FolderDetail_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, name
                            , "@Note", SqlDbType.NVarChar, content != null ? content : ""
                            , "@ParentID", SqlDbType.Int, parentid
                            , "@type", SqlDbType.Int, type
                            , "@Created_By", SqlDbType.Int, user.sys_userid
                        );
                    }
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    using (Models.ExecuteDataBase func = new Models.ExecuteDataBase())
                    {
                        dt = await func.ExecuteDataTable("[YYY_sp_ST_FolderDetail_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, name
                            , "@Note", SqlDbType.NVarChar, content != null ? content : ""
                            , "@CurrentID", SqlDbType.Int, id
                            , "@type", SqlDbType.Int, type
                            , "@Created_By", SqlDbType.Int, user.sys_userid
                        );
                    }
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }


            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteData(string id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                DataTable dtFolder = new DataTable();
                using (Models.ExecuteDataBase func = new Models.ExecuteDataBase())
                {
                    dt = await func.ExecuteDataTable("[YYY_sp_ST_FolderDetail_Delete]", CommandType.StoredProcedure,
                         "@CurrentID", SqlDbType.Int, id
                        , "@Created_By", SqlDbType.Int, user.sys_userid
                    );
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
