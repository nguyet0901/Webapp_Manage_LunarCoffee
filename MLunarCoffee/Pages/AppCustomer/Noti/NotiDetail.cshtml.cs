using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.AppCustomer.Noti
{
    public class NotiDetailModel : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Notification_AC_Article]" ,CommandType.StoredProcedure);
                    dt.TableName = "Article";
                    ds.Tables.Add(dt);
                }
                if (ds != null)
                    return Content(Comon.DataJson.Dataset(ds));
                return Content("[]");

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }



        public async Task<IActionResult> OnPostExecute(string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                NotiDetail notiDetail = JsonConvert.DeserializeObject<NotiDetail>(data);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_AppCustomer_Notification_Insert]" ,CommandType.StoredProcedure ,
                        "@Title" ,SqlDbType.NVarChar ,notiDetail.Title ,
                        "@Body" ,SqlDbType.NVarChar ,notiDetail.Body ,
                        "@Type" ,SqlDbType.NVarChar ,notiDetail.Type ,
                        "@ArticleID" ,SqlDbType.Int ,notiDetail.ArticleID ,
                        "@Sender" ,SqlDbType.Int ,notiDetail.Sender ,
                        "@Device" ,SqlDbType.Int ,notiDetail.IsDevice ,
                        "@CustomerID" ,SqlDbType.Int ,notiDetail.CustomerID ,
                        "@UsingLink" ,SqlDbType.Int ,notiDetail.UsingLink ,

                        
                        "@UserID" ,SqlDbType.Int ,user.sys_userid);
                }
                if (dt != null)
                    return Content(Comon.DataJson.GetFirstValue(dt));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }



    }

    public class NotiDetail
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
        public int ArticleID { get; set; }
        public int Sender { get; set; }
        public int IsDevice { get; set; }
        public int CustomerID { get; set; }
        public int UsingLink { get; set; }

        
    }
}
