using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.DrugStore
{
    public class GeneralModel : PageModel
    {
        public string layout { get; set; }
        //public string _BranchID { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
            //_BranchID = user.sys_branchID.ToString();
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
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtProduct = new DataTable();
                        dtProduct = await connFunc.ExecuteDataTable("[MLU_sp_DrugStore_Product_Get]" ,CommandType.StoredProcedure);
                        dtProduct.TableName = "Product";
                        return dtProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtUnitProduct = new DataTable();
                        dtUnitProduct = await connFunc.ExecuteDataTable("[MLU_sp_Product_Unit_Product]" ,CommandType.StoredProcedure);
                        dtUnitProduct.TableName = "UnitProduct";
                        return dtUnitProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_LoadCombo_EmployeeFull_v2" ,CommandType.StoredProcedure
                            ,"@BranchToken" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                            ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                            );
                        dt.TableName = "EmpFull";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dt.TableName = "Branch";
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
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostLoadData(string detailID ,string dateFrom ,string dateTo ,string beginID ,string limit ,string medCode)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_WareHouse_DrugStore_LoadListv2]" ,CommandType.StoredProcedure
                        ,"@DetailID" ,SqlDbType.Int ,Convert.ToInt32(detailID)
                        ,"@limit" ,SqlDbType.Int ,Convert.ToInt32(limit)
                        ,"@beginID" ,SqlDbType.Int ,Convert.ToInt32(beginID)
                        ,"@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        ,"@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        ,"@medCode" ,SqlDbType.DateTime ,!String.IsNullOrEmpty(medCode) ? Comon.Comon.ConvertToUnsign(medCode).ToLower() : ""
                        ,"@IsAllBranch" ,SqlDbType.Int, user.sys_AllBranchID
                        ,"@BranchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch
                        );
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


    }
}
