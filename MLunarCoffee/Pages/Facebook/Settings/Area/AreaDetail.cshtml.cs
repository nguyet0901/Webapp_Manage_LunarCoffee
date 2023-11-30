using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;


namespace MLunarCoffee.Pages.Facebook.Settings.Area
{
    public class AreaDetailModel : PageModel
    {
        public string _fbarea_ID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _fbarea_ID = curr.ToString();
            }
            else
            {
                _fbarea_ID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_FB_Area_LoadDetail]" ,CommandType.StoredProcedure ,
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
                fb_Area DataMain = JsonConvert.DeserializeObject<fb_Area>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_FB_Area_Insert]" ,CommandType.StoredProcedure ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Name" ,SqlDbType.Int ,DataMain.Name.Replace("'" ,"").Trim() 
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
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_FB_Area_Update]" ,CommandType.StoredProcedure ,
                            "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@currentID " ,SqlDbType.Int ,CurrentID ,
                            "@Name" ,SqlDbType.Int ,DataMain.Name.Replace("'" ,"").Trim()
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
    public class fb_Area
    {
        public string Name { get; set; }        
    }

}
