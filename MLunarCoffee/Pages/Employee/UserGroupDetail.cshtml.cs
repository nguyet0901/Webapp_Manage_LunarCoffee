using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Employee
{
    public class UserGroupDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public string _dtAllGroupMainDataBase { get; set; }
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
        public async Task<IActionResult> OnPostLoadGroupMainDataBase()
        {
            return Content(JsonConvert.SerializeObject(Global.sys_AllGroupMailDataBase));
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_User_Group_LoadDetail]" ,CommandType.StoredProcedure ,
                      "@ID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id));
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

        public async Task<IActionResult> OnPostDeleteTypeItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_User_Group_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
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
                UserGroup DataMain = JsonConvert.DeserializeObject<UserGroup>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_User_Group_Insert]" ,CommandType.StoredProcedure ,
                              "@Name " ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                              "@GroupID" ,SqlDbType.Int ,(DataMain.GroupID != null) ? DataMain.GroupID : "" ,
                              "@TokenTopic" ,SqlDbType.NVarChar ,DataMain.TokenTopic ,
                              "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                              "@TimeOut" ,SqlDbType.Int ,Convert.ToInt32(DataMain.TimeOut) ,
                              "@Created" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                              "@Note " ,SqlDbType.Int ,DataMain.Note.Replace("'" ,"").Trim()
                          );

                        if (dt != null)
                        {
                            return Content(Comon.DataJson.Datatable(dt));
                        }
                        else
                        {
                            return Content("");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("YYY_sp_User_Group_Update" ,CommandType.StoredProcedure ,
                            "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@GroupID" ,SqlDbType.Int ,(DataMain.GroupID != null) ? DataMain.GroupID : "" ,
                            "@TokenTopic" ,SqlDbType.NVarChar ,DataMain.TokenTopic ,
                            "@TimeOut" ,SqlDbType.Int ,Convert.ToInt32(DataMain.TimeOut) ,
                            "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Modified" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                            "@currentID " ,SqlDbType.Int ,CurrentID ,
                            "@Note " ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"").Trim()

                        );
                        if (dt != null)
                        {
                            return Content(Comon.DataJson.Datatable(dt));
                        }
                        else
                        {
                            return Content("");
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
    public class UserGroup
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public string GroupID { get; set; }
        public string TokenTopic { get; set; }
        public string TimeOut { get; set; }
    }
}
