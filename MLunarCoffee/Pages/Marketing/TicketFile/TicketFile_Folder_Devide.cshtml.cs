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
    public class TicketFile_Folder_DevideModel : PageModel
    {
        public string _FileID { get; set; }
        public string _BlockIndex { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public TicketFile_Folder_DevideModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            string _currentid = Request.Query["CurrentID"];
            _FileID = _currentid != null ? _currentid.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadDataInitialize(int FileID)
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Ticket_File_Folder_Last_Devide_v2]", CommandType.StoredProcedure
                       , "@FileID", SqlDbType.Int, FileID
                       , "@UserID", SqlDbType.Int, Convert.ToInt32(user.sys_userid));
                    dt.TableName = "dataTicket";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_GroupTele_LoadCombo]", CommandType.StoredProcedure,
                    "@UserID", SqlDbType.Int, user.sys_userid);
                    dt.TableName = "dataGroup";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_TeleLevel_LoadCombo]", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, user.sys_userid);
                    dt.TableName = "dataLevel";
                    ds.Tables.Add(dt);
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


        public async Task<IActionResult> OnPostExcuteData(string data, int FileID)
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
                DataTable dt = new DataTable();
                if (FileID != 0)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_Folder_Devide_v2]", CommandType.StoredProcedure,
                           "@FileID", SqlDbType.Int, FileID,
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
