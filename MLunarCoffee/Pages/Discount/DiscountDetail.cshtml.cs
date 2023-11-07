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

namespace MLunarCoffee.Pages.Discount
{
    public class DiscountDetailModel : PageModel
    {
        public string _CurrentDiscountID { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _CurrentDiscountID = curr.ToString();
            }
            else
            {
                _CurrentDiscountID = "0";
            }
        }


        public async Task<IActionResult> OnPostLoadInitialize(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                //load combo
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                      , "@UserID", SqlDbType.Int, user.sys_userid
                      , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "cbbBranch";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Discount_Service_Type_LoadList", CommandType.StoredProcedure);
                    dt.TableName = "ServiceType";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Discount_Service_LoadList", CommandType.StoredProcedure);
                    dt.TableName = "Service";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Discount_LoadDetail]", CommandType.StoredProcedure,
                     "@Current", SqlDbType.Int, Convert.ToInt32(CurrentID));
                    dt.TableName = "dataDetail";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
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
                DiscountDetail DataMain = JsonConvert.DeserializeObject<DiscountDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("YYY_sp_Discount_Insert", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                             "@Rule", SqlDbType.NVarChar, DataMain.Rule,
                             "@IsAllBranch", SqlDbType.Int, DataMain.IsAllBranch,
                             "@Numberuse", SqlDbType.Int, DataMain.Numberuse,
                             "@Numberfrom", SqlDbType.Int, DataMain.Numberfrom,
                             "@Numberto", SqlDbType.Int, DataMain.Numberto,
                             "@DiscountBrachID", SqlDbType.Int, DataMain.DiscountBranchID,
                             "@Date_From", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateFrom),
                             "@Date_To", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateTo),
                             "@Note", SqlDbType.NVarChar, DataMain.Content
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("YYY_sp_Discount_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                             "@CurrentID", SqlDbType.Int, CurrentID,
                             "@Rule", SqlDbType.NVarChar, DataMain.Rule,
                             "@IsAllBranch", SqlDbType.Int, DataMain.IsAllBranch,
                             "@Numberuse", SqlDbType.Int, DataMain.Numberuse,
                             "@Numberfrom", SqlDbType.Int, DataMain.Numberfrom,
                             "@Numberto", SqlDbType.Int, DataMain.Numberto,
                             "@DiscountBrachID", SqlDbType.Int, DataMain.DiscountBranchID,
                             "@Date_From", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateFrom),
                             "@Date_To", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateTo),
                             "@Note", SqlDbType.NVarChar, DataMain.Content
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
    public class DiscountDetail
    {
        public string Name { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Content { get; set; }
        public string Rule { get; set; }
        public int IsAllBranch { get; set; }
        public string DiscountBranchID { get; set; }
        public int Numberuse { get; set; }
        public int Numberfrom { get; set; }
        public int Numberto { get; set; }
    }
}