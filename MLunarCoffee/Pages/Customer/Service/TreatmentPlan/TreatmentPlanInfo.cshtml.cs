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

namespace MLunarCoffee.Pages.Customer.Service.TreatmentPlan
{

    public class TreatmentPlanInfoModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _TreatmentPlanID { get; set; }
        public string _PatientRecordID { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var plan = Request.Query["TreatmentPlanID"];
            var patient = Request.Query["PatientRecordID"];
            if (cus.ToString() != String.Empty)
            {
                _CustomerID = cus.ToString();
                if (plan.ToString() != String.Empty)
                {
                    _TreatmentPlanID = plan.ToString();
                }
                else
                {
                    _TreatmentPlanID = "0";
                }
                if (patient.ToString() != String.Empty)
                {
                    _PatientRecordID = patient.ToString();
                }
                else
                {
                    _PatientRecordID = "0";
                }

            }
            else
            {
                _CustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostLoad_Chart_Teeth_Detail(string CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Patient_Record_Load_Teeth_Detail]", CommandType.StoredProcedure
                        , "@StatusTeethID", SqlDbType.Int, Convert.ToInt32(CurrentID));
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadata(int TreatmentPlanID, int PatientRecordID, int CustomeID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    var user = Session.GetUserSession(HttpContext);
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_Treatment_Plan_LoadInfo]", CommandType.StoredProcedure,
                          "@TreatmentPlanID", SqlDbType.Int, Convert.ToInt32(TreatmentPlanID)
                          , "@PatientRecordID", SqlDbType.Int, Convert.ToInt32(PatientRecordID)
                          , "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomeID)
                          , "@BranchID", SqlDbType.Int, Convert.ToInt32(user.sys_branchID == 0 ? 0 : user.sys_branchID));
                }
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ds.Tables[4].Rows[i]["link"] = String.Format(@"{0}{1}/{2}/{3}", Comon.Global.sys_HTTPImageRoot, ds.Tables[4].Rows[i]["CustomerID"], ds.Tables[4].Rows[i]["FolderName"], ds.Tables[4].Rows[i]["RealName"]);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string CurrentID, string name, string dateBegin, string dateEnd, string note)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Tab_Treatment_Plan_Update]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID,
                        "@Name", SqlDbType.NVarChar, name,
                        "@DateBegin", SqlDbType.NVarChar, (dateBegin != null) ? dateBegin : "",
                        "@DateEnd", SqlDbType.NVarChar, (dateEnd != null) ? dateEnd : "",
                        "@Note", SqlDbType.NVarChar, note,
                        "@Created_By", SqlDbType.Int, user.sys_userid
                    );
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
