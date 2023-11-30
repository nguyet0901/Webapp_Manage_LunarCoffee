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
    public class TicketFile_File_MoveModel : PageModel
    {
        public string _ImportID { get; set; }
        public string _UserID { get; set; }
        private readonly IHubContext<NotiHub> hubContext;
        public TicketFile_File_MoveModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _UserID = user.sys_userid.ToString();
            string importID = Request.Query["ImportID"];
            if (importID != null)
            {
                _ImportID = importID.ToString();
            }
            else
            {
                _ImportID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadDataInitialize(string UserID, string ImportID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_Files_LoadData_Move]", CommandType.StoredProcedure,
                    "@ImportID", SqlDbType.Int, ImportID);
                    dt.TableName = "dataTicket";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_GroupTele_LoadCombo]", CommandType.StoredProcedure,
                    "@UserID", SqlDbType.Int, UserID);
                    dt.TableName = "dataGroup";
                    ds.Tables.Add(dt);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Ticket_File_TeleLevel_LoadCombo]", CommandType.StoredProcedure
                        , "@UserID", SqlDbType.Int, UserID);
                    dt.TableName = "dataLevel";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception e)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadDataUser(string GroupID, string LevelID, string UserID)
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
                    , "@UserID", SqlDbType.Int, Convert.ToInt32(UserID)
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
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Number", typeof(int));
                dt.Columns.Add("Name", typeof(string));
                string stringuser = "";
                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = dataDetail.Rows[i]["ID"];
                    stringuser = stringuser + dataDetail.Rows[i]["ID"].ToString() + ",";
                    dr["Number"] = dataDetail.Rows[i]["Number"];
                    dr["Name"] = dataDetail.Rows[i]["Name"];
                    dt.Rows.Add(dr);
                }
                DataTable dtFile = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dtFile = await connFunc.ExecuteDataTable("MLU_sp_Ticket_File_Files_Move_Insert_v2", CommandType.StoredProcedure,
                        "@Created_By", SqlDbType.Int, user.sys_userid,
                        "@ImportID", SqlDbType.Int, ImportID,
                        "@data_table", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                    );
                }
                if (dtFile != null)
                {
                    return Content(dtFile.Rows[0][0].ToString());
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
