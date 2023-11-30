using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Report.WareHouse.Inventory
{
    public class StockGridModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _WareID { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _WareID = Request.Query["WareID"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Combo_Unit]", CommandType.StoredProcedure);
                    dt.TableName = "Unit";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_v2_Product_Combo_Supplier]", CommandType.StoredProcedure);
                    dt.TableName = "Supplier";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Combo_Product_Type]", CommandType.StoredProcedure);
                    dt.TableName = "ProductType";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadata(string dateFrom, string dateTo, string WareID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_rp_WareHouse_InventoryV2]", CommandType.StoredProcedure
                        , "@WareID", SqlDbType.Int, WareID
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(dateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(dateTo)
                        , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                        , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadStockAll(string dateFrom, string dateTo, string WareID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_rp_WareHouse_StockAll]", CommandType.StoredProcedure
                        , "@WareID", SqlDbType.Int, WareID
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(dateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(dateTo)
                        , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                        , "@branchTokenUser", SqlDbType.NVarChar, user.sys_AreaBranch);
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
