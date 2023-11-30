using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace MLunarCoffee.Pages.Marketing.TicketFile
{
    public class TicketFile_File_DevideModel : PageModel
    {
        public string _ImportID { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public TicketFile_File_DevideModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _ImportID = curr.ToString();
            }
            else
            {
                _ImportID = "0";
            }
        }


        public async Task<IActionResult> OnPostLoadDataInitialize(string ImportID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtDevide = new DataTable();
                        dtDevide = await confunc.LoadPara("DevideType");
                        dtDevide.TableName = "dtDevideType";
                        return dtDevide;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtTicket = new DataTable();
                        dtTicket = await confunc.ExecuteDataTable("[MLU_sp_Ticket_Import_Last_Devide]", CommandType.StoredProcedure
                           , "@ImportID", SqlDbType.Int, Convert.ToInt32(ImportID)
                           , "@UserID", SqlDbType.Int, Convert.ToInt32(user.sys_userid)
                          );
                        dtTicket.TableName = "dataTicket";
                        return dtTicket;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGroup = new DataTable();
                        dtGroup = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_GroupTele_LoadCombo]", CommandType.StoredProcedure,
                        "@UserID", SqlDbType.Int, user.sys_userid);
                        dtGroup.TableName = "dataGroup";
                        return dtGroup;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtLevel = new DataTable();
                        dtLevel = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_TeleLevel_LoadCombo]", CommandType.StoredProcedure
                            , "@UserID", SqlDbType.Int, user.sys_userid);
                        dtLevel.TableName = "dataLevel";
                        return dtLevel;
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
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadDataUser(string GroupID, string LevelID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_TicketDevide_LoadUser]", CommandType.StoredProcedure
                    , "@GroupID", SqlDbType.Int, Convert.ToInt32(GroupID)
                    , "@LevelID", SqlDbType.Int, Convert.ToInt32(LevelID)
                    , "@UserID", SqlDbType.Int, Convert.ToInt32(user.sys_userid)
                                        );
                    dt.TableName = "dataUser";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception e)
            {
                return Content("");
            }
        }


        public async Task<IActionResult> OnPostExcuteData(string data, string ImportID)
        {
            try
            {
                DataTable dataDevide = new DataTable();
                dataDevide = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);

                DataTable TableDevide = new DataTable();
                TableDevide.Columns.Add("Index", typeof(int));
                TableDevide.Columns.Add("ID", typeof(int));
                TableDevide.Columns.Add("Amount", typeof(int));
                int Index = 1;
                for (int i = 0; i < dataDevide.Rows.Count; i++)
                {
                    if (dataDevide.Rows[i]["isDevide"].ToString() == "1")
                    {
                        DataRow dr = TableDevide.NewRow();
                        dr["Index"] = Index;
                        dr["ID"] = Convert.ToInt32(dataDevide.Rows[i]["ID"].ToString());
                        dr["Amount"] = Convert.ToInt32(dataDevide.Rows[i]["Amount"].ToString());
                        TableDevide.Rows.Add(dr);
                        Index++;
                    }
                }
                if (ImportID != "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("MLU_sp_Ticket_Devide", CommandType.StoredProcedure,
                              "@ImportID", SqlDbType.Int, ImportID,
                              "@CreatedBy", SqlDbType.Int, user.sys_userid,
                              "@table_data", SqlDbType.Structured, TableDevide.Rows.Count > 0 ? TableDevide : null
                          );
                    }

                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
