using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting
{

    public class TreatmentStageDetailModel : PageModel
    {
        public string _DataDetailID { get; set; }
        public string sys_ExportConsumableAllowStage { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _DataDetailID = curr.ToString();
            }
            else
            {
                _DataDetailID = "0";
            }
            sys_ExportConsumableAllowStage = Comon.Global.sys_ExportConsumableAllowStage.ToString();
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_TreatmentStage_LoadDetail]" ,CommandType.StoredProcedure ,
                      "@ID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id)
                      );
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else return Content("[]");
            }
            catch
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string dataCons ,string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DiseaseStatus DataMain = JsonConvert.DeserializeObject<DiseaseStatus>(data);
                // VTTH
                DataTable DataCons = new DataTable();
                DataCons.Columns.Add("productID" ,typeof(String));
                DataCons.Columns.Add("unitID" ,typeof(String));
                DataCons.Columns.Add("minNumber" ,typeof(decimal));
                DataCons.Columns.Add("maxNumber" ,typeof(decimal));
                DataCons.Columns.Add("note" ,typeof(String));

                DataTable _DataCons = new DataTable();
                _DataCons = JsonConvert.DeserializeObject<DataTable>(dataCons);
                for (int i = 0; i < _DataCons.Rows.Count; i++)
                {
                    DataRow dr = DataCons.NewRow();
                    dr["productID"] = _DataCons.Rows[i]["productID"];
                    dr["unitID"] = _DataCons.Rows[i]["unitID"];
                    dr["minNumber"] = Convert.ToDecimal(_DataCons.Rows[i]["minNumber"]);
                    dr["maxNumber"] = Convert.ToDecimal(_DataCons.Rows[i]["maxNumber"]);
                    dr["note"] = _DataCons.Rows[i]["note"];
                    DataCons.Rows.Add(dr);
                }
                if (CurrentID.Trim() == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_TreatmentStage_Insert]" ,CommandType.StoredProcedure ,
                            "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@DataCons" ,SqlDbType.Structured ,DataCons.Rows.Count > 0 ? DataCons : null ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid
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
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_TreatmentStage_Update]" ,CommandType.StoredProcedure ,
                            "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@DataCons" ,SqlDbType.Structured ,DataCons.Rows.Count > 0 ? DataCons : null ,
                            "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                            "@currentID " ,SqlDbType.Int ,CurrentID

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
        }


    }
}
