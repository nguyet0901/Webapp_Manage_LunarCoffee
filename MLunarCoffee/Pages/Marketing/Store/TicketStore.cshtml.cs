using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing.Store
{
    public class TicketStoreModel : PageModel
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
        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Group_ComboLoad]", CommandType.StoredProcedure);
                    dt.TableName = "Group";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Type_ComboLoad]", CommandType.StoredProcedure);
                    dt.TableName = "TicketSource";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Branch_Load]" ,CommandType.StoredProcedure
                        ,"@LoadNormal", SqlDbType.Int, 1
                        ,"@UserID", SqlDbType.Int, user.sys_userid
                        );
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                //using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //{
                //    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_LoadTele]", CommandType.StoredProcedure,
                //      "@UserID", SqlDbType.Int, user.sys_userid);
                //    dt.TableName = "Tele";
                //    ds.Tables.Add(dt);
                //}

                //using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //{
                //    dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                //        , "@UserID", SqlDbType.Int, user.sys_userid
                //        , "@LoadNormal", SqlDbType.Int, 1);
                //    dt.TableName = "Branch";
                //    ds.Tables.Add(dt);
                //}

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostTicketStoreLoad(string beginId, string source, string group, string limit, string DateFrom, string DateTo, string branchID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_No_Telesale]", CommandType.StoredProcedure
                         , "@BeginID", SqlDbType.Int, Convert.ToInt32(beginId)
                         , "@Source", SqlDbType.Int, Convert.ToInt32(source)
                         , "@Group", SqlDbType.Int, Convert.ToInt32(group)
                         , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateFrom)
                         , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DateTo)
                         , "@Limit", SqlDbType.Int, Convert.ToInt32(limit)
                         , "@branchID" ,SqlDbType.Int ,Convert.ToInt32(branchID)
                         , "@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                         , "@branchTokenUser" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


    }
}
