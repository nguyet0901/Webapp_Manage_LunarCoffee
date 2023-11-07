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

namespace MLunarCoffee.Pages.WareHouse.Setting
{
    public class WareHouseDetailModel : PageModel
    {
        public string _WareHouseID { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _WareHouseID = curr.ToString();
            }
            else
            {
                _WareHouseID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoad_Update(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await confunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

        public async Task<IActionResult> OnPostWarehouseLoadBranch(string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_WareHouse_LoadList_TypeExport]", CommandType.StoredProcedure
                         , "@CurrentID ", SqlDbType.Int, CurrentID);
                }
                return Content(Comon.DataJson.Datatable(dt));
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
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                WareHouseDetail DataMain = JsonConvert.DeserializeObject<WareHouseDetail>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                         dt = await connFunc.ExecuteDataTable("[YYY_sp_v2_WareHouse_Insert]", CommandType.StoredProcedure,
                              "@Name ", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                              "@IsPackageNum", SqlDbType.Int, DataMain.IsPackageNum,
                              "@IsAutoExportSale", SqlDbType.Int, DataMain.IsAutoExportSale,
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                              "@Code ", SqlDbType.Int, DataMain.Code.Replace("'", "").Trim(),
                              "@Address ", SqlDbType.NVarChar, DataMain.Address,
                              "@Sale", SqlDbType.NVarChar, DataMain.BranchSale,
                              "@Treatment", SqlDbType.NVarChar, DataMain.BranchTreatment,
                              "@Medicine", SqlDbType.NVarChar, DataMain.BranchMedicine,
                              "@Area", SqlDbType.NVarChar, DataMain.BranchArea
                          );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                         dt = await connFunc.ExecuteDataTable("YYY_sp_v2_WareHouse_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@currentID ", SqlDbType.Int, CurrentID,
                            "@IsPackageNum", SqlDbType.Int, DataMain.IsPackageNum,
                            "@IsAutoExportSale", SqlDbType.Int, DataMain.IsAutoExportSale,
                            "@Code", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                            "@Address", SqlDbType.NVarChar, DataMain.Address,
                            "@Sale", SqlDbType.NVarChar, DataMain.BranchSale,
                            "@Treatment", SqlDbType.NVarChar, DataMain.BranchTreatment,
                            "@Medicine", SqlDbType.NVarChar, DataMain.BranchMedicine,
                            "@Area", SqlDbType.NVarChar, DataMain.BranchArea

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
        public class WareHouseDetail
        {
            public string Name { get; set; }
            public string IsPackageNum { get; set; }
            public string IsAutoExportSale { get; set; }
            public string Address { get; set; }
            public string Code { get; set; }
            public string BranchSale { get; set; }
            public string BranchTreatment { get; set; }
            public string BranchMedicine { get; set; }
            public string BranchArea { get; set; }
        }
    }
}
