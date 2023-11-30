using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Dash
{
    public class ProductStatus : PageModel
    {

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadData(string WareID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt=await confunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_ProductStatus]",CommandType.StoredProcedure

                        ,"@WareID",SqlDbType.Int,WareID);
                    dt.TableName="Norm";
                    ds.Tables.Add(dt);
                }
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataSet dse = new DataSet();
                    dse=await confunc.ExecuteDataSet("[MLU_sp_v2_WareHouse_ProductExpired]",CommandType.StoredProcedure
                        ,"@WareID",SqlDbType.Int,WareID);
                    DataTable dt = new DataTable();
                    dt=dse.Tables[0].Copy();
                    dt.TableName="ExpiredTime";

                    DataTable dt1 = new DataTable();
                    dt1=dse.Tables[1].Copy();
                    dt1.TableName="ExpiredData";

                    ds.Tables.Add(dt);
                    ds.Tables.Add(dt1);
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
