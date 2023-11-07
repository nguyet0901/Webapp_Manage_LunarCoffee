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


namespace MLunarCoffee.Pages.Setting
{
    public class ScheduleStatusColorCodeDetailModel : PageModel
    {
        public string _scheduleColorID { get; set; }
        public string _dataColor { get; set; }
         public string _dataType { get; set; }
        public void OnGet()
        {

            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _scheduleColorID = curr.ToString();
                 _dataType = Request.Query["Type"].ToString();
            }
            else
            {
                _scheduleColorID = "0";

            }
        }
        public async Task<IActionResult> OnPostLoadata(int id,int type )
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Schedule_Status_Color_LoadDetail]", CommandType.StoredProcedure,
                  "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id),
                  "@type", SqlDbType.Int, Convert.ToInt32(type == 0 ? 0 : type));
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

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID,int type)
        {
            try
            {
                ScheduleStatusColor DataMain = JsonConvert.DeserializeObject<ScheduleStatusColor>(data);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Schedule_Status_Color_Update]", CommandType.StoredProcedure,
                        "@ColorCode", SqlDbType.NVarChar, DataMain.ColorCode.Replace("'", "").Trim(),
                        "@currentID ", SqlDbType.Int, CurrentID,
                        "@type ", SqlDbType.Int, type

                    );
                    return Content("1");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class ScheduleStatusColor
    {
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public string Note { get; set; }
    }

}
