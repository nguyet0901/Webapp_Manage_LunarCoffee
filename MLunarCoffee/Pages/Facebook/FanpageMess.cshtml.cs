using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Facebook
{

    public class FanpageMessModel : PageModel
    {
        public string Face_appid { get; set; }
        public string Face_version { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {

            Face_appid = Comon.Global.sys_face_appid;
            Face_version = Comon.Global.sys_face_version;
            string _layout = Request.Query["layout"];
            layout = String.IsNullOrEmpty(_layout) ? "" : _layout;
        }

        public async Task<IActionResult> OnPostIni()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_FB_Tag_LoadCombo" ,CommandType.StoredProcedure);
                        dt.TableName = "Tag";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_FB_LoadPage]" ,CommandType.StoredProcedure
                        ,"@userid" ,SqlDbType.NVarChar ,user.sys_userid);
                        dt.TableName = "Page";
                        return dt;
                    }
                }));

                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
}
