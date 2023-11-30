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
    public class SmsOptionDetailModel : PageModel
    {
        public string _OptionCurrentID { get; set; }
        public string _Optionsystem { get; set; }
        public int _SMSAfterPayment { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string system = Request.Query["system"];
            _SMSAfterPayment = Comon.Global.sys_isSMSAfterPayment;
            _OptionCurrentID = (curr != null) ? curr.ToString() : "0";
            _Optionsystem = (system != null) ? system.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadInit(string branchID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                 
                dt = Comon.Global.sys_SMS_NotInBrandName.Copy();
                dt.TableName = "dataSmsRules";
                ds.Tables.Add(dt);

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Setting_SmsOption_LoadInfoGen" ,CommandType.StoredProcedure
                        ,"@BranchID" ,SqlDbType.Int ,Convert.ToInt32(branchID)
                        );
                    dt.TableName = "infoGen";
                    ds.Tables.Add(dt);
                }
                


                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_SMS_Option_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_SMS_Option_Manualdelete]", CommandType.StoredProcedure,
                           "@CurrentID", SqlDbType.Int, id,
                          "@Modified_By", SqlDbType.Int, user.sys_userid );
                }
                return Content(dt.Rows[0]["RESULT"].ToString());
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostChangeStatus(int CurrentID,int status)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_SMS_Option_ChangeStatus]", CommandType.StoredProcedure,
                           "@CurrentID", SqlDbType.Int, CurrentID,
                           "@status", SqlDbType.Int, status,
                          "@Modified_By", SqlDbType.Int, user.sys_userid );
                }
                return Content(dt.Rows[0]["RESULT"].ToString());
            }
            catch
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                SmsOption DataMain = JsonConvert.DeserializeObject<SmsOption>(data);
                var user = Session.GetUserSession(HttpContext);
                string checkContentSMSVi = Comon.Comon.CheckContentSMS(DataMain.Value.Replace("'", "").Trim());
                string checkContentSMSEng = Comon.Comon.CheckContentSMS(DataMain.ValueEng.Replace("'", "").Trim());
                if (checkContentSMSVi != "1")
                {
                    return Content(checkContentSMSVi);
                };
                if (checkContentSMSEng != "1")
                {
                    return Content(checkContentSMSEng);
                };
                int system = Convert.ToInt32(DataMain.System);
                if (system == 1)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_SMS_Option_Update]", CommandType.StoredProcedure,
                            "@Value", SqlDbType.NVarChar, DataMain.Value.Replace("'", "").Trim(),
                            "@ValueEng", SqlDbType.NVarChar, DataMain.ValueEng.Replace("'", "").Trim(),
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                            "@IsZNS" ,SqlDbType.Int ,DataMain.IsZNS,
                            "@Modified_By" , SqlDbType.Int, user.sys_userid,
                            "@CurrentID ", SqlDbType.Int, CurrentID
                        );
                        return Content("1");
                    }
                }
                else
                {
                    if (CurrentID == "0")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_SMS_Option_Manualinsert]", CommandType.StoredProcedure,
                               "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                               "@Value", SqlDbType.NVarChar, DataMain.Value.Replace("'", "").Trim(),
                               "@ValueEng", SqlDbType.NVarChar, DataMain.ValueEng.Replace("'", "").Trim(),
                               "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                               "@IsZNS" ,SqlDbType.Int ,DataMain.IsZNS,
                               "@Created_By" , SqlDbType.Int, user.sys_userid);
                            return Content(dt.Rows[0]["RESULT"].ToString());
                        }

                    }
                    else
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_SMS_Option_Manualupdate]", CommandType.StoredProcedure,
                               "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                               "@Value", SqlDbType.NVarChar, DataMain.Value.Replace("'", "").Trim(),
                               "@ValueEng", SqlDbType.NVarChar, DataMain.ValueEng.Replace("'", "").Trim(),
                               "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim(),
                               "@IsZNS", SqlDbType.Int, DataMain.IsZNS,
                               "@Modified_By", SqlDbType.Int, user.sys_userid ,
                               "@CurrentID " , SqlDbType.Int, CurrentID);
                            return Content(dt.Rows[0]["RESULT"].ToString());
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

    public class SmsOption
    {
        public string System { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueEng { get; set; }
        public string Note { get; set; }
        public int IsZNS { get; set; }
    }
}
