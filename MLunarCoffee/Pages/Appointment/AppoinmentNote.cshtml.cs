using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Appointment
{
    public class AppoinmentNoteModel : PageModel
    { 
        public string _CurrentID { get; set; }
        public string _descre { get; set; }
        public void OnGet()
        { 
            string curr = Request.Query["CurrentID"];
            string descre = Request.Query["descre"];
            if (curr != null)
            {
                _CurrentID = curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
            if (descre != null)
            {
                _descre = descre.ToString();
            }
            else
            {
                _descre = "0";
            }
        }
         
         public async Task<IActionResult> OnPostLoadAppointmentNote(string appID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (appID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt =await connFunc.ExecuteDataTable("MLU_sp_Customer_Appointment_LoadNote", CommandType.StoredProcedure,
                            "@appID", SqlDbType.Int, appID
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
         
         public async Task<IActionResult> OnPostLoadAppointmentTeamplate()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("MLU_sp_Customer_Appointment_LoadQuickTeamplate", CommandType.StoredProcedure
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
         
         public async Task<IActionResult> OnPostExcuteData(string _Content, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    return Content("0");
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[MLU_sp_Customer_Appointment_Note]", CommandType.StoredProcedure,
                        "@AppID", SqlDbType.Int, CurrentID,
                        "@Content", SqlDbType.NVarChar, _Content,
                        "@Created_By", SqlDbType.Int, user.sys_userid);
                    }
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Content("0");
            }
        }
    }
}
