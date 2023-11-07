using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Permission
{
    public class ControlAllowGroupModel : PageModel
    {
        public string _Group { get; set; }
        public string _Password { get; set; }
        public void OnGet()
        {
            if (Global.sys_ClientSystempass != null)
            {
                string syspass = Encrypt.DecryptString(Global.sys_ClientSystempass, Settings.SemiSecret);
                _Password = Encrypt.EncryptString(syspass + DateTime.Now.ToString("yyyyMMdd"), Settings.SemiSecret);
            }
            else _Password = "";

            
        }
        public async Task<IActionResult> OnPostLoadData()
        {
            DataSet ds = new DataSet();
            var tasks = new List<Task<DataTable>>();
            tasks.Add(Task.Run(async () =>
            {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_User_Group_LoadList]", CommandType.StoredProcedure);
                        dt.TableName = "GroupList";
                        return dt;
                    }
                }));
            tasks.Add(Task.Run(async () =>
            {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_PerGetLock]", CommandType.StoredProcedure
                            , "@Type", SqlDbType.Int, 1);
                        dt.TableName = "Lock";
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

        public async Task<IActionResult> OnPostLoadDataDetail(string CurrentID)
        {
            int groupID = Convert.ToInt32(CurrentID);

            DataTable Extent = new DataTable();
            Extent = await LoadExtensionCient(groupID); // su dung group  id cua client de xem nhung quyen add them hoac xoa bo
            DataTable Control = new DataTable();
            Control.Columns.Add("ID", typeof(Int32));
            Control.Columns.Add("Name", typeof(String));
            Control.Columns.Add("GroupNameLangKey", typeof(String));
            Control.Columns.Add("GroupID", typeof(Int32)); // Group tren server
            Control.Columns.Add("GroupName", typeof(String)); // Group tren server
            Control.Columns.Add("DescriptionLangKey", typeof(String));
            Control.Columns.Add("isBlock", typeof(Int32));
            Control = CreateInitializeAllPer(Control);
            for (int i = 0; i < Control.Rows.Count; i++)
            {
                int ControlTabID = Convert.ToInt32(Control.Rows[i]["ID"]);
                int checkClient = CheckPermisionExtension(Extent, ControlTabID);
                if (checkClient == 1)
                {
                    Control.Rows[i]["isBlock"] = 0;
                }
                else if (checkClient == 2)
                {
                    Control.Rows[i]["isBlock"] = 1;
                }
                else Control.Rows[i]["isBlock"] = 0;
     
            }
            return Content(Comon.DataJson.Datatable(Control));
        }
        private static DataTable CreateInitializeAllPer(DataTable dt)
        {
            // Tat ca cac menu ma client co the thay
            for (int i = 0; i < Comon.Global.sys_List_TabControl_Allowed.Rows.Count; i++)
            {
                if (Comon.Global.sys_List_TabControl_Allowed.Rows[i]["IsHaving"].ToString() == "1")
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = Convert.ToInt32(Comon.Global.sys_List_TabControl_Allowed.Rows[i]["ID"].ToString());
                    dr["Name"] = Comon.Global.sys_List_TabControl_Allowed.Rows[i]["Description"].ToString();
                    dr["GroupNameLangKey"] = Comon.Global.sys_List_TabControl_Allowed.Rows[i]["GroupNameLangKey"].ToString();
                    dr["GroupID"] = Convert.ToInt32(Comon.Global.sys_List_TabControl_Allowed.Rows[i]["GroupID"].ToString());
                    dr["GroupName"] = Comon.Global.sys_List_TabControl_Allowed.Rows[i]["GroupName"].ToString();
                    dr["DescriptionLangKey"] = Comon.Global.sys_List_TabControl_Allowed.Rows[i]["DescriptionLangKey"].ToString();
                    dr["isBlock"] = 0;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        private static async Task<DataTable> LoadExtensionCient(int GroupID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_Per_GeExtentControlTab", CommandType.StoredProcedure,
                          "@GroupID", SqlDbType.Int, GroupID
                      );

                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        /// <summary>
        /// 0: Check tren server
        /// 1: Cho phep xem
        /// 2: Tu Choi thang
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="menuID"></param>
        /// <returns></returns>
        private static int CheckPermisionExtension(DataTable dt, int ControlTabID)
        {
            try
            {
                // neu chua co du lieu extension , se chuyen xuong coi co quyen tren server hay khong
                if (dt == null || dt.Rows.Count == 0) return 0;
                // kiem tra control trong he thong phan quyen tren main server
                // dt co du lieu co nghia la da co thao tac phan quyen tren nay
                DataRow[] rows = dt.Select(String.Format(@"ControlTabID={0}", ControlTabID));
                if (rows[0]["State"].ToString() == "0") return 2;
                return 1;

            }
            catch (Exception ex)
            {
                // TIm khong thay doi voi me nu nay nghia la menu moi cap phat tu tren server xuong
                return 0;

            }
        }
        //private static bool CheckPermisionOfGroupFROMSERVER(int GroupID,int menuID)
        //{
        //    try
        //    {
        //        // kiem tra control trong he thong phan quyen tren main server
        //        DataRow[] rows = Comon.Global.sys_PermissionTableMenu_Table.Select(String.Format(@"PermissionGroupID = {0} AND MenuID={1}", GroupID, menuID));
        //        return (rows.Length>0);
        //    }
        //    catch(Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public async Task<IActionResult> OnPostExecuteData(string data, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("YYY_sp_ControlGroupAllow_Execute", CommandType.StoredProcedure,
                          "@GroupID", SqlDbType.Int, CurrentID,
                          "@Data", SqlDbType.Structured, dataDetail.Rows.Count > 0 ? dataDetail : null
                      );

                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("Error");
            }

        }
    }
}
