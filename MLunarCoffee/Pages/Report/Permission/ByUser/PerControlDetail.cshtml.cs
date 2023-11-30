using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.Permission.ByUser
{
    public class PerControlDetailModel : PageModel
    {
        private static Dictionary<int ,Dictionary<int ,int>> dictExtent;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadata(string data)
        {
            try
            {
                DataTable DataUserGroup = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                dt = await LoadPerControlDetail(DataUserGroup);
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        private async Task<DataTable> LoadPerControlDetail(DataTable dtGroup)
        {
            try
            {
                DataTable Extent = new DataTable();
                Extent = await LoadExtensionControl(0); // su dung group  id cua client de xem nhung quyen add them hoac xoa bo

                dictExtent = await keyValuePairs(Extent);

                DataTable Control = new DataTable();
                Control.Columns.Add("ID" ,typeof(Int32));
                Control.Columns.Add("Name" ,typeof(String));
                Control.Columns.Add("NameLangKey" ,typeof(String));
                Control.Columns.Add("UserGroup" ,typeof(Int32));
                Control.Columns.Add("GroupID" ,typeof(Int32)); // Group tren server
                                                               //Control.Columns.Add("PermissionGroupID", typeof(Int32));
                Control.Columns.Add("GroupName" ,typeof(String)); // Group tren server
                Control.Columns.Add("GroupNameLangKey" ,typeof(String));
                Control.Columns.Add("isBlock" ,typeof(Int32));

                for (int j = 0; j < dtGroup.Rows.Count; j++)
                {
                    int groupID = Convert.ToInt32(dtGroup.Rows[j]["ID"]);
                    var allControl = Comon.Global.sys_List_TabControl_Allowed;
                    for (int i = 0; i < allControl.Rows.Count; i++)
                    {
                        var drAllControl = allControl.Rows[i];
                        if (drAllControl["IsHaving"].ToString() == "1")
                        {
                            int ControlTabID = Convert.ToInt32(drAllControl["ID"]);
                            var dictExtItem = dictExtent.ContainsKey(groupID) ? dictExtent[groupID] : null;
                            int checkClient = CheckPermisionControl(dictExtItem ,ControlTabID);
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
                                isBlock = 0;
                            }

                            DataRow dr = Control.NewRow();
                            Control.Rows.Add(AddDataControl(Control.NewRow() ,drAllControl ,isBlock ,groupID));
                        }

                    }
                }

                return Control;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataRow AddDataControl(DataRow dr ,DataRow drAllControl ,int isBlock ,int groupID)
        {
            if (drAllControl["IsHaving"].ToString() == "1")
            {
                dr["ID"] = Convert.ToInt32(drAllControl["ID"].ToString());
                dr["Name"] = drAllControl["Description"].ToString();
                dr["NameLangKey"] = drAllControl["DescriptionLangKey"].ToString();
                dr["GroupID"] = Convert.ToInt32(drAllControl["GroupID"].ToString());
                dr["UserGroup"] = Convert.ToInt32(groupID);
                dr["GroupName"] = drAllControl["GroupName"].ToString();
                dr["GroupNameLangKey"] = drAllControl["GroupNameLangKey"].ToString();
                dr["isBlock"] = isBlock;
            }
            return dr;
        }

        private static async Task<Dictionary<int ,Dictionary<int ,int>>> keyValuePairs(DataTable dt)
        {
            Dictionary<int ,Dictionary<int ,int>> keyAll = new Dictionary<int ,Dictionary<int ,int>>();
            int currGroup = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                int group = Convert.ToInt32(dr["GroupID"]);
                int control = Convert.ToInt32(dr["ControlTabID"]);
                int state = Convert.ToInt32(dr["State"]);

                if (group != 0 && control != 0)
                {
                    if (currGroup != group && !keyAll.ContainsKey(group))
                    {
                        var detail = new Dictionary<int ,int>()
                        {
                            [control] = state
                        };
                        keyAll.Add(group ,detail);
                        currGroup = group;
                    }
                    else
                    {
                        if (!keyAll[group].ContainsKey(control))
                            keyAll[group].Add(control ,state);
                    }
                }
            }
            return keyAll;
        }

        private static async Task<DataTable> LoadExtensionControl(int GroupID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_Per_GeExtentControlTab" ,CommandType.StoredProcedure ,
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

        private static int CheckPermisionControl(Dictionary<int ,int> dict ,int ControlTabID)
        {
            try
            {
                // neu chua co du lieu extension , se chuyen xuong coi co quyen tren server hay khong
                if (dictExtent == null || dictExtent.Count == 0) return 0;
                // kiem tra control trong he thong phan quyen tren main server
                // dt co du lieu co nghia la da co thao tac phan quyen tren nay
                if (dict != null && dict.ContainsKey(ControlTabID) && dict[ControlTabID] == 0) return 2;
                return 1;

            }
            catch (Exception ex)
            {
                // TIm khong thay doi voi me nu nay nghia la menu moi cap phat tu tren server xuong
                return 0;

            }
        }
    }
}
