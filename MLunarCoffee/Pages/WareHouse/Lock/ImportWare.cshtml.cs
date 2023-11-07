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
    public class ImportWareModel : PageModel
    {
        public string _WareID { get; set; }
        public void OnGet()
        {
            string wareID = Request.Query["WareID"];
            _WareID = wareID.ToString();
        }

        public async Task<IActionResult> OnPostLoadDataProduct(int WareID)
        {
            DataTable dt1 = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {

                dt1 =await confunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_Product_LoadList]", CommandType.StoredProcedure
                    ,"@WareID",SqlDbType.Int,WareID
                    );
            }
            if (dt1 != null)
            {
                return Content(Comon.DataJson.Datatable(dt1));
            }
            else
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string wareID, string dateImport)
        {
            try
            {

                DataTable DataDetail = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                DataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtDetail = new DataTable();
                DataTable dt = new DataTable();

                //những trường có giá trị
                dtDetail.Columns.Add("UnitID", typeof(int));
                dtDetail.Columns.Add("ProductID", typeof(int));
                dtDetail.Columns.Add("Number", typeof(decimal));
                dtDetail.Columns.Add("Amount", typeof(decimal));
                dtDetail.Columns.Add("IsComplete", typeof(int));

                //những trường không có giá trị
                dtDetail.Columns.Add("Num_Input", typeof(int));
                dtDetail.Columns.Add("Num_Input_Transfer", typeof(int));
                dtDetail.Columns.Add("Num_Output", typeof(int));
                dtDetail.Columns.Add("Num_Output_Transfer", typeof(int));
                dtDetail.Columns.Add("Num_Output_Sale", typeof(int));
                dtDetail.Columns.Add("Num_Output_Treat", typeof(int));

                for (int i = 0; i < DataDetail.Rows.Count; i++)
                {
                    DataRow dr = dtDetail.NewRow();
                    dr["ProductID"] = Convert.ToInt32(DataDetail.Rows[i]["ProductID"]);
                    dr["UnitID"] = Convert.ToInt32(DataDetail.Rows[i]["Unit_ID"]);
                    dr["Number"] = Convert.ToDecimal(DataDetail.Rows[i]["NumberLeft"]);
                    dr["Amount"] = Convert.ToDecimal(DataDetail.Rows[i]["UnitPrice"]);
                    dr["Num_Input"] = 0;
                    dr["IsComplete"] = 1;
                    dr["Num_Input_Transfer"] = 0;
                    dr["Num_Output"] = 0;
                    dr["Num_Output_Transfer"] = 0;
                    dr["Num_Output_Sale"] = 0;
                    dr["Num_Output_Treat"] = 0;

                    dtDetail.Rows.Add(dr);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_WareHouse_Lock_Import]", CommandType.StoredProcedure,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@WareID", SqlDbType.Int, wareID,
                            "@DateLock", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMMSS(dateImport),
                            "@table_data", SqlDbType.Structured, dtDetail.Rows.Count > 0 ? dtDetail : null
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
