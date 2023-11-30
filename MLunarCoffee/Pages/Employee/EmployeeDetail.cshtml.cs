using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Employee
{
    public class EmployeeDetailModel : PageModel
    {
        public string _EmployeeDetailID { get; set; }
        public string _EmployeeGroupID { get; set; }
        public string _type { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            var user = Session.GetUserSession(HttpContext);
            string curr = Request.Query["CurrentID"];
            string group = Request.Query["GroupID"];
            string Type = Request.Query["Type"];
            if (curr != null)
            {
                _EmployeeDetailID = curr.ToString();
            }
            else
            {
                if (Type != null && Type != "undefined")
                {
                    _EmployeeDetailID = user.sys_employeeid.ToString();
                    _type = Type.ToString();
                }
                else
                {
                    _type = "";
                    _EmployeeDetailID = "0";
                }
            }
            if(group != null)
            {
                _EmployeeGroupID = group.ToString();
            }
            else
            {
                _EmployeeGroupID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadSalaryType()
        {
            try
            {

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Salary_Type_LoadCombo]" ,CommandType.StoredProcedure);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Employee_LoadDetail]" ,CommandType.StoredProcedure ,
                      "@ID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id));
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
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadComboMain()
        {
            DataSet ds = new DataSet();
            var user = Session.GetUserSession(HttpContext);
            //LoadGroup
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("MLU_sp_Employee_LoadCombo_Group" ,CommandType.StoredProcedure);
                ds.Tables.Add(dt);
            }

            //LoadGender
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.LoadPara("Gender");
                ds.Tables.Add(dt);
            }

            //LoadBranch
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
                    ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                    ,"@LoadNormal" ,SqlDbType.Int ,1);
                ds.Tables.Add(dt);
            }
            //LoadLevel
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[MLU_sp_Employee_Level_LoadCombo]" ,CommandType.StoredProcedure);
                ds.Tables.Add(dt);
            }
            //LoadContract
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[MLU_sp_Employee_Contract_LoadCombo]" ,CommandType.StoredProcedure);
                ds.Tables.Add(dt);
            }
            //Syntax
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[MLU_sp_Employee_Syntax_Load]" ,CommandType.StoredProcedure);
                ds.Tables.Add(dt);
            }
            return Content(Comon.DataJson.Dataset(ds));
        }
        private static DataTable ExecuteSalary(DataTable dtSalary)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id" ,typeof(string));
            dt.Columns.Add("SalaryID" ,typeof(string));
            dt.Columns.Add("Amount" ,typeof(decimal));

            for (int i = 0; i < dtSalary.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();

                dr["id"] = dtSalary.Rows[i]["id"];
                dr["SalaryID"] = dtSalary.Rows[i]["SalaryID"];
                dr["Amount"] = dtSalary.Rows[i]["Amount"];

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID ,string type ,string dataAllowrance)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                EmployeeDetail DataMain = JsonConvert.DeserializeObject<EmployeeDetail>(data);

                DataTable dtSalary = ExecuteSalary(JsonConvert.DeserializeObject<DataTable>(dataAllowrance));
                DataTable dt = new DataTable();

                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Employee_Insert" ,CommandType.StoredProcedure ,
                            "@Email" ,SqlDbType.NVarChar ,DataMain.Email1.Replace("'" ,"").Trim() ,
                            "@Nickname" ,SqlDbType.NVarChar ,DataMain.Nickname.Replace("'" ,"").Trim() ,
                            "@Address" ,SqlDbType.NVarChar ,DataMain.Address.Replace("'" ,"").Trim() ,
                            "@Phone" ,SqlDbType.NVarChar ,DataMain.Phone1.Replace("'" ,"").Trim() ,
                            "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@Code" ,SqlDbType.NVarChar ,DataMain.Code.Replace("'" ,"").Trim() ,                            
                            "@Gender_ID" ,SqlDbType.Int ,DataMain.Gender_ID ,
                            "@Group_ID" ,SqlDbType.Int ,DataMain.Group ,                            
                            "@relImage" ,SqlDbType.NVarChar ,DataMain.relImage.ToString() ,
                            "@feaImage" ,SqlDbType.NVarChar ,DataMain.feaImage.ToString() ,
                            "@Brithday" ,SqlDbType.NVarChar ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.Birthday) ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Branch" ,SqlDbType.NVarChar ,DataMain.Branch ,
                            "@isAllBranch" ,SqlDbType.Int ,DataMain.isAllBranch ,
                            "@levelID" ,SqlDbType.Int ,DataMain.Level ,
                            "@Contract" ,SqlDbType.Int ,DataMain.Contract ,
                            "@AnnuaLeave" ,SqlDbType.Int ,DataMain.AnnuaLeave ,
                            "@BasicSalary" ,SqlDbType.Decimal ,DataMain.BasicSalary ,
                            "@SalaryAgreed" ,SqlDbType.Decimal ,DataMain.SalaryAgreed ,
                            "@TotalAlowance" ,SqlDbType.Decimal ,DataMain.TotalAlowance ,
                            "@dt_Salary" ,SqlDbType.Structured ,(dtSalary != null && dtSalary.Rows.Count > 0) ? dtSalary : null ,
                            "@Ini_Date" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.Ini_Date)
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Employee_Update" ,CommandType.StoredProcedure ,
                            "@Email" ,SqlDbType.NVarChar ,DataMain.Email1.Replace("'" ,"").Trim() ,
                            "@Nickname" ,SqlDbType.NVarChar ,DataMain.Nickname.Replace("'" ,"").Trim() ,
                            "@Address" ,SqlDbType.NVarChar ,DataMain.Address.Replace("'" ,"").Trim() ,
                            "@Phone" ,SqlDbType.NVarChar ,DataMain.Phone1.Replace("'" ,"").Trim() ,
                            "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@Code" ,SqlDbType.NVarChar ,DataMain.Code.Replace("'" ,"").Trim() ,                            
                            "@Gender_ID" ,SqlDbType.Int ,DataMain.Gender_ID ,
                            "@Group_ID" ,SqlDbType.Int ,DataMain.Group ,                            
                            "@relImage" ,SqlDbType.NVarChar ,DataMain.relImage.ToString() ,
                            "@feaImage" ,SqlDbType.NVarChar ,DataMain.feaImage.ToString() ,
                            "@Brithday" ,SqlDbType.NVarChar ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.Birthday) ,
                            "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Branch" ,SqlDbType.NVarChar ,DataMain.Branch ,
                            "@isAllBranch" ,SqlDbType.Int ,DataMain.isAllBranch ,
                            "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                            "@levelID" ,SqlDbType.Int ,DataMain.Level ,
                            "@Contract" ,SqlDbType.Int ,DataMain.Contract ,
                            "@AnnuaLeave" ,SqlDbType.Int ,DataMain.AnnuaLeave ,
                            "@BasicSalary" ,SqlDbType.Decimal ,DataMain.BasicSalary ,
                            "@SalaryAgreed" ,SqlDbType.Decimal ,DataMain.SalaryAgreed ,
                            "@TotalAlowance" ,SqlDbType.Decimal ,DataMain.TotalAlowance ,
                            "@dt_Salary" ,SqlDbType.Structured ,(dtSalary != null && dtSalary.Rows.Count > 0) ? dtSalary : null ,
                            "@Ini_Date" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(DataMain.Ini_Date)
                        );
                    }
                    // if update user's info
                    //if (type == "updatePersonal")
                    //{
                    //    Comon.Comon.UpdateUserInfo_AtGlobal(DataMain.Avatar.ToString());
                    //}
                }
                return Content(DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }



    }
    public class EmployeeDetail
    {
        public string Email1 { get; set; }
        public string Nickname { get; set; }
        public string relImage { get; set; }
        public string feaImage { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }        
        public string CustomerID { get; set; }
        public int Gender_ID { get; set; }
        public int Group { get; set; }
        public string Birthday { get; set; }
        public string Avatar { get; set; }
        public string Branch { get; set; }
        public int isAllBranch { get; set; }
        public int Level { get; set; }
        public int Contract { get; set; }
        public int AnnuaLeave { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal SalaryAgreed { get; set; }
        public decimal TotalAlowance { get; set; }
        public string Ini_Date { get; set; }
    }
}
