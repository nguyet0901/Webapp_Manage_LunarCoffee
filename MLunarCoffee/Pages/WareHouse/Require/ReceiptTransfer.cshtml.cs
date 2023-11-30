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
    public class ReceiptTransfer : PageModel
    {
        public string _Detail { get; set; }
        public void OnGet()
        {
            string Detail = Request.Query["Detail"];
            _Detail=(Detail==null) ? "0" : Detail.ToString();
        }
        public async Task<IActionResult> OnPostInitialize(int DetailID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_WareHouse_ReceiptTransfer_Info", CommandType.StoredProcedure
                    ,"@DetailID", SqlDbType.Int,DetailID);
                }
                if (ds!= null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }


        public async Task<IActionResult> OnPostUnImport(int DetailID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("[MLU_sp_WareHouse_Export_UnImportTransfer]",CommandType.StoredProcedure
                         ,"@MasterID",SqlDbType.Int,DetailID);
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostCheckQuantityRealTime(string data, string ware, string DateExecute, string CurrentID)
        {
            try
            {
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                dt.Columns.Add("PackageNumber", typeof(string));
                dt.Columns.Add("ProductID", typeof(int));
                dt.Columns.Add("Number", typeof(decimal));
                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["PackageNumber"] = dataDetail.Rows[i]["package"];
                    dr["ProductID"] = dataDetail.Rows[i]["productID"];
                    dr["Number"] = dataDetail.Rows[i]["number"];
                    dt.Rows.Add(dr);
                }
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("MLU_sp_Product_StockIn_Quantity_RealTime", CommandType.StoredProcedure,
                        "@DateExecute", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                        "@WareID", SqlDbType.Int, Convert.ToInt32(ware),
                        "@MasterID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                        "@dt", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data,string DateExecute,string IsPackageNumber,string CurrentID)
        {
            try
            {

                DataTable DataExport = new DataTable();
                DataExport.Columns.Add("productID",typeof(Int32));
                DataExport.Columns.Add("package",typeof(string));
                DataExport.Columns.Add("current",typeof(Int32));
                DataTable _DataExport = new DataTable();
                _DataExport=JsonConvert.DeserializeObject<DataTable>(data);

                for(int i = 0;i<_DataExport.Rows.Count;i++)
                {
                    DataRow dr = DataExport.NewRow();
                    dr["current"]=Convert.ToInt32(_DataExport.Rows[i]["current"]);
                    dr["productID"]=Convert.ToInt32(_DataExport.Rows[i]["productID"]);
                    dr["package"]=_DataExport.Rows[i]["package"];
                    DataExport.Rows.Add(dr);
                }
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("MLU_sp_WareHouse_Export_ImportTransfer",CommandType.StoredProcedure,
                        "@DateExecute",SqlDbType.DateTime,Comon.Comon.DateTimeDMY_To_YMDHHMM(DateExecute),
                        "@MasterID",SqlDbType.Int,CurrentID,
                        "@IsPackageNumber",SqlDbType.Int,IsPackageNumber,
                        "@dt",SqlDbType.Structured,DataExport.Rows.Count>0 ? DataExport : null
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostUpdateSign(int id,string sign)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[MLU_sp_WareHouse_SignImportOrder]",CommandType.StoredProcedure,
                        "@CurrentID",SqlDbType.Int,Convert.ToInt32(id),
                        "@userID",SqlDbType.Int,user.sys_userid,
                        "@Sign",SqlDbType.NVarChar,sign
                    );
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostDeleteItem(int id, string ware)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_WareHouse_ReceiptTransfer_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@WareID", SqlDbType.Int, Convert.ToInt32(ware)
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
