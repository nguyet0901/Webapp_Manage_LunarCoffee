using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace SourceMain.Pages.Customer.Diagnose
{
    public class DiagnoseDetailTM : PageModel
    {
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoadInitialize(int CurrentID,int CustomerID)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                //tasks.Add(Task.Run(async () =>
                //{
                //    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //    {
                //        DataTable dt = new DataTable();
                //        dt=await confunc.ExecuteDataTable("[YYY_sp_print_v2_GetForm]",CommandType.StoredProcedure
                //               ,"@TYPE",SqlDbType.NVarChar,"dia_form");
                //        dt.TableName="Question";
                //        return dt;
                //    }
                //}));
                //tasks.Add(Task.Run(async () =>
                //{
                //    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //    {
                //        DataTable dt = new DataTable();
                //        dt=await confunc.ExecuteDataTable("[YYY_sp_print_v2_Dianotic]",CommandType.StoredProcedure
                //             ,"@id",SqlDbType.Int,CustomerID);
                //        dt.TableName="QuestionField";
                //        return dt;
                //    }
                //}));
                //tasks.Add(Task.Run(async () =>
                //{
                //    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //    {
                //        DataTable dt = new DataTable();
                //        dt=await confunc.ExecuteDataTable("[YYY_sp_Customer_Diagnose_BStatus]",CommandType.StoredProcedure);
                //        dt.TableName="DiagnoseStatus";
                //        return dt;
                //    }
                //}));
                // tasks.Add(Task.Run(async () =>
                //{
                //    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //    {
                //        DataTable dt = new DataTable();
                //        dt=await confunc.ExecuteDataTable("[YYY_sp_Customer_Diagnose_BTemplate]",CommandType.StoredProcedure);
                //        dt.TableName="Template";
                //        return dt;
                //    }
                //}));
                //tasks.Add(Task.Run(async () =>
                //{
                //    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //    {
                //        DataTable dt = new DataTable();
                //        dt=await confunc.ExecuteDataTable("[YYY_sp_Diagnose_AreaName]",CommandType.StoredProcedure);
                //        dt.TableName="AreaName";
                //        return dt;
                //    }
                //}));

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
        public async Task<IActionResult> OnPostExcuteData(string CurrentID,string CustomerID,string dataFace,string dataBody,string dataQues, string Note, string NoteFace, string NoteBody)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if(CurrentID=="0")
                {
                    using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_DiagnoseTM_Insert]",CommandType.StoredProcedure,
                            "@CustomerID",SqlDbType.Int,CustomerID,
                            "@dataFace",SqlDbType.NVarChar,dataFace,
                            "@dataBody",SqlDbType.NVarChar,dataBody,
                            "@dataQues",SqlDbType.NVarChar,dataQues,
                            "@Note", SqlDbType.NVarChar, Note,
                            "@NoteFace", SqlDbType.NVarChar, NoteFace,
                            "@NoteBody", SqlDbType.NVarChar, NoteBody,
                            "@Created_By",SqlDbType.Int,user.sys_userid
                        );
                    }
                }
                else
                {
                    using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Customer_DiagnoseTM_Update]",CommandType.StoredProcedure,
                            "@dataFace",SqlDbType.NVarChar,dataFace,
                            "@dataBody",SqlDbType.NVarChar,dataBody,
                            "@dataQues",SqlDbType.NVarChar,dataQues,
                            "@Note", SqlDbType.NVarChar,Note,
                            "@NoteFace", SqlDbType.NVarChar, NoteFace,
                            "@NoteBody", SqlDbType.NVarChar, NoteBody,
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
