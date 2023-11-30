using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing.Finance
{
    public class ExpenseAdsDetailModel : PageModel
    {
        public string _expenseAdsDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _expenseAdsDetailID = curr.ToString();
            }
            else
            {
                _expenseAdsDetailID = "0";
            }
        }

        public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ServiceCare_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "ServiceCare";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                    , "@UserID", SqlDbType.Int, user.sys_userid
                    , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_MarketingChannel_LoadCompo]", CommandType.StoredProcedure);
                    dt.TableName = "Channel";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));

            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Marketing_ExpenseAds_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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


        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                ExpenseAds DataMain = JsonConvert.DeserializeObject<ExpenseAds>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("MLU_sp_Marketing_ExpenseAds_Insert", CommandType.StoredProcedure,
                            "@ServiceCare_ID", SqlDbType.Int, DataMain.ServiceCare_ID,
                            "@Branch_ID", SqlDbType.Int, DataMain.Branch_ID,
                            "@Date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.Date),
                            "@Data", SqlDbType.Int, DataMain.Data,
                            "@Display", SqlDbType.Int, DataMain.Display,
                            "@Click", SqlDbType.Int, DataMain.Click,
                            "@ChannelID", SqlDbType.Int, DataMain.Channel_ID,
                            "@Budget", SqlDbType.Decimal, DataMain.Budget,
                            "@Cost", SqlDbType.Decimal, DataMain.Cost,
                            "@Revenue", SqlDbType.Decimal, DataMain.Revenue,
                            "@NumberCmt", SqlDbType.Int, DataMain.NumberCmt,
                            "@NumberInbox", SqlDbType.Int, DataMain.NumberInbox,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("MLU_sp_Marketing_ExpenseAds_Update", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@ServiceCare_ID", SqlDbType.Int, DataMain.ServiceCare_ID,
                            "@Branch_ID", SqlDbType.Int, DataMain.Branch_ID,
                            "@Date", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.Date),
                            "@Data", SqlDbType.Int, DataMain.Data,
                            "@Display", SqlDbType.Int, DataMain.Display,
                            "@Click", SqlDbType.Int, DataMain.Click,
                            "@ChannelID", SqlDbType.Int, DataMain.Channel_ID,
                            "@Budget", SqlDbType.Decimal, DataMain.Budget,
                            "@Cost", SqlDbType.Decimal, DataMain.Cost,
                            "@Revenue", SqlDbType.Decimal, DataMain.Revenue,
                            "@NumberCmt", SqlDbType.Int, DataMain.NumberCmt,
                            "@NumberInbox", SqlDbType.Int, DataMain.NumberInbox,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
                    );
                    }
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
    public class ExpenseAds
    {
        public string ServiceCare_ID { get; set; }
        public string Date { get; set; }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        public decimal Budget { get; set; }
        public string Branch_ID { get; set; }
        public int NumberCmt { get; set; }
        public int NumberInbox { get; set; }
        public int Data { get; set; }
        public int Click { get; set; }
        public int Display { get; set; }
        public int Channel_ID { get; set; }
    }
}
