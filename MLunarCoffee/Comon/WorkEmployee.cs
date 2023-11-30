using MLunarCoffee.Models.Model.WorkScheduler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MLunarCoffee.Comon
{
    public class WorkEmployee
    {
        public static async Task<int> LoadMinuteWordInMonth( Microsoft.AspNetCore.Http.HttpContext httpContext,int empid, string dateLoad)
        {
            try
            {
                int minuteWord = 0;
                DataSet ds = new DataSet();
                var user = Session.Session.GetUserSession (httpContext);
                using (MLunarCoffee.Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds =await connFunc.ExecuteDataSet("[MLU_sp_WorK_Schedule_InMonth]", CommandType.StoredProcedure,
                        "@Employee", SqlDbType.Int, empid,
                        "@Date", SqlDbType.DateTime, Comon.DateTimeDMY_To_YMD(dateLoad)
                    );
                }
                WorkScheduler_TimeLine[] timeline = Comon.GetArrayTimeLine(ds.Tables[0]);
                WorkScheduler_TimeLine[] extension = Comon.GetArrayTimeLine(ds.Tables[1]);

                DataTable result = new DataTable();
                result.Columns.Add("Date_From", typeof(DateTime));
                result.Columns.Add("Date_To", typeof(DateTime));
                result.Columns.Add("Off", typeof(String));
                result.Columns.Add("Content", typeof(String));
                result.Columns.Add("Color", typeof(String));
                result.Columns.Add("Text", typeof(String));
                result.Columns.Add("Date", typeof(String));
                result.Columns.Add("Date_Sheduler", typeof(String)); // Ngay trong Cua VTT_Work_Schedule,neu la extension thi ngay nay la rong

                DateTime DateFrom = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateFrom"]);
                DateTime DateTo = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateTo"]);

                for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1.0))
                {
                    WorkScheduler[] ws = null;
                    bool Havingextension = false;
                    // Tim trong extension truoc
                    for (int i = 0; i < extension.Length; i++)
                    {
                        if (date == Convert.ToDateTime(extension[i].Date))
                        {
                            Havingextension = true;
                            ws = extension[i].workScheduler;
                            i = extension.Length;
                        }
                    }

                    ///////////////////////////////
                    if (Havingextension)
                    {
                        if (ws.Length == 1)// extension thi chi co 1 gia tri data
                        {
                            bool off = ws[0].off;
                            if (off) // off ngay hom do
                            {
                            }
                            else
                            {
                                shifts[] shift = ws[0].shift;
                                for (int i = 0; i < shift.Length; i++)
                                {
                                    DataTable dataTableShift = GetShiftDetail(ds.Tables[3], shift[i].shift);
                                    if (dataTableShift != null)
                                    {
                                        for (int j = 0; j < dataTableShift.Rows.Count; j++)
                                        {
                                            string fromHour = dataTableShift.Rows[j]["HourFrom"].ToString().Trim();
                                            int _hf = Convert.ToInt32(fromHour.Split(':')[0]);
                                            int _mf = Convert.ToInt32(fromHour.Split(':')[1]);

                                            string toHour = dataTableShift.Rows[j]["HourTo"].ToString().Trim();
                                            int _ht = Convert.ToInt32(toHour.Split(':')[0]);
                                            int _mt = Convert.ToInt32(toHour.Split(':')[1]);

                                            if (_ht > _hf)
                                                minuteWord = minuteWord + (_ht - _hf) * 60 - _mf + _mt;
                                            else
                                                minuteWord = minuteWord + ((24 - _hf) + _ht) * 60 - _mf + _mt;
                                        }
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        for (int i = timeline.Length - 1; i >= 0; i--)
                        {
                            if (date >= timeline[i].Date)
                            {
                                ws = timeline[i].workScheduler;
                                i = -1;
                            }
                        }
                        if (ws != null)
                        {
                            int dayofweek = (int)date.DayOfWeek;
                            bool off = false;
                            shifts[] shift = null;
                            for (int i = 0; i < ws.Length; i++)
                            {
                                if (ws[i].dayofweek == dayofweek.ToString())
                                {
                                    off = ws[i].off;
                                    shift = ws[i].shift;
                                    i = ws.Length;
                                }
                            }
                            if (shift != null && off == false)
                            {
                                for (int i = 0; i < shift.Length; i++)
                                {
                                    DataTable dataTableShift = GetShiftDetail(ds.Tables[3], shift[i].shift);
                                    if (dataTableShift != null)
                                    {
                                        for (int j = 0; j < dataTableShift.Rows.Count; j++)
                                        {
                                            string fromHour = dataTableShift.Rows[j]["HourFrom"].ToString().Trim();
                                            int _hf = Convert.ToInt32(fromHour.Split(':')[0]);
                                            int _mf = Convert.ToInt32(fromHour.Split(':')[1]);

                                            string toHour = dataTableShift.Rows[j]["HourTo"].ToString().Trim();
                                            int _ht = Convert.ToInt32(toHour.Split(':')[0]);
                                            int _mt = Convert.ToInt32(toHour.Split(':')[1]);
                                            if (_ht > _hf)
                                                minuteWord = minuteWord + (_ht - _hf) * 60 - _mf + _mt;
                                            else
                                                minuteWord = minuteWord + ((24 - _hf) + _ht) * 60 - _mf + _mt;
                                        }
                                    }

                                }

                                // _Content = shift[0].shift;

                                // logic here


                            }
                        }
                    }
                }

                return minuteWord;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static async Task<string> LoadScheduler( Microsoft.AspNetCore.Http.HttpContext httpContext, int empid, string dateFrom, string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.Session.GetUserSession ( httpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds =await connFunc.ExecuteDataSet("[MLU_sp_WorK_Schedule_LoadList]", CommandType.StoredProcedure,
                        "@EmployeeID", SqlDbType.Int, empid,
                        "@dateFrom", SqlDbType.DateTime, dateFrom,
                        "@dateTo", SqlDbType.DateTime, dateTo
                    );
                }
                WorkScheduler_TimeLine[] timeline = Comon.GetArrayTimeLine(ds.Tables[0]);
                WorkScheduler_TimeLine[] extension = Comon.GetArrayTimeLine(ds.Tables[1]);

                DataTable result = new DataTable();
                result.Columns.Add("Date_From", typeof(DateTime));
                result.Columns.Add("Date_To", typeof(DateTime));
                result.Columns.Add("Off", typeof(String));
                result.Columns.Add("Content", typeof(String));
                result.Columns.Add("Color", typeof(String));
                result.Columns.Add("Text", typeof(String));
                result.Columns.Add("BranchName", typeof(String));
                result.Columns.Add("Date", typeof(String));
                result.Columns.Add("Date_Sheduler", typeof(String)); // Ngay trong Cua VTT_Work_Schedule,neu la extension thi ngay nay la rong

                DateTime DateFrom = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateFrom"]);
                DateTime DateTo = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateTo"]);

                for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1.0))
                {
                    WorkScheduler[] ws = null;
                    bool Havingextension = false;
                    // Tim trong extension truoc
                    for (int i = 0; i < extension.Length; i++)
                    {
                        if (date == Convert.ToDateTime(extension[i].Date))
                        {
                            Havingextension = true;
                            ws = extension[i].workScheduler;
                            i = extension.Length;
                        }
                    }

                    ///////////////////////////////
                    if (Havingextension)
                    {
                        if (ws.Length == 1)// extension thi chi co 1 gia tri data
                        {
                            bool off = ws[0].off;
                            if (off) // off ngay hom do
                            {
                                DataRow dr = result.NewRow();
                                dr["Date_From"] = date;
                                dr["Date_To"] = date.AddHours(23);
                                dr["Off"] = "1";
                                dr["Color"] = "#7a1717";
                                dr["Content"] = "Off";
                                dr["Text"] = "Off";
                                dr["Date"] = date.ToString("yyyy-MM-dd");
                                dr["Date_Sheduler"] = "";
                                result.Rows.Add(dr);
                            }
                            else
                            {
                                shifts[] shift = ws[0].shift;
                                for (int i = 0; i < shift.Length; i++)
                                {
                                    DataTable dataTableShift = GetShiftDetail(ds.Tables[3], shift[i].shift);
                                    if (dataTableShift != null)
                                    {
                                        for (int j = 0; j < dataTableShift.Rows.Count; j++)
                                        {
                                            DataRow dr = result.NewRow();
                                            string fromHour = dataTableShift.Rows[j]["HourFrom"].ToString().Trim();
                                            int _hf = Convert.ToInt32(fromHour.Split(':')[0]);
                                            int _mf = Convert.ToInt32(fromHour.Split(':')[1]);

                                            string toHour = dataTableShift.Rows[j]["HourTo"].ToString().Trim();
                                            int _ht = Convert.ToInt32(toHour.Split(':')[0]);
                                            int _mt = Convert.ToInt32(toHour.Split(':')[1]);

                                            dr["Date_From"] = date.AddHours(_hf).AddMinutes(_mf);
                                            dr["Date_To"] = date.AddHours(_ht).AddMinutes(_mt);
                                            dr["Off"] = "0";
                                            dr["Color"] = dataTableShift.Rows[j]["ColorCode"];
                                            dr["Content"] = dataTableShift.Rows[j]["Code"];
                                            dr["Text"] = GetBranchCode(ds.Tables[4], shift[i].branch);
                                            dr["BranchName"] = GetBranchName(ds.Tables[4], shift[i].branch);
                                            dr["Date"] = date.ToString("yyyy-MM-dd");
                                            dr["Date_Sheduler"] = "";
                                            result.Rows.Add(dr);
                                        }
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        for (int i = timeline.Length - 1; i >= 0; i--)
                        {
                            if (date >= timeline[i].Date)
                            {
                                ws = timeline[i].workScheduler;
                                i = -1;
                            }
                        }
                        if (ws != null)
                        {
                            int dayofweek = (int)date.DayOfWeek;
                            bool off = false;
                            shifts[] shift = null;
                            for (int i = 0; i < ws.Length; i++)
                            {
                                if (ws[i].dayofweek == dayofweek.ToString())
                                {
                                    off = ws[i].off;
                                    shift = ws[i].shift;
                                    i = ws.Length;
                                }
                            }
                            if (shift != null && off == false)
                            {
                                for (int i = 0; i < shift.Length; i++)
                                {
                                    DataTable dataTableShift = GetShiftDetail(ds.Tables[3], shift[i].shift);
                                    if (dataTableShift != null)
                                    {
                                        for (int j = 0; j < dataTableShift.Rows.Count; j++)
                                        {
                                            DataRow dr = result.NewRow();
                                            string fromHour = dataTableShift.Rows[j]["HourFrom"].ToString().Trim();
                                            int _hf = Convert.ToInt32(fromHour.Split(':')[0]);
                                            int _mf = Convert.ToInt32(fromHour.Split(':')[1]);

                                            string toHour = dataTableShift.Rows[j]["HourTo"].ToString().Trim();
                                            int _ht = Convert.ToInt32(toHour.Split(':')[0]);
                                            int _mt = Convert.ToInt32(toHour.Split(':')[1]);

                                            dr["Date_From"] = date.AddHours(_hf).AddMinutes(_mf);
                                            dr["Date_To"] = date.AddHours(_ht).AddMinutes(_mt);
                                            dr["Off"] = "0";
                                            dr["Color"] = dataTableShift.Rows[j]["ColorCode"];
                                            dr["Content"] = dataTableShift.Rows[j]["Code"];
                                            dr["Text"] = GetBranchCode(ds.Tables[4], shift[i].branch);
                                            dr["BranchName"] = GetBranchName(ds.Tables[4], shift[i].branch);
                                            dr["Date"] = date.ToString("yyyy-MM-dd");
                                            //dr["Date_Sheduler"] = "";
                                            dr["Date_Sheduler"] = timeline[i].Date.ToString("yyyy-MM-dd");
                                            result.Rows.Add(dr);
                                        }
                                    }

                                }

                                // _Content = shift[0].shift;

                                // logic here


                            }
                        }
                    }
                }

                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public static async Task<DataTable> LoadScheduler_By_Doctor_Branch ( Microsoft.AspNetCore.Http.HttpContext httpContext, int empid, int branchid, string dateFrom, string dateTo)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.Session.GetUserSession (httpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_WorK_Schedule_LoadList]", CommandType.StoredProcedure,
                        "@EmployeeID", SqlDbType.Int, empid,
                        "@dateFrom", SqlDbType.DateTime, dateFrom,
                        "@dateTo", SqlDbType.DateTime, dateTo
                    );
                }
                WorkScheduler_TimeLine[] timeline = Comon.GetArrayTimeLine(ds.Tables[0]);
                WorkScheduler_TimeLine[] extension = Comon.GetArrayTimeLine(ds.Tables[1]);

                DataTable result = new DataTable();
                result.Columns.Add("Date_From", typeof(DateTime));
                result.Columns.Add("Date_To", typeof(DateTime));
                result.Columns.Add("Off", typeof(String));

                DateTime DateFrom = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateFrom"]);
                DateTime DateTo = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateTo"]);

                for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1.0))
                {
                    WorkScheduler[] ws = null;
                    bool Havingextension = false;
                    // Tim trong extension truoc
                    for (int i = 0; i < extension.Length; i++)
                    {
                        if (date == Convert.ToDateTime(extension[i].Date))
                        {
                            Havingextension = true;
                            ws = extension[i].workScheduler;
                            i = extension.Length;
                        }
                    }

                    ///////////////////////////////
                    if (Havingextension)
                    {
                        if (ws.Length == 1)// extension thi chi co 1 gia tri data
                        {
                            bool off = ws[0].off;
                            if (off) // off ngay hom do
                            {
                                DataRow dr = result.NewRow();
                                dr["Date_From"] = date;
                                dr["Date_To"] = date.AddHours(23);
                                dr["Off"] = "1";
                                result.Rows.Add(dr);
                            }
                            else
                            {
                                shifts[] shift = ws[0].shift;
                                for (int i = 0; i < shift.Length; i++)
                                {
                                    DataTable dataTableShift = GetShiftDetail(ds.Tables[3], shift[i].shift);
                                    if (Convert.ToInt32(shift[i].branch) == branchid )
                                    {
                                        if (dataTableShift != null)
                                        {
                                            for (int j = 0; j < dataTableShift.Rows.Count; j++)
                                            {
                                                DataRow dr = result.NewRow();
                                                string fromHour = dataTableShift.Rows[j]["HourFrom"].ToString().Trim();
                                                int _hf = Convert.ToInt32(fromHour.Split(':')[0]);
                                                int _mf = Convert.ToInt32(fromHour.Split(':')[1]);

                                                string toHour = dataTableShift.Rows[j]["HourTo"].ToString().Trim();
                                                int _ht = Convert.ToInt32(toHour.Split(':')[0]);
                                                int _mt = Convert.ToInt32(toHour.Split(':')[1]);

                                                dr["Date_From"] = date.AddHours(_hf).AddMinutes(_mf);
                                                dr["Date_To"] = date.AddHours(_ht).AddMinutes(_mt);
                                                dr["Off"] = "0";

                                                result.Rows.Add(dr);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        for (int i = timeline.Length - 1; i >= 0; i--)
                        {
                            if (date >= timeline[i].Date)
                            {
                                ws = timeline[i].workScheduler;
                                i = -1;
                            }
                        }
                        if (ws != null)
                        {
                            int dayofweek = (int)date.DayOfWeek;
                            bool off = false;
                            shifts[] shift = null;
                            for (int i = 0; i < ws.Length; i++)
                            {
                                if (ws[i].dayofweek == dayofweek.ToString())
                                {
                                    off = ws[i].off;
                                    shift = ws[i].shift;
                                    i = ws.Length;
                                }
                            }
                            if (shift != null && off == false)
                            {
                                for (int i = 0; i < shift.Length; i++)
                                {
                                    if (Convert.ToInt32(shift[i].branch) == branchid || Convert.ToInt32(shift[i].branch) == 0)
                                    {
                                        DataTable dataTableShift = GetShiftDetail(ds.Tables[3], shift[i].shift);
                                        if (dataTableShift != null)
                                        {
                                            for (int j = 0; j < dataTableShift.Rows.Count; j++)
                                            {
                                                DataRow dr = result.NewRow();
                                                string fromHour = dataTableShift.Rows[j]["HourFrom"].ToString().Trim();
                                                int _hf = Convert.ToInt32(fromHour.Split(':')[0]);
                                                int _mf = Convert.ToInt32(fromHour.Split(':')[1]);

                                                string toHour = dataTableShift.Rows[j]["HourTo"].ToString().Trim();
                                                int _ht = Convert.ToInt32(toHour.Split(':')[0]);
                                                int _mt = Convert.ToInt32(toHour.Split(':')[1]);

                                                dr["Date_From"] = date.AddHours(_hf).AddMinutes(_mf);
                                                dr["Date_To"] = date.AddHours(_ht).AddMinutes(_mt);
                                                dr["Off"] = "0";
                                                result.Rows.Add(dr);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = result.NewRow();
                                dr["Date_From"] = date;
                                dr["Date_To"] = date.AddHours(23);
                                dr["Off"] = "1";
                                result.Rows.Add(dr);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static async Task<DataTable> LoadSchedulerDoctor ( Microsoft.AspNetCore.Http.HttpContext httpContext, DateTime datefrom, DateTime dateto, string tokenDocID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.Session.GetUserSession (httpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds =await connFunc.ExecuteDataSet("[MLU_sp_Work_Schedule_Appointment_Doctor]", CommandType.StoredProcedure,
                          "@DateFrom", SqlDbType.DateTime, datefrom
                          , "@DateTo", SqlDbType.DateTime, dateto
                          , "@TokenDocID", SqlDbType.NVarChar, tokenDocID
                    );
                }
                WorkScheduler_TimeLine_Employee[] timeline = Comon.GetArrayTimeLineAllEmployee(ds.Tables[0]);
                WorkScheduler_TimeLine_Employee[] extension = Comon.GetArrayTimeLineAllEmployee(ds.Tables[1]);

                DataTable dtMain = ds.Tables[0].Copy();
                DataTable result = new DataTable();
                result.Columns.Add("EmpID", typeof(String));
                result.Columns.Add("Date_From", typeof(DateTime));
                result.Columns.Add("Date_To", typeof(DateTime));
                result.Columns.Add("BranchID", typeof(Int32));
                result.Columns.Add("Color", typeof(String));
                result.Columns.Add("Date", typeof(String));


                DateTime DateFrom = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateFrom"]);
                DateTime DateTo = Convert.ToDateTime(ds.Tables[2].Rows[0]["DateTo"]);

                int NumEmployee = Convert.ToInt32(ds.Tables[4].Rows.Count);

                for (int numEmp = 0; numEmp < NumEmployee; numEmp++)
                {
                    int EmpID = Convert.ToInt32(ds.Tables[4].Rows[numEmp]["EmpID"]);

                        string EmpName = ds.Tables[4].Rows[numEmp]["Name"].ToString();
                        for (DateTime date = DateFrom; date <= DateTo; date = date.AddDays(1.0))
                        {
                            WorkScheduler[] ws = null;
                            bool Havingextension = false;
                            // Tim trong extension truoc
                            for (int i = 0; i < extension.Length; i++)
                            {
                                if (date == Convert.ToDateTime(extension[i].Date) && EmpID == Convert.ToInt32(extension[i].EmployeeID))
                                {
                                    Havingextension = true;
                                    ws = extension[i].workScheduler;
                                    i = extension.Length;
                                }
                            }

                            ///////////////////////////////
                            if (Havingextension)
                            {
                                if (ws.Length == 1)// extension thi chi co 1 gia tri data
                                {
                                    bool off = ws[0].off;
                                    if (off) // off ngay hom do
                                    {
                                        DataRow dr = result.NewRow();
                                        dr["EmpID"] = EmpName + "__" + EmpID;
                                        dr["Date_From"] = date;
                                        dr["Date_To"] = date.AddHours(23);
                                        dr["BranchID"] = 0;
                                        dr["Color"] = "#7a1717";
                                        dr["Date"] = date.ToString("yyyy-MM-dd");
                                        result.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        shifts[] shift = ws[0].shift;
                                        for (int i = 0; i < shift.Length; i++)
                                        {
                                            DataTable dataTableShift = GetShiftDetail(ds.Tables[3], shift[i].shift);
                                            if (dataTableShift != null)
                                            {
                                                for (int j = 0; j < dataTableShift.Rows.Count; j++)
                                                {
                                                    DataRow dr = result.NewRow();
                                                    string fromHour = dataTableShift.Rows[j]["HourFrom"].ToString().Trim();
                                                    int _hf = Convert.ToInt32(fromHour.Split(':')[0]);
                                                    int _mf = Convert.ToInt32(fromHour.Split(':')[1]);

                                                    string toHour = dataTableShift.Rows[j]["HourTo"].ToString().Trim();
                                                    int _ht = Convert.ToInt32(toHour.Split(':')[0]);
                                                    int _mt = Convert.ToInt32(toHour.Split(':')[1]);
                                                    dr["EmpID"] = EmpName + "__" + EmpID;
                                                    dr["Date_From"] = date.AddHours(_hf).AddMinutes(_mf);
                                                    dr["Date_To"] = date.AddHours(_ht).AddMinutes(_mt);
                                                    dr["Color"] = dataTableShift.Rows[j]["ColorCode"];
                                                    dr["BranchID"] = shift[i].branch;
                                                    dr["Date"] = date.ToString("yyyy-MM-dd");
                                                    result.Rows.Add(dr);
                                                }
                                            }

                                        }

                                    }
                                }
                            }
                            else
                            {
                                for (int i = timeline.Length - 1; i >= 0; i--)
                                {
                                    if (date >= timeline[i].Date && EmpID == timeline[i].EmployeeID)
                                    {
                                        ws = timeline[i].workScheduler;
                                        i = -1;
                                    }
                                }
                                if (ws != null)
                                {
                                    int dayofweek = (int)date.DayOfWeek;
                                    bool off = false;
                                    shifts[] shift = null;
                                    for (int i = 0; i < ws.Length; i++)
                                    {
                                        if (ws[i].dayofweek == dayofweek.ToString())
                                        {
                                            off = ws[i].off;
                                            shift = ws[i].shift;
                                            i = ws.Length;
                                        }
                                    }
                                    Console.WriteLine(shift);
                                    if (shift != null && off == false)
                                    {
                                        for (int i = 0; i < shift.Length; i++)
                                        {
                                            if (shift[i].branch == "84")
                                            {
                                                DataTable dataTableShift = GetShiftDetail(ds.Tables[3], shift[i].shift);
                                                if (dataTableShift != null)
                                                {
                                                    for (int j = 0; j < dataTableShift.Rows.Count; j++)
                                                    {
                                                        DataRow dr = result.NewRow();
                                                        string fromHour = dataTableShift.Rows[j]["HourFrom"].ToString().Trim();
                                                        int _hf = Convert.ToInt32(fromHour.Split(':')[0]);
                                                        int _mf = Convert.ToInt32(fromHour.Split(':')[1]);

                                                        string toHour = dataTableShift.Rows[j]["HourTo"].ToString().Trim();
                                                        int _ht = Convert.ToInt32(toHour.Split(':')[0]);
                                                        int _mt = Convert.ToInt32(toHour.Split(':')[1]);
                                                        dr["EmpID"] = EmpName + "__" + EmpID;
                                                        dr["Date_From"] = date.AddHours(_hf).AddMinutes(_mf);
                                                        dr["Date_To"] = date.AddHours(_ht).AddMinutes(_mt);
                                                        dr["Color"] = dataTableShift.Rows[j]["ColorCode"];
                                                        dr["BranchID"] = shift[i].branch;
                                                        dr["Date"] = date.ToString("yyyy-MM-dd");

                                                        result.Rows.Add(dr);
                                                    }
                                                }

                                            }

                                            // _Content = shift[0].shift;

                                            // logic here
                                        }

                                    }
                                }
                            }

                        }
                    }


                return result;
            }
            catch (Exception ex)
            {
                DataTable result = new DataTable();
                return result;
            }

        }
        internal static string LoadScheduler()
        {
            throw new NotImplementedException();
        }

        public static async Task<string> OffScheduler ( Microsoft.AspNetCore.Http.HttpContext httpContext, int empid, string date)
        {
            try
            {
                WorkScheduler_Extension[] ws = new WorkScheduler_Extension[1];
                shifts s = new shifts();
                shifts[] ss = new shifts[1];
                ss[0] = s;

                WorkScheduler_Extension w = new WorkScheduler_Extension();
                w.off = true;
                w.shift = ss;
                ws[0] = w;
                DataTable dt = new DataTable();
                var user = Session.Session.GetUserSession (httpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[MLU_sp_WorK_Schedule_OFF]", CommandType.StoredProcedure,
                        "@Employee", SqlDbType.Int, empid,
                         "@Data", SqlDbType.NVarChar, JsonConvert.SerializeObject(ws),
                        "@Date", SqlDbType.DateTime, Comon.DateTimeDMY_To_YMDHHMM(date + " 00:00")
                    );
                }
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public static DataTable GetShiftDetail(DataTable shidtList, string shiftString)
        {
            try
            {
                DataTable DataResulf = new DataTable();
                DataResulf = shidtList.Clone();
                shiftString = "," + shiftString + ",";
                for (int i = 0; i < shidtList.Rows.Count; i++)
                {
                    string id = shidtList.Rows[i]["ID"].ToString();
                    id = "," + id + ",";
                    if (shiftString.Contains(id))
                    {
                        DataRow dr = DataResulf.NewRow();
                        dr["Code"] = shidtList.Rows[i]["Code"].ToString();
                        dr["ColorCode"] = shidtList.Rows[i]["ColorCode"].ToString();
                        dr["HourFrom"] = shidtList.Rows[i]["HourFrom"].ToString();
                        dr["HourTo"] = shidtList.Rows[i]["HourTo"].ToString();
                        DataResulf.Rows.Add(dr);
                    }
                }
                return DataResulf;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string GetBranchCode(DataTable dt, string branchid)
        {
            try
            {
                var resultRow = from myRow in dt.AsEnumerable()
                                where myRow.Field<Int32>("ID") == Convert.ToInt32(branchid)
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                return dtResult.Rows[0]["Name"].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        private static string GetBranchName(DataTable dt, string branchid)
        {
            try
            {
                var resultRow = from myRow in dt.AsEnumerable()
                                where myRow.Field<Int32>("ID") == Convert.ToInt32(branchid)
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                return dtResult.Rows[0]["BranchName"].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}