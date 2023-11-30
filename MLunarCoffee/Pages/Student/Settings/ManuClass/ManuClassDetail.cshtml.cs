﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Settings.ManuClass
{
    public class ManuClassDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _CurrentID = (curr != null) ? curr.ToString() : "0";
        }


        public async Task<IActionResult> OnPostLoadData(string currentid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Settings_ManuClass_LoadDetail]", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, Convert.ToInt32(currentid));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadInitializeData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Product_Combo_Branch]", CommandType.StoredProcedure);
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
        public async Task<IActionResult> OnPostExcuteDataDetail(string data, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                Source DataMain = JsonConvert.DeserializeObject<Source>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_ST_Settings_ManuClass_Insert", CommandType.StoredProcedure,
                          "@BranchID", SqlDbType.Int, DataMain.BranchID,
                          "@Code", SqlDbType.NVarChar, DataMain.Code.ToString().Replace("'", "").Trim(),
                          "@Name", SqlDbType.NVarChar, DataMain.Name.ToString().Replace("'", "").Trim(),
                          "@Note", SqlDbType.NVarChar, DataMain.Note.ToString().Replace("'", "").Trim(),
                          "@UserID", SqlDbType.Int, user.sys_userid
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_ST_Settings_ManuClass_Update", CommandType.StoredProcedure,
                          "@CurrentID", SqlDbType.Int, CurrentID,
                          "@BranchID", SqlDbType.Int, DataMain.BranchID,
                          "@Code", SqlDbType.NVarChar, DataMain.Code.ToString().Replace("'", "").Trim(),
                          "@Name", SqlDbType.NVarChar, DataMain.Name.ToString().Replace("'", "").Trim(),
                          "@Note", SqlDbType.NVarChar, DataMain.Note.ToString().Replace("'", "").Trim(),
                          "@UserID", SqlDbType.Int, user.sys_userid
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
    }
    public class Source
    {
        public int BranchID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
