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

namespace MLunarCoffee.Pages.Student.Slide
{
    public class ClassSlideView : PageModel
    {

        public string _SlideID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) _SlideID = curr.ToString();
            else _SlideID = "0";
        }


        public async Task<IActionResult> OnPostLoadSlide(string TrainslideID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_TrainSlide_Load]", CommandType.StoredProcedure,
                        "@TrainslideID", SqlDbType.Int, TrainslideID);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }
    }
}
