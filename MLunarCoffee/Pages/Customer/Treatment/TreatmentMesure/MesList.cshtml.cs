using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer.Treatment.TreatmentMesure
{

    public class MesList : PageModel
    {
        public string _CustomerID { get; set; }
        public string _TreatmentID { get; set; }
        public void OnGet()
        {
            var CurrentID = Request.Query["CustomerID"];
            var TreatmentID = Request.Query["TreatmentID"];
            _TreatmentID = TreatmentID != String.Empty ? TreatmentID.ToString() : "0";
            if (CurrentID.ToString() != String.Empty)
            {
                _CustomerID = CurrentID.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostLoadIni(int CustomerID)
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
                        dt = await confunc.ExecuteDataTable("YYY_sp_Customer_MeasureContent", CommandType.StoredProcedure);
                        dt.TableName = "Content";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Customer_MeasureGetTypeName"
                            , CommandType.StoredProcedure
                            , "@Customer_ID", SqlDbType.Int, CustomerID);
                        dt.TableName = "CustType";
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
            catch (Exception)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadata(string CustomerID, string TreatmentID, string limit, string begin, string currentid)
        {
            DataSet ds = new DataSet();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_MeasureList]", CommandType.StoredProcedure,
                  "@Customer_ID", SqlDbType.Int, CustomerID
                  , "@TreatmentID", SqlDbType.Int, TreatmentID
                  , "@UserId", SqlDbType.Int, user.sys_userid
                  , "@Limit", SqlDbType.Int, limit
                  , "@CurrentID", SqlDbType.Int, currentid
                  , "@Begin", SqlDbType.Int, begin);
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


    }

}
