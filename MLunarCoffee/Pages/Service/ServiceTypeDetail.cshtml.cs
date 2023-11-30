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

namespace MLunarCoffee.Pages.Service
{
    public class ServiceTypeDetailModel : PageModel
    {
        public string _ServiceTypeCurrentID { get; set; }
        public string _templatePatientRecord { get; set; }
        public int _sys_Patient_Record { get; set; }

        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            string curr = Request.Query["CurrentID"];
            _sys_Patient_Record = Comon.Global.sys_Patient_Record;

            if (curr != null)
            {
                _ServiceTypeCurrentID = curr.ToString();
            }
            else
            {
                _ServiceTypeCurrentID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadInitialize(string CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_PatientRecord_LoadCombo_ByServiceType]", CommandType.StoredProcedure);
                    dt.TableName = "PatientRecord";
                    ds.Tables.Add(dt);
                }
                if (CurrentID != "0")
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[MLU_sp_ServiceCate_LoadDetail]", CommandType.StoredProcedure,
                          "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID));
                        dt.TableName = "Detail";
                        ds.Tables.Add(dt);
                    }
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        //recheck

        public async Task<IActionResult> OnPostDeleteTypeItem(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_v2_ServiceCate_Delete]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                ServiceType DataMain = JsonConvert.DeserializeObject<ServiceType>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                           dt = await connFunc.ExecuteDataTable("[MLU_sp_ServiceCate_Insert]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                              "@Code", SqlDbType.Int, DataMain.Code.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@IsPro", SqlDbType.Int, Convert.ToInt32(DataMain.IsPro),
                              "@IsExamination", SqlDbType.Int, Convert.ToInt32(DataMain.IsExamination),
                              "@Note", SqlDbType.Int, DataMain.Note.Replace("'", "").Trim(),
                              "@Color", SqlDbType.NVarChar, DataMain.Color.Replace("'", "").Trim(),
                              "@PatientRecordID", SqlDbType.Int, Convert.ToInt32(DataMain.PatientRecordID)
                          );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                         dt = await connFunc.ExecuteDataTable("MLU_sp_ServiceCate_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim()
                            , "@Code", SqlDbType.Int, DataMain.Code.Replace("'", "").Trim()
                            , "@Modified_By", SqlDbType.Int, user.sys_userid
                            , "@IsPro", SqlDbType.Int, Convert.ToInt32(DataMain.IsPro)
                            , "@IsExamination", SqlDbType.Int, Convert.ToInt32(DataMain.IsExamination)
                            , "@currentID ", SqlDbType.Int, CurrentID
                            , "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                            , "@Color ", SqlDbType.NVarChar, DataMain.Color.Replace("'", "").Trim()
                            , "@PatientRecordID", SqlDbType.Int, Convert.ToInt32(DataMain.PatientRecordID)
                            , "@IsCosmetic", SqlDbType.Int, Convert.ToInt32(Comon.Global.sys_DentistCosmetic)
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class ServiceType
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public string IsPro { get; set; }
        public string Color { get; set; }
        public string IsExamination { get; set; }
        public string PatientRecordID { get; set; }
    }
}
