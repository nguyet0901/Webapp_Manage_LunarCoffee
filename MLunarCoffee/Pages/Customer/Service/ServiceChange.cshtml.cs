using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Customer.Service
{

    public class ServiceChangeModel : PageModel
    {
        public string _treatplan { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public string _ServiceCurrentID { get; set; }
        public string sys_DentistCosmetic { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);

            var curr = Request.Query["CurrentID"];
            var treatplan = Request.Query["Treatplan"];
            _treatplan = treatplan.ToString();
            sys_DentistCosmetic = Comon.Global.sys_DentistCosmetic.ToString();
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            _ServiceCurrentID = !String.IsNullOrEmpty(curr) ? curr.ToString() : "0";

        }

        public async Task<IActionResult> OnPostLoadServiceChange(int tabid)
        {
            try
            {
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataSet ds = new DataSet();
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_TabChange_Service]" ,CommandType.StoredProcedure ,
                      "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(tabid == 0 ? 0 : tabid));
                    return Content(Comon.DataJson.Dataset(ds));
                }

            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadata(int tabid)
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
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabChange_Reason]" ,CommandType.StoredProcedure);
                        dt.TableName = "ReasonChange";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSourceService = new DataTable();
                        dtSourceService = await confunc.ExecuteDataTable("[YYY_LoadCombo_Source_CustService]" ,CommandType.StoredProcedure);
                        dtSourceService.TableName = "SourceService";
                        return dtSourceService;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtStaff = new DataTable();
                        dtStaff = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabMulti_LoadComboTele]" ,CommandType.StoredProcedure);
                        dtStaff.TableName = "Tele";
                        return dtStaff;
                    }
                }));
                tasks.Add(Task.Run(async () =>
             {
                 using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                 {
                     DataTable dt = new DataTable();
                     dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabChange_Info]" ,CommandType.StoredProcedure ,
                          "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(tabid == 0 ? 0 : tabid));
                     dt.TableName = "ServiceCurrent";
                     return dt;
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
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID ,string Treatplan)
        {
            try
            {
                int DentistCosmetic = Comon.Global.sys_DentistCosmetic;
                ServiceChangeDetail DataMain = JsonConvert.DeserializeObject<ServiceChangeDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Customer_TabService_Change" ,CommandType.StoredProcedure ,
                             "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID) ,
                             "@Treatplan" ,SqlDbType.Int ,Convert.ToInt32(Treatplan) ,
                             "@reasonID" ,SqlDbType.Int ,DataMain.reasonID ,
                             "@serviceTabChangeID" ,SqlDbType.Int ,DataMain.serviceTabChange ,
                             "@newAmount" ,SqlDbType.Decimal ,Convert.ToDecimal(DataMain.newAmount) ,
                             "@Quantity" ,SqlDbType.Int ,DataMain.Quantity ,
                             "@TimeToTreatment" ,SqlDbType.Int ,DataMain.TimeToTreatment ,
                             "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                             "@branchID" ,SqlDbType.Int ,user.sys_branchID ,
                             "@DentistCosmetic" ,SqlDbType.Int ,DentistCosmetic ,
                             "@SourceService" ,SqlDbType.Int ,Convert.ToInt32(DataMain.SourceService) ,
                             "@ChanTelesale" ,SqlDbType.Int ,Convert.ToInt32(DataMain.ChanTelesale) ,
                             "@ChanTelesale1" ,SqlDbType.Int ,Convert.ToInt32(DataMain.ChanTelesale1) ,
                             "@ChanConsult" ,SqlDbType.Int ,Convert.ToInt32(DataMain.ChanConsult) ,
                             "@ChanConsult1" ,SqlDbType.Int ,Convert.ToInt32(DataMain.ChanConsult1) ,
                             "@ChanNote" ,SqlDbType.NVarChar ,DataMain.ChanNote != null ? DataMain.ChanNote : "" ,
                             "@ChooseDateCustomer" ,SqlDbType.Int ,user.sys_ChooseDateCustomer ,
                             "@Created" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Created.ToString())
                         );
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("Error");
            }
        }
        //public async Task<IActionResult> OnPostLoadDataComboService(string input, string CurrentID)
        //{
        //    try
        //    {
        //        if (input.Length < 3) return Content("[]");
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt = await confunc.ExecuteDataTable("MLU_sp_Customer_SearchService_ToChange", CommandType.StoredProcedure,
        //                "@input", SqlDbType.NVarChar, input.Trim(),
        //                "@CurrentID", SqlDbType.Int, CurrentID);
        //        }
        //        return Content(Comon.DataJson.Datatable(dt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("[]");
        //    }

        //}
    }
    public class ServiceChangeDetail
    {
        public int reasonID { get; set; }
        public int serviceTabChange { get; set; }
        public int Quantity { get; set; }
        public int TimeToTreatment { get; set; }
        public decimal newAmount { get; set; }
        public string Created { get; set; }
        public string SourceService { get; set; }
        public string ChanTelesale { get; set; }
        public string ChanTelesale1 { get; set; }
        public string ChanConsult { get; set; }
        public string ChanConsult1 { get; set; }
        public string ChanNote { get; set; }
    }
}
 

