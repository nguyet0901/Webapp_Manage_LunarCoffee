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
    public class DiaItem : PageModel
    {

        public void OnGet()
        {

        }
         public async Task<IActionResult> OnPostLoadData(int CurrentID )
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
 
                tasks.Add(Task.Run(async () =>
                {
                    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt=await confunc.ExecuteDataTable("[YYY_sp_Customer_Diagnose_LoadDetail]",CommandType.StoredProcedure,
                              "@ID",SqlDbType.Int,Convert.ToInt32(CurrentID),
                              "@UserId",SqlDbType.Int,user.sys_userid,
                              "@EditCustomerPassingDate",SqlDbType.Int,user.sys_EditCustomerPassingDate);
                        dt.TableName="DataDetail";
                        return dt;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach(var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }
         public async Task<IActionResult> OnPostExcuteData(string CurrentID,string CustomerID,string dataSVG,string dataQues
             ,string dataFree
             ,string StatusICD
             ,string Note,string Type)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if(CurrentID=="0")
                {
                    using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_DiagItem_Insert]",CommandType.StoredProcedure,
                            "@CustomerID",SqlDbType.Int,CustomerID,
                            "@dataSVG",SqlDbType.NVarChar,dataSVG,
                            "@dataQues",SqlDbType.NVarChar,dataQues,
                            "@StatusICD" ,SqlDbType.NVarChar ,StatusICD ,
                            "@dataFree" ,SqlDbType.NVarChar,dataFree,
                            "@Note", SqlDbType.NVarChar, Note!=null ?Note:"",
                             "@Type", SqlDbType.NVarChar, Type!=null ?Type:"",
                            "@Created_By",SqlDbType.Int,user.sys_userid
                        );
                    }
                }
                else
                {
                    using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Customer_DiagItem_Update]",CommandType.StoredProcedure,
                            "@dataSVG",SqlDbType.NVarChar,dataSVG,
                            "@dataQues",SqlDbType.NVarChar,dataQues,
                             "@StatusICD" ,SqlDbType.NVarChar ,StatusICD ,
                            "@dataFree" ,SqlDbType.NVarChar,dataFree,
                            "@Note", SqlDbType.NVarChar,Note!=null ?Note:"",
                            "@Created_By",SqlDbType.Int,user.sys_userid,
                            "@CurrentID",SqlDbType.Int,CurrentID
                        );
                    }
                }
                return Content("1");

            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

    }

}
