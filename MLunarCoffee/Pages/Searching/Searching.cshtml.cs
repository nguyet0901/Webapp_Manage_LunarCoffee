using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Searching
{
    public class SearchingModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }
        public string _SeachingText { get; set; }

        public void OnGet()
        {
            _dtPermissionCurrentPage=HttpContext.Request.Path;
            string v = Request.Query["SeachingText"];

            if(v!=null)
            {
                _SeachingText=v.ToString();

            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostInitSearch()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Member_Load]" ,CommandType.StoredProcedure);
                        dt.TableName = "Member";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Branch_LoadLocation]" ,CommandType.StoredProcedure);
                        dt.TableName = "BranchLocation";
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
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostSearchByOption(string data)
        {
            try
            {
                DataTable DataFilter = new DataTable();
                DataFilter.Columns.Add("name",typeof(String));
                DataFilter.Columns.Add("value",typeof(String));

                DataTable _DataFilter = new DataTable();
                _DataFilter=JsonConvert.DeserializeObject<DataTable>(data);
                for(int i = 0;i<_DataFilter.Rows.Count;i++)
                {
                    DataRow dr = DataFilter.NewRow();
                    dr["name"]=_DataFilter.Rows[i]["name"];
                    dr["value"]=_DataFilter.Rows[i]["value"].ToString().Replace("'","").Trim();
                    DataFilter.Rows.Add(dr);
                }

                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    if(Comon.Global.sys_SearchingInBranch==0||user.sys_AllBranchID==1)// khong set search theo chi nhanh hoac user la toan chi nhanh
                    {
                        ds=await confunc.ExecuteDataSet("[YYY_sp_Searching_List_By_Option]",CommandType.StoredProcedure
                            ,"@data",SqlDbType.Structured,DataFilter.Rows.Count>0 ? DataFilter : null
                            ,"@BranchID",SqlDbType.Int,0);
                    }
                    else
                    {
                        ds=await confunc.ExecuteDataSet("[YYY_sp_Searching_List_By_Option]",CommandType.StoredProcedure
                            ,"@data",SqlDbType.Structured,DataFilter.Rows.Count>0 ? DataFilter : null
                            ,"@BranchID",SqlDbType.Int,user.sys_branchID);
                    }
                }
                if(ds!=null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("0");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }
        }

    }
}
