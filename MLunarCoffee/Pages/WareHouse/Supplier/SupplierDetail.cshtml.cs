using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Threading.Tasks;
using System;
using static SourceMain.Pages.Setting.AppCustomer.About.AboutDetailModel;
using Newtonsoft.Json;

namespace SourceMain.Pages.WareHouse.Supplier
{
    public class SupplierDetailModel : PageModel
    {
        public string layout { get; set; }
        public string supplierID { get; set; }
        public void OnGet()
        {
            string _supp = Request.Query["SupplierID"];
            supplierID = !string.IsNullOrEmpty(_supp) ? _supp : "0";

            string _layout = Request.Query["layout"];
            layout = !string.IsNullOrEmpty(_layout) ? _layout : "0";

        }

        public async Task<IActionResult> OnPostInit(string suppid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_Supplier_LoadInfo", CommandType.StoredProcedure
                        , "@SuppID", SqlDbType.Int, Convert.ToInt32(suppid));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }

        }



        public async Task<IActionResult> OnPostLoadInvoice(int suppid, int currentID, string data)
        {
            try
            {
                DataTable dt = new DataTable();
                SupplierInvoice dataMain = JsonConvert.DeserializeObject<SupplierInvoice>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Account_SupplierDetail]", CommandType.StoredProcedure
                        , "@SuppID", SqlDbType.Int, Convert.ToInt32(suppid)
                        , "@CurrentID", SqlDbType.Int, Convert.ToInt32(currentID)
                        , "@BeginPayID", SqlDbType.Int, Convert.ToInt32(dataMain.beginPayID)
                        , "@BeginDesID", SqlDbType.Int, Convert.ToInt32(dataMain.beginDesID)
                        , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dataMain.dateFrom)
                        , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dataMain.dateTo)
                        , "@Type", SqlDbType.NVarChar, !String.IsNullOrEmpty(dataMain.type) ? dataMain.type.ToString() : ""
                        , "@Search", SqlDbType.NVarChar, Comon.Comon.ConvertToUnsign((dataMain.search != null) ? dataMain.search : "").ToLower());
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }
        }
    }

    public class SupplierInvoice
    {
        public int beginPayID { get; set; }
        public int beginDesID { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string type { get; set; }
        public string search { get; set; }
    }
}
