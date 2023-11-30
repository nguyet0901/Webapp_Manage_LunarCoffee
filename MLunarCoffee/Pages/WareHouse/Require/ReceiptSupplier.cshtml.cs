using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Require
{
    public class ReceiptSupplier : PageModel
    {
        public string _Detail { get; set; }
        public void OnGet()
        {
            string Detail = Request.Query["Detail"];
            _Detail = (Detail==null) ? "0" : Detail.ToString();
        }
        public async Task<IActionResult> OnPostGetInfo(int DetailID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds=await confunc.ExecuteDataSet("MLU_sp_WareHouse_ReceiptSup_Info",CommandType.StoredProcedure
                    ,"@DetailID",SqlDbType.Int,DetailID);
                }
                if(ds!=null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch(Exception ex)
            {
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[MLU_sp_v2_Product_Combo_Supplier]",CommandType.StoredProcedure);
                    dt.TableName="Supplier";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch(Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostGetExpiredDate(string productid,string packagenumber)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[MLU_sp_WareHouse_GetExpired]",CommandType.StoredProcedure,
                          "@productid",SqlDbType.Int,productid,
                           "@packagenumber",SqlDbType.NVarChar,packagenumber
                        );

                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch(Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string data,string DateExecute,string IsPackageNumber,string CurrentID, string SupplierID, string InvoiceNumber, string ContractNumber)
        {
            try
            {

                DataTable DataExport = new DataTable();
                DataExport.Columns.Add("productID",typeof(Int32));
                DataExport.Columns.Add("package",typeof(string));
                DataExport.Columns.Add("supid",typeof(string));

                DataExport.Columns.Add("number",typeof(Decimal));
                DataExport.Columns.Add("amount",typeof(Decimal));
                DataExport.Columns.Add("expiredDay",typeof(DateTime));
                DataExport.Columns.Add("current",typeof(Int32));

                DataTable _DataExport = new DataTable();
                _DataExport=JsonConvert.DeserializeObject<DataTable>(data);

                for(int i = 0;i<_DataExport.Rows.Count;i++)
                {
                    if(Convert.ToInt32(_DataExport.Rows[i]["isreceipt"])==0
                        &&Convert.ToInt32(_DataExport.Rows[i]["receipt"])==1)
                    {
                        DataRow dr = DataExport.NewRow();
                        dr["current"]=Convert.ToInt32(_DataExport.Rows[i]["current"]);
                        dr["productID"]=Convert.ToInt32(_DataExport.Rows[i]["productID"]);
                        dr["package"]=_DataExport.Rows[i]["package"];
                        dr["supid"]=Convert.ToInt32(_DataExport.Rows[i]["supid"]);
                        dr["number"]=Convert.ToDecimal(_DataExport.Rows[i]["number"]);
                        dr["amount"]=Convert.ToDecimal(_DataExport.Rows[i]["amount"]);
                        dr["expiredDay"]=Comon.Comon.DateTimeDMY_To_YMD(_DataExport.Rows[i]["expiredDay"].ToString());
                        DataExport.Rows.Add(dr);
                    }
                }
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_WareHouse_Export_ImportSupplier",CommandType.StoredProcedure,
                        "@DateExecute",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                        "@MasterID",SqlDbType.Int,CurrentID,
                        "@SupplierID", SqlDbType.Int, SupplierID,
                        "@InvoiceNumber", SqlDbType.NVarChar, InvoiceNumber,
                        "@ContractNumber", SqlDbType.NVarChar, ContractNumber,
                        "@IsPackageNumber",SqlDbType.Int, IsPackageNumber,
                        "@Created_By",SqlDbType.Int, user.sys_userid,
                        "@dt",SqlDbType.Structured, DataExport.Rows.Count > 0 ? DataExport : null
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));

            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

    }
}
