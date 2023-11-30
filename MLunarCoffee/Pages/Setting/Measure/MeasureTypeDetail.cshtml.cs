﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.Measure.MeasureTypeDetail
{
    public class MesureTypeDetailModel : PageModel
    {
        public string _MsTypeID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _MsTypeID = curr.ToString();
            }
            else
            {
                _MsTypeID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_MeasureType_LoadDetail]", CommandType.StoredProcedure,
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
            catch
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string CurrentID,string DataDetail)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                MSType DataMain = JsonConvert.DeserializeObject<MSType>(DataDetail);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_MeasureType_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name,
                            "@Description", SqlDbType.NVarChar, DataMain.Description,
                            "@MessureUnit", SqlDbType.NVarChar, DataMain.MeasureUnit,
                            "@IsCalDiff", SqlDbType.Int, DataMain.IsCalDiff,
                            "@UserID", SqlDbType.Int, user.sys_userid
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
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_MeasureType_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name,
                            "@Description", SqlDbType.NVarChar, DataMain.Description,
                            "@MessureUnit", SqlDbType.NVarChar, DataMain.MeasureUnit,
                            "@IsCalDiff", SqlDbType.Int, DataMain.IsCalDiff,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@currentID ", SqlDbType.Int, CurrentID

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
        public class MSType {
            public string Name { get; set; }
            public string Description { get; set; }
            public string MeasureUnit { get; set; }
            public int IsCalDiff { get; set; }
        }
    }
}
