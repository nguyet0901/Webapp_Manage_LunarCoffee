using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.Insurance
{
    public class InsuranceDetailModel : PageModel
    {
        public string _DataDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _DataDetailID = curr.ToString();
            }
            else
            {
                _DataDetailID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {

            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Insurance_LoadDetail]", CommandType.StoredProcedure,
                      "@CurrentID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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
            catch
            {
                return Content("");
            }

        }



        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                Insurance DataMain = JsonConvert.DeserializeObject<Insurance>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Insurance_Insert]", CommandType.StoredProcedure,
                              "@Name ", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                              "@Image", SqlDbType.NVarChar, DataMain.Image,
                              "@Phone ", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                              "@Person ", SqlDbType.NVarChar, DataMain.Person.Replace("'", "").Trim(),
                              "@Address ", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                              "@SigningTime ", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.SigningTime),
                              "@LinkFile ", SqlDbType.NVarChar, DataMain.LinkFile,
                              "@Status ", SqlDbType.Int, DataMain.Status,
                              "@Note ", SqlDbType.Int, DataMain.Note,
                              "@UserID", SqlDbType.Int, user.sys_userid
                          );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Insurance_Update]", CommandType.StoredProcedure,
                              "@Name ", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                              "@Image", SqlDbType.NVarChar, DataMain.Image,
                              "@Phone ", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                              "@Person ", SqlDbType.NVarChar, DataMain.Person.Replace("'", "").Trim(),
                              "@Address ", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                              "@SigningTime ", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.SigningTime),
                              "@LinkFile ", SqlDbType.NVarChar, DataMain.LinkFile,
                              "@Status ", SqlDbType.Int, DataMain.Status,
                              "@Note ", SqlDbType.Int, DataMain.Note,
                              "@UserID", SqlDbType.Int, user.sys_userid,
                              "@currentID ", SqlDbType.Int, CurrentID
                        );
                        if (dt.Rows.Count > 0)
                        {
                            return Content(Comon.DataJson.GetFirstValue(dt));
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public class Insurance
        {
            public string Name { get; set; }
            public string Image { get; set; }
            public string Person { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string LinkFile { get; set; }
            public string SigningTime { get; set; }
            public int Status { get; set; }
            public string Note { get; set; }
        }
    }
}
