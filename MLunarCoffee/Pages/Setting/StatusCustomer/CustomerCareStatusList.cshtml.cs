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

namespace MLunarCoffee.Pages.Setting.StatusCustomer
{
    public class CustomerCareStatusListModel : PageModel
    {
        public string sys_StatusTypeID { get; set; }
        public void OnGet()
        {
            string _type = Request.Query["TypeID"];
            sys_StatusTypeID = (_type == "") ? "0" : _type.ToString();
        }
        public async Task<IActionResult> OnPostLoadataStatus(int TypeStatusID)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("MLU_sp_Setting_CustomerCareStatus_LoadList", CommandType.StoredProcedure,
                  "@TypeID", SqlDbType.Int, TypeStatusID);
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
        public async Task<IActionResult> OnPostLoadataQuickNote(int TypeStatusID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_Setting_CustomerCare_QuickTemplate_LoadList", CommandType.StoredProcedure,
                  "@TypeID", SqlDbType.Int, TypeStatusID);
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
        public async Task<IActionResult> OnPostDeleteItem(string StatusID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Setting_CustomerCareStatus_Delete]", CommandType.StoredProcedure,
                         "@User_ID", SqlDbType.Int, user.sys_userid,
                         "@CurrentID", SqlDbType.Int, StatusID,
                         "@Modified", SqlDbType.Int, DateTime.Now
                     );
                    return Content("1");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteItemTemplate(string TemplateID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Setting_CustomerCare_QuickTemplate_Delete]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@CurrentID", SqlDbType.Int, TemplateID
                    );
                    return Content("1");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}


