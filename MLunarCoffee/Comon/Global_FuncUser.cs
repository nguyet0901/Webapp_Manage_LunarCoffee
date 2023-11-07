using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MLunarCoffee.Models.API.APIClient;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MLunarCoffee.Comon
{
    public class Global_FuncUser
    {
        // Get user information
        public async Task<DataSet> UserInfo(int usserid)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[YYY_sp_GetGlobal_User]"
                    , CommandType.StoredProcedure, "@UserID", SqlDbType.Int, usserid);
            }
            return ds;
        }

        // Get user call
        public async Task<DataTable> UserCall(int usserid)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_GetGlobal_CallExtension]"
                    , CommandType.StoredProcedure, "@UserID", SqlDbType.Int, usserid);
            }
            return dt;
        }
        // Get webhook
        public async Task<DataTable> Webhook()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Option_GetWebhook]", CommandType.StoredProcedure);
            }
            return dt;
        }
        // Get user menu (based on role's user)
        public async Task<DataTable> UserMenu(int roleid)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Permission_LoadMenuGroup]", CommandType.StoredProcedure,
                   "@GroupID", SqlDbType.Int, Convert.ToInt32(roleid));
            }
            return dt;
        }

        // Get tabcontrol of user in current db .Based on role's user
        public async Task<DataTable> UserTabControl(int roleid)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Per_GeExtentControlTab]", CommandType.StoredProcedure,
                    "@GroupID", SqlDbType.Int, Convert.ToInt32(roleid));
            }
            return dt;
        }

        //Get dashboard is exist or not .Based on role's user
        public async Task<DataTable> UserDashboard(int roleid)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_GetHas_Dashboard]", CommandType.StoredProcedure
                    , "@GroupUserID", SqlDbType.Int, Convert.ToInt32(roleid));
            }
            return dt;
        }

        // Get Email
        public async Task<DataTable> Email()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Option_Mail_Get]", CommandType.StoredProcedure);
            }
            return dt;
        }


        // Get user permission edit role
        public async Task<DataTable> UserPerEditRole(int usserid)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_GetPermissionEditRole]", CommandType.StoredProcedure, "@UserID"
                    , SqlDbType.Int, usserid);
            }
            return dt;
        }



        // Lock User
        public async Task UserLock(int id)
        {
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                await confunc.ExecuteDataTable("[YYY_sp_LockUserExpire]", CommandType.StoredProcedure, "@UserID", SqlDbType.Int, id);
            }
        }

        //Check Using Confirm Service
        public int User_ConfirmServiceCheck(DataTable AllowedClient, DataTable ControlClient)
        {
            try
            {
                int result = 0;
                var JoinResult = (from p in AllowedClient.AsEnumerable()
                                  join t in ControlClient.AsEnumerable()
                                  on p.Field<int>("ID") equals t.Field<int>("ControlTabID") into tempJoin
                                  from leftJoin in tempJoin.DefaultIfEmpty()
                                  where p.Field<int>("IsHaving") == 1 && p.Field<string>("Name") == "confirm_using_service"
                                  select new
                                  {
                                      State = leftJoin == null ? 1 : leftJoin.Field<int>("State")// null thi gan la 1
                                  }).ToList();




                var FinalResult = JoinResult.Where(m => m.State == 1).ToList();
                if (FinalResult.Count == 1) result = 1;
                else result = 0;
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Get control by tabControl base on group
        public DataTable User_GetControl_TabControl(DataTable AllowedClient, DataTable ControlClient)
        {
            try
            {
                var JoinResult = (from p in AllowedClient.AsEnumerable()
                                  join t in ControlClient.AsEnumerable()
                                  on p.Field<int>("ID") equals t.Field<int>("ControlTabID") into tempJoin
                                  from leftJoin in tempJoin.DefaultIfEmpty()
                                  select new
                                  {
                                      Name = p.Field<string>("Name"),
                                      IsGrid = p.Field<int>("IsGrid"),
                                      IsHaving = p.Field<int>("IsHaving"),
                                      State = leftJoin == null ? 1 : leftJoin.Field<int>("State")// null thi gan la 1
                                  }).ToList();
                //Cho phep ishaving =0 ( khong phan quyen)
                //ishaving =1 : cho phep state=1
                var result = JoinResult.Where(m => m.IsHaving == 0 || m.State == 1)
                               .Select(m => new
                               {
                                   Name = m.Name,
                                   IsGrid = m.IsGrid,
                               }).ToList();

                DataTable dtResulf1 = new DataTable();
                dtResulf1.Columns.Add("Name", typeof(string));
                dtResulf1.Columns.Add("IsGrid", typeof(string));
                foreach (var rowObj in result)
                {
                    DataRow dr = dtResulf1.NewRow();
                    dr["Name"] = rowObj.Name.ToString();
                    dr["IsGrid"] = rowObj.IsGrid.ToString();
                    dtResulf1.Rows.Add(dr);
                }
                return dtResulf1;


            }
            catch (Exception)
            {
                return null;
            }
        }

        //Get control  base on group
        public DataTable User_GetControl(DataTable dt, string inheritanceGroupID)
        {
            try
            {
                DataRow[] dr = dt.Select("PermissionGroupID=" + inheritanceGroupID);
                return dr.CopyToDataTable();
            }
            catch (Exception)
            {
                return null;
            }
        }


        #region // GetMenu base on group
        public async Task<DataTable> User_GetMenu(DataTable dtServerGroup, DataTable dtServer_ByGroup, string inheritanceGroupID, string GroupID)
        {
            try
            {
                DataTable dtExt = await User_GetMenuFromDB(GroupID);
                DataTable dtServerGroupCopy = dtServerGroup.Copy();

                if (dtExt != null && dtExt.Rows.Count != 0)
                {

                    foreach (DataRow orow in dtServerGroupCopy.Select())
                    {
                        string menuid = orow["MenuID"].ToString();
                        int checkClient = User_FindMenuExtension(dtExt, menuid);
                        if (checkClient == 1)
                        {
                            // Cho phep
                        }
                        else if (checkClient == 2)
                        {
                            // Khong cho phep
                            dtServerGroupCopy.Rows.Remove(orow);
                        }

                        else
                        {
                            // Check Server
                            if (User_CheckMenuServer(dtServer_ByGroup, inheritanceGroupID, menuid))
                            {
                                // Cho phep
                            }
                            else
                            {
                                dtServerGroupCopy.Rows.Remove(orow);
                            }

                        }
                    }
                    dtServerGroupCopy.AcceptChanges();
                    return dtServerGroupCopy;
                }
                else
                {
                    DataRow[] dr = dtServer_ByGroup.Select("PermissionGroupID=" + inheritanceGroupID);
                    return dr.CopyToDataTable();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// 0: Check tren server
        /// 1: Cho phep xem
        /// 2: Tu Choi thang
        private int User_FindMenuExtension(DataTable dt, string menuID)
        {
            try
            {
                DataRow[] dr = dt.Select("MenuID=" + menuID);
                if (dr[0]["State"].ToString() == "0") return 2;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private bool User_CheckMenuServer(DataTable dtServer, string GroupID, string menuID)
        {
            try
            {
                // kiem tra control trong he thong phan quyen tren main server
                DataRow[] rows = dtServer.Select(String.Format(@"PermissionGroupID = {0} AND MenuID={1}", GroupID, menuID));
                return (rows.Length > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task<DataTable> User_GetMenuFromDB(string GroupID)
        {
            try
            {
                DataTable dtExt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dtExt = await connFunc.ExecuteDataTable("YYY_sp_Per_GeExtentMenu", CommandType.StoredProcedure,
                          "@GroupID", SqlDbType.Int, GroupID
                      );

                }
                DataRow[] dr = dtExt.Select("GroupID=" + GroupID);
                return dr.CopyToDataTable();
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        #region // Register API SMS NOTI

        public async Task<JToken> GetAuthorMulti(string baseurl, string username, string password)
        {
            try
            {
                var r = await PostAuthorAPI(baseurl, "/api/author/Authomulti", username, password);
                var db = JsonConvert.DeserializeObject<JToken>(r);

                return db;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        private static async Task<string> PostAuthorAPI(string baseurl,string url,string username,string password)
        {
            using(HttpClient client = new HttpClient() { BaseAddress=new Uri(baseurl) })
            {
                APIAuthen sms = new APIAuthen() { Name=username,Password=password,Type="MLunarCoffee" };
                var content = new StringContent(JsonConvert.SerializeObject(sms),Encoding.UTF8,"application/json");
                var result = await client.PostAsync(url,content);
                string resultContent = await result.Content.ReadAsStringAsync();
                return (resultContent);
            }
        }
        #endregion

        // Get initialize info
        //public async Task<DataTable> GetOption()
        //{
        //    DataTable dt = new DataTable();
        //    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //    {
        //        dt = await confunc.ExecuteDataTable("[YYY_sp_Option_Get]", CommandType.StoredProcedure);
        //    }
        //    return dt;
        //}

        #region // Option Extension
        public async Task<DataTable> GetOptionExtension()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Option_GetExtension]", CommandType.StoredProcedure);
            }
            return dt;
        }

        public OptionExtension User_OptionExtension(DataTable dtExtension)
        {
            try
            {
               
                OptionExtension optionExtension = new OptionExtension();
                optionExtension.Before_16 = new OptionValue();
                optionExtension.PreHistory = new OptionValue();
                optionExtension.TimeInvoice = new OptionValue();
                optionExtension.CustomerGroup = new OptionValue();
                optionExtension.ValidProfile = new OptionValue();
                optionExtension.InputTeethChart = new OptionValue();

                if (dtExtension != null && dtExtension.Rows.Count != 0)
                {
                    for (int i = 0; i < dtExtension.Rows.Count; i++)
                    {
                        DataRow drOption = dtExtension.Rows[i];
                        OptionValue optionValue = new OptionValue()
                        {
                            Value = Convert.ToInt32(drOption["Value"].ToString()) ,
                            TypeRule = Convert.ToInt32(drOption["TypeRule"].ToString()) ,
                        };

                        switch (drOption["Name"].ToString())
                        {
                            case "Rule_Before_16":
                                optionExtension.Before_16 = optionValue;
                                break;
                            case "Rule_PreHistory_FirstTime":
                                optionExtension.PreHistory = optionValue;
                                break;
                            case "Rule_Time_Input_Invoice":
                                optionExtension.TimeInvoice = optionValue;
                                break;
                            case "Rule_Customer_Group":
                                optionExtension.CustomerGroup = optionValue;
                                break;
                            case "Rule_ValidProfile":
                                optionExtension.ValidProfile = optionValue;
                                break;
                            case "Rule_InputTeethChart":
                                optionExtension.InputTeethChart = optionValue;
                                break;
                        }
                    }
                }
                return optionExtension;
            }
            catch
            {
                return new OptionExtension();
            }
        }
        #endregion
    }
}

#region
//DataTable dtResulf = new DataTable ();
//dtResulf.Columns.Add ("Name", typeof (string));
//dtResulf.Columns.Add ("IsGrid", typeof (string));
//for (int i = 0; i < AllowedClient.Rows.Count; i++)
//{
//    if (AllowedClient.Rows[i]["IsHaving"].ToString() == "1")
//    {
//        int checkClient = FindControlInExtension(ControlClient, AllowedClient.Rows[i]["ID"].ToString());
//        if (checkClient == 1 || checkClient == 0)
//        {
//            // cho phep
//            DataRow dr = dtResulf.NewRow();
//            dr["Name"] = AllowedClient.Rows[i]["Name"].ToString();
//            dr["IsGrid"] = AllowedClient.Rows[i]["IsGrid"].ToString();
//            dtResulf.Rows.Add(dr);
//        }
//    }
//    else
//    {
//        // cho phep
//        DataRow dr = dtResulf.NewRow();
//        dr["Name"] = AllowedClient.Rows[i]["Name"].ToString();
//        dr["IsGrid"] = AllowedClient.Rows[i]["IsGrid"].ToString();
//        dtResulf.Rows.Add(dr);
//    }
//}
//return dtResulf;
#endregion

#region
//private int FindControlInExtension(DataTable dt,string ControlTabID) {
//   try {
//      DataRow[] dr = dt.Select("ControlTabID=" + ControlTabID);
//      if(dr[0]["State"].ToString() == "0") return 2;
//      return 1;
//   }
//   catch(Exception) {
//      return 0;
//   }
//}
#endregion
