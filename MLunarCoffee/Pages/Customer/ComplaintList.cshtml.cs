using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer
{
    public class ComplaintListModel : PageModel
    {
        public string _ComplaintCustomerID { get; set; }
        public void OnGet()
        {
            var v = Request.Query["CustomerID"];
            if (v.ToString() != "")
            {
                _ComplaintCustomerID = v.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }

        public async Task<IActionResult> OnPostInitialization()
        {
            int custcareType = 6;
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerCare_LoadStatus_ByType]" ,CommandType.StoredProcedure ,
                   "@TypeID" ,SqlDbType.Int ,custcareType);
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

        public async Task<IActionResult> OnPostLoadata(int CustomerID ,int currentID)
        {
            DataSet ds = new DataSet();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_Complaint_LoadList]" ,CommandType.StoredProcedure
                  ,"@Customer_ID" ,SqlDbType.Int ,CustomerID
                  ,"@currentID" ,SqlDbType.Int ,currentID
                  ,"@UserId" ,SqlDbType.Int ,user.sys_userid
                  ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate
                  );
            }
            if (ds != null)
            {
                return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadataDetail(int CurrentID)
        {
            DataTable dt = new DataTable();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Complaint_LoadDetail]" ,CommandType.StoredProcedure
                  ,"@CurrentID" ,SqlDbType.Int ,CurrentID
                  ,"@CustID" ,SqlDbType.Int ,0
                  );
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

        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_sp_Customer_Complaint_Delete]" ,CommandType.StoredProcedure ,
                        "@ID" ,SqlDbType.Int ,id ,
                        "@User" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
