using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;


namespace MLunarCoffee.Pages.Facebook.Settings.Message
{
    public class MessageTemplateDetailModel : PageModel
    {
        public string _fbmessTemplate_ID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _fbmessTemplate_ID = curr.ToString();
            }
            else
            {
                _fbmessTemplate_ID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_FB_Mess_Template_LoadDetail]" ,CommandType.StoredProcedure ,
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
            catch
            {
                return Content("");
            }

        }


        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID)
        {
            try
            {
                fb_mess DataMain = JsonConvert.DeserializeObject<fb_mess>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_FB_Mess_Template_Insert]" ,CommandType.StoredProcedure ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Title " ,SqlDbType.NVarChar ,DataMain.Title.Replace("'" ,"").Trim() ,
                            "@Type " ,SqlDbType.Int ,DataMain.Type ,
                            "@Description" ,SqlDbType.NVarChar ,DataMain.Description.Replace("'" ,"").Trim()
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
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_FB_Mess_Template_Update]" ,CommandType.StoredProcedure ,
                            "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@currentID " ,SqlDbType.Int ,CurrentID ,
                            "@Title " ,SqlDbType.NVarChar ,DataMain.Title.Replace("'" ,"").Trim() ,
                            "@Type " ,SqlDbType.Int ,DataMain.Type,
                            "@Description" ,SqlDbType.NVarChar ,DataMain.Description.Replace("'" ,"").Trim()
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
    }
    public class fb_mess
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
    }

}
