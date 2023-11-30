using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.QR
{
    public class QRDetailModel : PageModel
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
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_SysQR_LoadDetail]" ,CommandType.StoredProcedure ,
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


        public async Task<IActionResult> OnPostExcuteData(string name ,string value ,string note ,string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_SysQR_Update]" ,CommandType.StoredProcedure ,
                        "@Name" ,SqlDbType.NVarChar ,name.ToString().Replace("'" ,"").Trim() ,
                        "@Value" ,SqlDbType.NVarChar ,value.ToString().Replace("'" ,"").Trim() ,
                        "@Note" ,SqlDbType.NVarChar ,note.ToString().Replace("'" ,"").Trim() ,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                        "@ID" ,SqlDbType.Int ,CurrentID
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
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
