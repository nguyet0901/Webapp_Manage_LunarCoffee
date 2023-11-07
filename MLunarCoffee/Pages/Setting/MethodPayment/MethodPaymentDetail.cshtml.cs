using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.MethodPayment
{
    public class MethodPaymentDetailModel : PageModel
    {
        public string _MethodPaymentDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _MethodPaymentDetailID = curr.ToString();
            }
            else
            {
                _MethodPaymentDetailID = "0";
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_MethodPayment_LoadDetail]" ,CommandType.StoredProcedure ,
                      "@CurrentID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id));
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
        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_MethodPaymentTypeAll_LoadCombo]" ,CommandType.StoredProcedure);
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

        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                MethodPayment DataMain = JsonConvert.DeserializeObject<MethodPayment>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_MethodPayment_Insert]" ,CommandType.StoredProcedure ,
                          "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                          "@Type" ,SqlDbType.Int ,DataMain.Type ,
                          "@Note" ,SqlDbType.NVarChar ,Regex.Replace(DataMain.Note ,"\"|\'" ,"") ,
                          "@UserID" ,SqlDbType.Int ,user.sys_userid
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "-1")
                            {
                                return Content("1");
                            }
                            else
                            {
                                return Content("-1");
                            }
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
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_MethodPayment_Update]" ,CommandType.StoredProcedure ,
                            "@Name" ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@Type" ,SqlDbType.Int ,DataMain.Type ,
                            "@Note" ,SqlDbType.NVarChar ,Regex.Replace(DataMain.Note ,"\"|\'" ,"") ,
                            "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                            "@CurrentID" ,SqlDbType.Int ,CurrentID
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("-1");
                            }
                            else
                            {
                                return Content("1");
                            }
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
    }
    public class MethodPayment
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
    }
}
