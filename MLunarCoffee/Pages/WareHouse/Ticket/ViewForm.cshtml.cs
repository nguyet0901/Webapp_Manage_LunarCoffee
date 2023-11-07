using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.WareHouse.Ticket
{
    public class ViewForm : PageModel
    {

        public string Random { get; set; }
        public string type { get; set; }
        public string code { get; set; }
        public string ticketproduct { get; set; }
        public void OnGet()
        {
            Random rnd = new Random();
            Random = Math.Abs(rnd.Next()).ToString();
            type = (Request.Query["type"].ToString() != null) ? Request.Query["type"].ToString() : "";
            code = (Request.Query["code"].ToString() != null) ? Request.Query["code"].ToString() : "";
            ticketproduct = (Request.Query["ticketproduct"].ToString() != null) ? Request.Query["ticketproduct"].ToString() : "";

        }


        public async Task<IActionResult> OnPostLoadDataViewForm(string type, string code)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[YYY_sp_v2_WareHouse_ViewForm]", CommandType.StoredProcedure,
                      "@type", SqlDbType.Int, type,
                      "@code", SqlDbType.NVarChar, code
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadStockQuantity(string DataProduct, string WareID, string CurrentID, string IsPackageNumber)
        {
            try
            {
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(DataProduct);
                var user = Session.GetUserSession(HttpContext);
                DataTable dtProduct = new DataTable();
                dtProduct.Columns.Add("PackageNumber", typeof(string));
                dtProduct.Columns.Add("ProductID", typeof(int));
                dtProduct.Columns.Add("Number", typeof(decimal));
                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    DataRow dr = dtProduct.NewRow();
                    dr["PackageNumber"] = dataDetail.Rows[i]["package"];
                    dr["ProductID"] = dataDetail.Rows[i]["productID"];
                    dr["Number"] = Convert.ToDecimal(dataDetail.Rows[i]["number"]);
                    dtProduct.Rows.Add(dr);
                }
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_Order_Product_Quantity_Stock" , CommandType.StoredProcedure,
                        "@IsPackageNumber", SqlDbType.Int, IsPackageNumber,
                        "@WareID", SqlDbType.Int, WareID,
                        "@MasterSTID", SqlDbType.Int, CurrentID,
                        "@dt", SqlDbType.Structured, dtProduct.Rows.Count > 0 ? dtProduct : null
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

    }
}
