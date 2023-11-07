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

namespace MLunarCoffee.Pages.Service.PrescriptionMedicine
{
    public class PrescriptionMedicineDetailModel : PageModel
    {
        public int _PrescriptionMedicineID { get; set; }
        public void OnGet()
        {
            string id = Request.Query["PrescriptionMedicineID"];
            _PrescriptionMedicineID = id != null ? Convert.ToInt32(id.ToString()) : 0;
        }
        public async Task<IActionResult> OnPostLoad_Data_Initialize()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataSet dt = new DataSet();
                    dt = await confunc.ExecuteDataSet("[YYY_sp_Medicine_Combo_Load]", CommandType.StoredProcedure);

                    DataTable dt1 = dt.Tables[0].Copy();
                    dt1.TableName = "Medicine";

                    DataTable dt2 = dt.Tables[1].Copy();
                    dt2.TableName = "MedicineUnit";

                    ds.Tables.Add(dt1);
                    ds.Tables.Add(dt2);

                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDataDetail(int ID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[YYY_sp_PrescriptionMedicine_LoadDetail]", CommandType.StoredProcedure
                        , "@ID", SqlDbType.Int, ID);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(int PrescriptionMedicineID, string data, string Name, string Amount, string Code, string Note)
        {
            try
            {
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("Product_ID", typeof(Int32)); 
                dtMain.Columns.Add("Quantity", typeof(Int32));
                dtMain.Columns.Add("Unit_ID", typeof(Int32));
                dtMain.Columns.Add("Price", typeof(Decimal));
                dtMain.Columns.Add("Dosage1", typeof(String));
                dtMain.Columns.Add("Dosage2", typeof(String));
                dtMain.Columns.Add("Dosage3", typeof(String));
                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["Product_ID"] = Convert.ToInt32(DataMain.Rows[i]["Medicine_ID"]);
                    dr["Quantity"] = Convert.ToInt32(DataMain.Rows[i]["Quantity"]);
                    dr["Unit_ID"] = Convert.ToInt32(DataMain.Rows[i]["Unit"]);
                    dr["Price"] = Convert.ToDecimal(DataMain.Rows[i]["Price"]);
                    dr["Dosage1"] = Convert.ToString(DataMain.Rows[i]["Dosage1"]);
                    dr["Dosage2"] = Convert.ToString(DataMain.Rows[i]["Dosage2"]);
                    dr["Dosage3"] = Convert.ToString(DataMain.Rows[i]["Dosage3"]);
                    dtMain.Rows.Add(dr);
                }
                var user = Session.GetUserSession(HttpContext);
                if (PrescriptionMedicineID == 0)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_PrescriptionMedicine_Insert", CommandType.StoredProcedure,
                             "@Name", SqlDbType.NVarChar, Name.Replace("'", "").Trim(),
                             "@Amount", SqlDbType.Decimal, Convert.ToDecimal(Amount),
                             "@Code", SqlDbType.NVarChar, Code.Replace("'", "").Trim(),
                             "@Note", SqlDbType.NVarChar, Note != null ? Note.Replace("'", "").Trim() : "",
                             "@Created_By", SqlDbType.Int, user.sys_userid,
                             "@table_data", SqlDbType.Structured, (dtMain.Rows.Count > 0) ? dtMain : null
                         ); ;
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_PrescriptionMedicine_Update]", CommandType.StoredProcedure,
                             "@CurrentID", SqlDbType.Int, PrescriptionMedicineID,
                             "@Name", SqlDbType.NVarChar, Name.Replace("'", "").Trim(),
                             "@Amount", SqlDbType.Decimal, Convert.ToDecimal(Amount),
                             "@Code", SqlDbType.NVarChar, Code.Replace("'", "").Trim(),
                             "@Note", SqlDbType.NVarChar, Note != null ? Note.Replace("'", "").Trim() : "",
                             "@Modified_By", SqlDbType.Int, user.sys_userid,
                             "@table_data", SqlDbType.Structured, (dtMain.Rows.Count > 0) ? dtMain : null
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
}
