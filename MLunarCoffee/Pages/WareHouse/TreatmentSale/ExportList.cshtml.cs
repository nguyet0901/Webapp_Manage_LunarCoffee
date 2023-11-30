using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.TreatmentSale
{
    public class ExportList : PageModel
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
 

        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                var user = Session.GetUserSession(HttpContext);

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                            , "@UserID", SqlDbType.Int, user.sys_userid
                            , "@LoadNormal", SqlDbType.Int, 1);
                        dt.TableName = "Branch";
                        return dt;
                    }

                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtProduct = new DataTable();
                        dtProduct = await connFunc.ExecuteDataTable("[MLU_sp_Product_Get]", CommandType.StoredProcedure);
                        dtProduct.TableName = "Product";
                        return dtProduct;
                    }

                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtUnitProduct = new DataTable();
                        dtUnitProduct = await connFunc.ExecuteDataTable("[MLU_sp_Product_Unit_Product]", CommandType.StoredProcedure);
                        dtUnitProduct.TableName = "UnitProduct";
                        return dtUnitProduct;
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
                return Content("[]");
            }

        }


        public async Task<IActionResult> OnPostLoadData(string DetailID, string BranchID, string dateFrom, string dateTo
            , string idbegin, string limit)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_WareHouse_ExportByBranch]", CommandType.StoredProcedure
                        , "@DetailID", SqlDbType.Int, Convert.ToInt32(DetailID)
                        , "@BranchID", SqlDbType.Int, BranchID
                        , "@limit", SqlDbType.Int, Convert.ToInt32(limit)
                        , "@idbegin", SqlDbType.Int, Convert.ToInt32(idbegin)
                        , "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
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
            catch (Exception ex)
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostUpdateSign(int id, string sign, string typeid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_WareHouse_Export_Update_Sign]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(id),
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@Sign", SqlDbType.NVarChar, sign
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
