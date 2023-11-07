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
using MLunarCoffee.Pages.Customer.Payment;

namespace MLunarCoffee.Pages.Facebook.Settings.FacebookPage
{
    public class FacebookPageEmpDetail : PageModel
    {
        public string _SerFanpageID { get; set; }
        public void OnGet()
        {
            string Curr = Request.Query["CurrentID"];
            if (Curr != null)
            {
                _SerFanpageID= Curr.ToString();
            }
            else {
                _SerFanpageID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadInit()
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
                        DataTable SerType = new DataTable();
                        SerType = await confunc.ExecuteDataTable("YYY_sp_Setting_FB_Fanpage_Emp_LoadComboUE" ,CommandType.StoredProcedure);
                        SerType.TableName = "DataEmpUser";
                        return SerType;
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
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(int ID)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Setting_FB_Fanpage_Emp_LoadDetail]" , CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, Convert.ToInt32(ID == 0 ? 0 : ID));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch
            {
                return Content("");
            }

        }
        public async Task<IActionResult> OnPostExecuted(string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                fb_fanpageEmp DataMain = JsonConvert.DeserializeObject<fb_fanpageEmp>(data);
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(DataMain.DataDetail);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("YYY_sp_Setting_FB_Fanpage_Emp_Insert" , CommandType.StoredProcedure                        
                        ,"@FanpageID" ,SqlDbType.Int ,DataMain.FanpageID
                        ,"@DataDetail" ,SqlDbType.Structured ,dataDetail.Rows.Count > 0 ? dataDetail : null
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content("1");
                
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class fb_fanpageEmp
        {            
            public int FanpageID { get; set; }
            
            public string DataDetail { get; set; }
        }
    }
}
