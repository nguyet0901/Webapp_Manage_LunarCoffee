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

namespace MLunarCoffee.Pages.Customer.PatientRecord
{

    public class PatientRecordDetailModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _CurrentID { get; set; }
        public string _DataComboPatientRecord { get; set; }
        public string _DataComboStatusTeeth { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["PatientRecordID"];
            if (cus.ToString() != String.Empty)
            {
                _CustomerID = cus.ToString();
                _CurrentID = curr.ToString() != String.Empty ? curr.ToString() : "0";
            }
            else
            {
                _CustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }
        }

        public async Task<IActionResult> OnPostLoadInitializeData(string customerID)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[MLU_sp_PatientRecord_LoadCombo]", CommandType.StoredProcedure
                    , "@Customer", SqlDbType.Int, Convert.ToInt32(customerID));
                if (dt != null)
                {
                    dt.TableName = "ComboPatientRecord";
                    ds.Tables.Add(dt);
                }
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_StatusTeeth_LoadCombo]", CommandType.StoredProcedure
                     , "@customerID", SqlDbType.Int, Convert.ToInt32(customerID));
                if (dt != null)
                {
                    dt.TableName = "ComboStatusTeeth";
                    ds.Tables.Add(dt);
                }
            }
            return Content(Comon.DataJson.Dataset(ds));
        }

        public async Task<IActionResult> OnPostLoadForm(int formID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_print_v2_GetDetail]", CommandType.StoredProcedure
                        , "@templateid", SqlDbType.Int, Convert.ToInt32(formID));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        //public async Task<IActionResult> OnPostLoad_Chart_Teeth_Detail(string CurrentID)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            ds = await confunc.ExecuteDataSet("[MLU_sp_Patient_Record_Load_Teeth_Detail]", CommandType.StoredProcedure
        //                , "@StatusTeethID", SqlDbType.Int, Convert.ToInt32(CurrentID));
        //        }
        //        if (ds != null)
        //        {
        //            return Content(Comon.DataJson.Dataset(ds));
        //        }
        //        else
        //        {
        //            return Content("[]");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("[]");
        //    }
        //}

        public async Task<IActionResult> OnPostLoadata(int CurrentID, int CustomerID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    var user = Session.GetUserSession(HttpContext);
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_Patient_Record_Detail]", CommandType.StoredProcedure,
                        "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID)
                      , "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID == 0 ? 0 : CustomerID)
                      , "@BranchID", SqlDbType.Int, Convert.ToInt32(user.sys_branchID == 0 ? 0 : user.sys_branchID));
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, string CustomerID)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                PatientDetail DataMain = JsonConvert.DeserializeObject<PatientDetail>(data);
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_PatientRecord_Insert]", CommandType.StoredProcedure,
                            "@Customer_ID", SqlDbType.Int, CustomerID,
                            "@TemplateID", SqlDbType.Int, DataMain.TemplateID,
                            "@StatusTeethID", SqlDbType.Int, DataMain.StatusTeethID,
                            "@branch_ID", SqlDbType.Int, user.sys_branchID,
                            "@DataTemplate", SqlDbType.NVarChar, DataMain.DataTemplate.Replace("'", "").Trim(),
                            "@Created_By", SqlDbType.Int, user.sys_userid
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_PatientRecord_Update]", CommandType.StoredProcedure,
                             "@DataTemplate", SqlDbType.NVarChar, DataMain.DataTemplate.Replace("'", "").Trim(),
                             "@StatusTeethID", SqlDbType.Int, DataMain.StatusTeethID,
                             "@CurrentID", SqlDbType.Int, CurrentID,
                             "@Modified_By", SqlDbType.Int, user.sys_userid
                             , "@DataAnswer", SqlDbType.NVarChar, DataMain.DataAnswer
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

        public async Task<IActionResult> OnPostExcuteDelete(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_PatientRecord_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                if (dt.Rows.Count > 0)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadDataValue(int commandid, int id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_print_v2_ExeCommand]", CommandType.StoredProcedure,
                      "@commandid", SqlDbType.Int, Convert.ToInt32(commandid)
                      , "@id", SqlDbType.Int, Convert.ToInt32(id)
                      , "@idstring", SqlDbType.NVarChar, "");
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }

            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostCheckStogeSign(string CustomerID, string StogeRule)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_PrintForm_StogeRule_Check]", CommandType.StoredProcedure,
                      "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                      , "@StogeRule", SqlDbType.Int, Convert.ToInt32(StogeRule)
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostSaveStogeSign(string CustomerID, string StogeSign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataSet("[MLU_sp_PrintForm_StogeRule_Insert]", CommandType.StoredProcedure,
                      "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                      , "@StogeRule", SqlDbType.Int, Convert.ToInt32(StogeSign)
                      , "@CreatedBy", SqlDbType.Int, user.sys_userid
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

    public class PatientDetail
    {
        public int TemplateID { get; set; }
        public int LaboServiceID { get; set; }
        public string DataTemplate { get; set; }
        //public string DataAnswer { get; set; }
        public string StatusTeethID { get; set; }
        public string DataAnswer { get; set; }
    }
}
