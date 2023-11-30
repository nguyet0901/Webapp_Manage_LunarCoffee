using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
namespace MLunarCoffee.Pages.Employee
{
    public class EmployeeGroupDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _CurrentID = curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Employee_Group_LoadDetail]" ,CommandType.StoredProcedure ,
                      "@ID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id));
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteGroup(int GroupID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Employee_Group_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,GroupID ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID)
        {
            try
            {
                EmployeeGroup DataMain = JsonConvert.DeserializeObject<EmployeeGroup>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Employee_Group_Insert]" ,CommandType.StoredProcedure ,
                              "@GroupName" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                              "@IsCashier" ,SqlDbType.Int ,DataMain.IsCashier ,
                              "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"").Trim() ,
                              "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                              "@IsMarketing" ,SqlDbType.Int ,DataMain.IsMarketing ,
                              "@IsLabo" ,SqlDbType.Int ,DataMain.IsLabo,
                              "@IsTech" ,SqlDbType.Int ,DataMain.IsTech
                          );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.Datatable(dt));
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
                        DataTable dt = await connFunc.ExecuteDataTable("MLU_sp_Employee_Group_Update" ,CommandType.StoredProcedure ,
                              "@GroupName" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                              "@IsCashier" ,SqlDbType.Int ,DataMain.IsCashier ,
                              "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"").Trim() ,
                              "@CurrentID " ,SqlDbType.Int ,CurrentID ,
                              "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                              "@IsMarketing" ,SqlDbType.Int ,DataMain.IsMarketing ,
                              "@IsLabo" ,SqlDbType.Int ,DataMain.IsLabo,
                              "@IsTech" ,SqlDbType.Int ,DataMain.IsTech

                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.Datatable(dt));
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
    }
    public class EmployeeGroup
    {
        public string Name { get; set; }
        public int IsCashier { get; set; }
        public string Note { get; set; }
        public int IsMarketing { get; set; }
        public int IsLabo { get; set; }
        public int IsTech { get; set; }
    }
}
