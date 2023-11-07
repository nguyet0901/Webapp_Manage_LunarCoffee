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

namespace MLunarCoffee.Pages.Customer.ConsultationRegimen
{

    public class ConsultLessFastModel : PageModel
    {
        public string _ConsultLessCustomerID { get; set; }
        public string _ConsultLessCurrentID { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var cur = Request.Query["CurrentID"];
            if (cus.ToString() != String.Empty)
            {
                _ConsultLessCustomerID = cus.ToString();
                if (cur.ToString() != String.Empty)
                {
                    _ConsultLessCurrentID = cur.ToString();
                }
                else
                {
                    _ConsultLessCurrentID = "0";
                }
            }
            else
            {
                _ConsultLessCustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Consult_Less_LoadDetail]", CommandType.StoredProcedure,
                  "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            return Content(Comon.DataJson.Datatable(dt));
        }

        public async Task<IActionResult> OnPostLoadInitializeData()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerStatus_LoadType]", CommandType.StoredProcedure);
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
         public async Task<IActionResult> OnPostExcuteData ( string data, string CustomerID, string CurrentID ) {
            try {
                var user = Session.GetUserSession (HttpContext);
                ConsultLess DataMain = JsonConvert.DeserializeObject<ConsultLess> (data);
                if (CurrentID == "0") {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                        await connFunc.ExecuteDataTable ("[YYY_sp_Customer_Consult_Less_Insert]", CommandType.StoredProcedure,
                            "@Customer_ID", SqlDbType.Int, CustomerID,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@BranchID", SqlDbType.Int, user.sys_branchID,
                            "@Content", SqlDbType.NVarChar, DataMain.Content,
                            "@TypeID", SqlDbType.Int, DataMain.TypeStatus,
                            "@Advice", SqlDbType.NVarChar, DataMain.Advice
                        );

                    }
                }
                else {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                        await connFunc.ExecuteDataTable ("[YYY_sp_Customer_Consult_Less_Update]", CommandType.StoredProcedure,
                            "@Current_ID", SqlDbType.Int, CurrentID,
                            "@Customer_ID", SqlDbType.Int, CustomerID,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Content", SqlDbType.NVarChar, DataMain.Content,
                            "@TypeID", SqlDbType.Int, DataMain.TypeStatus,
                            "@Advice", SqlDbType.NVarChar, DataMain.Advice
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

        public class ConsultLess
        {
            public string TypeStatus { get; set; }
            public string Content { get; set; }
            public string Advice { get; set; }
        }
    }
}
