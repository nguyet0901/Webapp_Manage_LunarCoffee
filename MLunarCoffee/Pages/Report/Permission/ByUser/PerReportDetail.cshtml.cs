using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MLunarCoffee.Pages.Report.Permission.ByUser
{
    public class PerReportDetailModel : PageModel
    {
        private static Dictionary<int ,List<int>> dictExtItem;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoadata(string dataGroup ,string dataReport)
        {
            try
            {
                DataTable DataUserGroup = JsonConvert.DeserializeObject<DataTable>(dataGroup);
                DataTable DataReport = JsonConvert.DeserializeObject<DataTable>(dataReport);
                DataTable dt = new DataTable();
                dt = await LoadPerReportDetail(DataUserGroup ,DataReport);
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        private async Task<DataTable> LoadPerReportDetail(DataTable dtGroup ,DataTable allReport)
        {
            try
            {
                DataTable Extent = new DataTable();
                Extent = await LoadExtensionReport(0); // su dung group  id cua client de xem nhung quyen add them hoac xoa bo

                dictExtItem = await keyValuePairs(Extent);

                DataTable Report = new DataTable();
                Report.Columns.Add("ID" ,typeof(Int32));
                Report.Columns.Add("Name" ,typeof(String));
                Report.Columns.Add("NameLangKey" ,typeof(String));
                Report.Columns.Add("UserGroup" ,typeof(Int32));
                Report.Columns.Add("GroupID" ,typeof(Int32)); // Group tren server
                                                              //Report.Columns.Add("PermissionGroupID", typeof(Int32));
                Report.Columns.Add("GroupName" ,typeof(String)); // Group tren server
                Report.Columns.Add("GroupNameLangKey" ,typeof(String));
                Report.Columns.Add("isBlock" ,typeof(Int32));

                for (int j = 0; j < dtGroup.Rows.Count; j++)
                {
                    int groupID = Convert.ToInt32(dtGroup.Rows[j]["ID"]);

                    for (int i = 0; i < allReport.Rows.Count; i++)
                    {
                        var drAllControl = allReport.Rows[i];
                        int templateid = Convert.ToInt32(allReport.Rows[i]["ID"]);
                        int isBlock = CheckPermisionReport(dictExtItem ,groupID ,templateid);
                        DataRow dr = Report.NewRow();
                        Report.Rows.Add(AddDataReport(Report.NewRow() ,drAllControl ,isBlock ,groupID));
                    }
                }

                return Report;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataRow AddDataReport(DataRow dr ,DataRow drAllReport ,int isBlock ,int groupID)
        {
            dr["ID"] = Convert.ToInt32(drAllReport["ID"].ToString());
            dr["Name"] = drAllReport["Name"].ToString();
            dr["NameLangKey"] = drAllReport["NameLangKey"].ToString();
            dr["GroupID"] = Convert.ToInt32(drAllReport["GroupID"].ToString());
            dr["UserGroup"] = Convert.ToInt32(groupID);
            dr["GroupName"] = drAllReport["GroupName"].ToString();
            dr["GroupNameLangKey"] = drAllReport["GroupNameLangKey"].ToString();
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
                int template = Convert.ToInt32(dr["TeamplateID"]);

                if (!keyAll.ContainsKey(group))
                {
                    keyAll[group] = new List<int>
                    {
                        template
                    };
                }
                else
                {
                    if (!keyAll[group].Contains(template)) keyAll[group].Add(template);
                }
            }
            return keyAll;
        }


        private static async Task<DataTable> LoadExtensionReport(int GroupID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_rp_PerUser_GeExtentReport" ,CommandType.StoredProcedure ,
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

        private static int CheckPermisionReport(Dictionary<int ,List<int>> dict ,int groupid ,int templateid)
        {
            try
            {
                if (dict == null || dict.Count == 0) return 0;
                if (dict != null && dict.ContainsKey(groupid) && dict[groupid].Contains(templateid)) return 1;
                return 0;

            }
            catch (Exception ex)
            {
                // TIm khong thay doi voi me nu nay nghia la menu moi cap phat tu tren server xuong
                return 0;

            }
        }
    }
}
