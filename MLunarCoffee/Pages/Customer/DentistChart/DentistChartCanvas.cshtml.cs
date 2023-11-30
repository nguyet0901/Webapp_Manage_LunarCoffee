using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Models.Model.Media;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.DentistChart
{

    public class DentistChartCanvas : PageModel
    {
        public string _Type { get; set; }
        public string _CurrentID { get; set; }

        public void OnGet()
        {
            _Type = Request.Query["Type"] != "" ? Request.Query["Type"].ToString() : "0";
            _CurrentID = Request.Query["CurrentID"] != "" ? Request.Query["CurrentID"].ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadIni(string Type)
        {
            DataSet ds = new DataSet();
            var user = Session.GetUserSession(HttpContext);
            var tasks = new List<Task<DataTable>>();
            tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_DentistChart_LoadTeeth]" ,CommandType.StoredProcedure
                         ,"@Type" ,SqlDbType.Int ,Type);
                        dt.TableName = "TeethDefault";
                        return dt;
                    }
                }));
            tasks.Add(Task.Run(async () =>
            {
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_DentistChart_LoadClassi]" ,CommandType.StoredProcedure);
                    dt.TableName = "Classi";
                    return dt;
                }
            }));
            tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_DentistChart_LoadDisease]" ,CommandType.StoredProcedure);
                        dt.TableName = "TeethDisease";
                        return dt;
                    }
                }));
            tasks.Add(Task.Run(async () =>
              {
                  using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                  {
                      DataTable dt = new DataTable();
                      dt = await confunc.ExecuteDataTable("[MLU_sp_DentistChart_LoadImage]" ,CommandType.StoredProcedure);
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
        public async Task<IActionResult> OnPostLoadTemplate()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_print_v2_GetForm]" ,CommandType.StoredProcedure
                             ,"@TYPE" ,SqlDbType.NVarChar ,"teeth_status");
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }

            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadDataForm(int commandid ,int currentID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_print_v2_ExeCommand]" ,CommandType.StoredProcedure ,
                      "@commandid" ,SqlDbType.Int ,Convert.ToInt32(commandid)
                      ,"@id" ,SqlDbType.Int ,Convert.ToInt32(currentID)
                      ,"@idstring" ,SqlDbType.NVarChar ,"");
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
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
        public async Task<IActionResult> OnPostLoadDetail(string currentid)
        {
            DataSet ds = new DataSet();

            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[MLU_sp_DentistChart_LoadDetail]" ,CommandType.StoredProcedure
                    ,"@currentid" ,SqlDbType.Int ,Convert.ToInt32(currentid));
            }
            if (ds != null)
            {
                return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostExecute(string data ,string CurrentID ,string dataDetail)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                TeethChartv2[] DataMain = JsonConvert.DeserializeObject<TeethChartv2[]>(data);
                DataTable dtMain = Comon.Comon.ConvertToDataTable(DataMain);
                DentistChartDetail dtDetail = JsonConvert.DeserializeObject<DentistChartDetail>(dataDetail);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_DentistChart_Insert" ,CommandType.StoredProcedure ,
                             "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(dtDetail.CustomerID) ,
                             "@Type" ,SqlDbType.Int ,Convert.ToInt32(dtDetail.Type) ,
                             "@Note" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.Note) ? dtDetail.Note : "" ,
                             "@Subclinical" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.Subclinical) ? dtDetail.Subclinical : "" ,
                             "@Diagnose" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.Diagnose) ? dtDetail.Diagnose : "" ,
                             "@IndicationsTreat" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.IndicationsTreat) ? dtDetail.IndicationsTreat : "" ,
                             "@EstimatedTreat" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.EstimatedTreat) ? dtDetail.EstimatedTreat : "" ,
                             "@data" ,SqlDbType.Structured ,dtMain.Rows.Count > 0 ? dtMain : null ,
                             "@Created_By" ,SqlDbType.Int ,user.sys_userid
                         );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_DentistChart_Update" ,CommandType.StoredProcedure ,
                            "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID) ,
                            "@Note" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.Note) ? dtDetail.Note : "" ,
                            "@Subclinical" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.Subclinical) ? dtDetail.Subclinical : "" ,
                            "@Diagnose" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.Diagnose) ? dtDetail.Diagnose : "" ,
                            "@IndicationsTreat" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.IndicationsTreat) ? dtDetail.IndicationsTreat : "" ,
                            "@EstimatedTreat" ,SqlDbType.NVarChar ,!String.IsNullOrEmpty(dtDetail.EstimatedTreat) ? dtDetail.EstimatedTreat : "" ,
                            "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(dtDetail.CustomerID) ,
                            "@data" ,SqlDbType.Structured ,dtMain.Rows.Count > 0 ? dtMain : null ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid
                         );
                    }

                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }

    public class DentistChartDetail
    {
        public string CustomerID { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public string Subclinical { get; set; }
        public string Diagnose { get; set; }
        public string IndicationsTreat { get; set; }
        public string EstimatedTreat { get; set; }
    }
}
