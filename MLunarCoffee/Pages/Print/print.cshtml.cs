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
using MLunarCoffee.Comon.Crypt;

namespace MLunarCoffee.Pages.Print
{
    public class print : PageModel
    {
        public string Type { get; set; }
        public string DetailID { get; set; }
        public string CurrentPath { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            var _Type = Request.Query["Type"];
            var _DateFrom = Request.Query["datefrom"];
            var _DateTo = Request.Query["dateto"];

            if (_Type.ToString() != String.Empty) Type = _Type.ToString();
            if (_DateFrom.ToString() != String.Empty) DateFrom = _DateFrom.ToString();
            else DateFrom = "0";
            if (_DateTo.ToString() != String.Empty) DateTo = _DateTo.ToString();
            else DateTo = "0";

            DetailID = Request.Query["DetailID"].ToString();
        }
        public async Task<IActionResult> OnPostLoadInitialize(string type)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_print_v2_GetForm]", CommandType.StoredProcedure
                             , "@TYPE", SqlDbType.NVarChar, type);
                        dt.TableName = "Form";
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
        public async Task<IActionResult> OnPostLoadData(int commandid, int idint, string idstring)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_print_v2_ExeCommand]", CommandType.StoredProcedure,
                      "@commandid", SqlDbType.Int, Convert.ToInt32(commandid)
                      , "@id", SqlDbType.Int, Convert.ToInt32(idint)
                      , "@idstring", SqlDbType.NVarChar, idstring != null ? idstring.ToString() : ""
                    );
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
        //public IActionResult OnPostEncryptApi(string TemplateID, string CommanID, string ID, string IDString, string ExchangeRate, string Value)
        //{
        //    try
        //    {
        //        var systaxString = TemplateID + "/" + CommanID + "/" + ID + "/" + IDString + "/" + ExchangeRate + "/" + Value;
        //        string EncryptString = "/api/Document/getForm/" + Encrypt.encryptApiDocPara(systaxString, Settings.SemiSecret);

        //        return Content(EncryptString);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("0");
        //    }
        //}

        public async Task<IActionResult> OnPostCheckStogeSign(string CustomerID, string StogeRule)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_PrintForm_StogeRule_Check]", CommandType.StoredProcedure,
                      "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                      , "@StogeRule", SqlDbType.Int, Convert.ToInt32(StogeRule)
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostSaveStogeSign(string CustomerID, string StogeSign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataSet("[MLU_sp_PrintForm_StogeRule_Insert]", CommandType.StoredProcedure,
                      "@CustomerID", SqlDbType.Int, Convert.ToInt32(CustomerID)
                      , "@StogeRule", SqlDbType.Int, Convert.ToInt32(StogeSign)
                      , "@CreatedBy", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
