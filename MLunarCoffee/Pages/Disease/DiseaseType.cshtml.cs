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

namespace MLunarCoffee.Pages.Disease
{
    public class DiseaseTypeModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet()
        {
            _dtPermissionCurrentPage = HttpContext.Request.Path;

        }

        public async Task<IActionResult> OnPostLoadData()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_DiseaseType_Load", CommandType.StoredProcedure);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("0");
            }
        }
         public async Task<IActionResult> OnPostLoadImage()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_DentistChart_LoadImage", CommandType.StoredProcedure);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem(int id )
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("MLU_sp_DiseaseType_Delete", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id
                    );
                }
                //if (img != "") {
                //    string des = Comon.Global.sys_ServerImageFolder + "App/" + img.ToString ();
                //    Comon.Comon.DeleteFile (des);
                //}
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
