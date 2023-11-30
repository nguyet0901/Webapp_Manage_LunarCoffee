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
using MLunarCoffee.Models.Model.AlrmScheduler;

namespace MLunarCoffee.Pages.ToDo.AlarmSchedule
{
    public class MyAlarmSchedulerModel : PageModel
    {
        public void OnGet()
        {
        }
        public string _CurrentEmployeeID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Session.GetUserSession(HttpContext);
            _CurrentEmployeeID = user.sys_employeeid.ToString();
        }


        
         public async Task<IActionResult> OnPostLoadScheduler(int empid, string dateFrom, string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[YYY_sp_Todo_AlarmSchedule_Load_List]", CommandType.StoredProcedure,
                        "@CreateadBy", SqlDbType.Int, user.sys_userid,
                        "@Employee", SqlDbType.Int, empid,
                        "@dateFrom", SqlDbType.DateTime, dateFrom,
                        "@dateTo", SqlDbType.DateTime, dateTo
                    );
                }
                AlrmScheduler[] timeline = Comon.Comon.GetArrayTimeLine_AlarmScheduler(ds.Tables[0]);
                AlrmScheduler_Extension[] extension = Comon.Comon.GetArrayTimeLine_AlarmSchedulerExtension(ds.Tables[1]);

                DataTable result = new DataTable();
                result.Columns.Add("ID", typeof(String));// ID cua alram
                result.Columns.Add("Date_From", typeof(DateTime));
                result.Columns.Add("Date_To", typeof(DateTime));
                result.Columns.Add("Status", typeof(String)); // Status cua ex
                result.Columns.Add("Title", typeof(String)); // Title cua alram
                result.Columns.Add("Content", typeof(String)); //  Content cua alram
                result.Columns.Add("Hour", typeof(String));
                result.Columns.Add("Date", typeof(String));


                DateTime DateFrom = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateFrom"]);
                DateTime DateTo = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateTo"]);
                for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1.0))
                {
                    int dayofweek = (int)date.DayOfWeek;
                    int dayofDate = (int)date.Day;
                    // Tim trong timeline 
                    for (int i = 0; i < timeline.Length; i++)
                    {
                        int type = timeline[i].type;
                        int id = timeline[i].id;
                        int day = timeline[i].day;
                        string hour = timeline[i].hour;
                        int hourInt = Convert.ToInt32(hour.Split(':')[0]);
                        int hourMinute = Convert.ToInt32(hour.Split(':')[1]);
                        int havingInTimeline = 0;
                        int havingInEx = 0;
                        int StatusEx = 0;
                        string title = "";
                        string Content = "";

                        switch (type)
                        {
                            case 1: // ngay cu the
                                {
                                    if (date.ToString("yyyy-MM-dd") == timeline[i].date)
                                        havingInTimeline = 1;
                                }
                                break;
                            case 2: // Hang ngay
                                {
                                    if (date >= timeline[i].dateCreated) // ngay co thong bao nay, phai lon hon ngay tao cua alrm
                                        havingInTimeline = 1;
                                }
                                break;
                            case 3: // Thứ nào đó , hàng tuần
                                {
                                    if (date >= timeline[i].dateCreated && dayofweek == timeline[i].dayofweek) // ngay co thong bao nay, phai lon hon ngay tao cua alrm
                                        havingInTimeline = 1;
                                }
                                break;
                            case 4: // Ngày nào đó , hàng tháng
                                {
                                    if (date >= timeline[i].dateCreated && dayofDate == timeline[i].day) // ngay co thong bao nay, phai lon hon ngay tao cua alrm
                                        havingInTimeline = 1;
                                }
                                break;
                            default: break;
                        }

                        if (havingInTimeline == 1)
                        {
                            title = timeline[i].title;
                            Content = timeline[i].content;
                            AlrmScheduler_Extension _ex = CheckInExtension(extension, id, date);
                            if (_ex != null)
                            {
                                havingInEx = 1;
                                StatusEx = _ex.status;

                            }

                            DataRow dr = result.NewRow();
                            DateTime dF = date.AddHours(hourInt).AddMinutes(hourMinute);
                            dr["ID"] = id;
                            dr["Date_From"] = dF;
                            dr["Date_To"] = AddOneHour(dF);
                            dr["Status"] = StatusEx;
                            dr["Title"] = title;
                            dr["Content"] = Content;
                            dr["Date"] = date.ToString("yyyy-MM-dd");
                            dr["Hour"] = hour;

                            result.Rows.Add(dr);
                        }
                    }
                }

                return Content( JsonConvert.SerializeObject(result));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        private static DateTime AddOneHour(DateTime dF)
        {
            if (dF.Hour < 23) return dF.AddHours(1);
            else return new DateTime(dF.Year, dF.Month, dF.Day, 23, 59, 59);

        }
        
         public async Task<IActionResult> OnPostOffScheduler(int alrmid, int status, string date)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Todo_AlarmSchedule_Update_Status]", CommandType.StoredProcedure,
                        "@alrmid", SqlDbType.Int, alrmid,
                         "@status", SqlDbType.Int, status,
                         "@Created_By", SqlDbType.Int, user.sys_userid,
                       "@Date", SqlDbType.NVarChar, date
                   );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        private static AlrmScheduler_Extension CheckInExtension(AlrmScheduler_Extension[] dt, int alarmID, DateTime date)
        {
            try
            {
                int foundid = -1;
                for (int i = 0; i < dt.Length; i++)
                {
                    if (dt[i].alarmID == alarmID && dt[i].date == date)
                    {
                        foundid = i;
                        i = dt.Length;
                    }
                }
                if (foundid != -1) return dt[foundid];
                else return null;

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
