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

namespace MLunarCoffee.Pages.Customer.Service.TabList
{
    public class TabList_ServiceModel : PageModel
    {
        public string _TabCustomerID { get; set; }
        public string _KeyHistoryPrepareBefore { get; set; }

        public int _UsingConfirmService { get; set; }
        public int _UserCurrent { get; set; }
        public int _sys_BlockService_Permission { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public int _sys_Treatment_Plan { get; set; }
        public int _sys_Patient_Record { get; set; }
        public int _ViewAll { get; set; }
        public async Task OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            var v = Request.Query["CustomerID"];
            var viewAll = Request.Query["ViewAll"];
            _UsingConfirmService = user.sys_UsingConfirmService;
            _UserCurrent = user.sys_userid;
            _sys_BlockService_Permission = 1;

            _sys_Treatment_Plan = Comon.Global.sys_Treatment_Plan;
            _sys_Patient_Record = Comon.Global.sys_Patient_Record;

            if (v.ToString() != String.Empty)
            {
                _TabCustomerID = v.ToString();
                _ViewAll = (viewAll.ToString() != String.Empty ? Convert.ToInt32(viewAll.ToString()) : 0);
            }

            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }



        public async Task<IActionResult> OnPostLoadInitialize(int CustomerID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_v2_Customer_Tab_Treatment_Plan_Status_Load", CommandType.StoredProcedure);
                    dt.TableName = "Status";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("YYY_sp_v2_Customer_PatientRecord_TabService_LoadList", CommandType.StoredProcedure
                        , "@Customer_ID", SqlDbType.Int, CustomerID);
                    dt.TableName = "PatientRecord";
                    ds.Tables.Add(dt);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        #region // Service

        public async Task<IActionResult> OnPostLoadataTab(int CustomerID, int Record, int Plan, int ViewAll)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_v2_Customer_TabService_LoadList", CommandType.StoredProcedure,
                      "@Customer_ID", SqlDbType.Int, CustomerID
                      , "@Record", SqlDbType.Int, Record
                      , "@Plan", SqlDbType.Int, Plan
                      , "@UserId", SqlDbType.Int, user.sys_userid
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                      , "@DisableService_Permission", SqlDbType.Int, user.sys_DisableActionButton
                       , "@tab_show_full", SqlDbType.Int, ViewAll);
                }

                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadInfo_Treatment_Plant(string custID, string plantid)
        {
            try
            {
                DataSet ds = new DataSet();
                if (custID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        ds = await connFunc.ExecuteDataSet("YYY_sp_Customer_TreatmentPlan_Info", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, custID
                            , "@TreamentPlant", SqlDbType.Int, plantid
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


        public async Task<IActionResult> OnPostTabService_Change_Choose(string data, string customerid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable data_table = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_TabService_Change_Choose]", CommandType.StoredProcedure,
                           "@CustomerID", SqlDbType.Int, customerid
                         , "@UserID", SqlDbType.Int, user.sys_userid
                         , "@table_data", SqlDbType.Structured, (data_table.Rows.Count > 0 ? data_table : null)
                    );
                }
                if (dt != null && dt.Rows.Count > 0)
                    return Content(Comon.DataJson.Datatable(dt));
                return Content(Comon.DataJson.Datatable(data_table));
            }
            catch (Exception ex)
            {
                return Content(data);
            }
        }


        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_TabService_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLockAndUnlockTab(int id, int type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_TabService_ChangeLock]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Type", SqlDbType.Int, type,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadServiceTab()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                DataSet ds = new DataSet();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Tab_Service_Load]", CommandType.StoredProcedure,
                         "@Branch", SqlDbType.Int, user.sys_branchID);
                        dt.TableName = "Service";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Tab_Service_Type]", CommandType.StoredProcedure);
                        dt.TableName = "ServiceType";
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

        public async Task<IActionResult> OnPostDisableAndUndisableTab(int id, int type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_TabService_ChangeDisable]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Type", SqlDbType.Int, type,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostFinishTab(int id, int type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_TabService_ChangeFinish]", CommandType.StoredProcedure,
                         "@CurrentID", SqlDbType.Int, id,
                         "@Type", SqlDbType.Int, type,
                         "@userID", SqlDbType.Int, user.sys_userid
                     );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        #endregion

        #region // Patient Record


        public async Task<IActionResult> OnPostPatient_Record_Checking_Button(int RecordID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_v2_Customer_PatientRecord_TabService_Checking_Button", CommandType.StoredProcedure,
                        "@RecordID", SqlDbType.Int, RecordID
                      , "@UserId", SqlDbType.Int, user.sys_userid
                      , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                    );
                    return Content(Comon.DataJson.Datatable(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        //public async Task<IActionResult> OnPostIsClosePatientRecord(int PatientRecordID, int Status)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //        {
        //            dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_PatientRecord_IsClose]", CommandType.StoredProcedure,
        //                "@PatientRecordID", SqlDbType.Int, PatientRecordID,
        //                "@StatusIsClose", SqlDbType.Int, Status
        //            );
        //            return Content(Comon.DataJson.GetValueRowProperty(dt, 0, "RESULT"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("-1");
        //    }
        //}


        #endregion

        #region // Treatment plant


        //public async Task<IActionResult> OnPostLoad_Treatment_Plan_Compare(int CustomerID, string treatment_plan)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt = await confunc.ExecuteDataTable("YYY_sp_v2_Customer_Tab_Treatment_Plan_LoadList_Compare", CommandType.StoredProcedure,
        //              "@CustomerID", SqlDbType.Int, CustomerID
        //              , "@Treatment_String", SqlDbType.NVarChar, treatment_plan);
        //        }

        //        if (dt != null)
        //        {
        //            return Content(Comon.DataJson.Datatable(dt));
        //        }
        //        else
        //        {
        //            return Content("");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("");
        //    }
        //}


        //public async Task<IActionResult> OnPostTreatment_Plan_Checking_Button(int Tretment_plant_id)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt = await confunc.ExecuteDataTable("YYY_sp_v2_Customer_Tab_Treatment_Plan_Checking_Button", CommandType.StoredProcedure,
        //              "@Tretment_plant_id", SqlDbType.Int, Tretment_plant_id
        //              , "@UserID", SqlDbType.Int, user.sys_userid
        //              , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
        //        }

        //        if (dt != null)
        //        {
        //            return Content(Comon.DataJson.Datatable(dt));
        //        }
        //        else
        //        {
        //            return Content("");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("");
        //    }
        //}

        public async Task<IActionResult> OnPostIsCloseTreatmentPlan(int TreatmentPlanID, int Status)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt =  await connFunc.ExecuteDataTable("[YYY_sp_Customer_Treatment_Plan_IsClose]", CommandType.StoredProcedure,
                        "@TreatmentPlanID", SqlDbType.Int, TreatmentPlanID,
                        "@StatusIsClose", SqlDbType.Int, Status
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDelete_Treatment_Plan(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Treatment_Plan_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid);
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostClone_Treatment_Plan(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_v2_Customer_Treatment_Plan_Clone]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@BranchID", SqlDbType.Int, user.sys_branchID
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        //public async Task<IActionResult> OnPostChooseTreatmentPlan(int TreatmentPlanID)
        //{
        //    try
        //    {
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //        {
        //            await connFunc.ExecuteDataTable("[YYY_sp_Customer_Treatment_Plan_Choose]", CommandType.StoredProcedure,
        //                "@TreatmentPlanID", SqlDbType.Int, TreatmentPlanID,
        //                "@UserID", SqlDbType.Int, user.sys_userid
        //            );
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}


        public async Task<IActionResult> OnPostLoadImgTreatPlan(int CustomerID, int TreatPlanID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Treatment_Plan_LoadFolder]", CommandType.StoredProcedure,
                      "@CustomerID", SqlDbType.Int, CustomerID,
                      "@TreatPlanID", SqlDbType.Int, TreatPlanID
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }



        //public async Task<IActionResult> OnPostUpdateSign_Plant(int id, string sign)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //        {
        //            dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Treatment_UpdateSign]", CommandType.StoredProcedure,
        //                "@CurrentID", SqlDbType.Int, id,
        //                "@Sign", SqlDbType.NVarChar, sign
        //            );
        //        }
        //        return Content(Comon.DataJson.GetFirstValue(dt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}
        #endregion

        #region // Treatment plant status


        public async Task<IActionResult> OnPostTreatment_Plan_Status_Exchange(string StatusID, string TreatmentPlantID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_v2_Customer_Tab_Treatment_Plan_Status_Exchange", CommandType.StoredProcedure,
                            "@StatusID", SqlDbType.Int, StatusID,
                            "@TreatmentPlantID", SqlDbType.Int, TreatmentPlantID,
                            "@UserId", SqlDbType.Int, user.sys_userid
                        );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        #endregion
    }
}
