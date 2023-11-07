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

namespace MLunarCoffee.Pages.Student.Settings.Discount
{
    public class DiscountDetailModel : PageModel
    {
        public string CurrentID { get; set; }
        public void OnGet()
        {
            string _CurrentID = Request.Query["CurrentID"];
            if (_CurrentID != null)
            {
                CurrentID = _CurrentID;
            }
            else CurrentID = "0";
        }
        public async Task<IActionResult> OnPostLoadInitialize()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                //load combo
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Training_Load", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid
                        , "@LoadNormal", SqlDbType.Int, 1);
                    dt.TableName = "cbbBranch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_ST_ListCourseCombo", CommandType.StoredProcedure);
                    dt.TableName = "ListCount";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_ST_Settings_Discount_LoadDetail", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string Data, string CurrentId)
        {
            try
            {
                DiscountDetail DataMain = JsonConvert.DeserializeObject<DiscountDetail>(Data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentId == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_ST_Settings_Discount_Insert", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name
                            , "@Code", SqlDbType.NVarChar, DataMain.Code
                            , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateFrom.ToString())
                            , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateTo.ToString())
                            , "@BranchToken", SqlDbType.NVarChar, DataMain.BranchToken
                            , "@IsAllBranch", SqlDbType.NVarChar, DataMain.IsAllBranch
                            , "@Note", SqlDbType.NVarChar, DataMain.Note
                            , "@Rule", SqlDbType.NVarChar, DataMain.Rule
                            , "@Created_By", SqlDbType.Int, user.sys_userid
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_ST_Settings_Discount_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name
                            , "@Code", SqlDbType.NVarChar, DataMain.Code
                            , "@DateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateFrom.ToString())
                            , "@DateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.DateTo.ToString())
                            , "@BranchToken", SqlDbType.NVarChar, DataMain.BranchToken
                            , "@IsAllBranch", SqlDbType.NVarChar, DataMain.IsAllBranch
                            , "@Note", SqlDbType.NVarChar, DataMain.Note
                            , "@Rule", SqlDbType.NVarChar, DataMain.Rule
                            , "@Created_By", SqlDbType.Int, user.sys_userid
                            , "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentId)
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
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
        public string Code { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string BranchToken { get; set; }
        public string IsAllBranch { get; set; }
        public string Note { get; set; }
        public string Rule { get; set; }
        public int Created_By { get; set; }
        public string Created { get; set; }
    }
}
