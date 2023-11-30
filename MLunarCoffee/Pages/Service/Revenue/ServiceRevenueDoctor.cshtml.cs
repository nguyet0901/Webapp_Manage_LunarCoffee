using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;
using System.Data;
using System.Threading.Tasks;
using MLunarCoffee.Comon.Session;
using System.Linq;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Service.Revenue
{
    public class ServiceRevenueDoctorModel : PageModel
    {
        public string _ServiceCurrentID { get; set; }
        public void OnGet()
        {
            string serviceid = Request.Query["CurrentID"];
            _ServiceCurrentID = serviceid != null ? serviceid.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.LoadEmployeeFull(0, 1);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }


        public async Task<IActionResult> OnPostLoadDetail(int ServiceID)
        {
            try
            {

                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_v2_ServiceDetail_LoadRevenue", CommandType.StoredProcedure
                        , "@ServiceID", SqlDbType.Int, ServiceID);

                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string ServiceID, string datadoc, string dataass, string datacon)
        {

            try
            {

                DataTable DataServiceStage = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                DataTable DataMainDoc = TabelRevenue(datadoc);
                DataTable DataMainAss = TabelRevenue(dataass);
                DataTable DataMainCon = TabelRevenue(datacon);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_ServiceDetail_SettingsRevenue", CommandType.StoredProcedure,
                        "@ServiceID", SqlDbType.Int, ServiceID,
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@DataDoc", SqlDbType.Structured, DataMainDoc.Rows.Count > 0 ? DataMainDoc : null,
                        "@DataAss", SqlDbType.Structured, DataMainAss.Rows.Count > 0 ? DataMainAss : null,
                        "@DataCon", SqlDbType.Structured, DataMainCon.Rows.Count > 0 ? DataMainCon : null
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }




        private DataTable TabelRevenue(string data)
        {
            try
            {
                DataTable DataRevenue = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable DataMain = new DataTable();
                DataMain.Columns.Add("EmployeeID", typeof(int));
                DataMain.Columns.Add("ReAmount", typeof(decimal));
                DataMain.Columns.Add("RePer", typeof(decimal));
                for (int i = 0; i < DataRevenue.Rows.Count; i++)
                {
                    DataRow dr = DataMain.NewRow();
                    dr["EmployeeID"] = Convert.ToInt32(DataRevenue.Rows[i]["EmployeeID"]);
                    dr["ReAmount"] = Convert.ToDecimal(DataRevenue.Rows[i]["ReAmount"]);
                    dr["RePer"] = Convert.ToDecimal(DataRevenue.Rows[i]["RePer"]);
                    DataMain.Rows.Add(dr);
                }
                return DataMain;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
