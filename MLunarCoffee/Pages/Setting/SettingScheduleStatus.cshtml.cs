using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting
{
    public class SettingScheduleStatusModel : PageModel
    {
        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostLoadData(int ID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_Schedule_Status_LoadList]" ,CommandType.StoredProcedure
                        ,"@CurrentID" ,SqlDbType.Int ,ID);
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
                return Content("[]");
            }

        }


        public async Task<IActionResult> OnPostExecuteStatus(string CurrentID ,string StatusName ,string StatusNameLangOther ,string Color
            ,string IsTreatment ,string IsCashier ,string IsXquang ,string IsChoosingRoom ,string IsForectTime ,string TimeExpired ,string IsExamination, string RoomToken)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID != "0")
                {

                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_Schedule_Status_UpdateStatus]" ,CommandType.StoredProcedure ,
                               "@CurrentID " ,SqlDbType.Int ,Convert.ToInt32(CurrentID) ,
                               "@StatusName " ,SqlDbType.NVarChar ,StatusName ,
                               "@StatusNameLangOther" ,SqlDbType.NVarChar ,StatusNameLangOther ,
                               "@Color " ,SqlDbType.NVarChar ,Color ,
                               "@IsTreatment " ,SqlDbType.Int ,Convert.ToInt32(IsTreatment) ,
                               "@IsCashier " ,SqlDbType.Int ,Convert.ToInt32(IsCashier) ,
                               "@IsXquang " ,SqlDbType.Int ,Convert.ToInt32(IsXquang) ,
                               "@IsChoosingRoom " ,SqlDbType.Int ,Convert.ToInt32(IsChoosingRoom) ,
                               "@IsForectTime " ,SqlDbType.Int ,Convert.ToInt32(IsForectTime) ,
                               "@TimeExpired " ,SqlDbType.Int ,Convert.ToInt32(TimeExpired) ,
                               "@Modified_By " ,SqlDbType.Int ,user.sys_userid ,
                               "@IsExamination" ,SqlDbType.Int ,Convert.ToInt32(IsExamination) 
                               
                           );
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() != "0")
                        {
                            return Content(CurrentID.ToString());
                        }

                    }
                    else
                    {
                        return Content("0");
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_Schedule_Status_InsertStatus]" ,CommandType.StoredProcedure ,
                               "@StatusName " ,SqlDbType.NVarChar ,StatusName ,
                               "@StatusNameLangOther" ,SqlDbType.NVarChar ,StatusNameLangOther ,
                               "@Color " ,SqlDbType.NVarChar ,Color ,
                               "@IsTreatment " ,SqlDbType.Int ,Convert.ToInt32(IsTreatment) ,
                               "@IsCashier " ,SqlDbType.Int ,Convert.ToInt32(IsCashier) ,
                               "@IsXquang " ,SqlDbType.Int ,Convert.ToInt32(IsXquang) ,
                               "@IsChoosingRoom " ,SqlDbType.Int ,Convert.ToInt32(IsChoosingRoom) ,
                               "@TimeExpired " ,SqlDbType.Int ,Convert.ToInt32(TimeExpired) ,
                               "@CreatedBy" ,SqlDbType.Int ,user.sys_userid ,
                               "@IsExamination" ,SqlDbType.Int ,Convert.ToInt32(IsExamination) 
                           );
                    }
                    if (dt.Rows.Count > 0)
                    {
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                    else
                    {
                        return Content("0");
                    }
                }
                return Content("0");

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        public async Task<IActionResult> OnPostUpdateStatusFollow(string CurrentID ,string StatusFollow)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_Schedule_Status_UpdateStatusFollow]" ,CommandType.StoredProcedure ,
                                "@CurrentID " ,SqlDbType.Int ,Convert.ToInt32(CurrentID) ,
                                "@StatusFollow " ,SqlDbType.NVarChar ,StatusFollow
                            );
                    }
                    return Content("1");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteStatus(string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_Schedule_Status_DeleteStatus]" ,CommandType.StoredProcedure ,
                                "@CurrentID " ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
                            );
                    }
                    return Content("1");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
