using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting
{

    public class DiseaseStatusDetailModel : PageModel
    {        
        public string _DataDetailID { get; set; }
        public string _IsTeeth { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _IsTeeth = Request.Query["IsTeeth"];
            if (curr != null)
            {
                _DataDetailID = curr.ToString();                
            }
            else
            {
                _DataDetailID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadInitializeData()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtType = new DataTable();
                        dtType = await confunc.ExecuteDataTable("[YYY_sp_DiseaseStatus_LoadType]", CommandType.StoredProcedure);
                        dtType.TableName = "Type";
                        return dtType;
                    }

                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSignID = new DataTable();
                        dtSignID = await confunc.ExecuteDataTable("[YYY_sp_DiseaseStatus_LoadComboSign]", CommandType.StoredProcedure);
                        dtSignID.TableName = "Sign";
                        return dtSignID;
                    }

                }));                
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostLoadata(int id, int IsTeeth)
        {
            try
            {
                DataSet dt = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataSet("[YYY_sp_DiseaseStatus_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id),
                      "@IsTeeth", SqlDbType.Int, Convert.ToInt32(IsTeeth == 0 ? 0 : IsTeeth)
                      );
                }
                if (dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                {
                    return Content(Comon.DataJson.Datatable(dt.Tables[0]));
                }
                else return Content("[]");
            }
            catch
            {
                return Content("[]");
            }
        }        
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, int IsTeeth)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DiseaseStatus DataMain = JsonConvert.DeserializeObject<DiseaseStatus>(data);
                if (CurrentID.Trim() == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_DiseaseStatus_Insert]", CommandType.StoredProcedure,
                                "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                                "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                                "@Color", SqlDbType.NVarChar, DataMain.Color.Replace("'", "").Trim(),
                                "@Type", SqlDbType.NVarChar, DataMain.Type.Replace("'", "").Trim(),
                                "@SignID", SqlDbType.Int, DataMain.SignID.Replace("'", "").Trim(),
                                "@Created_By", SqlDbType.Int, user.sys_userid,
                                "@IsTeeth", SqlDbType.Int, Convert.ToInt32(IsTeeth == 0 ? 0 : IsTeeth)
                            );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_DiseaseStatus_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@Color", SqlDbType.NVarChar, DataMain.Color.Replace("'", "").Trim(),
                            "@Type", SqlDbType.NVarChar, DataMain.Type.Replace("'", "").Trim(),
                            "@SignID", SqlDbType.Int, DataMain.SignID.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,                                
                            "@currentID ", SqlDbType.Int, CurrentID,
                            "@IsTeeth", SqlDbType.Int, Convert.ToInt32(IsTeeth == 0 ? 0 : IsTeeth)

                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class DiseaseStatus
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public string Color { get; set; }

            public string Type { get; set; }
            public string SignID { get; set; }
        }


    }
}