using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.Traffic
{
    public class TrafficGeneralModel : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branchID { get; set; }
        public string _sourceID { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            _dateFrom = Request.Query["dateFrom"].ToString();
            _dateTo = Request.Query["dateTo"].ToString();
            _branchID = Request.Query["branch"].ToString();
            _sourceID = Request.Query["SourceID"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }
        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Appointment_LoadType", CommandType.StoredProcedure);
                        dt.TableName = "Type";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Scheduler_Type_LoadList", CommandType.StoredProcedure);
                        dt.TableName = "SchedulerType";
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


        public async Task<IActionResult> OnPostLoadata(string branchID, string dateFrom, string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[MLU_sp_Report_TrafficGeneral]", CommandType.StoredProcedure
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID));

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
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadataDetail(string branchID, string date, string dateFrom, string dateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Report_TrafficGeneral_Detail]", CommandType.StoredProcedure
                       , "@Date", SqlDbType.DateTime, date == "0" ? "1900-01-01" : Comon.Comon.DateTimeDMY_To_YMD(date)
                       , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                       , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                       , "@BranchID", SqlDbType.Int, Convert.ToInt32(branchID));
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
            catch (Exception)
            {
                return Content("0");
            }
        }

    }
}
