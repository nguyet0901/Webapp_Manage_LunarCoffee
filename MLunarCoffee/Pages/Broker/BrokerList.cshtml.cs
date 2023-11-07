using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Broker
{
    public class BrokerListModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = (_layout != null) ? _layout : "";
        }

        public async Task<IActionResult> OnPostLoadList(int dateType ,int limit ,int beginID ,string dateFrom ,string dateTo ,int brokerID ,int isSearchAll ,string textSearch)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Broker_LoadList_V2]" ,CommandType.StoredProcedure
                        ,"@DateType" ,SqlDbType.Int ,dateType
                        ,"@DateFrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom ?? Comon.Comon.DateTimeYMD_To_DMY(DateTime.Now))
                        ,"@DateTo" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo ?? Comon.Comon.DateTimeYMD_To_DMY(DateTime.Now))
                        ,"@BrokerID" ,SqlDbType.Int ,brokerID
                        ,"@IsSearchAll" ,SqlDbType.Int ,isSearchAll
                        ,"@textSearch" ,SqlDbType.NVarChar ,textSearch ?? ""
                        ,"@limit" ,SqlDbType.Int ,limit
                        ,"@idbegin" ,SqlDbType.Int ,beginID);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(int BrokerID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>
                {
                    Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtInfo = new DataTable();
                            dtInfo = await confunc.ExecuteDataTable("[YYY_sp_Broker_LoadDetail_Info]" ,CommandType.StoredProcedure
                              ,"@BrokerID" ,SqlDbType.Int ,BrokerID);
                            dtInfo.TableName = "Info";
                            return dtInfo;
                        }
                    }) ,
                    Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = new DataTable();
                            dt = await confunc.ExecuteDataTable("[YYY_sp_Broker_LoadDetail]" ,CommandType.StoredProcedure
                              ,"@BrokerID" ,SqlDbType.Int ,BrokerID
                              ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                              ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate);
                            dt.TableName = "Payment";
                            return dt;
                        }
                    }),
                    Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtPDetail = new DataTable();
                            dtPDetail = await confunc.ExecuteDataTable("[YYY_sp_Broker_LoadPayDetail]" ,CommandType.StoredProcedure
                              ,"@BrokerID" ,SqlDbType.Int ,BrokerID);
                            dtPDetail.TableName = "PaymentDetail";
                            return dtPDetail;
                        }
                    })
                };
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }


        public async Task<IActionResult> OnPostLoadPoint(int brokerID ,int custID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_Broker_LoadDetail_Point]" ,CommandType.StoredProcedure
                      ,"@BrokerID" ,SqlDbType.Int ,brokerID
                      ,"@CustID" ,SqlDbType.Int ,custID
                      ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                      ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
        public async Task<IActionResult> OnPostDeleteItemBroker(int id ,int type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Customer_PaymentBroker_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@Type" ,SqlDbType.Int ,type ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
