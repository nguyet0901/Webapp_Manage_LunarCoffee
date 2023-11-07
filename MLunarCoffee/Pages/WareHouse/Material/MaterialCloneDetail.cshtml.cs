using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Material
{
    public class MaterialCloneDetailModel : PageModel
    {
        public string _MaterialID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _MaterialID = curr.ToString();
            }
            else
            {
                _MaterialID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_v2_Material_LoadDetailClone]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                ProductClone DataMain = JsonConvert.DeserializeObject<ProductClone>(data);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_v2_Material_Clone]", CommandType.StoredProcedure,
                      "@ProductID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                      "@Name", SqlDbType.NVarChar, DataMain.Name,
                      "@Code", SqlDbType.NVarChar, DataMain.Code,
                      "@SyntaxCode", SqlDbType.NVarChar, DataMain.SyntaxCode,
                      "@UserID", SqlDbType.Int, user.sys_userid);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch
            {
                return Content("0");
            }

        }

        public class ProductClone
        {
            public string Name { get; set; }
            public string Code { get; set; }
            public string SyntaxCode { get; set; }
        }
    }
}
