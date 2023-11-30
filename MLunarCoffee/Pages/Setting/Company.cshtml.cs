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

namespace MLunarCoffee.Pages.Setting
{
    public class CompanyModel : PageModel
    {

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt=await connFunc.ExecuteDataTable("[MLU_sp_CompanyLoad]",CommandType.StoredProcedure);
                }

                if(dt!=null)
                {

                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch(Exception ex)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteCompany(string data)
        {

            try
            {
                CompanyValue DataMain = JsonConvert.DeserializeObject<CompanyValue>(data);
                var user = Session.GetUserSession(HttpContext);

                using(Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Company_Update]",CommandType.StoredProcedure,
                    "@Image ",SqlDbType.NVarChar,DataMain.ImageSmall,
                    "@Image1 ",SqlDbType.NVarChar,DataMain.ImageBig,
                    "@Name ",SqlDbType.NVarChar,DataMain.Name.Replace("'","").Trim(),
                    "@Phone ",SqlDbType.NVarChar,DataMain.Hotline.Replace("'","").Trim(),
                    "@Address ",SqlDbType.NVarChar,DataMain.Address.Replace("'","").Trim(),
                    "@Modified_By",SqlDbType.Int,user.sys_userid,
                    "@Modified",SqlDbType.DateTime,Comon.Comon.GetDateTimeNow(),
                    "@Email",SqlDbType.NVarChar,DataMain.Email,
                    "@Website",SqlDbType.NVarChar,DataMain.Website,
                    "@SoftwareLogo",SqlDbType.NVarChar,DataMain.SoftwareLogo,
                    "@SoftwareSmallLogo",SqlDbType.NVarChar,DataMain.SoftwareSmallLogo,
                    "@BranchAlias",SqlDbType.NVarChar,DataMain.BranchAlias
                );
                }
                return Content("1");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Content("0");
            }


        }
    }
    public class CompanyValue
    {
        public string Name { get; set; }
        public string Hotline { get; set; }
        public string Address { get; set; }
        public string ImageSmall { get; set; }
        public string ImageBig { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string SoftwareSmallLogo { get; set; }
        public string SoftwareLogo { get; set; }
        public string BranchAlias { get; set; }
    }
}

