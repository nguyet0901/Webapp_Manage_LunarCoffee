using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.Treatment.TreatmentMesure
{

    public class MesDetail : PageModel
    {
        public string _CurrentID { get; set; }
         public string _TreatmentID { get; set; }
        public void OnGet()
        {
            var CurrentID = Request.Query["Current"];
            var TreatmentID = Request.Query["TreatmentID"];
            if (CurrentID.ToString() != String.Empty)
            {
                _CurrentID = CurrentID.ToString();
            }
            if (TreatmentID.ToString() != String.Empty)
            {
                _TreatmentID = TreatmentID.ToString();
            }
        }
         public async Task<IActionResult> OnPostLoadIni()
        {
            try
            {


                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Customer_MeasureType", CommandType.StoredProcedure
                             );
                        dt.TableName = "Type";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Customer_MeasureContent", CommandType.StoredProcedure);
                        dt.TableName = "Content";
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
            catch (Exception)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadataDetail(string CurrentID, string CustomerID)
        {
            DataSet ds = new DataSet();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_MeasureDetail]", CommandType.StoredProcedure,
                  "@CurrentID", SqlDbType.Int, CurrentID
                  , "@CustomerID", SqlDbType.Int, CustomerID
                   , "@UserId", SqlDbType.Int, user.sys_userid
                  , "@PassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
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
        public async Task<IActionResult> OnPostDeleteItem(string CustomerID, string currentid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Customer_MeasureDelete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, currentid,
                        "@CreatedBy", SqlDbType.DateTime, user.sys_userid,
                        "@CustomerID", SqlDbType.Int, CustomerID
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostExcuteData(string CurrentID, string CustomerID, string TreatmentID, string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dtInfo = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                DataTable dtResult = new DataTable();
                dt.Columns.Add("ID", typeof(Int64));
                dt.Columns.Add("MeaTypeID", typeof(Int64));
                dt.Columns.Add("MeaContentID", typeof(Int64));
                dt.Columns.Add("Value", typeof(Decimal));
                dt.Columns.Add("Content", typeof(string));

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = Convert.ToInt64(dtInfo.Rows[i]["ID"]);
                    dr["MeaTypeID"] = Convert.ToInt64(dtInfo.Rows[i]["MeaTypeID"]);
                    dr["MeaContentID"] = Convert.ToInt64(dtInfo.Rows[i]["MeaContentID"]);
                    dr["Value"] = Convert.ToDecimal(dtInfo.Rows[i]["Value"]);
                    dr["Content"] = dtInfo.Rows[i]["Content"] != null ? dtInfo.Rows[i]["Content"].ToString() : "";
                    dt.Rows.Add(dr);

                }
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Measure_Insert]", CommandType.StoredProcedure,
                            "@CustomerID", SqlDbType.Int, CustomerID,
                            "@TreatmentID", SqlDbType.Int, TreatmentID,
                            "@BranchID", SqlDbType.NVarChar, user.sys_branchID,
                            "@data", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null,
                            "@CreatedBy", SqlDbType.Int, user.sys_userid);
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Measure_Update]", CommandType.StoredProcedure,
                            "@CustomerID", SqlDbType.Int, CustomerID,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@BranchID", SqlDbType.NVarChar, user.sys_branchID,
                            "@data", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null,
                            "@CreatedBy", SqlDbType.Int, user.sys_userid);
                    }
                }

                return Content(dtResult.Rows[0]["Result"].ToString());
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

}
