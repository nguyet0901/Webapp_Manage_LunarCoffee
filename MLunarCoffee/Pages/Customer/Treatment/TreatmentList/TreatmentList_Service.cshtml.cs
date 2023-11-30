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

namespace MLunarCoffee.Pages.Customer.Treatment.TreatmentList
{

    public class TreatmentList_ServiceModel : PageModel
    {
        public int _keyTreamentPercent { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public string _PatientRecordID { get; set; }
        public int _sys_Patient_Record { get; set; }
        public int _sys_Treatment_Plan { get; set; }
        public int _ViewAll { get; set; }
        public void OnGet()
        {
            _sys_Patient_Record = Comon.Global.sys_Patient_Record;
            _sys_Treatment_Plan = Comon.Global.sys_Treatment_Plan;
            //var v = Request.Query["CustomerID"];
            var viewAll = Request.Query["ViewAll"];
            var PatientRecord = Request.Query["PatientRecordID"];
            if (PatientRecord.ToString() != String.Empty)
            {
                _PatientRecordID = PatientRecord;
            }
            else
            {
                _PatientRecordID = "0";
            }
            _ViewAll = (viewAll.ToString() != String.Empty ? Convert.ToInt32(viewAll.ToString()) : 0);
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }



        public async Task<IActionResult> OnPostLoadComboMain(int CustomerID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt2 = new DataTable();
                    dt2 = await confunc.ExecuteDataTable("[MLU_sp_Customer_Treatment_PatientRecord_LoadCombo]", CommandType.StoredProcedure
                        , "@CustomerID", SqlDbType.Int, CustomerID);
                    dt2.TableName = "PatientRecord";
                    ds.Tables.Add(dt2);
                }
                //using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //{
                //    DataTable dt1 = new DataTable();
                //    dt1 = await confunc.ExecuteDataTable("[MLU_sp_ServiceStage_Load]", CommandType.StoredProcedure);
                //    dt1.TableName = "ServiceStage";
                //    ds.Tables.Add(dt1);
                //}
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_TreatmentPlan_LoadCombo]", CommandType.StoredProcedure
                        , "@CustomerID", SqlDbType.Int, CustomerID);
                    dt.TableName = "TreatmentPlan";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Treatment_TabService_LoadCombo]", CommandType.StoredProcedure
                        , "@CustomerID", SqlDbType.Int, CustomerID);
                    dt.TableName = "ServiceTab";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostLoadataTreatment(int CustomerID, int PatientRecordID, int TreatmentPlanID, int ServiceTabID, int idbegin, int idbeginless, int limit)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_v2_Customer_Treatment_LoadList]", CommandType.StoredProcedure,
                      "@Customer_ID", SqlDbType.Int, CustomerID
                      , "@UserId", SqlDbType.Int, user.sys_userid
                      , "@PatientRecordID", SqlDbType.Int, PatientRecordID
                      , "@TreatmentPlanID", SqlDbType.Int, TreatmentPlanID
                      , "@ServiceTabID", SqlDbType.Int, ServiceTabID
                      , "@idbegin", SqlDbType.Int, idbegin
                      , "@idbeginless", SqlDbType.Int, idbeginless
                      , "@limit", SqlDbType.Int, limit
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem_Treat_Record(int id, string CustomerId)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_TreatmentLess_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@CustomerId",SqlDbType.Int, Convert.ToInt32(CustomerId)
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem_Treat_Service(int id, int tabid, int dencos, string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_v3_Customer_Treatment_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@TabID", SqlDbType.Int, tabid,
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@dencos", SqlDbType.Int, dencos,
                        "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception)
            {
                return Content("-1");
            }
        }
        public async Task<IActionResult> OnPostGetSign_Treatment(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Treatment_Get_Sign]", CommandType.StoredProcedure,
                        "@TreatID", SqlDbType.Int, id
                    );
                    return Content(Comon.DataJson.Datatable(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostUpdateSignTreatment(int id, string sign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Customer_Treatment_Update_Sign]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@Sign", SqlDbType.NVarChar, sign
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostUpdateSignAllday(int id, int customerid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Customer_Treatment_Update_SignAllDay]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@CustomerID", SqlDbType.Int, customerid,
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
