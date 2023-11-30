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
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MLunarCoffee.Pages.Broker
{
    public class BrokerDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string v = Request.Query["BrokerID"];
            if (v != "" && v != null)
            {
                _CurrentID = v == null ? "0" : v.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
        }
        //public async Task<IActionResult> OnPostLoadInit(string BrokerID)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt = await confunc.ExecuteDataTable("[MLU_sp_Broker_LoadCombo_Employee]", CommandType.StoredProcedure
        //                , "@BrokerID", SqlDbType.Int, 0
        //            );
        //        }

        //        return Content(Comon.DataJson.Datatable(dt));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("[]");
        //    }

        //}
        public async Task<IActionResult> OnPostLoadUpdate(string BrokerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Broker_Detail]" ,CommandType.StoredProcedure
                    ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(BrokerID));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostActionDisabled(string id ,string Type, int MatchPhone)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Broker_ActionDisabled]" ,CommandType.StoredProcedure
                    ,"@CurrentID" ,SqlDbType.NVarChar ,id
                    ,"@Type" ,SqlDbType.Int ,Convert.ToInt32(Type)
                    ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                    ,"@MatchPhone" , SqlDbType.Int, MatchPhone
                    );
                }

                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadBrokerSearching(string keyword,string type)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Broker_Searching]" , CommandType.StoredProcedure
                    ,"@type" ,SqlDbType.Int ,type != null ? Convert.ToInt32(type) : 0
                    , "@searchText", SqlDbType.NVarChar, keyword.Replace("'", "").Trim());
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                BrokerNew DataMain = JsonConvert.DeserializeObject<BrokerNew>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    if (CurrentID == "0")
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Broker_Insert]" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.NVarChar ,Convert.ToInt32(DataMain.CustomerID) ,
                            "@EmployeeID" ,SqlDbType.NVarChar ,Convert.ToInt32(DataMain.EmployeeID) ,
                            "@BrokerName" ,SqlDbType.NVarChar ,DataMain.NameBroker.Replace("'" ,"") ,
                            "@BrokerPhone" ,SqlDbType.NVarChar ,DataMain.PhoneBroker.Replace("'" ,"") ,
                            "@BrokerPhone2" ,SqlDbType.NVarChar ,DataMain.PhoneBroker2.Replace("'" ,"") ,
                            "@BrokerMail" ,SqlDbType.NVarChar ,DataMain.EmailBroker.Replace("'" ,"") ,
                            "@BrokerAddress" ,SqlDbType.NVarChar ,DataMain.AddressBroker.Replace("'" ,"").Trim() ,
                            "@BrokerNote" ,SqlDbType.NVarChar ,DataMain.NoteBroker.Replace("'" ,"").Trim() ,
                            "@Image" ,SqlDbType.NVarChar ,DataMain.Image.ToString() ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Matchphone" ,SqlDbType.Int ,DataMain.Matchphone ,
                            "@Code" ,SqlDbType.NVarChar ,DataMain.Code.Replace("'" ,"").Trim().ToUpper()
                        );
                    }
                    else {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Broker_Update]" ,CommandType.StoredProcedure ,
                            "@CustomerID" ,SqlDbType.NVarChar ,Convert.ToInt32(DataMain.CustomerID) ,
                            "@EmployeeID" ,SqlDbType.NVarChar ,Convert.ToInt32(DataMain.EmployeeID) ,
                            "@BrokerName" ,SqlDbType.NVarChar ,DataMain.NameBroker.Replace("'" ,"") ,
                            "@BrokerCode" ,SqlDbType.NVarChar ,DataMain.Code.Replace("'" ,"").Trim().ToUpper() ,
                            "@BrokerPhone" ,SqlDbType.NVarChar ,DataMain.PhoneBroker.Replace("'" ,"") ,
                            "@BrokerPhone2" ,SqlDbType.NVarChar ,DataMain.PhoneBroker2.Replace("'" ,"") ,
                            "@BrokerMail" ,SqlDbType.NVarChar ,DataMain.EmailBroker.Replace("'" ,"") ,
                            "@BrokerAddress" ,SqlDbType.NVarChar ,DataMain.AddressBroker.Replace("'" ,"").Trim() ,
                            "@BrokerNote" ,SqlDbType.NVarChar ,DataMain.NoteBroker.Replace("'" ,"").Trim() ,
                            "@Image" ,SqlDbType.NVarChar ,DataMain.Image.ToString() ,
                            "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Matchphone" ,SqlDbType.Int ,DataMain.Matchphone ,
                            "@Code" ,SqlDbType.NVarChar ,DataMain.Code.Replace("'" ,"").Trim().ToUpper(),
                            "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
                        );
                    }
                    
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
 
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
    }
    public class BrokerNew
    {
        public string NameBroker { get; set; }
        public string PhoneBroker { get; set; }
        public string PhoneBroker2 { get; set; }
        public string Image { get; set; }
        public string CustomerID { get; set; }
        public string AddressBroker { get; set; }
        public string NoteBroker { get; set; }
        public string EmailBroker { get; set; }
        public string EmployeeID { get; set; }
        public string Code { get; set; }
        public string Matchphone { get; set; }

        
    }
}
