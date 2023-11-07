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
using SourceMain.Models.Model.Media;
using System.ComponentModel.Design;

namespace SourceMain.Pages.Customer.DentistChart
{

    public class DentistChartDetailModel : PageModel
    {
        public string _imageFeature_Disease { get; set; }
        public void OnGet()
        {
            _imageFeature_Disease = Comon.Global.sys_HTTPImageRoot + "Sys_Disease_Feature/";
        }

        public async Task<IActionResult> OnPostLoad_Chart_Teeth_Detail_Info(string currentid)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_Chart_Teeth_LoadDetail_Info]", CommandType.StoredProcedure
                    , "@currentid", SqlDbType.Int, Convert.ToInt32(currentid));
            }
            if (ds != null)
            {
                return Content(Comon.DataJson.Dataset(ds));
            }
            else
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExecute(string data, string note, string CurrentID, string Type, string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                TeethChart[] DataMain = JsonConvert.DeserializeObject<TeethChart[]>(data);
                DataTable dtMain = Comon.Comon.ConvertToDataTable(DataMain);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Customer_Chart_Teeth_Insert", CommandType.StoredProcedure,
                             "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID),
                              "@Type", SqlDbType.Int, Convert.ToInt32(Type),
                             "@note", SqlDbType.NVarChar, note,
                              "@data", SqlDbType.Structured, dtMain.Rows.Count > 0 ? dtMain : null,
                             "@Created_By", SqlDbType.Int, user.sys_userid
                         );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Customer_Chart_Teeth_Update", CommandType.StoredProcedure,
                             "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                              "@note", SqlDbType.NVarChar, note,
                            "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID),
                              "@data", SqlDbType.Structured, dtMain.Rows.Count > 0 ? dtMain : null,
                             "@Created_By", SqlDbType.Int, user.sys_userid
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
        public async Task<IActionResult> OnPostLoadTemplate()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_GetForm]", CommandType.StoredProcedure
                             , "@TYPE", SqlDbType.NVarChar, "teeth_status");
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
        public async Task<IActionResult> OnPostLoadDataForm(int commandid, int currentID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_print_v2_ExeCommand]", CommandType.StoredProcedure,
                      "@commandid", SqlDbType.Int, Convert.ToInt32(commandid)
                      , "@id", SqlDbType.Int, Convert.ToInt32(currentID)
                      , "@idstring", SqlDbType.NVarChar, "");
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

    }
}
