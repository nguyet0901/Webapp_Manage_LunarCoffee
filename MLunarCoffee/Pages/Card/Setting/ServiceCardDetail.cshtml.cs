using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using System.Web;
//using System.Web.Script.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Card.Setting
{
    public class ServiceCardDetailModel : PageModel
    {
        public string layout { get; set; }
        public string _ServiceCardCurrentID { get; set; }
        public string _DataMembership { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = (_layout != null) ? _layout : "";
            CurrentPath = HttpContext.Request.Path;
            var user = Session.GetUserSession(HttpContext);
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _ServiceCardCurrentID = curr.ToString();
            }
            else
            {
                _ServiceCardCurrentID = "0";
            }


        }


        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ServiceCard_LoadService]", CommandType.StoredProcedure);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }



        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[YYY_sp_ServiceCard_LoadDetail]", CommandType.StoredProcedure,
                        "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }

                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_ServiceCard_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@UserID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                DataServiceCard DataMain = JsonConvert.DeserializeObject<DataServiceCard>(data);
                //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                DataTable DataServiceStage = new DataTable();
                DataTable dtResult = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("YYY_sp_ServiceCardDetail_Insert", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.NameServiceCard.Replace("'", "").Trim(),
                            "@Price_Sale", SqlDbType.Decimal, DataMain.Price_Sale,
                            "@Price_Use", SqlDbType.Decimal, DataMain.Price_Use,
                            "@AmountConsult", SqlDbType.Decimal, DataMain.AmountConsult,
                            "@PerConsult", SqlDbType.Decimal, DataMain.PerConsult,
                            "@Content", SqlDbType.Int, DataMain.CardNote.Replace("'", "").Trim(),
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@EndLess", SqlDbType.Int, DataMain.EndLess,
                            "@ExpiredDate", SqlDbType.Int, DataMain.ExpiredDate,
                            "@AllService", SqlDbType.Int, DataMain.AllService,
                            "@ServiceRange", SqlDbType.Int, DataMain.ServiceRange,
                            "@IsTimesUsed", SqlDbType.Int, DataMain.IsTimesUsed,
                            "@TimesUsed", SqlDbType.Int, DataMain.TimesUsed,
                            "@Card_Code", SqlDbType.Int, DataMain.Card_Code.Trim(),
                            "@isFinishPay", SqlDbType.Int, Convert.ToInt32(DataMain.isFinishPay),
                            "@CardCodePrint", SqlDbType.NVarChar, DataMain.Card_CodePrint,
                            "@IsIssue",SqlDbType.Int,DataMain.IsIssue,
                            "@FamilyUsing",SqlDbType.Int,DataMain.FamilyUsing,
                            "@IssueQuantity" ,SqlDbType.Int,DataMain.IssueQuantity,
                            "@IssueDate",SqlDbType.NVarChar,Comon.Comon.DateTimeDMY_To_YMD(DataMain.IssueDate)
                        );
                    }
                }
                else
                {
                    //int datatext = DataMembership.Rows.Count;
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtResult = await connFunc.ExecuteDataTable("YYY_sp_ServiceCardDetail_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.NameServiceCard.Replace("'", "").Trim(),
                            "@Price_Sale", SqlDbType.Decimal, DataMain.Price_Sale,
                            "@Price_Use", SqlDbType.Decimal, DataMain.Price_Use,
                            "@AmountConsult", SqlDbType.Decimal, DataMain.AmountConsult,
                            "@PerConsult", SqlDbType.Decimal , DataMain.PerConsult,
                            "@Content", SqlDbType.Int, DataMain.CardNote.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@EndLess", SqlDbType.Int, DataMain.EndLess,
                            "@ExpiredDate", SqlDbType.Int, DataMain.ExpiredDate,
                            "@AllService", SqlDbType.Int, DataMain.AllService,
                            "@ServiceRange", SqlDbType.Int, DataMain.ServiceRange,
                            "@IsTimesUsed", SqlDbType.Int, DataMain.IsTimesUsed,
                            "@TimesUsed", SqlDbType.Int, DataMain.TimesUsed,
                            "@Card_Code", SqlDbType.Int, DataMain.Card_Code.Trim(),
                            "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                            "@isFinishPay", SqlDbType.Int, Convert.ToInt32(DataMain.isFinishPay),
                            "@CardCodePrint", SqlDbType.NVarChar, DataMain.Card_CodePrint,
                            "@IsIssue",SqlDbType.Int,DataMain.IsIssue,
                            "@FamilyUsing" ,SqlDbType.Int ,DataMain.FamilyUsing ,
                            "@IssueQuantity" ,SqlDbType.Int,DataMain.IssueQuantity,
                            "@IssueDate",SqlDbType.NVarChar,Comon.Comon.DateTimeDMY_To_YMD(DataMain.IssueDate)
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dtResult));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

    public class DataServiceCard
    {
        public string NameServiceCard { get; set; }
        public string Price_Sale { get; set; }
        public string Price_Use { get; set; }
        public string AmountConsult { get; set; }
        public string PerConsult { get; set; }

        public string CardNote { get; set; }
        public int EndLess { get; set; }
        public int ExpiredDate { get; set; }
        public int AllService { get; set; }
        public string ServiceRange { get; set; }
        public int IsTimesUsed { get; set; }
        public int TimesUsed { get; set; }
        public string Card_Code { get; set; }
        public string isFinishPay { get; set; }
        public string Card_CodePrint { get; set; }
        public string IsIssue { get; set; }
        public string FamilyUsing { get; set; }
        public string IssueDate { get; set; }
        public string IssueQuantity { get; set; }
    }
}
