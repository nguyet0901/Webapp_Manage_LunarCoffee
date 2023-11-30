using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.AccountEntry
{
    public class AccountEntryListModel : PageModel
    {
        public async Task<IActionResult> OnPostInit()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Account_Reason" ,CommandType.StoredProcedure
                        );
                        dt.TableName = "reason";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_LoadCombo_Para_Method" ,CommandType.StoredProcedure
                        );
                        dt.TableName = "method";
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
        public async Task<IActionResult> OnPostLoadData(int currentAcc)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_AccountEntry_LoadList]" ,CommandType.StoredProcedure
                        ,"@currentAcc" ,SqlDbType.Int ,currentAcc);
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
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_AccountEntry_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
    }
}
