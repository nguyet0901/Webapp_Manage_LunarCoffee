using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System.Linq;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Customer.Service
{
    public class TabComboMultiModel : PageModel
    {
        public string _CustomerID { get;set; }
        public string _PlanID { get;set; }
        public string _PatientRecordID { get;set; }
        public string _ChooseDateCustomer { get;set; }
        public string _dtPermissionCurrentPage { get; set; }

        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);

            var CustomerID = Request.Query["CustomerID"];
            var PlanID = Request.Query["Plan"];
            var PatientRecordID = Request.Query["PatientRecordID"];

            _CustomerID = !string.IsNullOrEmpty(CustomerID) ? CustomerID : "0";
            _PlanID = !string.IsNullOrEmpty(PlanID) ? PlanID :  "0";
            _PatientRecordID = !string.IsNullOrEmpty(PatientRecordID) ? PatientRecordID : "0";
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }


        public async Task<IActionResult> OnPostInit()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtServiceCombo = new DataTable();
                        dtServiceCombo = await confunc.ExecuteDataTable("MLU_sp_Customer_Tab_Service_ComboType_Load" ,CommandType.StoredProcedure);
                        dtServiceCombo.TableName = "ServiceComboType";
                        return dtServiceCombo;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtServiceCombo = new DataTable();
                        dtServiceCombo = await confunc.ExecuteDataTable("MLU_sp_Customer_Tab_Service_Combo_Load" ,CommandType.StoredProcedure);
                        dtServiceCombo.TableName = "ServiceCombo";
                        return dtServiceCombo;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSourceService = new DataTable();
                        dtSourceService = await confunc.ExecuteDataTable("[YYY_LoadCombo_Source_CustService]" ,CommandType.StoredProcedure);
                        dtSourceService.TableName = "ServiceSource";
                        return dtSourceService;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSourceService = new DataTable();
                        dtSourceService = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabMulti_LoadComboTele]" ,CommandType.StoredProcedure);
                        dtSourceService.TableName = "Tele";
                        return dtSourceService;
                    }
                }));

                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostExecuteData(string data ,string CustomerID ,string Patient ,string Plan ,string Plan_Name, string ComboID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtMain = new DataTable();
                DataTable dt = new DataTable();
                dtMain.Columns.Add("Index" ,typeof(Int32));
                dtMain.Columns.Add("ServiceID" ,typeof(Int32));
                dtMain.Columns.Add("Content" ,typeof(String));
                dtMain.Columns.Add("TimeToTreat" ,typeof(Int32));
                dtMain.Columns.Add("Quantity" ,typeof(Int32));
                dtMain.Columns.Add("Price_Unit" ,typeof(decimal));
                dtMain.Columns.Add("Price_Root" ,typeof(decimal));

                dtMain.Columns.Add("Discount_Amount_Doctor" ,typeof(decimal));
                dtMain.Columns.Add("DiscountForCus" ,typeof(Int32));
                dtMain.Columns.Add("Price_Discounted" ,typeof(decimal));
                dtMain.Columns.Add("Telesale", typeof(Int32));
                dtMain.Columns.Add("Consult1", typeof(Int32));
                dtMain.Columns.Add("Consult2", typeof(Int32));
                dtMain.Columns.Add("Consult3", typeof(Int32));
                dtMain.Columns.Add("Consult4", typeof(Int32));
                dtMain.Columns.Add("KTV" ,typeof(Int32));
                dtMain.Columns.Add("Telesale2" ,typeof(Int32));
                dtMain.Columns.Add("dateCreated" ,typeof(DateTime));
                dtMain.Columns.Add("Patient" ,typeof(Int32));
                dtMain.Columns.Add("TreatmentPlan" ,typeof(Int32)); 

                dtMain.Columns.Add("IsChoose" ,typeof(Int32));
                dtMain.Columns.Add("ChooseUser" ,typeof(Int32));
                dtMain.Columns.Add("ChooseDate" ,typeof(DateTime));
                dtMain.Columns.Add("State" ,typeof(Int32));
                dtMain.Columns.Add("SourceService" ,typeof(Int32));

                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["Index"] = i + 1;

                    dr["ServiceID"] = Convert.ToInt32(DataMain.Rows[i]["ServiceID"]);
                    dr["Content"] = DataMain.Rows[i]["Content"];
                    dr["TimeToTreat"] = DataMain.Columns.Contains("TimeToTreat") ? Convert.ToInt32(DataMain.Rows[i]["TimeToTreat"]) : 0;
                    dr["Quantity"] = DataMain.Columns.Contains("Quantity") ? Convert.ToInt32(DataMain.Rows[i]["Quantity"]) : 0; 
                    dr["Price_Unit"] = DataMain.Columns.Contains("PriceUnit") ? Convert.ToInt32(DataMain.Rows[i]["PriceUnit"]) : 0;
                    dr["Price_Root"] = DataMain.Columns.Contains("PriceRoot") ? Convert.ToInt32(DataMain.Rows[i]["PriceRoot"]) : 0;
                    dr["Discount_Amount_Doctor"] = Convert.ToDecimal(DataMain.Rows[i]["DiscountAmountDoc"]);
                    dr["DiscountForCus"] = Convert.ToInt32(DataMain.Rows[i]["DiscountPer"]);
                    dr["Price_Discounted"] = Convert.ToDecimal(DataMain.Rows[i]["PriceDiscounted"]);

                    dr["Telesale"] = DataMain.Columns.Contains("Telesale") ? Convert.ToInt32(DataMain.Rows[i]["Telesale"]) : 0;
                    dr["Consult1"] = Convert.ToInt32(DataMain.Rows[i]["Consult1"]);
                    dr["Consult2"] = Convert.ToInt32(DataMain.Rows[i]["Consult2"]);
                    dr["Consult3"] = Convert.ToInt32(DataMain.Rows[i]["Consult3"]);
                    dr["Consult4"] = Convert.ToInt32(DataMain.Rows[i]["Consult4"]);
                    dr["KTV"] = Convert.ToInt32(DataMain.Rows[i]["KTV"]);
                    dr["Telesale2"] = Convert.ToInt32(DataMain.Rows[i]["Telesale2"]);

                    dr["dateCreated"] = Convert.ToDateTime(Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Rows[i]["dateCreated"].ToString()));
                    dr["Patient"] = Convert.ToInt32(DataMain.Rows[i]["Patient"]);
                    dr["TreatmentPlan"] = Convert.ToInt32(Plan);

                    dr["IsChoose"] = Convert.ToInt32(DataMain.Rows[i]["IsChoose"]);
                    dr["ChooseUser"] = (Convert.ToInt32(DataMain.Rows[i]["IsChoose"]) == 1) ? user.sys_userid : 0;

                    dr["ChooseDate"] = Comon.Comon.GetDateTimeNow();
                    dr["State"] = (Convert.ToInt32(DataMain.Rows[i]["IsChoose"]) == 1) ? 1 : 0;
                    dr["SourceService"] = Convert.ToInt32(DataMain.Rows[i]["SourceService"]);
                    dtMain.Rows.Add(dr);

                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_Customer_TabComboMulti_Insert" ,CommandType.StoredProcedure ,
                        "@Customer_ID" ,SqlDbType.Int ,CustomerID ,
                        "@ComboID" ,SqlDbType.Int ,ComboID ,
                        "@Patient" ,SqlDbType.Int ,Patient ,
                        "@Plan" ,SqlDbType.Int ,Plan ,
                        "@IsUsingPlan" ,SqlDbType.Int ,Comon.Global.sys_Treatment_Plan ,
                        "@Plan_Name" ,SqlDbType.NVarChar ,(Plan_Name != null) ? Plan_Name.Replace("'" ,"") : "" ,
                        "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                        "@Branch_ID" ,SqlDbType.Int ,user.sys_branchID ,
                        "@ChooseDateCustomer" ,SqlDbType.Int ,user.sys_ChooseDateCustomer.ToString() ,
                        "@table_data" ,SqlDbType.Structured ,(dtMain.Rows.Count > 0) ? dtMain : null);
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
