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
using MLunarCoffee.Models.Model.WorkScheduler;


namespace MLunarCoffee.Pages.Employee
{
    public class WorkScheduleDetailModel : PageModel
    {
        public string _DateTimeCurrent { get; set; }
        public string _CurrentID { get; set; }
        public void OnGet()
        {

            string DateTimeCurrent = Request.Query["DateTimeCurrent"];
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {

                _DateTimeCurrent = DateTimeCurrent.ToString();
                _CurrentID = curr.ToString();
            }
            else Response.Redirect("/assests/Error/index.html");
        }
        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Shift_Combo]", CommandType.StoredProcedure);
                    dt.TableName = "dataShift";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    var user = Session.GetUserSession(HttpContext);
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "dataBranch";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception e)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadDataDetail(string empid, string datecurrent)
        {
           
            if (datecurrent != "0")
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                { 
                    
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Work_Schedule_LoadExtention]"
                        , CommandType.StoredProcedure,
                        "@EmpID", SqlDbType.Int, Convert.ToInt32(empid)
                       , "@datecurrent", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(datecurrent + " 00:00"));
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                else return Content("");
            }
            else
            { 
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Work_Schedule_LoadUpdate]", CommandType.StoredProcedure,
                      "@EmpID", SqlDbType.Int, Convert.ToInt32(empid));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string empid, string date, string datecurrent)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (datecurrent != "0")
                {
                    WorkScheduler_Extension[] DataMain = JsonConvert.DeserializeObject<WorkScheduler_Extension[]>(data);
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                       dt= await connFunc.ExecuteDataTable("YYY_sp_Work_Schedule_Extension_Update", CommandType.StoredProcedure,
                            "@EmpID", SqlDbType.NVarChar, empid,
                            "@Date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(datecurrent + " 00:00"),
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Data", SqlDbType.NVarChar, JsonConvert.SerializeObject(DataMain)
                        );
                    }
                }
                else
                {
                    WorkScheduler[] DataMain = JsonConvert.DeserializeObject<WorkScheduler[]>(data);
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {

                        dt= await connFunc.ExecuteDataTable("YYY_sp_Work_Schedule_Update", CommandType.StoredProcedure,
                            "@EmpID", SqlDbType.NVarChar, empid,
                            "@Date", SqlDbType.DateTime, Comon.Comon.StringDMY_To_DateTime(date),
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Data", SqlDbType.NVarChar, JsonConvert.SerializeObject(DataMain)
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
