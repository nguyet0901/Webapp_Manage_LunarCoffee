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

namespace MLunarCoffee.Pages.Customer.Assay
{

    public class AssayDetailModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];
            if (cus.ToString() != String.Empty)
            {
                _CustomerID = cus.ToString();
                if (curr.ToString() != String.Empty)
                {
                    _CurrentID = curr.ToString();
                }
                else
                {
                    _CurrentID = "0";
                }

            }
            else
            {
                _CustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }
        }

        public async Task<IActionResult> OnPostLoadInitializeData(string CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtForm = new DataTable();
                        dtForm = await confunc.ExecuteDataTable("[YYY_sp_print_v2_GetForm]", CommandType.StoredProcedure
                             , "@TYPE", SqlDbType.NVarChar, "assay_form");
                        dtForm.TableName = "form";
                        return dtForm;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtField = new DataTable();
                        dtField = await confunc.ExecuteDataTable("[YYY_sp_print_v2_Assay]", CommandType.StoredProcedure
                            , "@id", SqlDbType.Int, CurrentID);
                        dtField.TableName = "field";
                        return dtField;
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

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Assay_Customer_LoadDetail]", CommandType.StoredProcedure,
                  "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id)
                    , "@UserId", SqlDbType.Int, user.sys_userid
                   , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                  );
            }
            return Content(Comon.DataJson.Datatable(dt));
        }
        public async Task<IActionResult> OnPostCheckStogeSign(string CustomerID ,string StogeRule)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_PrintForm_StogeRule_Check]" ,CommandType.StoredProcedure ,
                      "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID)
                      ,"@StogeRule" ,SqlDbType.Int ,Convert.ToInt32(StogeRule)
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostSaveStogeSign(string CustomerID ,string StogeSign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataSet("[YYY_sp_PrintForm_StogeRule_Insert]" ,CommandType.StoredProcedure ,
                      "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID)
                      ,"@StogeRule" ,SqlDbType.Int ,Convert.ToInt32(StogeSign)
                      ,"@CreatedBy" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, string Note, string AssayID, string CustomerID, string BranchID)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Customer_Assay_Insert]", CommandType.StoredProcedure,
                           "@CustomerID", SqlDbType.Int, CustomerID,
                           "@AssayID", SqlDbType.Int, AssayID,
                           "@Note", SqlDbType.NVarChar, (Note != null ? Note.Replace("'", "").Trim() : ""),
                           "@Data", SqlDbType.NVarChar, data.Replace("'", "").Trim(),
                           "@Created_By", SqlDbType.Int, user.sys_userid,
                           "@BranchID", SqlDbType.Int, BranchID
                       );

                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Customer_Assay_Update]", CommandType.StoredProcedure,
                            "@Data", SqlDbType.NVarChar, data.Replace("'", "").Trim(),
                            "@AssayID", SqlDbType.Int, AssayID,
                            "@Note", SqlDbType.NVarChar, (Note != null ? Note.Replace("'", "").Trim() : ""),
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@BranchID", SqlDbType.Int, BranchID

                        );
                    }
                }
                return Content("1");

            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {

            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Assay_Customer_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid
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
