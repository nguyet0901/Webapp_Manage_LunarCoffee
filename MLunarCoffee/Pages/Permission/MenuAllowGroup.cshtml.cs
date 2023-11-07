using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;


namespace MLunarCoffee.Pages.Permission
{
    public class MenuAllowGroupModel : PageModel
    {
        public string _Group { get; set; }
        public void OnGet()
        {
        }
         public async Task<IActionResult> OnPostLoadData()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
            {
                dt =await connFunc.ExecuteDataTable("[YYY_sp_User_Group_LoadList]", CommandType.StoredProcedure);
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("[]");
            }
        }

         public async Task<IActionResult> OnPostLoadDataDetail(string CurrentID, string GroupFromServerID)
        {
            try
            {
                int groupID = Convert.ToInt32(CurrentID);
                int groupServerID = Convert.ToInt32(GroupFromServerID);
                DataTable Extent = new DataTable();
                Extent = await LoadExtensionCient(groupID); // su dung group  id cua client de xem nhung quyen add them hoac xoa bo
                DataTable Menu = new DataTable();
                Menu.Columns.Add("ID", typeof(Int32));
                Menu.Columns.Add("Name", typeof(String));
                Menu.Columns.Add("NameLangKey", typeof(String));
                Menu.Columns.Add("GroupID", typeof(Int32)); // Group tren server
                                                            //Menu.Columns.Add("PermissionGroupID", typeof(Int32));
                Menu.Columns.Add("GroupName", typeof(String)); // Group tren server
                Menu.Columns.Add("GroupNameLangKey", typeof(String));
                Menu.Columns.Add("isBlock", typeof(Int32));
                Menu = CreateInitializeAllPer(Menu);
                for (int i = 0; i < Menu.Rows.Count; i++)
                {
                    int menuID = Convert.ToInt32(Menu.Rows[i]["ID"]);
                    int checkClient = CheckPermisionExtension(Extent, menuID);
                    if (checkClient == 1)
                    {
                        Menu.Rows[i]["isBlock"] = 0;
                    }
                    else if (checkClient == 2)
                    {
                        Menu.Rows[i]["isBlock"] = 1;
                    }

                    else
                    {
                        if (CheckPermisionOfGroupFROMSERVER(groupServerID, menuID))
                        {
                            Menu.Rows[i]["isBlock"] = 0;
                        }
                        else
                        {
                            Menu.Rows[i]["isBlock"] = 1;
                        }

                    }
                }
                return Content(Comon.DataJson.Datatable(Menu));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        private static DataTable CreateInitializeAllPer(DataTable dt)
        {
            // Tat ca cac menu ma client co the thay
            for (int i = 0; i < Comon.Global.sys_PermissionAllMenu.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = Convert.ToInt32(Comon.Global.sys_PermissionAllMenu.Rows[i]["ID"].ToString());
                dr["Name"] = Comon.Global.sys_PermissionAllMenu.Rows[i]["Name"].ToString();
                dr["NameLangKey"] = Comon.Global.sys_PermissionAllMenu.Rows[i]["NameLangKey"].ToString();
                dr["GroupID"] = Convert.ToInt32(Comon.Global.sys_PermissionAllMenu.Rows[i]["GroupID"].ToString());
                dr["GroupName"] = Comon.Global.sys_PermissionAllMenu.Rows[i]["GroupName"].ToString();
                dr["GroupNameLangKey"] = Comon.Global.sys_PermissionAllMenu.Rows[i]["GroupNameLangKey"].ToString();
                dr["isBlock"] = 0;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        private static async Task<DataTable>  LoadExtensionCient(int GroupID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("YYY_sp_Per_GeExtentMenu", CommandType.StoredProcedure,
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
        private static int CheckPermisionExtension(DataTable dt, int menuID)
        {
            try
            {
                // neu chua co du lieu extension , se chuyen xuong coi co quyen tren server hay khong
                if (dt == null || dt.Rows.Count == 0) return 0;
                // kiem tra control trong he thong phan quyen tren main server
                // dt co du lieu co nghia la da co thao tac phan quyen tren nay
                DataRow[] rows = dt.Select(String.Format(@"MenuID={0}", menuID));
                if (rows[0]["State"].ToString() == "0") return 2;
                return 1;

            }
            catch (Exception ex)
            {
                // TIm khong thay doi voi me nu nay nghia la menu moi cap phat tu tren server xuong
                return 0;

            }
        }
        private static bool CheckPermisionOfGroupFROMSERVER(int GroupID, int menuID)
        {
            try
            {
                // kiem tra control trong he thong phan quyen tren main server
                DataRow[] rows = Comon.Global.sys_PermissionTableMenu_Table.Select(String.Format(@"PermissionGroupID = {0} AND MenuID={1}", GroupID, menuID));
                return (rows.Length > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

         public async Task<IActionResult> OnPostExecuteData(string data, string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("YYY_sp_MenuGroupAllow_Execute", CommandType.StoredProcedure,
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
