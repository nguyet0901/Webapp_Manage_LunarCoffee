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
    public class LockNew : PageModel
    {
        public string _WareID { get; set; }
        public string _DateTo { get; set; }
        public void OnGet()
        {
            string WareID = Request.Query["WareID"];
            string DateTo = Request.Query["DateTo"];
            if (DateTo != null && DateTo.Contains("x")) DateTo = DateTo.ToString().Replace("x", " ");
            else if (DateTo != null) DateTo = DateTo + " 23:59";
            if (WareID != null)
            {
                _WareID = WareID;
                _DateTo = (DateTo == null) ? "" : DateTo.ToString();
            }
            else
            {
                _WareID = "0";
                _DateTo = "";
            }

        }

        public async Task<IActionResult> OnPostInitialize(string wareid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_Lock_Info]", CommandType.StoredProcedure,
                    "@wareid", SqlDbType.Int,wareid);
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadData(string WareID, string ser_preID, string DateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_Lock_New2]",CommandType.StoredProcedure,
                      "@WareID",SqlDbType.Int,WareID,
                      "@ser_preID",SqlDbType.Int,ser_preID,
                      "@DateTo",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMDHHMM(DateTo)
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostLoadTicket(string WareID, string ProductID, string ser_preID, string DateTo)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_Norm_Ticket]", CommandType.StoredProcedure,
                      "@WareID", SqlDbType.Int, WareID,
                      "@ProductID", SqlDbType.Int, ProductID,
                      "@ser_preID", SqlDbType.Int,ser_preID,
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

        public async Task<IActionResult> OnPostExcuteData(string data, string wareID, string Datelock)
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
                dtDetail.Columns.Add("NumTerm", typeof(decimal));
                dtDetail.Columns.Add("Number", typeof(decimal));
                dtDetail.Columns.Add("NumberTemp", typeof(decimal));
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
                    dr["NumTerm"] = Convert.ToDecimal(DataDetail.Rows[i]["NumTerm"]);
                    dr["Number"] = Convert.ToDecimal(DataDetail.Rows[i]["Number"]);
                    dr["NumberTemp"] = Convert.ToDecimal(DataDetail.Rows[i]["NumberTemp"]);
                    dr["IsComplete"] = Convert.ToInt32(DataDetail.Rows[i]["IsComplete"]);
                    dr["Amount"] = Convert.ToDecimal(DataDetail.Rows[i]["Amount"]);
                    dr["Num_Input"] = Convert.ToDecimal(DataDetail.Rows[i]["Num_Input"]);
                    dr["Num_Input_Transfer"] = Convert.ToDecimal(DataDetail.Rows[i]["Num_Input_Transfer"]);
                    dr["Num_Output"] = Convert.ToDecimal(DataDetail.Rows[i]["Num_Output"]);
                    dr["Num_Output_Transfer"] = Convert.ToDecimal(DataDetail.Rows[i]["Num_Output_Transfer"]);
                    dr["Num_Output_Sale"] = Convert.ToDecimal(DataDetail.Rows[i]["Num_Output_Sale"]);
                    dr["Num_Output_Treat"] = Convert.ToDecimal(DataDetail.Rows[i]["Num_Output_Treat"]);
                    dtDetail.Rows.Add(dr);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_Norm_Lock]",CommandType.StoredProcedure,
                         "@Created_By",SqlDbType.Int,user.sys_userid,
                         "@WareID",SqlDbType.Int,wareID,
                         "@DateLock",SqlDbType.NVarChar,Comon.Comon.DateTimeDMY_To_YMDHHMM(Datelock),
                         "@table_data",SqlDbType.Structured, dtDetail.Rows.Count >0 ? dtDetail : null
                     );
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
