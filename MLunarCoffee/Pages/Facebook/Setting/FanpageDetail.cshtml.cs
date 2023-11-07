using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Fanpage.Comon.Session;

namespace Fanpage.Pages.Facebook.Setting
{
    public class FanpageDetailModel : PageModel
    {
        public string _FanpageID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _FanpageID = curr.ToString();
            }
            else
            {
                _FanpageID = "0";
            }
        }
 
         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[YYY_sp_Ticket_PageFacebook_LoadDetails]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
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
       
         public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_PageFacebookUser_UsernameCombo]", CommandType.StoredProcedure);
                    dt.TableName = "dataUser";
                    ds.Tables.Add(dt);
                }

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("ID", typeof(int));
                dt1.Columns.Add("Name", typeof(string));
                DataRow dr = dt1.NewRow();
                dr["ID"] = 0;
                dr["Name"] = "Không Chia";
                DataRow dr1 = dt1.NewRow();
                dr1["ID"] = 1;
                dr1["Name"] = "Chia Xoay Vòng";
                DataRow dr2 = dt1.NewRow();
                dr2["ID"] = 2;
                dr2["Name"] = "Chia Đều Nhân Viên Online";
                dt1.Rows.Add(dr);
                dt1.Rows.Add(dr1);
                dt1.Rows.Add(dr2);
                dt1.TableName = "dataDivision";
                ds.Tables.Add(dt1);

                //hide comment
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("ID", typeof(int));
                dt2.Columns.Add("Name", typeof(string));
                DataRow dr3 = dt2.NewRow();
                dr3["ID"] = 0;
                dr3["Name"] = "Không Ẩn";
                DataRow dr4 = dt2.NewRow();
                dr4["ID"] = 1;
                dr4["Name"] = "Ẩn";
                dt2.Rows.Add(dr3);
                dt2.Rows.Add(dr4);
                dt2.TableName = "dataHideComment";
                ds.Tables.Add(dt2);
                return Content(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
 
         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                string _divisionPer = Comon.Comon.Accending_String(DataMain.Rows[0]["UserToken"].ToString());
                string _chattingDividePer = Comon.Comon.Accending_String(DataMain.Rows[0]["ChattingDivide"].ToString());
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Ticket_PageFacebook_Insert]", CommandType.StoredProcedure,
                          "@Name", SqlDbType.NVarChar, DataMain.Rows[0]["Name"],
                          "@Url", SqlDbType.NVarChar, DataMain.Rows[0]["Url"],
                          "@KeyPage", SqlDbType.NVarChar, DataMain.Rows[0]["KeyPage"],
                          "@Note", SqlDbType.NVarChar, DataMain.Rows[0]["Note"],
                          "@Alias", SqlDbType.NVarChar, DataMain.Rows[0]["Alias"],
                          "@UserToken", SqlDbType.NVarChar, _divisionPer,
                          "@ChattingDivide", SqlDbType.NVarChar, _chattingDividePer,
                          "@ChattingDivision", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["ChattingDivision"].ToString()),
                          "@TeleSale", SqlDbType.NVarChar, DataMain.Rows[0]["TeleSale"],
                          "@TeleSaleDivision", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["TeleSaleDivision"].ToString()),
                          "@HideComment", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["hideComment"].ToString()),
                          "@UserID", SqlDbType.Int, user.sys_userid,
                          "@Created", SqlDbType.Int, DateTime.Now,
                          "@sys_limit_Fanpage", SqlDbType.Int, Comon.Global.sys_limit_Fanpage
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                if (dt.Rows[0][0].ToString() == "1")
                                    return Content("KeyPage đã tồn tại, vui lòng kiểm tra lại!");
                                else
                                    return Content("FanPage đã đạt số lượng tối đa cho phép!");
                            }
                            else
                            {
                                //CacheHelper.Set_FanPage_Person_List();
                                //CacheHelper.Set_Face_Comment_Hide_List();
                                //CacheHelper.Set_Face_Key_List();
                                return Content("1");
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
                        DataTable dt =await connFunc.ExecuteDataTable("[YYY_sp_Ticket_PageFacebook_Update]", CommandType.StoredProcedure,
                          "@CurrentID", SqlDbType.Int, CurrentID,
                          "@KeyPage", SqlDbType.NVarChar, DataMain.Rows[0]["KeyPage"],
                          "@Note", SqlDbType.NVarChar, DataMain.Rows[0]["Note"],
                          "@Alias", SqlDbType.NVarChar, DataMain.Rows[0]["Alias"],
                          "@UserToken", SqlDbType.NVarChar, _divisionPer,
                          "@ChattingDivide", SqlDbType.NVarChar, _chattingDividePer,
                          "@ChattingDivision", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["ChattingDivision"].ToString()),
                          "@TeleSale", SqlDbType.NVarChar, DataMain.Rows[0]["TeleSale"],
                          "@TeleSaleDivision", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["TeleSaleDivision"].ToString()),
                          "@HideComment", SqlDbType.Int, Convert.ToInt32(DataMain.Rows[0]["hideComment"].ToString()),
                          "@UserID", SqlDbType.Int, user.sys_userid,
                          "@Modified", SqlDbType.Int, DateTime.Now

                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("KeyPage đã tồn tại, vui lòng kiểm tra lại!");
                            }
                            else
                            {
                                //CacheHelper.Set_FanPage_Person_List();
                                //CacheHelper.Set_Face_Comment_Hide_List();
                                //CacheHelper.Set_Face_Key_List();
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
}
