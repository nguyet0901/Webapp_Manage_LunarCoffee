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

namespace MLunarCoffee.Pages.Customer.Diagnose
{

    public class DiagnoseListModel : PageModel
    {

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadData(string CustomerID,string ICDArea)
        {
            DataSet ds = new DataSet();
            var tasks = new List<Task<DataTable>>();
            var user = Session.GetUserSession(HttpContext);

            tasks.Add(Task.Run(async () =>
           {
               using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
               {
                   DataTable dt = new DataTable();
                   dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Diagnose_BTemplate]", CommandType.StoredProcedure);
                   dt.TableName = "Template";
                   return dt;
               }
           }));
            tasks.Add(Task.Run(async () =>
            {
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_ICDLoadCombo]" ,CommandType.StoredProcedure,
                         "@Area" ,SqlDbType.NVarChar ,ICDArea
                        );
                    dt.TableName = "ICD";
                    return dt;
                }
            }));
            tasks.Add(Task.Run(async () =>
           {
               using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
               {
                   DataTable dt = new DataTable();
                   dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Diagnose_LoadList]", CommandType.StoredProcedure,
                     "@Customer_ID", SqlDbType.Int, CustomerID,
                     "@UserId", SqlDbType.Int, user.sys_userid
                     , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
                   dt.TableName = "List";
                   return dt;
               }
           }));
            var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
            foreach (var r in result)
            {
                ds.Tables.Add(r);
            }
            return Content(Comon.DataJson.Dataset(ds));
        }
        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Customer_Diagnose_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@Datenow", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadIni(string CustomerID)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
 
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_print_v2_Dianotic]", CommandType.StoredProcedure
                             , "@id", SqlDbType.Int, CustomerID);
                        dt.TableName = "QuestionField";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Diagnose_BStatus]", CommandType.StoredProcedure);
                        dt.TableName = "DiagnoseStatus";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Diagnose_BSign]", CommandType.StoredProcedure);
                        dt.TableName = "DiagnoseSign";
                        return dt;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
