using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Student.General.Course
{
    public class StudentListModel : PageModel
    {
        public string _ClassID { get; set; }
        public void OnGet()
        {
            string classID = Request.Query["ClassID"];
            _ClassID = (classID != null) ? classID.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadData(string classID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_ST_Student_LoadByClass]", CommandType.StoredProcedure,
                        "@ClassID", SqlDbType.Int, classID);
                }
                if (ds != null) return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }



    }
}
