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

namespace MLunarCoffee.Pages.Setting.StatusCustomer
{
    public class CustomerCareGroupStatusDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _TypeStatusID { get; set; }
        public void OnGet()
        {
            string ser = Request.Query["TypeStatusID"];
            string curr = Request.Query["CurrentID"];
            _CurrentID = curr != null ? curr.ToString() : "0";
            _TypeStatusID = ser != null ? ser.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_CustomerCareStatus_LoadDetail", CommandType.StoredProcedure,
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

        public async Task<IActionResult> OnPostExcuteDataStatusParent(string data, string CurrentID, string TypeStatusID)
        {
            try
            {
                CustomerGroupParentDetail DataMain = JsonConvert.DeserializeObject<CustomerGroupParentDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_CustomerCareStatus_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.StatusName.Replace("'", "").Trim(),
                            "@ColorCode", SqlDbType.NVarChar, DataMain.ColorCode,
                            "@TypeStatusID", SqlDbType.Int, TypeStatusID,
                            "@UserID", SqlDbType.Int, user.sys_userid,
                            "@Created", SqlDbType.Int, Comon.Comon.GetDateTimeNow(),
                            "@IsChooseEmp", SqlDbType.Int, DataMain.IsChooseEmp
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_CustomerCareStatus_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.StatusName.Replace("'", "").Trim(),
                            "@ColorCode", SqlDbType.NVarChar, DataMain.ColorCode,
                            "@User_ID", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@TypeStatusID", SqlDbType.Int, TypeStatusID,
                            "@Modified", SqlDbType.Int, Comon.Comon.GetDateTimeNow(),
                            "@IsChooseEmp", SqlDbType.Int, DataMain.IsChooseEmp
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
        public class CustomerGroupParentDetail
        {
            public string StatusName { get; set; }
            public string ColorCode { get; set; }
            public int IsChooseEmp { get; set; }
        }
    }

}
