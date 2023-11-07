using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer
{

    public class GeneralInfoModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }

        public void OnGet()
        {
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoadIni(string custID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.LoadEmployeeFull(user.sys_branchID ,user.sys_AllBranchID);
                        dt.TableName = "EmpFull";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_ListTele" ,CommandType.StoredProcedure);
                        dt.TableName = "Tele";
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
        public async Task<IActionResult> OnPostLoadata(int CustomerID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_LoadGeneralInfo]" ,CommandType.StoredProcedure ,
                      "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID == 0 ? 0 : CustomerID)
                      ,"@BranchID" ,SqlDbType.Int ,Convert.ToInt32(user.sys_branchID));

                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostBackStatus(string appID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (appID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Customer_Appointment_BackStatus" ,CommandType.StoredProcedure ,
                            "@appID" ,SqlDbType.Int ,appID ,
                            "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                            "@BranchID" ,SqlDbType.Int ,user.sys_branchID ,
                            "@EmployeeID" ,SqlDbType.Int ,user.sys_employeeid
                        );
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadCustomerGroup(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Customer_Group_Customer_Load" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadCustCare(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_CustomerInfo_EmpLoad" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadRelation(string custID)
        {
            try
            {
                DataSet ds = new DataSet();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        ds = await connFunc.ExecuteDataSet("YYY_sp_Customer_Relationship_Load" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,custID ,
                            "@descustid" ,SqlDbType.Int ,0
                        );
                    }
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadHistory(string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (CustomerID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Customer_LoadInfo_History" ,CommandType.StoredProcedure ,
                            "@CurrentID" ,SqlDbType.Int ,CustomerID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadApp(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYYY_sp_Customer_LoadInfo_App" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadAnamnesis(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Customer_Anamnesis_LoadLastDetail" ,CommandType.StoredProcedure ,
                            "@CustID" ,SqlDbType.Int ,custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadConsultStatus(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYYY_sp_Customer_LoadInfo_ConsultStatus" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        //public async Task<IActionResult> OnPostLoadOverview(string custID)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        if (custID != "0")
        //        {
        //            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //            {
        //                dt = await connFunc.ExecuteDataTable("YYYY_sp_Customer_LoadInfo_Overview", CommandType.StoredProcedure,
        //                    "@CustID", SqlDbType.Int, custID
        //                );
        //            }
        //        }
        //        return Content(Comon.DataJson.Datatable(dt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        public async Task<IActionResult> OnPostLoadService(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYYY_sp_Customer_LoadInfo_Service" ,CommandType.StoredProcedure ,
                            "@CustID" ,SqlDbType.Int ,custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadProperties(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_CustomerProperties_Load]" ,CommandType.StoredProcedure ,
                            "@CustID" ,SqlDbType.Int ,custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadTreat(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYYY_sp_Customer_LoadInfo_Treat" ,CommandType.StoredProcedure ,
                            "@CustID" ,SqlDbType.Int ,custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadPayment(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYYY_sp_Customer_LoadInfo_Payment" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.Int ,custID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadRating(string custID ,string branchID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_AC_Rating_LoadByCust" ,CommandType.StoredProcedure ,
                            "@CustID" ,SqlDbType.Int ,custID ,
                            "@BranchID" ,SqlDbType.Int ,branchID
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadBroker(string custID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYYY_sp_Customer_LoadInfo_Broker" ,CommandType.StoredProcedure ,
                            "@CustID", SqlDbType.Int, Convert.ToInt32(custID)
                        );
                    }
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
