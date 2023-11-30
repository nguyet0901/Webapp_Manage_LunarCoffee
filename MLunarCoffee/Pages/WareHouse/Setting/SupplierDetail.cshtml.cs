using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.WareHouse.Setting
{
    public class SupplierDetailModel : PageModel
    {
        public string _SupplierDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _SupplierDetailID = curr.ToString();
            }
            else
            {
                _SupplierDetailID = "0";
            }
        }

        public async Task<IActionResult> OnPostInitialize(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Supplier_LoadProductList]", CommandType.StoredProcedure);
                    dt.TableName = "cbbProduct";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Product_Supplier_LoadDetail]", CommandType.StoredProcedure,
                    "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                    dt.TableName = "dataDetail";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }


        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                SupplierDetail DataMain = JsonConvert.DeserializeObject<SupplierDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Product_Supplier_Insert]", CommandType.StoredProcedure,
                             "@Name ", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                             "@Code ", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                             "@Phone ", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                             "@Created_By", SqlDbType.Int, user.sys_userid,
                             "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                             "@Person ", SqlDbType.Int, DataMain.Person.Replace("'", "").Trim(),
                             "@Email ", SqlDbType.Int, DataMain.Email.Replace("'", "").Trim(),
                             "@Note ", SqlDbType.Int, DataMain.Note.Replace("'", "").Trim(),
                             "@Bank_code ", SqlDbType.NVarChar, DataMain.Bank_code.Replace("'", "").Trim(),
                             "@Bank_number ", SqlDbType.NVarChar, DataMain.Bank_number.Replace("'", "").Trim(),
                             "@Product", SqlDbType.NVarChar, DataMain.Product,
                             "@Website", SqlDbType.NVarChar, DataMain.Website,
                             "@Deposit", SqlDbType.Decimal, DataMain.Deposit
                         );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Product_Supplier_Update", CommandType.StoredProcedure,
                           "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                           "@Code ", SqlDbType.NVarChar, DataMain.Code.Replace("'", "").Trim(),
                           "@Modified_By", SqlDbType.Int, user.sys_userid,
                           "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                           "@currentID ", SqlDbType.Int, CurrentID,
                           "@Phone", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                           "@Person", SqlDbType.NVarChar, DataMain.Person.Replace("'", "").Trim(),
                           "@Email ", SqlDbType.NVarChar, DataMain.Email.Replace("'", "").Trim(),
                           "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                           "@Bank_code ", SqlDbType.NVarChar, DataMain.Bank_code.Replace("'", "").Trim(),
                           "@Bank_number ", SqlDbType.NVarChar, DataMain.Bank_number.Replace("'", "").Trim(),
                           "@Product", SqlDbType.NVarChar, DataMain.Product,
                           "@Website", SqlDbType.NVarChar, DataMain.Website,
                           "@Deposit", SqlDbType.Decimal, DataMain.Deposit

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
        public class SupplierDetail
        {
            public string Name { get; set; }
            public string Code { get; set; }
            public string Person { get; set; }
            public string Email { get; set; }
            public string Note { get; set; }
            public string Phone { get; set; }
            public string Product { get; set; }
            public string Bank_code { get; set; }
            public string Bank_number { get; set; }
            public string Website { get; set; }
            public decimal Deposit { get; set; }
        }
    }
}
