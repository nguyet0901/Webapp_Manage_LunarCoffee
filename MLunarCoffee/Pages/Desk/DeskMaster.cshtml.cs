using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Desk
{
    public class DeskMasterModel : PageModel
    {
    
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";

        }

        public async Task<IActionResult> OnPostLoadataDeskMaster(int GroupID)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Desk_Manage_LoadByGroup]", CommandType.StoredProcedure
                    , "@GroupID", SqlDbType.Int, GroupID);
            }
            if (ds != null)
            {
                return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadataScheduleIndicator(int BranchID, int DoctorID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Desk_Manage_Get_Point]", CommandType.StoredProcedure
                    , "@BranchID", SqlDbType.Int, BranchID
                    , "@DoctorID", SqlDbType.Int, DoctorID
                    );
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadApp(int AppID, int DoctorID, int StatusID, int BranchID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_DeskAppointment]", CommandType.StoredProcedure
                    , "@AppID", SqlDbType.Int, AppID
                    , "@DoctorID", SqlDbType.Int, DoctorID
                     , "@StatusID", SqlDbType.Int, StatusID
                    , "@BranchID", SqlDbType.Int, BranchID);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoad_Invidual_Appointment_InDay_Cashier(int BranchID, int AppID)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Appointment_Inday]", CommandType.StoredProcedure
                    , "@AppID", SqlDbType.Int, AppID
                    , "@Is_Cashier", SqlDbType.Int, 1
                    , "@BranchID", SqlDbType.Int, BranchID
                    , "@DateFrom", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
                    );
            }
            if (ds != null)
            {
                return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoad_Invidual_Appointment_InDay_Xquang(int BranchID, int AppID)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Appointment_Inday]", CommandType.StoredProcedure
                    , "@AppID", SqlDbType.Int, AppID
                    , "@Is_Xquang", SqlDbType.Int, 1
                    , "@BranchID", SqlDbType.Int, BranchID
                    , "@DateFrom", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
                    );
            }
            if (ds != null)
            {
                return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoad_Invidual_Appointment_Doctor(int BranchID, int DoctorID, int AppID)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Appointment_Inday]", CommandType.StoredProcedure
                    , "@AppID", SqlDbType.Int, AppID
                    , "@DoctorID", SqlDbType.Int, DoctorID
                    , "@BranchID", SqlDbType.Int, BranchID
                    , "@DateFrom", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
                    );
            }
            if (ds != null)
            {
                return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
                return Content("[]");
            }
        }
    }
}
