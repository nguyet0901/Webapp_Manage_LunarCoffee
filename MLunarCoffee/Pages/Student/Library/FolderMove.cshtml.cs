using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Student.Library
{

    public class FolderMove : PageModel
    {

        public string _file { get; set; }
        public string _currentfolder { get; set; }
        public void OnGet()
        {
            string file = Request.Query["file"];
            if (file != null) _file = file.ToString();
            else _file = "";

            string currentfolder = Request.Query["currentfolder"];
            if (currentfolder != null) _currentfolder = currentfolder.ToString();
            else _currentfolder = "0";

        }
        public async Task<IActionResult> OnPostLoadCombo(string currentfolder, string file)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_MediaComBo_Moving]", CommandType.StoredProcedure
                        , "@FolderID", SqlDbType.Int, currentfolder
                        , "@file", SqlDbType.NVarChar, file != null ? file : ""
                        , "@CreatedID", SqlDbType.Int, user.sys_userid);

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

        public async Task<IActionResult> OnPostExcuteData(string folderid, string stringfile)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase func = new Models.ExecuteDataBase())
                {
                    dt = await func.ExecuteDataTable("[MLU_sp_ST_Media_Moving]", CommandType.StoredProcedure,
                        "@folderid", SqlDbType.Int, folderid
                        , "@stringfile", SqlDbType.NVarChar, stringfile != null ? stringfile : ""
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
        public async Task<IActionResult> OnPostDeleteData(string id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                DataTable dtFolder = new DataTable();
                using (Models.ExecuteDataBase func = new Models.ExecuteDataBase())
                {
                    dt = await func.ExecuteDataTable("[MLU_sp_ST_FolderDetail_Delete]", CommandType.StoredProcedure,
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
