using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Lock
{
    public class LockEdit : PageModel
    {
        public string _LockID { get; set; }
        public string _ViewOnly { get; set; }  //0:Normal, 1: View
        public void OnGet()
        {
            string LockID = Request.Query["LockID"];
            string ViewOnly = Request.Query["ViewOnly"];
            _LockID=(LockID==null) ? "0" : LockID.ToString();
            _ViewOnly=(ViewOnly==null) ? "0" : ViewOnly.ToString();

        }

        public async Task<IActionResult> OnPostInitialize(string LockID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_Lock_EditInfo]", CommandType.StoredProcedure,
                    "@LockID", SqlDbType.NVarChar, LockID );
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadData(string LockID)
        {
            try
            {
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_Norm_Current]",CommandType.StoredProcedure,
                         "@LockID",SqlDbType.Int,LockID);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostLoadTicket(string WareID, string ProductID, string DateFrom, string DateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_Norm_Ticket]", CommandType.StoredProcedure,
                      "@WareID", SqlDbType.Int, WareID,
                      "@ProductID", SqlDbType.Int, ProductID,
                      "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateFrom),
                      "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateTo)
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string data,  string LockID)
        {
            try
            {
                DataTable DataDetail = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                DataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtDetail = new DataTable();
                DataTable dt = new DataTable();

                dtDetail.Columns.Add("UnitID", typeof(int));
                dtDetail.Columns.Add("ProductID", typeof(int));
                dtDetail.Columns.Add("Number", typeof(decimal));
                dtDetail.Columns.Add("Amount", typeof(decimal));
                dtDetail.Columns.Add("IsComplete", typeof(int));
                dtDetail.Columns.Add("Num_Input", typeof(decimal));
                dtDetail.Columns.Add("Num_Input_Transfer", typeof(decimal));
                dtDetail.Columns.Add("Num_Output", typeof(decimal));
                dtDetail.Columns.Add("Num_Output_Transfer", typeof(decimal));
                dtDetail.Columns.Add("Num_Output_Sale", typeof(decimal));
                dtDetail.Columns.Add("Num_Output_Treat", typeof(decimal));

                for (int i = 0; i < DataDetail.Rows.Count; i++)
                {
                    DataRow dr = dtDetail.NewRow();
                    dr["ProductID"] = Convert.ToInt32(DataDetail.Rows[i]["ProductID"]);
                    dr["UnitID"] = Convert.ToInt32(DataDetail.Rows[i]["UnitID"]);
                    dr["Number"] = Convert.ToDecimal(DataDetail.Rows[i]["Num_Left"]);
                    dr["IsComplete"] = Convert.ToInt32(DataDetail.Rows[i]["IsComplete"]);
                    dr["Amount"] =Convert.ToDecimal(DataDetail.Rows[i]["Amount"]);
                    dr["Num_Input"] = Convert.ToDecimal(DataDetail.Rows[i]["Import"]);
                    dr["Num_Input_Transfer"] = Convert.ToDecimal(DataDetail.Rows[i]["Import_Transfer"]);
                    dr["Num_Output"] = Convert.ToDecimal(DataDetail.Rows[i]["Export"]);
                    dr["Num_Output_Transfer"] = Convert.ToDecimal(DataDetail.Rows[i]["Export_Transfer"]);
                    dr["Num_Output_Sale"] = Convert.ToDecimal(DataDetail.Rows[i]["Export_Sale"]);
                    dr["Num_Output_Treat"] = Convert.ToDecimal(DataDetail.Rows[i]["Export_Treat"]);

                    dtDetail.Rows.Add(dr);
                }

                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_Norm_Lock_Update]",CommandType.StoredProcedure,
                         "@Modified_By",SqlDbType.Int,user.sys_userid,
                         "@LockID",SqlDbType.Int,LockID,
                         "@table_data",SqlDbType.Structured,dtDetail.Rows.Count>0 ? dtDetail : null
                     );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_Norm_Lock_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
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
