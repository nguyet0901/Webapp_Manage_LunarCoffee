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

namespace MLunarCoffee.Pages.Setting.Employee
{
    public class ContractTypeDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string Curr = Request.Query["CurrentID"];
            _CurrentID = Curr.ToString();
        }
        public async Task<IActionResult> OnPostLoadDetail(string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_EmployeeContractType_LoadDetail]" ,CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string CurrentID ,string TypeName, string Note)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_EmployeeContractType_Insert" ,CommandType.StoredProcedure
                            ,"@TypeName", SqlDbType.NVarChar,TypeName
                            ,"@Note", SqlDbType.NVarChar,Note
                            ,"@UserID",SqlDbType.Int, user.sys_userid
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_EmployeeContractType_Update" ,CommandType.StoredProcedure ,
                            "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID)
                            ,"@TypeName" ,SqlDbType.NVarChar ,TypeName
                            ,"@Note" ,SqlDbType.NVarChar ,Note
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        );
                    }
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
