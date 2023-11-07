using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.HR.Checkin
{
    public class CheckinDetail : PageModel
    {

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataSet ds = new DataSet();
                var _user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_HR_CheckIn_LoadDataDetail]", CommandType.StoredProcedure
                            , "@EmpID", SqlDbType.Int, _user.sys_employeeid);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
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
        public async Task<IActionResult> OnPostExcuteData(string data)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                CheckinDetailData DataMain = JsonConvert.DeserializeObject<CheckinDetailData>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_HR_CheckIn_Update_v2]" ,CommandType.StoredProcedure ,
                        "@Shift" ,SqlDbType.Int ,DataMain.Shift ,
                        "@BranchID" ,SqlDbType.Int ,DataMain.BranchID ,
                        "@EmpID" ,SqlDbType.Int ,user.sys_employeeid ,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content("1");


            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

    }
    public class CheckinDetailData
    {        
        public int Shift { get; set; }
        public int BranchID { get; set; }

    }
}
