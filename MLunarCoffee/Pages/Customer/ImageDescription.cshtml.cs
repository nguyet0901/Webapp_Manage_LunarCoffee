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
using Microsoft.AspNetCore.Http.Extensions;


namespace MLunarCoffee.Pages.Customer
{
    public class ImageDescriptionModel : PageModel
    {
        public string _CurrentImageID { get; set; }
        public void OnGet ( ) {
            var cus = Request.Query["CustomerID"];
            var currenrid = Request.Query["CurrentID"];
            if (cus.ToString() != "")
            {
                _CurrentImageID = (currenrid.ToString () != "") ? currenrid.ToString() : "0";
            }
            else
            {
                _CurrentImageID = "0";
                Response.Redirect("/assests/Error/index.html");
            }
        }
         public async Task<IActionResult> OnPostLoadata ( int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt =await  confunc.ExecuteDataTable("[YYY_sp_CustomerImage_Description_Load]", CommandType.StoredProcedure,
                  "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content ("0");
            }

        }
         public async Task<IActionResult> OnPostExcuteData ( string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession (HttpContext);
                ImageDescription DataMain = JsonConvert.DeserializeObject<ImageDescription>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_Description_Update]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, CurrentID
                        , "@Modified_By", SqlDbType.Int, user.sys_userid
                        , "@Description", SqlDbType.NVarChar, DataMain.Description
                    );
                }           
                return Content ("1");
            }            
            catch (Exception ex)
            {
                return Content ("Something wrongs");
            }
        }
        public class ImageDescription
        {
            public string Description { get; set; }
        }
    }
}
