using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Facebook.Chating
{
    public class Profileinfo : PageModel
    {
 
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSource = new DataTable();
                        dtSource = await confunc.ExecuteDataTable("MLU_sp_TicketTag_LoadTele" ,CommandType.StoredProcedure ,"@UserID" ,SqlDbType.Int ,user.sys_userid);
                        dtSource.TableName = "User";
                        return dtSource;
                    }
                }));

                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostLoadInfo(string PID ,string PSID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_FB_Info_Load]" ,CommandType.StoredProcedure
                        ,"@PID" ,SqlDbType.NVarChar ,PID
                        ,"@PSID" ,SqlDbType.NVarChar ,PSID
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

       
        //public async Task<IActionResult> OnPostExecutedTag(string PID ,string PSID ,string Tag ,string TagID)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        var user = Session.GetUserSession(HttpContext);
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt = await confunc.ExecuteDataTable("[MLU_sp_FB_Tag_Insert]" ,CommandType.StoredProcedure
        //                ,"@PID" ,SqlDbType.NVarChar ,PID
        //                ,"@PSID" ,SqlDbType.NVarChar ,PSID
        //                ,"@Tag" ,SqlDbType.NVarChar ,Tag
        //                ,"@TagID" ,SqlDbType.Int ,Convert.ToInt32(TagID)
        //                ,"@UserID" ,SqlDbType.Int ,user.sys_userid
        //                );
        //        }
        //        if (dt != null)
        //        {
        //            return Content(Comon.DataJson.Datatable(dt));
        //        }
        //        else
        //        {
        //            return Content("[]");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("[]");
        //    }
        //}

        public async Task<IActionResult> OnPostLoadNote(string PID ,string PSID ,string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_FB_Content_Load]" ,CommandType.StoredProcedure
                        ,"@PID" ,SqlDbType.NVarChar ,PID
                        ,"@PSID" ,SqlDbType.NVarChar ,PSID
                        ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
                        );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExecutedNote(string PID ,string PSID ,string content ,string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_FB_Content_Insert]" ,CommandType.StoredProcedure
                            ,"@PID" ,SqlDbType.NVarChar ,PID
                            ,"@PSID" ,SqlDbType.NVarChar ,PSID
                            ,"@Content" ,SqlDbType.NVarChar ,content
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_FB_Content_Update]" ,CommandType.StoredProcedure
                            ,"@PID" ,SqlDbType.NVarChar ,PID
                            ,"@PSID" ,SqlDbType.NVarChar ,PSID
                            ,"@Content" ,SqlDbType.NVarChar ,content
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                            ,"@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
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
        public async Task<IActionResult> OnPostDeleteNote(string id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_FB_Content_Delete]" ,CommandType.StoredProcedure
                        ,"@ID" ,SqlDbType.Int ,Convert.ToInt32(id)
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        );
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecutedAssign(string PID ,string PSID ,string AssignID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_FB_FaceCon_Insert]" ,CommandType.StoredProcedure
                        ,"@PID" ,SqlDbType.NVarChar ,PID
                        ,"@PSID" ,SqlDbType.NVarChar ,PSID
                        ,"@AssignID" ,SqlDbType.Int ,AssignID
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
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
}
