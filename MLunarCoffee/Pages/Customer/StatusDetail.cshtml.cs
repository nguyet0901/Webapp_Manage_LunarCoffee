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

namespace MLunarCoffee.Pages.Customer
{

    public class StatusDetailModel : PageModel
    {
        public string _StatusDetailCustomerID { get; set; }
        public string _StatusDetailCurrentID { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];

            if (cus.ToString() != String.Empty)
            {
                _StatusDetailCustomerID = cus.ToString();
                if (curr.ToString() != String.Empty)
                {
                    _StatusDetailCurrentID = curr.ToString();
                }
                else
                {
                    _StatusDetailCurrentID = "0";
                }
            }
            else
            {
                _StatusDetailCustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }

        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Status_LoadDetail]", CommandType.StoredProcedure,
                  "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            return Content(Comon.DataJson.Datatable(dt));
        }

        public async Task<IActionResult> OnPostLoadInitializeData()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Service_Combo_Load", CommandType.StoredProcedure);
                    dt.TableName = "SpecifiedService";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_Product_Combo_Product_NotMedicine", CommandType.StoredProcedure);
                    dt.TableName = "ProductSupport";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.LoadEmployee(14, user.sys_branchID, user.sys_AllBranchID);
                    dt.TableName = "Doc";
                    ds.Tables.Add(dt);

                    DataTable dt1 = new DataTable();
                    dt1 = await confunc.LoadEmployee(17, user.sys_branchID, user.sys_AllBranchID);
                    dt1.TableName = "Assist";
                    ds.Tables.Add(dt1);

                    DataTable dtAll = new DataTable();
                    dtAll = dt.Copy();
                    dtAll.Merge(dt1);

                    dtAll.TableName = "Tech";
                    ds.Tables.Add(dtAll);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Tab_Service_Load]", CommandType.StoredProcedure,
                         "@Branch", SqlDbType.Int, user.sys_branchID
                        );
                    dt.TableName = "ServiceTab";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerStatus_LoadType]", CommandType.StoredProcedure);
                    dt.TableName = "TypeStatus";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }



        public async Task<IActionResult> OnPostExcuteData(string data, string CustomerID, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                StatusDetail DataMain = JsonConvert.DeserializeObject<StatusDetail>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("YYY_sp_Customer_Status_Insert", CommandType.StoredProcedure,
                            "@Customer_ID", SqlDbType.Int, CustomerID,
                            "@OriginalCondition", SqlDbType.NVarChar, DataMain.OriginalCondition,
                            "@SpecifiedService", SqlDbType.NVarChar, DataMain.SpecifiedService,
                            "@NumberSessions", SqlDbType.Int, DataMain.NumberSessions,
                            "@ProductSupport", SqlDbType.NVarChar, DataMain.ProductSupport,
                            "@ServiceSupport", SqlDbType.NVarChar, DataMain.ServiceSupport,
                            "@ServiceIncurred", SqlDbType.NVarChar, DataMain.ServiceIncurred,
                            "@Effective", SqlDbType.NVarChar, DataMain.Effective,
                            "@Consultants", SqlDbType.Int, DataMain.Consultants,
                            "@Responsible", SqlDbType.Int, DataMain.Responsible,
                            "@Tech_ID", SqlDbType.Int, DataMain.Tech_ID,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@BranchID", SqlDbType.Int, user.sys_branchID,
                            "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@TypeID", SqlDbType.Int, DataMain.TypeStatus_ID
                        );

                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("YYY_sp_Customer_Status_Update", CommandType.StoredProcedure,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@IDcurrent", SqlDbType.Int, CurrentID,
                            "@OriginalCondition", SqlDbType.NVarChar, DataMain.OriginalCondition,
                            "@SpecifiedService", SqlDbType.NVarChar, DataMain.SpecifiedService,
                            "@NumberSessions", SqlDbType.Int, DataMain.NumberSessions,
                            "@ProductSupport", SqlDbType.NVarChar, DataMain.ProductSupport,
                            "@ServiceSupport", SqlDbType.NVarChar, DataMain.ServiceSupport,
                            "@ServiceIncurred", SqlDbType.NVarChar, DataMain.ServiceIncurred,
                            "@Effective", SqlDbType.NVarChar, DataMain.Effective,
                            "@Consultants", SqlDbType.Int, DataMain.Consultants,
                            "@Responsible", SqlDbType.Int, DataMain.Responsible,
                            "@Tech_ID", SqlDbType.Int, DataMain.Tech_ID,
                            "@BranchID", SqlDbType.Int, user.sys_branchID,
                            "@TypeID", SqlDbType.Int, DataMain.TypeStatus_ID

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
    }
    public class StatusDetail
    {
        public string SpecifiedService { get; set; }
        public string OriginalCondition { get; set; }
        public string ProductSupport { get; set; }
        public string ServiceSupport { get; set; }
        public string Effective { get; set; }
        public int Customer_ID { get; set; }
        public int Consultants { get; set; }
        public int Responsible { get; set; }
        public int Tech_ID { get; set; }
        public int NumberSessions { get; set; }
        public string ServiceIncurred { get; set; }
        public int TypeStatus_ID { get; set; }
    }
}
