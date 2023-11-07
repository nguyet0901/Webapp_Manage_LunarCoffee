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
using MLunarCoffee.Models.Model.Manufacture;

namespace MLunarCoffee.Pages.Setting.Manufacture
{
    public class Manufacture_SettingModel : PageModel
    {
        public int _BranchID { get; set; }
        public string _Type { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _BranchID = user.sys_branchID;

            string Type = Request.Query["Type"];
            if (Type != null)
            {
                _Type = Type.ToString();
            }
            else _Type = "0";
        }
        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataSet dt = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataSet("[YYY_sp_Product_Combo_Branch]" ,CommandType.StoredProcedure);
                }
                return Content(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadData(string BranchID)
        {
            try
            {
                DataSet dt = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataSet("[YYY_sp_Manufacture_Load_v2]" ,CommandType.StoredProcedure,
                        "@BranchID" ,SqlDbType.NVarChar ,BranchID);
                } 
                return Content(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

    }
}
