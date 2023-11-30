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

namespace MLunarCoffee.Pages.Customer.Insurance
{

    public class InsuranceDetail : PageModel
    {

        public string _TabID { get; set; }
        public string _Viewonly { get; set; }
        public void OnGet()
        {
            var curr = Request.Query["TabID"];
            var viewonly = Request.Query["Viewonly"];
            if (viewonly.ToString() != String.Empty) _Viewonly = viewonly.ToString();
            else _Viewonly = "0";
            if (curr.ToString() != String.Empty)
            {
                _TabID = curr.ToString();

            }
            else
            {

                Response.Redirect("/assests/Error/index.html");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(string TabID)
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
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Insurance_LoadComBo]", CommandType.StoredProcedure);
                        dt.TableName = "Insurance";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                       DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_InsurDetailEdit]", CommandType.StoredProcedure
                         , "@TabID", SqlDbType.Int, TabID);
                         dt.TableName = "Detail";
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
        public async Task<IActionResult> OnPostCancel(string TabID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtForm = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_InsurDetailCancel]", CommandType.StoredProcedure
                         , "@TabID", SqlDbType.Int, TabID
                         , "@CreatedBy", SqlDbType.Int, user.sys_userid);

                }
                return Content(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(string TabID, string insuranceid, string insuramount)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtForm = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_InsurDetailSave]", CommandType.StoredProcedure
                         , "@TabID", SqlDbType.Int, TabID
                         , "@insuranceid", SqlDbType.Int, insuranceid
                         , "@insuramount", SqlDbType.Decimal, Convert.ToDecimal(insuramount)
                         , "@CreatedBy", SqlDbType.Int, user.sys_userid
                         );

                }
                return Content(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        //public async Task<IActionResult> OnPostLoadata(int id)
        //{
        //    DataTable dt = new DataTable();
        //    var user = Session.GetUserSession(HttpContext);
        //    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //    {
        //        dt = await confunc.ExecuteDataTable("[MLU_sp_Assay_Customer_LoadDetail]", CommandType.StoredProcedure,
        //          "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id)
        //            , "@UserId", SqlDbType.Int, user.sys_userid
        //           , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
        //          );
        //    }
        //    return Content(Comon.DataJson.Datatable(dt));
        //}

        //public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, string Note, string AssayID, string CustomerID, string BranchID)
        //{
        //    try
        //    {

        //        var user = Session.GetUserSession(HttpContext);
        //        if (CurrentID == "0")
        //        {
        //            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //            {
        //                await connFunc.ExecuteDataTable("[MLU_sp_Customer_Assay_Insert]", CommandType.StoredProcedure,
        //                   "@CustomerID", SqlDbType.Int, CustomerID,
        //                   "@AssayID", SqlDbType.Int, AssayID,
        //                   "@Note", SqlDbType.NVarChar, (Note != null ? Note.Replace("'", "").Trim() : ""),
        //                   "@Data", SqlDbType.NVarChar, data.Replace("'", "").Trim(),
        //                   "@Created_By", SqlDbType.Int, user.sys_userid,
        //                   "@BranchID", SqlDbType.Int, BranchID
        //               );

        //            }
        //        }
        //        else
        //        {
        //            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //            {
        //                await connFunc.ExecuteDataTable("[MLU_sp_Customer_Assay_Update]", CommandType.StoredProcedure,
        //                    "@Data", SqlDbType.NVarChar, data.Replace("'", "").Trim(),
        //                    "@AssayID", SqlDbType.Int, AssayID,
        //                    "@Note", SqlDbType.NVarChar, (Note != null ? Note.Replace("'", "").Trim() : ""),
        //                    "@Created_By", SqlDbType.Int, user.sys_userid,
        //                    "@CurrentID", SqlDbType.Int, CurrentID,
        //                    "@BranchID", SqlDbType.Int, BranchID

        //                );
        //            }
        //        }
        //        return Content("1");

        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }


        //}

        //public async Task<IActionResult> OnPostDeleteItem(int id)
        //{

        //    try
        //    {
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //        {
        //            await connFunc.ExecuteDataTable("[MLU_sp_Assay_Customer_Delete]", CommandType.StoredProcedure,
        //                "@CurrentID", SqlDbType.Int, id,
        //                "@userID", SqlDbType.Int, user.sys_userid
        //            );
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }

        //}
    }

}
