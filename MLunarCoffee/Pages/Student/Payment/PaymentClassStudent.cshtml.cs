using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Student.Payment
{
    public class PaymentClassStudentModel : PageModel
    {
        public string StudentID { get; set; }
        public void OnGet()
        {
            string _studentID = Request.Query["StudentID"];
            StudentID = _studentID != null ? _studentID : "0";
        }

        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.LoadPara("Method");
                    dt.TableName = "Method";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                       , "@UserID", SqlDbType.Int, user.sys_userid
                       , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_ST_Subject_LoadComBo", CommandType.StoredProcedure);
                    dt.TableName = "Subject";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_ST_InfoSms", CommandType.StoredProcedure
                        , "@BranchID", SqlDbType.Int, user.sys_branchID);
                    dt.TableName = "InfoSms";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostLoadPayment(string StudentID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_ST_Student_Payment_Load", CommandType.StoredProcedure
                        , "@StudentID", SqlDbType.Int, StudentID);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostCheckValidPayment(string dataDetail)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dtDetail = JsonConvert.DeserializeObject<DataTable>(dataDetail);
                DataTable dtPayment = new DataTable();
                dtPayment.Columns.Add("ClassStudentID", typeof(int));
                dtPayment.Columns.Add("Amount", typeof(decimal));

                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    DataRow dr = dtPayment.NewRow();
                    dr["ClassStudentID"] = Convert.ToInt32(dtDetail.Rows[i]["ID"]);
                    dr["Amount"] = Convert.ToDecimal(dtDetail.Rows[i]["Amount"]);
                    dtPayment.Rows.Add(dr);
                }

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_ST_Student_Payment_CheckValid", CommandType.StoredProcedure
                        , "@DataPay", SqlDbType.Structured, dtPayment);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostExcuteData(string StudentID, string data, string dataDetail)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                PayClassStu dtInfo = JsonConvert.DeserializeObject<PayClassStu>(data);
                DataTable dtDetail = JsonConvert.DeserializeObject<DataTable>(dataDetail);
                DataTable dtPayment = new DataTable();
                dtPayment.Columns.Add("ClassStudentID", typeof(int));
                dtPayment.Columns.Add("Amount", typeof(decimal));
                dtPayment.Columns.Add("MethodID", typeof(int));

                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    DataRow dr = dtPayment.NewRow();
                    dr["ClassStudentID"] = Convert.ToInt32(dtDetail.Rows[i]["ID"]);
                    dr["Amount"] = Convert.ToDecimal(dtDetail.Rows[i]["Amount"]);
                    dr["MethodID"] = Convert.ToInt32(dtDetail.Rows[i]["MethodID"]);
                    dtPayment.Rows.Add(dr);
                }

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_ST_Student_Payment_Insert", CommandType.StoredProcedure
                        , "@StudentID", SqlDbType.Int, StudentID
                        , "@MethodID", SqlDbType.Int, dtInfo.method
                        , "@Amount", SqlDbType.Decimal, dtInfo.amount
                        , "@Content", SqlDbType.NVarChar, dtInfo.content
                        , "@BranchID", SqlDbType.Int, dtInfo.branchid != 0 ? dtInfo.branchid : user.sys_branchID
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@DateCreated", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
                        , "@DataPay", SqlDbType.Structured, dtPayment);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
    }


    public class PayClassStu
    {
        public string content { get; set; }
        public int branchid { get; set; }
        public int method { get; set; }
        public string amount { get; set; }
    }
}
