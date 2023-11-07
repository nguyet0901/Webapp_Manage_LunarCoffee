using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Email
{
    public class EmailTemplateModel : PageModel
    {
        public string _HasOption { get; set; }
        public void OnGet()
        {
            var hasoption = Request.Query["hasoption"];
            _HasOption = hasoption.ToString() != null ? hasoption.ToString() : "0";
        }

        public async Task<IActionResult> OnPostInitialize(string type)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Mail_LoadOption", CommandType.StoredProcedure);
                    dt.TableName = "Option";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Mail_Multi_LoadBranch", CommandType.StoredProcedure
                        , "@isAllBranch", SqlDbType.Int, user.sys_AllBranchID
                        , "@currentBranch", SqlDbType.Int, user.sys_branchID
                        , "@areaBranch", SqlDbType.NVarChar, user.sys_AreaBranch);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string type, int masterID, int ID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_ST_Mail_LoadDataSend_V2", CommandType.StoredProcedure
                        , "@ID", SqlDbType.Int, ID
                        , "@MasterID", SqlDbType.Int, masterID
                        , "@Type", SqlDbType.NVarChar, type);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostUpdateSend(int paymentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_ST_Mail_UpdateIsMail", CommandType.StoredProcedure
                        , "@PaymentID", SqlDbType.Int, paymentID
                        , "@UserID", SqlDbType.Int, user.sys_userid);
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
