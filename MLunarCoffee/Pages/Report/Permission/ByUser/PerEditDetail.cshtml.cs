using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.Permission.ByUser
{
    public class PerEditDetailModel : PageModel
    {
        private static Dictionary<int ,List<int>> dictExtItem;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadata(string dataGroup ,string dataEdit)
        {
            try
            {
                DataTable DataUserGroup = JsonConvert.DeserializeObject<DataTable>(dataGroup);
                DataTable DataEdit = JsonConvert.DeserializeObject<DataTable>(dataEdit);
                DataTable dt = new DataTable();
                dt = await LoadPerEditDetail(DataUserGroup ,DataEdit);
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        private async Task<DataTable> LoadPerEditDetail(DataTable dtGroup ,DataTable allEdit)
        {
            try
            {
                DataTable Extent = new DataTable();
                Extent = await LoadExtensionEdit(0); // su dung group  id cua client de xem nhung quyen add them hoac xoa bo

                dictExtItem = await keyValuePairs(Extent);

                DataTable Edit = new DataTable();
                Edit.Columns.Add("ID" ,typeof(Int32));
                Edit.Columns.Add("Name" ,typeof(String));
                Edit.Columns.Add("NameLangKey" ,typeof(String));
                Edit.Columns.Add("UserGroup" ,typeof(Int32));
                Edit.Columns.Add("GroupID" ,typeof(Int32)); // Group tren server
                                                            //Edit.Columns.Add("PermissionGroupID", typeof(Int32));
                Edit.Columns.Add("GroupName" ,typeof(String)); // Group tren server
                Edit.Columns.Add("GroupNameLangKey" ,typeof(String));
                Edit.Columns.Add("isBlock" ,typeof(Int32));

                for (int j = 0; j < dtGroup.Rows.Count; j++)
                {
                    int groupID = Convert.ToInt32(dtGroup.Rows[j]["ID"]);

                    for (int i = 0; i < allEdit.Rows.Count; i++)
                    {
                        var drAllControl = allEdit.Rows[i];
                        int EditRoleID = Convert.ToInt32(allEdit.Rows[i]["ID"]);
                        int isBlock = CheckPermisionEdit(dictExtItem ,groupID ,EditRoleID);
                        DataRow dr = Edit.NewRow();
                        Edit.Rows.Add(AddDataEdit(Edit.NewRow() ,drAllControl ,isBlock ,groupID));
                    }
                }

                return Edit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataRow AddDataEdit(DataRow dr ,DataRow drAllEdit ,int isBlock ,int groupID)
        {
            dr["ID"] = Convert.ToInt32(drAllEdit["ID"].ToString());
            dr["Name"] = drAllEdit["Name"].ToString();
            dr["NameLangKey"] = drAllEdit["NameLangKey"].ToString();
            dr["GroupID"] = Convert.ToInt32(drAllEdit["GroupID"].ToString());
            dr["UserGroup"] = Convert.ToInt32(groupID);
            dr["GroupName"] = drAllEdit["GroupName"].ToString();
            dr["GroupNameLangKey"] = drAllEdit["GroupNameLangKey"].ToString();
            dr["isBlock"] = isBlock;
            return dr;
        }

        private static async Task<Dictionary<int ,List<int>>> keyValuePairs(DataTable dt)
        {
            Dictionary<int ,List<int>> keyAll = new Dictionary<int ,List<int>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                int group = Convert.ToInt32(dr["GroupID"]);
                int EditRoleID = Convert.ToInt32(dr["EditRoleID"]);

                if (!keyAll.ContainsKey(group))
                {
                    keyAll[group] = new List<int>
                    {
                        EditRoleID
                    };
                }
                else
                {
                    if (!keyAll[group].Contains(EditRoleID)) keyAll[group].Add(EditRoleID);
                }
            }
            return keyAll;
        }


        private static async Task<DataTable> LoadExtensionEdit(int GroupID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_rp_PerUser_GeExtentDetail" ,CommandType.StoredProcedure ,
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

        private static int CheckPermisionEdit(Dictionary<int ,List<int>> dict ,int groupid ,int EditRoleID)
        {
            try
            {
                if (dict == null || dict.Count == 0) return 0;
                if (dict != null && dict.ContainsKey(groupid) && dict[groupid].Contains(EditRoleID)) return 0;
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
