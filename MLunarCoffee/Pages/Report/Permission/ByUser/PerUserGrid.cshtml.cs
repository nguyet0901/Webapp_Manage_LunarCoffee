using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report.Permission.ByUser
{
    public class PerUserGridModel : PageModel
    {
        public string _branchID { get; set; }
        public string _SheetID { get; set; }
        public string _versionofWebApplication { get; set; }
        public void OnGet()
        {
            _versionofWebApplication = Comon.Global.sys_DBVersion;
            _SheetID = Request.Query["sheet"].ToString();
        }

        public async Task<IActionResult> OnPostLoaddata()
        {
            try
            {
                var tasks = new List<Task<DataTable>>();
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_rp_PerUser_AuthorizeFunction" ,CommandType.StoredProcedure
                            ,"@GroupID" ,SqlDbType.Int ,1);
                        dt.TableName = "PerType";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_rp_PerUser_LoadUser]" ,CommandType.StoredProcedure);
                        dt.TableName = "User";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_rp_PerUser_LoadGroup]" ,CommandType.StoredProcedure);
                        dt.TableName = "UserGroup";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_rp_PerUser_GetTemplateReport]" ,CommandType.StoredProcedure);
                        dt.TableName = "AllReport";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_rp_PerUser_GetEditDetail]" ,CommandType.StoredProcedure);
                        dt.TableName = "AllAction";
                        return dt;
                    }
                }));

                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                var dtAllMenu = Comon.Global.sys_PermissionAllMenu.Copy();
                dtAllMenu.TableName = "AllMenu";
                ds.Tables.Add(dtAllMenu);

                var dtAllControl = Comon.Global.sys_List_TabControl_Allowed.Copy();
                dtAllControl.TableName = "AllControl";
                ds.Tables.Add(dtAllControl);

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
    }
}
