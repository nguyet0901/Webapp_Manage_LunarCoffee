using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting
{
    public class TreatmentStageListModel : PageModel
    {
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoad_Data_Initialize()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                var tasks = new List<Task<DataTable>>
                {
                    Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtProduct = new DataTable();
                            dtProduct = await connFunc.ExecuteDataTable("[MLU_sp_Product_Get]" ,CommandType.StoredProcedure);
                            dtProduct.TableName = "Product";
                            return dtProduct;
                        }
                    }) ,
                    Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtUnitPro = new DataTable();
                            dtUnitPro = await connFunc.ExecuteDataTable("[MLU_sp_Product_Unit_Product]" ,CommandType.StoredProcedure);
                            dtUnitPro.TableName = "UnitProduct";
                            return dtUnitPro;
                        }
                    }) ,
                };

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

        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_TreatmentStage_Load]" ,CommandType.StoredProcedure);
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
        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_TreatmentStage_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
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
