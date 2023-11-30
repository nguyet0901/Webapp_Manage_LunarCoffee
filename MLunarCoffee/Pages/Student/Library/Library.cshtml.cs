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
    public class Library : PageModel
    {

        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";

        }


        //public async Task<IActionResult> OnPostLoadLibrary(string ParentID, string begin, string limit)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            ds = await confunc.ExecuteDataSet("[MLU_sp_ST_ClassLibrary_Load]", CommandType.StoredProcedure,
        //                "@ParentID", SqlDbType.Int, ParentID
        //                , "@begin", SqlDbType.Int, begin
        //                , "@limit", SqlDbType.Int, limit
        //                 , "@CreatedID", SqlDbType.Int, user.sys_userid);
        //        }
        //        if (ds != null)
        //        {
        //            return Content(Comon.DataJson.Dataset(ds));
        //        }
        //        else
        //        {
        //            return Content("[]");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Content("[]");
        //    }
        //}
        //public async Task<IActionResult> OnPostSearch(string name)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            ds = await confunc.ExecuteDataSet("[MLU_sp_ST_ClassLibrary_Search]", CommandType.StoredProcedure,
        //                "@name", SqlDbType.NVarChar, name != null ? name : ""
        //                 , "@CreatedID", SqlDbType.Int, user.sys_userid);
        //        }
        //        if (ds != null)
        //        {
        //            return Content(Comon.DataJson.Dataset(ds));
        //        }
        //        else
        //        {
        //            return Content("[]");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Content("[]");
        //    }
        //}
    }
}
