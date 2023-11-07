using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SourceMain.Comon.Session;

namespace SourceMain.Pages.Desk.Doctor
{
    public class RoomWorkModel : PageModel
    {
        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostLoadRoom()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_RoomStatus_Change_Room]" ,CommandType.StoredProcedure
                        ,"@Branchid" ,SqlDbType.Int ,user.sys_branchID);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

    }
}
