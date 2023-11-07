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

namespace MLunarCoffee.Pages.Customer.Treatment
{

    public class TreatLessDetailModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _CurrentID { get; set; }
        public string _Type { get; set; }

        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];
            var type = Request.Query["Type"];
            if (type.ToString() != String.Empty)
            {
                _Type = type.ToString();
            }
            else
            {
                _Type = "";
            }
            if (cus.ToString() != String.Empty)
            {
                _CustomerID = cus.ToString();
                if (curr.ToString() != String.Empty)
                {
                    _CurrentID = curr.ToString();
                }
                else
                {
                    _CurrentID = "0";
                }
            }
            else
            {
                _CustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_TreatmentLess_LoadDetail]", CommandType.StoredProcedure,
                  "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadComboMain(int CustomerID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.LoadEmployee(14, user.sys_branchID, user.sys_AllBranchID);
                    dt.TableName = "Doctor";
                    ds.Tables.Add(dt);

                    DataTable dt1 = new DataTable();
                    dt1 = await confunc.LoadEmployee(17, user.sys_branchID, user.sys_AllBranchID);
                    dt1.TableName = "Assist";
                    ds.Tables.Add(dt1);


                    DataTable dt2 = new DataTable();
                    dt2 = await confunc.ExecuteDataTable("[YYY_sp_CustomerTreatment_PatientRecord_LoadCombo]", CommandType.StoredProcedure
                        , "@CustomerID", SqlDbType.Int, CustomerID);
                    dt2.TableName = "PatientRecord";
                    ds.Tables.Add(dt2);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                TreatLess DataMain = JsonConvert.DeserializeObject<TreatLess>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_TreatmentLess_Insert]", CommandType.StoredProcedure,
                            "@CustomerID", SqlDbType.Int, DataMain.CustomerID,
                            "@Doctor", SqlDbType.Int, DataMain.Doctor,
                            "@Assist", SqlDbType.Int, DataMain.Assist,
                            "@ContentCurrent", SqlDbType.NVarChar, DataMain.ContentCurrent.Replace("'", "").Trim(),
                            "@ContentNext", SqlDbType.NVarChar, DataMain.ContentNext.Replace("'", "").Trim(),
                            "@TreatNote", SqlDbType.NVarChar, DataMain.TreatNote.Replace("'", "").Trim(),
                            "@PatientID", SqlDbType.Int, DataMain.PatientID,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@BranchID", SqlDbType.Int, user.sys_branchID
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_TreatmentLess_Update]", CommandType.StoredProcedure,
                            "@CustomerID", SqlDbType.Int, DataMain.CustomerID,
                            "@Doctor", SqlDbType.Int, DataMain.Doctor,
                            "@Assist", SqlDbType.Int, DataMain.Assist,
                            "@PatientID", SqlDbType.Int, DataMain.PatientID,
                            "@ContentCurrent", SqlDbType.NVarChar, DataMain.ContentCurrent.Replace("'", "").Trim(),
                            "@ContentNext", SqlDbType.NVarChar, DataMain.ContentNext.Replace("'", "").Trim(),
                            "@TreatNote", SqlDbType.NVarChar, DataMain.TreatNote.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@BranchID", SqlDbType.Int, user.sys_branchID,
                            "@CurrentID", SqlDbType.Int, CurrentID
                        );
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

    public class TreatLess
    {
        public string Doctor { get; set; }
        public string Assist { get; set; }
        public string CustomerID { get; set; }
        public string Tech { get; set; }
        public string ContentCurrent { get; set; }
        public string ContentNext { get; set; }
        public string TreatNote { get; set; }
        public int PatientID { get; set; }

    }
}
