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

namespace MLunarCoffee.Pages.Print
{
    public class setform : PageModel
    {

        public string Detail { get; set; }
        public void OnGet()
        {
            string _Detail = Request.Query["Detail"];
            if(_Detail!=null)
            {
                Detail=_Detail;
            }
            else Detail="";
        }
        public async Task<IActionResult> OnPostGetInitial(int id)
        {
            try
            {

                DataSet ds = new DataSet();
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt=await confunc.ExecuteDataTable("[MLU_sp_print_v2_GetCommand]",CommandType.StoredProcedure);
                    dt.TableName="Command";
                    ds.Tables.Add(dt);
                }
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt=await confunc.ExecuteDataTable("[MLU_sp_print_v2_GetType]",CommandType.StoredProcedure);
                    dt.TableName="Type";
                    ds.Tables.Add(dt);
                }
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_print_v2_LoadCombo_StogeSign]", CommandType.StoredProcedure);
                    dt.TableName = "StogeSign";
                    ds.Tables.Add(dt);
                }

                if (id!=0)
                {
                    using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt=await confunc.ExecuteDataTable("[MLU_sp_print_v2_GetDetail]",CommandType.StoredProcedure
                            ,"@templateid",SqlDbType.Int,id);
                        dt.TableName="Detail";
                        ds.Tables.Add(dt);
                    }
                }
                if(ds!=null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }

            }
            catch(Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostGetCol(int command)
        {
            try
            {
                DataSet ds = new DataSet();

                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds=await confunc.ExecuteDataSet("[MLU_sp_print_v2_GetCol]",CommandType.StoredProcedure,
                      "@commandid",SqlDbType.Int,command);
                }
                if(ds!=null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }

            }
            catch(Exception ex)
            {
                return Content("");
            }
        }
 

        public async Task<IActionResult> OnPostExcuteData(string form,string name,string width,string height
            ,string note ,string watermark
            ,string command,string type
            ,string dimesion,string paging,string stogesign
            , string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if(CurrentID=="0")
                {
                    using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt=await connFunc.ExecuteDataTable("[MLU_sp_print_v2_InsertForm]",CommandType.StoredProcedure,
                            "@form",SqlDbType.NVarChar,form,
                            "@name",SqlDbType.NVarChar,name,
                            "@width",SqlDbType.Int,Convert.ToInt32(width),
                            "@height",SqlDbType.Int,Convert.ToInt32(height),
                            "@note",SqlDbType.NVarChar,note!=null ? note.Replace("'","") : "",
                            "@watermark" ,SqlDbType.NVarChar ,watermark != null ? watermark.Replace("'" ,"") : "" ,
                            "@command" ,SqlDbType.Int,Convert.ToInt32(command),
                            "@stogesign", SqlDbType.NVarChar, stogesign,
                            "@type",SqlDbType.Int,Convert.ToInt32(type),
                            "@dimesion",SqlDbType.NVarChar,dimesion,
                            "@paging", SqlDbType.NVarChar, paging,
                            "@UserID",SqlDbType.Int,user.sys_userid
                       );
                    }
                }
                else
                {
                    using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt=await connFunc.ExecuteDataTable("[MLU_sp_print_v2_UpdateForm]",CommandType.StoredProcedure,
                           "@form",SqlDbType.NVarChar,form,
                            "@name",SqlDbType.NVarChar,name,
                            "@width",SqlDbType.Int,Convert.ToInt32(width),
                            "@height",SqlDbType.Int,Convert.ToInt32(height),
                            "@note",SqlDbType.NVarChar,note!=null ? note.Replace("'","") : "",
                             "@watermark" ,SqlDbType.NVarChar ,watermark != null ? watermark.Replace("'" ,"") : "" ,
                            "@command" ,SqlDbType.Int,Convert.ToInt32(command),
                            "@stogesign", SqlDbType.NVarChar, stogesign,
                            "@type",SqlDbType.Int,Convert.ToInt32(type),
                            "@dimesion",SqlDbType.NVarChar,dimesion,
                            "@paging", SqlDbType.NVarChar, paging,
                            "@UserID",SqlDbType.Int,user.sys_userid,
                            "@CurrentID",SqlDbType.Int,Convert.ToInt32(CurrentID) 
                        );
                    }
                }


                return Content(dt.Rows[0]["RESULT"].ToString());
            }
            catch(Exception ex)
            {
                return Content("");
            }
        }
    }
}
