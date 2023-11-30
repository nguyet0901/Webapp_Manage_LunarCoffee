using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;

namespace MLunarCoffee.Pages.Permission
{
    public class BlockReportGroupModel : PageModel
    {
        public string _AllowGroup { get; set; }
        public string _Password { get; set; }
        public void OnGet()
        {
            _AllowGroup = Comon.Global.sys_groupreport;
            if (Global.sys_ClientSystempass != null)
            {
                string syspass = Encrypt.DecryptString(Global.sys_ClientSystempass ,Settings.SemiSecret);
                _Password = Encrypt.EncryptString(syspass + DateTime.Now.ToString("yyyyMMdd") ,Settings.SemiSecret);
            }
            else _Password = "";
        }

        public async Task<IActionResult> OnPostLoadData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_User_Group_LoadList]" ,CommandType.StoredProcedure);
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

        public async Task<IActionResult> OnPostLoadDataDetail(string GroupID)
        {
            try
            {

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Per_GeExtentReport]" ,CommandType.StoredProcedure
                          ,"@GroupID" ,SqlDbType.Int ,GroupID);
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


        public async Task<IActionResult> OnPostExecuteData(string data ,string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_ReportBlock_Execute" ,CommandType.StoredProcedure ,
                          "@GroupID" ,SqlDbType.Int ,CurrentID ,
                          "@Data" ,SqlDbType.Structured ,dataDetail.Rows.Count > 0 ? dataDetail : null
                      );

                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("Error");
            }

        }
    }
}
