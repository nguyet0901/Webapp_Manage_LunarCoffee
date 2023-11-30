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
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Customer
{
    public class CustomerStepModel : PageModel
    {
        public string _defaultAvatar { get; set; }
        public int _sys_ChooseDateCustomer { get; set; }
        public int _BranchID { get; set; }

        public void OnGet()
        {
            _defaultAvatar = "/default.png";
            var user = Session.GetUserSession(HttpContext);
            _BranchID = user.sys_branchID;
            _sys_ChooseDateCustomer = user.sys_ChooseDateCustomer;
        }

        
         public async Task<IActionResult> OnPostLoadInitializeData()
        {
            DataSet ds = new DataSet();
            var user = Session.GetUserSession(HttpContext);
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await  confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
                    , "@UserID", SqlDbType.Int, user.sys_userid
                    , "@LoadNormal", SqlDbType.Int, 1);
                dt.TableName = "Branch";
                ds.Tables.Add(dt);
            }

            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await  confunc.ExecuteDataTable("[MLU_sp_Career_Load]", CommandType.StoredProcedure);
                dt.TableName = "Career";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await  confunc.ExecuteDataTable("[MLU_sp_Customer_Type_ComboLoad]", CommandType.StoredProcedure);
                dt.TableName = "Source";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.LoadPara("Gender");
                dt.TableName = "Gender";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await  confunc.ExecuteDataTable("[YYY_LoadCombo_National]", CommandType.StoredProcedure);
                dt.TableName = "National";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await  confunc.ExecuteDataTable("[MLU_sp_LoadCombo_LocationCity]", CommandType.StoredProcedure);
                dt.TableName = "City";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt =await  confunc.ExecuteDataTable("[MLU_sp_LoadCombo_LocationDistrict]", CommandType.StoredProcedure);
                dt.TableName = "District";
                ds.Tables.Add(dt);
            }

            return Content(JsonConvert.SerializeObject(ds, Formatting.Indented));
        }
        
         public async Task<IActionResult> OnPostCheckPhoneNumber(string Phone1)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("MLU_sp_Customer_Check_Phone_Number", CommandType.StoredProcedure
                        , "@Phone1", SqlDbType.NVarChar, Phone1.Replace("'", "").Trim()
                    );
                }
                return Content(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
        
         public async Task<IActionResult> OnPostExcuteData(string data)
        {
            CustomerStep DataMain = JsonConvert.DeserializeObject<CustomerStep>(data);
            try
            {
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    connFunc.ExecuteDataSet("MLU_sp_Customer_Step_Insert", CommandType.StoredProcedure,
                        "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                        "@Email1", SqlDbType.NVarChar, DataMain.Email1.Replace("'", "").Trim(),
                        "@Address", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                        "@Phone1", SqlDbType.NVarChar, DataMain.Phone1.Replace("'", "").Trim(),
                        "@Phone2", SqlDbType.NVarChar, DataMain.Phone2.Replace("'", "").Trim(),
                        "@Branch_ID", SqlDbType.Int, DataMain.BranchID,
                        "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                        "@Gender_ID", SqlDbType.Int, DataMain.Gender_ID,
                        "@Type_Cat_ID", SqlDbType.Int, DataMain.Type_Cat_ID,
                        "@Language_ID", SqlDbType.Int, DataMain.Language_ID,
                        "@Birthday", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.Birthday),
                        "@instgramurl", SqlDbType.NVarChar, DataMain.instgramurl.Replace("'", "").Trim(),
                        "@facebookurl", SqlDbType.NVarChar, DataMain.facebookurl.Replace("'", "").Trim(),
                        "@viberurl", SqlDbType.NVarChar, DataMain.viberurl.Replace("'", "").Trim(),
                        "@zalomurl", SqlDbType.NVarChar, DataMain.zalomurl.Replace("'", "").Trim(),
                        "@CityID", SqlDbType.Int, DataMain.City,
                        "@District", SqlDbType.Int, DataMain.District,
                        "@Career", SqlDbType.Int, DataMain.Career,
                        "@phonecode", SqlDbType.NVarChar, DataMain.phonecode.ToString(),
                        "@NationalID", SqlDbType.Int, DataMain.NationalID,
                        "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                        "@Created_By", SqlDbType.Int, 0
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
    public class CustomerStep
    {
        public string Note { get; set; }
        public string Email1 { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Name { get; set; }
        public int Gender_ID { get; set; }
        public int Type_Cat_ID { get; set; }
        public int Language_ID { get; set; }
        public string Birthday { get; set; }
        public string instgramurl { get; set; }
        public string facebookurl { get; set; }
        public string viberurl { get; set; }
        public string zalomurl { get; set; }
        public string phonecode { get; set; }
        public int NationalID { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int BranchID { get; set; }
        public int Career { get; set; }
    }
}
