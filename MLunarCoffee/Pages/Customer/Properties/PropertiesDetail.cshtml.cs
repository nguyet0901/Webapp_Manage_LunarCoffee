using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.Properties
{
    public class PropertiesDetailModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _DateCreated { get; set; }
        public void OnGet()
        {
            string CustomerID = Request.Query["CustomerID"];
            string DateCreated = Request.Query["DateCreated"];

            _CustomerID = CustomerID != null ? CustomerID : "";
            _DateCreated = DateCreated != null ? DateCreated : "0";
        }
        public async Task<IActionResult> OnPostLoadata(int CustomerID, string DateCreated)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_CustomerProperties_LoadDetail]" ,CommandType.StoredProcedure
                        ,"@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                        ,"@DateCreated", SqlDbType.NVarChar, DateCreated);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecuted(int CustomerID ,string DtPro, string DateCreated)
        {
            try
            {
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(DtPro);
                DataTable dtPro = new DataTable();
                dtPro.Columns.Add("IDPro" ,typeof(Int32));
                dtPro.Columns.Add("Value" ,typeof(String));
                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtPro.NewRow();
                    dr["IDPro"] = Convert.ToInt32(DataMain.Rows[i]["ID"]);
                    dr["Value"] = DataMain.Rows[i]["Value"];
                    dtPro.Rows.Add(dr);
                }
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (DateCreated == "0")
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[MLU_sp_CustomerProperties_Insert]" ,CommandType.StoredProcedure
                            ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID)
                            ,"@user" ,SqlDbType.Int ,user.sys_userid
                            ,"@dtPro", SqlDbType.Structured, (dtPro.Rows.Count > 0) ? dtPro : null
                            );
                    }
                }
                else {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[MLU_sp_CustomerProperties_Update]" ,CommandType.StoredProcedure
                            ,"@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID)
                            ,"@dtPro" ,SqlDbType.Structured ,(dtPro.Rows.Count > 0) ? dtPro : null
                            ,"@user" ,SqlDbType.Int ,user.sys_userid
                            ,"@ser_DateCreated", SqlDbType.NVarChar,DateCreated
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
