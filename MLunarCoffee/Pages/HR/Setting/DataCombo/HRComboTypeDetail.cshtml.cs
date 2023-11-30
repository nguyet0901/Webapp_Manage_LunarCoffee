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


namespace MLunarCoffee.Pages.HR.Setting.DataCombo
{
    public class HRComboTypeDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _TypeStatusID { get; set; }

        public void OnGet()
        {
            string ser = Request.Query["TypeStatusID"];
            string curr = Request.Query["CurrentID"];
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
        }

         public async Task<IActionResult> OnPostLoadInitializeData(string id)
        {
            try { 
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt =await  confunc.ExecuteDataTable("[MLU_sp_HR_ComboType_LoadTypeParent]", CommandType.StoredProcedure
                         , "@TypeStatusID", SqlDbType.NVarChar, id);

                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try { 
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("MLU_sp_HR_ComboType_LoadDetail", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                
                }
            }
            catch (Exception ex)
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
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Setting_HR_ComboType_Insert]", CommandType.StoredProcedure,
                          "@StatusParentID", SqlDbType.Int, DataMain.IdParent,
                          "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                          "@TypeStatusID", SqlDbType.Int, TypeStatusID,
                          "@UserID", SqlDbType.Int, user.sys_userid,
                          "@Created", SqlDbType.Int, DateTime.Now
                   );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content( "");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Setting_HR_ComboType_Update]", CommandType.StoredProcedure,
                            "@Name ", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                            "@ParentID ", SqlDbType.Int, DataMain.IdParent,
                            "@User_ID", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Modified", SqlDbType.Int, DateTime.Now
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
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