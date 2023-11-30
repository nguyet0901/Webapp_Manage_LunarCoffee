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

namespace MLunarCoffee.Pages.Customer.Installment
{

    public class InstallmentDetail : PageModel
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
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtForm = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_InstallmentDetailEdit]", CommandType.StoredProcedure
                         , "@TabID", SqlDbType.Int, TabID);

                }
                return Content(Comon.DataJson.Datatable(dt));
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
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtForm = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_InstallmentDetailCancel]", CommandType.StoredProcedure
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
        public async Task<IActionResult> OnPostExecuted(string TabID,string term,string preamount,string dayinmonth)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtForm = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_InstallmentDetailSave]", CommandType.StoredProcedure
                         , "@TabID", SqlDbType.Int, TabID
                         , "@term", SqlDbType.Int, term
                         , "@preamount", SqlDbType.Decimal, Convert.ToDecimal(preamount)
                         , "@dayinmonth", SqlDbType.Int, dayinmonth
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
