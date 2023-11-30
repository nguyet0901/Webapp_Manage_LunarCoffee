using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Customer.Schedule
{
    public class ScheduleStatusHistoryModel : PageModel
    {
        public string _ScheduleID { get; set; }
        public void OnGet()
        {
            string scheduleid = Request.Query["ScheduleID"];
            _ScheduleID = scheduleid != "" ? scheduleid : "0";
        }

        public async Task<IActionResult> OnPostLoadData(string ScheduleID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ScheduleStatus_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "Status";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataSet dsSchedule = new DataSet();
                    dsSchedule = await confunc.ExecuteDataSet("[MLU_sp_Customer_ScheduleStatus_LoadList]", CommandType.StoredProcedure,
                        "@ScheduleID" , SqlDbType.Int, Convert.ToInt32(ScheduleID));
                    if(dsSchedule != null)
                    {
                        dt = dsSchedule.Tables[0].Copy();
                        dt.TableName = "Info";
                        ds.Tables.Add(dt);

                        dt = dsSchedule.Tables[1].Copy();
                        dt.TableName = "StatusSchedule";
                        ds.Tables.Add(dt);
                    }
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

    }
}
