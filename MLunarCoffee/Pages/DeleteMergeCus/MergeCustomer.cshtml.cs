using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.DeleteMergeCus
{
    public class MergeCustomerModel : PageModel
    {
        public void OnGet()
        {
        }
         public async Task<IActionResult> OnPostLoadComboInitialize()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("MLU_sp_Customer_Reason_Merge", CommandType.StoredProcedure);
                    dt.TableName = "Reason";
                    ds.Tables.Add(dt);
                }
                return Content(JsonConvert.SerializeObject(ds));
            }
            catch(Exception ex)
            {
                return Content("[]");
            }

        }

         public async Task<IActionResult> OnPostLoadDataMergeCus(int branchid, string dateFilter)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Customer_Merge_Load]", CommandType.StoredProcedure,
                      "@BranchID", SqlDbType.Int, branchid,
                      "@dateFilter", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFilter));
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

         public async Task<IActionResult> OnPostSearchingCustomerToMerge(string keyword)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Customer_Merge_Searching]", CommandType.StoredProcedure,
                 "@searchText", SqlDbType.NVarChar, keyword
                 , "@user", SqlDbType.Int, user.sys_userid);
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

         public async Task<IActionResult> OnPostMergeCustomer(string CustomerFrom, string CustomerTo, string ReasonID)
        {
            try
            {

                Comon.Comon.MoveFolder(CustomerFrom, CustomerTo);
                if (CustomerFrom != "0" && CustomerTo != "0")
                {
                    var user = Session.GetUserSession(HttpContext);
                    DataTable dt = new DataTable();
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt =await  confunc.ExecuteDataTable("[MLU_sp_Customer_Merge_Customer]", CommandType.StoredProcedure,
                          "@CustomerFrom", SqlDbType.Int, CustomerFrom,
                           "@CustomerTo", SqlDbType.Int, CustomerTo,
                          "@user", SqlDbType.Int, user.sys_userid,
                          "@Reason", SqlDbType.Int, ReasonID,
                          "@BranchID", SqlDbType.Int, user.sys_branchID);
                    }
                    return Content("1");
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
