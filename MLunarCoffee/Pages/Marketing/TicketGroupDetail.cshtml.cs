using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing
{
    public class TicketGroupDetailModel : PageModel
    {
        public string _ticketGroupDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _ticketGroupDetailID = curr.ToString();
            }
            else
            {
                _ticketGroupDetailID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Ticket_GroupLoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostInitializeTicketGroup(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Ticket_Combo_UserName_Update]", CommandType.StoredProcedure,
                        "@Current_ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                    dt.TableName = "dataUser";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Combo_Group]", CommandType.StoredProcedure);
                    dt.TableName = "dataGroup";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Combo_Level]", CommandType.StoredProcedure);
                    dt.TableName = "dataLevel";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Combo_Extension]", CommandType.StoredProcedure,
                        "@Current_ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                    dt.TableName = "dataExtension";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception e)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                TicketGroup DataMain = JsonConvert.DeserializeObject<TicketGroup>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Group_Insert]", CommandType.StoredProcedure,
                            "@User_ID", SqlDbType.Int, DataMain.User_ID,
                              "@Level_ID", SqlDbType.Int, DataMain.Level_ID,
                              "@Group_ID", SqlDbType.Int, DataMain.Group_ID,
                              "@Call_ID", SqlDbType.Int, DataMain.Call_ID,
                              "@Created_By", SqlDbType.Int, user.sys_userid,
                              "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                              "@Note ", SqlDbType.Int, DataMain.Note.Replace("'", "").Trim()
                          );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("-1");//Nhóm Tele Đã Tồn Tại Vui Lòng Kiểm Tra Lại
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
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Group_Update]", CommandType.StoredProcedure,
                            "@User_ID", SqlDbType.Int, DataMain.User_ID,
                            "@Level_ID", SqlDbType.Int, DataMain.Level_ID,
                            "@Group_ID", SqlDbType.Int, DataMain.Group_ID,
                            "@Call_ID", SqlDbType.Int, DataMain.Call_ID,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@currentID", SqlDbType.Int, CurrentID,
                            "@Note ", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()

                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "1")
                            {
                                return Content("-1");
                            }
                            else
                            {
                                return Content("1");//Nhóm Tele Đã Tồn Tại Vui Lòng Kiểm Tra Lại
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
    public class TicketGroup
    {
        public string User_ID { get; set; }
        public string Group_ID { get; set; }
        public string Level_ID { get; set; }
        public string Call_ID { get; set; }
        public string Note { get; set; }
    }
}
