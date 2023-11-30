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

namespace MLunarCoffee.Pages.Disease
{

    public class DentistChartDiseaseListModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadData(string curid)
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
                      dt = await confunc.ExecuteDataTable("[MLU_sp_Dentist_Chart_Disease_LoadList]", CommandType.StoredProcedure
                          , "@curid", SqlDbType.Int, curid
                          );
                      dt.TableName = "List";
                      return dt;
                  }
              }));
                tasks.Add(Task.Run(async () =>
           {
               using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
               {
                   DataTable dt = new DataTable();
                   dt = await confunc.ExecuteDataTable("[MLU_sp_Dentist_Chart_DiseaseCat]", CommandType.StoredProcedure);
                   dt.TableName = "Cat";
                   return dt;
               }
           }));
                 tasks.Add(Task.Run(async () =>
              {
                  using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                  {
                      DataTable dt = new DataTable();
                      dt = await confunc.ExecuteDataTable("[MLU_sp_DentistChart_LoadImage]", CommandType.StoredProcedure);
                      dt.TableName = "ImageDisease";
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

        public async Task<IActionResult> OnPostDisableItem(int id,int active)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Dentist_Chart_Disease_Disable]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                         "@active", SqlDbType.Int, active,
                        "@userID", SqlDbType.Int, user.sys_userid);
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadDetail(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Dentist_Chart_Disease_LoadDetail]", CommandType.StoredProcedure,
                  "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostExcuteData(string cat, string image, string name, string note, string disease, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Dentist_Chart_Disease_Update]", CommandType.StoredProcedure,
                        "@Name", SqlDbType.NVarChar, name!=null ? name.Replace("'", "").Trim():"",  
                        "@Description", SqlDbType.NVarChar, note!=null ? note.Replace("'", "").Trim():"",  
                         "@disease", SqlDbType.NVarChar, disease!=null ? disease.Replace("'", "").Trim():"",  
                        "@image", SqlDbType.NVarChar, image!=null ? image.Replace("'", "").Trim():"",  
                        "@cat", SqlDbType.Int, cat,
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@CurrentID", SqlDbType.Int, CurrentID
                    );
                    return Content(dt.Rows[0][0].ToString());
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
