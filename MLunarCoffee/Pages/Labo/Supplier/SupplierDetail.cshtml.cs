using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Labo.Supplier
{
    public class SupplierDetailModel : PageModel
    {
        public string _LaboSupplierID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _LaboSupplierID = curr.ToString();
            }
            else
            {
                _LaboSupplierID = "0";
            }
        }

        public async Task<IActionResult> OnPostInitialize(int CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt=await confunc.ExecuteDataTable("[MLU_sp_print_v2_GetForm]",CommandType.StoredProcedure
                            ,"@TYPE",SqlDbType.NVarChar,"require_lab");
                    dt.TableName="LaboTemplate";
                    ds.Tables.Add(dt);
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
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadData(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Labo_Supplier_LoadDetail", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
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
                DataTable dt = new DataTable();
                LaboSupplierDetail DataMain = JsonConvert.DeserializeObject<LaboSupplierDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Supplier_Insert]", CommandType.StoredProcedure,
                              "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                              "@Code", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                              "@Phone", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Person", SqlDbType.Int, DataMain.Person.Replace("'", "").Trim(),
                              "@Address", SqlDbType.Int, DataMain.Address.Replace("'", "").Trim(),
                              "@FundLabo", SqlDbType.Int, DataMain.FundLabo.Replace("'", "").Trim(),
                              "@Avatar", SqlDbType.NVarChar, DataMain.Avatar.Replace("'", "").Trim(),
                              "@Mail", SqlDbType.NVarChar, DataMain.Mail.Replace("'", "").Trim(),
                              "@TemplateID", SqlDbType.Int, DataMain.TemplateID,
                              "@Owned", SqlDbType.Int, DataMain.Owned,
                              "@BankCode", SqlDbType.NVarChar, DataMain.BankCode.Trim(),
                              "@BankNumber", SqlDbType.NVarChar, DataMain.BankNumber.Trim()
                          );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Labo_Supplier_Update", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Code", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@currentID ", SqlDbType.Int, CurrentID,
                            "@Phone", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                            "@Person", SqlDbType.NVarChar, DataMain.Person.Replace("'", "").Trim(),
                            "@Address ", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                            "@FundLabo ", SqlDbType.Int, DataMain.FundLabo.Replace("'", "").Trim(),
                            "@Avatar", SqlDbType.NVarChar, DataMain.Avatar.Replace("'", "").Trim(),
                            "@Mail", SqlDbType.NVarChar, DataMain.Mail.Replace("'", "").Trim(),
                            "@TemplateID", SqlDbType.Int, DataMain.TemplateID,
                            "@Owned", SqlDbType.Int, DataMain.Owned,
                            "@BankCode", SqlDbType.NVarChar, DataMain.BankCode.Trim(),
                            "@BankNumber", SqlDbType.NVarChar, DataMain.BankNumber.Trim()
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

        public class LaboSupplierDetail
        {
            public string Name { get; set; }
            public string Code { get; set; }
            public string Person { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string FundLabo { get; set; }
            public string Avatar { get; set; }
            public string Mail { get; set; }
            public int TemplateID { get; set; }
            public int Owned { get; set; }
            public string BankCode { get; set; }
            public string BankNumber { get; set; }

        }
    }
}
