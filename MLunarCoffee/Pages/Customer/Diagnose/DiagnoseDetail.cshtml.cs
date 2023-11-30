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
    public class DiagnoseDetailModel : PageModel
    {
        public string _listRegimen { get; set; }
        public string _dataRegimenDetail { get; set; }
        public string _customerID { get; set; }
        public string _patientHistoryID { get; set; }
        public string _dataPatientHistory { get; set; }
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
                tasks.Add(Task.Run(async () =>
                {
                    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt=await confunc.ExecuteDataTable("[MLU_sp_print_v2_GetForm]",CommandType.StoredProcedure
                             ,"@TYPE",SqlDbType.NVarChar,"dia_form");
                        dt.TableName="Question";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt=await confunc.ExecuteDataTable("[MLU_sp_print_v2_Dianotic]",CommandType.StoredProcedure
                             ,"@id",SqlDbType.Int,CustomerID);
                        dt.TableName="QuestionField";
                        return dt;
                    }
                }));

        
                tasks.Add(Task.Run(async () =>
                {
                    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt=await confunc.ExecuteDataTable("[MLU_sp_Customer_Diagnose_Status]",CommandType.StoredProcedure
                            ,"@Type",SqlDbType.Int,Global.sys_DentistCosmetic);
                        dt.TableName="DiagnoseStatus";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt=await confunc.ExecuteDataTable("[MLU_sp_Customer_TabService_Load_Teeth]",CommandType.StoredProcedure);
                        dt.TableName="Teeth";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt=await confunc.ExecuteDataTable("[MLU_sp_Customer_Diagnose_LoadDetail]",CommandType.StoredProcedure,
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
        public async Task<IActionResult> OnPostExcuteData(string CurrentID,string CustomerID,string TeethType,string content,string dataTeeth,string dataQues
            ,string StatusICD)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                if(CurrentID=="0")
                {
                    using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Customer_Diagnose_Insert]",CommandType.StoredProcedure,
                            "@CustomerID",SqlDbType.Int,CustomerID,
                            "@TeethType",SqlDbType.Int,TeethType,
                            "@dataTeeth",SqlDbType.NVarChar,dataTeeth,
                            "@content" ,SqlDbType.NVarChar ,content ,
                            "@dataQues" ,SqlDbType.NVarChar,dataQues,
                             "@StatusICD" ,SqlDbType.NVarChar ,StatusICD!=null ? StatusICD :"" ,
                            "@Created_By" ,SqlDbType.Int,user.sys_userid
                        );
                    }
                }
                else
                {
                    using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[MLU_sp_Customer_Diagnose_Update]",CommandType.StoredProcedure,
                            "@dataTeeth",SqlDbType.NVarChar,dataTeeth,
                            "@content" ,SqlDbType.NVarChar ,content ,
                            "@TeethType" ,SqlDbType.Int,TeethType,
                            "@dataQues",SqlDbType.NVarChar,dataQues,
                            "@Created_By",SqlDbType.Int,user.sys_userid,
                             "@StatusICD" ,SqlDbType.NVarChar ,StatusICD != null ? StatusICD : "" ,
                            "@CurrentID" ,SqlDbType.Int,CurrentID
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
        public async Task<IActionResult> OnPostLoadDataTemplate()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase conFunc = new Models.ExecuteDataBase())
                {
                    dt = await conFunc.ExecuteDataTable("[MLU_sp_page_Diagnose_Template]",CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }

}
