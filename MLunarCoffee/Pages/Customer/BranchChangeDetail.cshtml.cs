using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System.Xml.Linq;

namespace MLunarCoffee.Pages.Customer
{
    public class BranchChangeDetailModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostInitialize(string CustID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("MLU_sp_Customer_ChangeBranch_Load" ,CommandType.StoredProcedure
                        ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                        ,"@currentBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                        ,"@areaBranch" ,SqlDbType.NVarChar ,user.sys_AreaBranch
                        ,"@custID" ,SqlDbType.NVarChar ,CustID
                        );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string custID,string branchid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_ChangeBranch_Update]" ,CommandType.StoredProcedure ,
                        "@CustID" ,SqlDbType.NVarChar ,custID,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid,
                        "@BranchID " ,SqlDbType.Int ,branchid

                    );
                    if (dt.Rows.Count > 0)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                    else
                    {
                        return Content("0");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
