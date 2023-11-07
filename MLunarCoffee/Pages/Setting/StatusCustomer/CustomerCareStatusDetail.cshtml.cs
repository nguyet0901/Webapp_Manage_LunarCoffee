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
    public class CustomerCareStatusDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _TypeStatusID { get; set; }
        public string _GroupStatusID { get; set; }
        public void OnGet()
        {
            string ser = Request.Query["TypeStatusID"];
            string curr = Request.Query["CurrentID"];
            string groupid = Request.Query["GroupStatusID"];
            if (curr != null)
            {
                _CurrentID = curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
            if (ser != null)
            {
                _TypeStatusID = ser.ToString();
            }
            else
            {
                _TypeStatusID = "0";
            }
            if (groupid != null)
            {
                _GroupStatusID = groupid.ToString();
            }
            else
            {
                _GroupStatusID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadInitializeData(string TypeStatusID)
        {
            try
            {

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCareStatus_LoadComboParent]", CommandType.StoredProcedure
                         , "@TypeStatusID", SqlDbType.NVarChar, TypeStatusID);

                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_CustomerCareStatus_LoadDetail", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostExcuteDataStatusChildren(string data, string CurrentID, string TypeStatusID)
        {
            try
            {
                CustomerGroupDetail DataMain = JsonConvert.DeserializeObject<CustomerGroupDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_CustomerCareStatus_Insert]", CommandType.StoredProcedure,
                            "@StatusParentID", SqlDbType.Int, DataMain.IdParent,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@TypeStatusID", SqlDbType.Int, TypeStatusID,
                            "@UserID", SqlDbType.Int, user.sys_userid,
                            "@Created", SqlDbType.Int, DateTime.Now
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
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_CustomerCareStatus_Update]", CommandType.StoredProcedure,
                            "@Name ", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                            "@ParentID ", SqlDbType.Int, DataMain.IdParent,
                            "@User_ID", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@TypeStatusID", SqlDbType.Int, TypeStatusID,
                            "@Modified", SqlDbType.Int, DateTime.Now
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

            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
        public class CustomerGroupDetail
        {
            public string Name { get; set; }
            public string IdParent { get; set; }
        }
    }
}