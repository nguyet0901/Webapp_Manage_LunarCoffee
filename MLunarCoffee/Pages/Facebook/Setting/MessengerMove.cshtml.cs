using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Fanpage.Pages.Facebook.Setting
{
    public class MessengerMoveModel : PageModel
    {
        public void OnGet()
        {
        }
        
         public async Task<IActionResult> OnPostLoadMessengerMove(string dateFrom, string dateTo)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await  confunc.ExecuteDataTable("[MLU_sp_Facebook_Page_Inbox_Move_Load]", CommandType.StoredProcedure
                  , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                  , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo));
            }
            if (dt != null)
            {
                try
                {
                    return Content(JsonConvert.SerializeObject(dt));
                }
                catch (Exception) // No datarow
                {
                    return Content("");
                }
            }
            else
            {
                return Content("");
            }
        }
    }
}
