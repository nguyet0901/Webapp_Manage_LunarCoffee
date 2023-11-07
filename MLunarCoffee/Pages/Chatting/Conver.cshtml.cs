using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Chatting
{
    public class Conver : PageModel
    {
      
        public string layout { get; set; }
        
        public void OnGet()
        {
           
        }
        public async Task<IActionResult> OnPostLoadConver(string detail,int limit,string time)
        {
           
            try
            {
                DataTable dt = new DataTable();
                var user = Comon.Session.Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("YYY_Chatting_CustConverLoad",CommandType.StoredProcedure
                        ,"@ConverID",SqlDbType.NVarChar,detail
                        ,"@limit",SqlDbType.BigInt,limit
                        ,"@begin",SqlDbType.BigInt,time
                        ,"@userid",SqlDbType.Int,user.sys_userid);
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
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
        public async Task<IActionResult> OnPostSearchConver(string search)
        {

            try
            {
                DataTable dt = new DataTable();
                var user = Comon.Session.Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("YYY_Chatting_CustConverSearch",CommandType.StoredProcedure
                        ,"@search",SqlDbType.NVarChar,search!=null ? Comon.Comon.ConvertToUnsign(search).ToLower().Trim() : "");
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
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
