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

namespace MLunarCoffee.Pages.Student.Trains.Detail
{
    public class ClassFile : PageModel
    {


        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadFile(string begin, string limit)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_ClassFile_Load]", CommandType.StoredProcedure,
                         "@begin", SqlDbType.Int, begin
                        , "@limit", SqlDbType.Int, limit
                         , "@CreatedID", SqlDbType.Int, user.sys_userid);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
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
                    ds = await confunc.ExecuteDataSet("[MLU_sp_ST_ClassFile_Search]", CommandType.StoredProcedure,
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


    }
}
