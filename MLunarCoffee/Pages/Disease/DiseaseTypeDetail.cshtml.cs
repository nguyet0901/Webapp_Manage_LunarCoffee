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

    public class DiseaseTypeDetailModel : PageModel
    {
        public int DistypeID { get; set; }
        public void OnGet()
        {
            var curr = Request.Query["CurrentID"];
            if (curr.ToString() != String.Empty)
            {
                DistypeID = Convert.ToInt32(curr);
            }
            else
            {
                DistypeID = 0;
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_DiseaseType_LoadDetail", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(id));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string Name, string Note, string Image, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();

                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_DiseaseType_Insert", CommandType.StoredProcedure,
                              "@Name", SqlDbType.NVarChar, Name != null ? Name : "",
                              "@Note", SqlDbType.NVarChar, Note != null ? Note : "",
                              "@Image", SqlDbType.NVarChar, Image != null ? Image : ""
                          );
                    }
                    if (dt.Rows.Count > 0)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                    else
                    {
                        return Content("0");
                    }

                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_DiseaseType_Update", CommandType.StoredProcedure,
                               "@Name", SqlDbType.NVarChar, Name != null ? Name : "",
                              "@Note", SqlDbType.NVarChar, Note != null ? Note : "",
                              "@Image", SqlDbType.NVarChar, Image != null ? Image : "",
                              "@CurrentID", SqlDbType.Int, CurrentID
                          );
                    }
                    if (dt.Rows.Count > 0)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                    else
                    {
                        return Content("0");
                    }

                }


            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
