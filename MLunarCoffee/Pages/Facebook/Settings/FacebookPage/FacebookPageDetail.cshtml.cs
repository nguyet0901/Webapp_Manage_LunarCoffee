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


namespace MLunarCoffee.Pages.Facebook.Settings.FacebookPage.FacebookPageDetail
{
    public class FacebookPageDetailModel : PageModel
    {
        public string _fbpage_Detail_ID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _fbpage_Detail_ID = curr.ToString();
            }
            else
            {
                _fbpage_Detail_ID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_FB_Fanpage_LoadDetail]", CommandType.StoredProcedure,
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


        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                fb_fanpage DataMain = JsonConvert.DeserializeObject<fb_fanpage>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_FB_Fanpage_Insert]", CommandType.StoredProcedure,
                            "@Created_By", SqlDbType.Int, user.sys_userid,                            
                            "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                            "@Key" ,SqlDbType.NVarChar ,DataMain.Key.Replace("'" ,"").Trim() ,
                            "@URL" ,SqlDbType.NVarChar ,DataMain.URL.Replace("'" ,"").Trim(),
                            "@About" ,SqlDbType.NVarChar ,DataMain.About.Replace("'" ,"").Trim() ,
                            "@Description" ,SqlDbType.NVarChar ,DataMain.Description.Replace("'" ,"").Trim() ,
                            "@Picurl" ,SqlDbType.NVarChar ,DataMain.Picurl.Replace("'" ,"").Trim()
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
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_FB_Fanpage_Update]", CommandType.StoredProcedure,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,                            
                            "@currentID", SqlDbType.Int, CurrentID,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Key", SqlDbType.NVarChar, DataMain.Key.Replace("'", "").Trim() ,
                            "@URL" , SqlDbType.NVarChar, DataMain.URL.Replace("'", "").Trim(),
                            "@About" ,SqlDbType.NVarChar ,DataMain.About.Replace("'" ,"").Trim() ,
                            "@Description" ,SqlDbType.NVarChar ,DataMain.Description.Replace("'" ,"").Trim(),
                            "@Picurl" ,SqlDbType.NVarChar ,DataMain.Picurl.Replace("'" ,"").Trim()
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
    public class fb_fanpage
    {
        public string URL { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string About { get; set; }
        public string Description { get; set; }
        public string Picurl { get; set; }
    }

}
