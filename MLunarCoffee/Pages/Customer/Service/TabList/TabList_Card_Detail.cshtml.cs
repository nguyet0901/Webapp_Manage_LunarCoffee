using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.Service.TabList
{
    public class TabList_Card_DetailModel : PageModel
    {
        public string _CardCustomerID { get; set; }
        public string _CardCurrentID { get; set; }
        public string _Type { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];
            var type = Request.Query["Type"];
            if (type.ToString() != String.Empty) _Type = type.ToString();
            else _Type = "";
            if (cus.ToString() != String.Empty && curr.ToString() != String.Empty)
            {
                _CardCustomerID = cus.ToString();
                _CardCurrentID = curr.ToString();
            }
            else if (cus.ToString() != String.Empty)
            {
                _CardCustomerID = cus.ToString();
                _CardCurrentID = "0";
            }
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoadComboMain(int CustomerID)
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
                        DataTable dtServiceCard = new DataTable();
                        dtServiceCard = await confunc.ExecuteDataTable("[MLU_sp_Service_Card_Customer_Combo]", CommandType.StoredProcedure
                             , "@CustomerID", SqlDbType.Int, CustomerID);
                        dtServiceCard.TableName = "ServiceCard";
                        return dtServiceCard;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtEmployeeCard = new DataTable();
                        dtEmployeeCard = await confunc.LoadEmployeeFull(user.sys_branchID, user.sys_AllBranchID);
                        dtEmployeeCard.TableName = "EmployeeCard";
                        return dtEmployeeCard;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSourceService = new DataTable();
                        dtSourceService = await confunc.ExecuteDataTable("[YYY_LoadCombo_Source_CustService]", CommandType.StoredProcedure);
                        dtSourceService.TableName = "SourceService";
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
        public async Task<IActionResult> OnPostLoadDetail(int CardCurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Card_LoadDetail]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(CardCurrentID == 0 ? 0 : CardCurrentID));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostExcuteData(string data, string CustomerID, string CurrentID)
        {
            try
            {
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                int UsingIssue = Convert.ToInt32(DataMain.Rows[0]["UsingIssue"]);
                string CodeCardQR = "0";
                if(UsingIssue==1 && DataMain.Rows[0]["CodeCardQR"] !=null && DataMain.Rows[0]["CodeCardQR"].ToString() !="" )
                {
                    CodeCardQR = DataMain.Rows[0]["CodeCardQR"].ToString();
                }
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {

                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Card_Update]", CommandType.StoredProcedure,
                             "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                             "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID),
                             "@CardID", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["CardID"]),
                             "@ConsultID", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["ConsultID"]),
                             "@ConsultID1", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["ConsultID1"]),
                             "@ConsultID2", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["ConsultID2"]) ,
                             "@SourceService" , SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["SourceService"]),
                             "@PriceDiscounted", SqlDbType.Decimal, DataMain.Rows[0]["PriceDiscounted"],
                             "@PriceRoot", SqlDbType.Decimal, DataMain.Rows[0]["PriceRoot"],
                             "@AmountConsult", SqlDbType.Decimal, DataMain.Rows[0]["AmountConsult"],
                             "@PerConsult", SqlDbType.Decimal, DataMain.Rows[0]["PerConsult"],
                             "@Quantity", SqlDbType.Int, DataMain.Rows[0]["Quantity"],
                             "@PriceUse", SqlDbType.Decimal, DataMain.Rows[0]["PriceUse"],
                             "@PerDiscount", SqlDbType.Int, DataMain.Rows[0]["PerDiscount"],
                             "@AmountDiscount", SqlDbType.Decimal, DataMain.Rows[0]["AmountDiscount"],
                             "@EndLess", SqlDbType.Int, DataMain.Rows[0]["EndLess"],
                             "@NumExpired", SqlDbType.Int, DataMain.Rows[0]["NumExpired"],
                             "@IsTimesUsed", SqlDbType.Int, DataMain.Rows[0]["IsTimesUsed"],
                             "@TimesUsed", SqlDbType.Int, DataMain.Rows[0]["TimesUsed"],
                             "@Note", SqlDbType.NVarChar, DataMain.Rows[0]["Note"],
                             "@BranchID", SqlDbType.Int, user.sys_branchID,
                             "@Modified_By", SqlDbType.Int, user.sys_userid,
                             "@CodeCardPrint", SqlDbType.NVarChar, DataMain.Rows[0]["CodeCardPrint"],
                             "@CodeCard", SqlDbType.NVarChar, DataMain.Rows[0]["CodeCard"],
                             "@CodeCardQR", SqlDbType.NVarChar,CodeCardQR
                             );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Card_Insert]", CommandType.StoredProcedure,
                             "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID),
                             "@CardID", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["CardID"]),
                             "@ConsultID" ,SqlDbType.Int ,Convert.ToInt32(DataMain.Rows[0]["ConsultID"]) ,
                             "@ConsultID1" ,SqlDbType.Int ,Convert.ToInt32(DataMain.Rows[0]["ConsultID1"]) ,
                             "@ConsultID2" ,SqlDbType.Int ,Convert.ToInt32(DataMain.Rows[0]["ConsultID2"]) ,
                             "@SourceService" , SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["SourceService"]),
                             "@PriceDiscounted", SqlDbType.Decimal, DataMain.Rows[0]["PriceDiscounted"],
                             "@PriceRoot", SqlDbType.Decimal, DataMain.Rows[0]["PriceRoot"],
                             "@AmountConsult", SqlDbType.Decimal, DataMain.Rows[0]["AmountConsult"],
                             "@PerConsult", SqlDbType.Decimal, DataMain.Rows[0]["PerConsult"],
                             "@Quantity", SqlDbType.Int, DataMain.Rows[0]["Quantity"],
                             "@PriceUse", SqlDbType.Decimal, DataMain.Rows[0]["PriceUse"],
                             "@PerDiscount", SqlDbType.Int, DataMain.Rows[0]["PerDiscount"],
                             "@AmountDiscount", SqlDbType.Decimal, DataMain.Rows[0]["AmountDiscount"],
                             "@EndLess", SqlDbType.Int, DataMain.Rows[0]["EndLess"],
                             "@NumExpired", SqlDbType.Int, DataMain.Rows[0]["NumExpired"],
                             "@IsTimesUsed", SqlDbType.Int, DataMain.Rows[0]["IsTimesUsed"],
                             "@TimesUsed", SqlDbType.Int, DataMain.Rows[0]["TimesUsed"],
                             "@Note", SqlDbType.NVarChar, DataMain.Rows[0]["Note"],
                             "@BranchID", SqlDbType.Int, user.sys_branchID,
                             "@Created_By", SqlDbType.Int, user.sys_userid,
                             "@IsChooseDate", SqlDbType.Int, user.sys_ChooseDateCustomer,
                             "@Created", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Rows[0]["Created"].ToString()),
                             "@CodeCardPrint", SqlDbType.NVarChar, DataMain.Rows[0]["CodeCardPrint"],
                             "@CodeCardQR", SqlDbType.NVarChar, CodeCardQR
                             );
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostTogglelockCard(int id, int type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_ServiceCard_ChangeLock]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Type", SqlDbType.Int, type,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid
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
