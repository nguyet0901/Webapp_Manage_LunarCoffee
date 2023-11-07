using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.Permission.ByUser
{
    public class PerMenuDetailModel : PageModel
    {
        private static Dictionary<int ,Dictionary<int ,int>> dictExtent;
        private static Dictionary<int ,List<int>> dictGroupServer;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadata(string data)
        {
            try
            {
                DataTable DataUserGroup = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                dt = await LoadPerMenuDetail(DataUserGroup);
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        private async Task<DataTable> LoadPerMenuDetail(DataTable dtGroup)
        {
            try
            {
                DataTable Extent = new DataTable();
                Extent = await LoadExtensionMenu(0); // su dung group  id cua client de xem nhung quyen add them hoac xoa bo

                await keyValuePairs(Extent ,Comon.Global.sys_PermissionTableMenu_Table);

                DataTable Menu = new DataTable();
                Menu.Columns.Add("ID" ,typeof(Int32));
                Menu.Columns.Add("Name" ,typeof(String));
                Menu.Columns.Add("NameLangKey" ,typeof(String));
                Menu.Columns.Add("UserGroup" ,typeof(Int32));
                Menu.Columns.Add("GroupID" ,typeof(Int32)); // Group tren server
                                                            //Menu.Columns.Add("PermissionGroupID", typeof(Int32));
                Menu.Columns.Add("GroupName" ,typeof(String)); // Group tren server
                Menu.Columns.Add("GroupNameLangKey" ,typeof(String));
                Menu.Columns.Add("isBlock" ,typeof(Int32));

                for (int j = 0; j < dtGroup.Rows.Count; j++)
                {
                    int groupID = Convert.ToInt32(dtGroup.Rows[j]["ID"]);
                    int groupServerID = Convert.ToInt32(dtGroup.Rows[j]["InheritanceGroup"]);

                    var allMenu = Comon.Global.sys_PermissionAllMenu;
                    for (int i = 0; i < allMenu.Rows.Count; i++)
                    {
                        int menuID = Convert.ToInt32(allMenu.Rows[i]["ID"]);
                        var dictExtItem = dictExtent.ContainsKey(groupID) ? dictExtent[groupID] : null;

                        int checkClient = CheckPermisionMenu(dictExtItem ,menuID);
                        int isBlock = 0;

                        if (checkClient == 1)
                        {
                            isBlock = 0;
                        }
                        else if (checkClient == 2)
                        {
                            isBlock = 1;
                        }

                        else
                        {
                            if (CheckPermisionOfGroupFROMSERVER(dictGroupServer ,groupServerID ,menuID))
                            {
                                isBlock = 0;
                            }
                            else
                            {
                                isBlock = 1;
                            }

                        }
                        DataRow dr = Menu.NewRow();
                        Menu.Rows.Add(AddDataMenu(Menu.NewRow() ,Comon.Global.sys_PermissionAllMenu.Rows[i] ,isBlock ,groupID));
                    }
                }

                return Menu;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataRow AddDataMenu(DataRow dr ,DataRow drAllMenu ,int isBlock ,int groupID)
        {
            dr["ID"] = Convert.ToInt32(drAllMenu["ID"].ToString());
            dr["Name"] = drAllMenu["Name"].ToString();
            dr["NameLangKey"] = drAllMenu["NameLangKey"].ToString();
            dr["GroupID"] = Convert.ToInt32(drAllMenu["GroupID"].ToString());
            dr["UserGroup"] = Convert.ToInt32(groupID);
            dr["GroupName"] = drAllMenu["GroupName"].ToString();
            dr["GroupNameLangKey"] = drAllMenu["GroupNameLangKey"].ToString();
            dr["isBlock"] = isBlock;
            return dr;
        }

        private static async Task keyValuePairs(DataTable dtExtent ,DataTable dtServer)
        {
            try
            {
                var tasks = new List<Task<int>>();
                DataSet ds = new DataSet();
                tasks.Add(Task.Run(async () =>
                {

                    dictExtent = await keyValuePairsExtent(dtExtent);
                    return 1;

                }));

                tasks.Add(Task.Run(async () =>
                {
                    dictGroupServer = keyValuePairsServer(dtServer);
                    return 1;
                }));

                var result = await Task.WhenAll(tasks.ToList());
            }
            catch (Exception ex)
            {

            }
        }

        private static async Task<Dictionary<int ,Dictionary<int ,int>>> keyValuePairsExtent(DataTable dt)
        {
            Dictionary<int ,Dictionary<int ,int>> keyAll = new Dictionary<int ,Dictionary<int ,int>>();
            int currGroup = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                int group = Convert.ToInt32(dr["GroupID"]);
                int menu = Convert.ToInt32(dr["MenuID"]);
                int state = Convert.ToInt32(dr["State"]);

                if (group != 0 && menu != 0)
                {
                    if (currGroup != group && !keyAll.ContainsKey(group))
                    {
                        var detail = new Dictionary<int ,int>()
                        {
                            [menu] = state
                        };
                        keyAll.Add(group ,detail);
                        currGroup = group;
                    }
                    else
                    {
                        if (!keyAll[group].ContainsKey(menu))
                            keyAll[group].Add(menu ,state);
                    }
                }



            }
            return keyAll;
        }
        private static Dictionary<int ,List<int>> keyValuePairsServer(DataTable dt)
        {
            Dictionary<int ,List<int>> keyAll = new Dictionary<int ,List<int>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                int menu = Convert.ToInt32(dr["MenuID"]);
                int pergroup = Convert.ToInt32(dr["PermissionGroupID"]);

                if (!keyAll.ContainsKey(menu))
                {
                    keyAll[menu] = new List<int>
                    {
                        pergroup
                    };
                }
                else
                {
                    if (!keyAll[menu].Contains(pergroup)) keyAll[menu].Add(pergroup);
                }

            }
            return keyAll;
        }

        private static async Task<DataTable> LoadExtensionMenu(int GroupID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("YYY_sp_Per_GeExtentMenu" ,CommandType.StoredProcedure ,
                          "@GroupID" ,SqlDbType.Int ,GroupID
                      );

                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private static int CheckPermisionMenu(Dictionary<int ,int> dict ,int menuID)
        {
            try
            {
                // neu chua co du lieu extension , se chuyen xuong coi co quyen tren server hay khong
                if (dictExtent == null || dictExtent.Count == 0) return 0;
                // kiem tra control trong he thong phan quyen tren main server
                // dt co du lieu co nghia la da co thao tac phan quyen tren nay
                if (dict != null && dict.ContainsKey(menuID) && dict[menuID] == 0) return 2;
                return 1;

            }
            catch (Exception ex)
            {
                // TIm khong thay doi voi me nu nay nghia la menu moi cap phat tu tren server xuong
                return 0;

            }
        }
        private static bool CheckPermisionOfGroupFROMSERVER(Dictionary<int ,List<int>> dict ,int GroupID ,int menuID)
        {
            try
            {
                // kiem tra control trong he thong phan quyen tren main server

                //dict[GroupID]
                if (dict.ContainsKey(menuID) && dict[menuID].Contains(GroupID)) return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
