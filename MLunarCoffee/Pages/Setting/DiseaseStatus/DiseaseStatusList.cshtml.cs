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

namespace MLunarCoffee.Pages.Setting
{
    public class DiseaseListModel : PageModel
    {
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadInitializeData()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtType = new DataTable();
                        dtType = await confunc.ExecuteDataTable("[YYY_sp_DiseaseStatus_LoadType]", CommandType.StoredProcedure);
                        dtType.TableName = "Type";
                        return dtType;
                    }

                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSignID = new DataTable();
                        dtSignID = await confunc.ExecuteDataTable("[YYY_sp_DiseaseStatus_LoadComboSign]", CommandType.StoredProcedure);
                        dtSignID.TableName = "Sign";
                        return dtSignID;
                    }

                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string IsTeeth)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    dt = await connFunc.ExecuteDataTable("[YYY_sp_DiseaseStatus_Load]", CommandType.StoredProcedure
                        , "@IsTeeth", SqlDbType.Int, IsTeeth
                        );
                }

                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostDeleteItem(int id, string IsTeeth)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_DiseaseStatus_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@IsTeeth", SqlDbType.Int, IsTeeth
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
