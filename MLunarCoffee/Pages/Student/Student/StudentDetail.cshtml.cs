using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Student.Student
{
    public class StudentDetailModel : PageModel
    {
        public string CurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null) CurrentID = curr.ToString();
            else CurrentID = "0";
        }
        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.LoadPara("Gender");
                        dt.TableName = "Gender";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Settings_Source_LoadCombo]", CommandType.StoredProcedure);
                        dt.TableName = "Source";
                        return dt;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));

            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostSearchCustomer(string TextSearch)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_ST_Settings_Student_SearchCustomer]", CommandType.StoredProcedure,
                        "@TextSearch", SqlDbType.NVarChar, TextSearch);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExecuted(int CurrentID, string data)
        {
            try
            {
                StudentData DataMain = JsonConvert.DeserializeObject<StudentData>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == 0)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_Student_Insert]", CommandType.StoredProcedure
                            , "@Avatar", SqlDbType.NVarChar, DataMain.Avatar
                            , "@Code", SqlDbType.NVarChar, DataMain.Code
                            , "@Name", SqlDbType.NVarChar, DataMain.Name
                            , "@Email", SqlDbType.NVarChar, DataMain.Email
                            , "@Address", SqlDbType.NVarChar, DataMain.Address
                            , "@Source", SqlDbType.Int, Convert.ToInt32(DataMain.Source)
                            , "@Gender", SqlDbType.Int, Convert.ToInt32(DataMain.Gender)
                            , "@Phone", SqlDbType.NVarChar, DataMain.Phone
                            , "@Birthday", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.Birthday.ToString())
                            , "@Identity", SqlDbType.NVarChar, DataMain.Identity
                            , "@Note", SqlDbType.NVarChar, DataMain.Note
                            , "@NameNoSign", SqlDbType.NVarChar, DataMain.NameNoSign
                            , "@CustomerID", SqlDbType.Int, DataMain.CustomerID
                            , "@CreateBy", SqlDbType.Int, user.sys_userid
                            );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_Student_Update]", CommandType.StoredProcedure
                            , "@Avatar", SqlDbType.NVarChar, DataMain.Avatar
                            , "@Code", SqlDbType.NVarChar, DataMain.Code
                            , "@Name", SqlDbType.NVarChar, DataMain.Name
                            , "@Email", SqlDbType.NVarChar, DataMain.Email
                            , "@Address", SqlDbType.NVarChar, DataMain.Address
                            , "@Source", SqlDbType.Int, Convert.ToInt32(DataMain.Source)
                            , "@Gender", SqlDbType.Int, Convert.ToInt32(DataMain.Gender)
                            , "@Phone", SqlDbType.NVarChar, DataMain.Phone
                            , "@Birthday", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.Birthday.ToString())
                            , "@Identity", SqlDbType.NVarChar, DataMain.Identity
                            , "@Note", SqlDbType.NVarChar, DataMain.Note
                            , "@NameNoSign", SqlDbType.NVarChar, DataMain.NameNoSign
                            , "@CustomerID", SqlDbType.Int, DataMain.CustomerID
                            , "@CurrentID", SqlDbType.NVarChar, CurrentID
                            , "@ModifyBy", SqlDbType.Int, user.sys_userid
                            );
                    }
                }

                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadDetail(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_Student_LoadDetail]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, CurrentID);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("0");
            }
            catch
            {
                return Content("0");
            }
        }
    }
    public class StudentData
    {
        public string Avatar { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Source { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public string Identity { get; set; }
        public string Note { get; set; }
        public string NameNoSign { get; set; }
        public int CustomerID { get; set; }
    }
}
