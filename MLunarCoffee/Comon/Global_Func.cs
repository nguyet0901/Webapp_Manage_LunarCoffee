using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Comon
{
    public class Global_Func
    {
        // Get client permission
        public async Task<DataSet> Permission(string db)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                ds = await confunc.ExecuteDataSet("[MLU_sp_Initialize]", CommandType.StoredProcedure, "@db", SqlDbType.NVarChar, db);
            }
            return ds;
        }

        // Get client api
        public async Task<DataTable> Api(string db)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_DB_GetAPI", CommandType.StoredProcedure, "@db", SqlDbType.NVarChar, db);
            }
            return dt;
        }
        // Get client call center
        public async Task<DataTable> CallCenter(string db)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_DB_DetectCallCenter", CommandType.StoredProcedure, "@db", SqlDbType.NVarChar, db);
            }
            return dt;
        }

        // Get client document
        public async Task<DataTable> Document(string db)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Document_GetAll]", CommandType.StoredProcedure, "@db", SqlDbType.NVarChar, db);
            }
            return dt;
        }

        // Get user popup
        public async Task<DataSet> UserPopup(string db)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[MLU_sp_Popup_GetPopup]", CommandType.StoredProcedure, "@db", SqlDbType.NVarChar, db);
                dt.TableName = "UserPopup";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[MLU_sp_AuthorizeFunction]", CommandType.StoredProcedure);
                dt.TableName = "Authorize";
                ds.Tables.Add(dt);
            }
            return ds;
        }

        // Get list setting
        public async Task<DataTable> ListSetting()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Per_GetList]", CommandType.StoredProcedure);
            }
            return dt;
        }


        // Get Keyword not-valid in brand sms
        public async Task<DataSet> KeyBanSms()
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[MLU_sp_SMS_GetKeyWord]", CommandType.StoredProcedure);
                dt.TableName = "KeywordRoot";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[MLU_sp_SMS_GetKeyWord]" , CommandType.StoredProcedure);
                dt.TableName = "Keyword";
                ds.Tables.Add(dt);
            }
            return ds;
        }
        // Get ZNS
        public async Task<DataTable> GetZNS(int ClientID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_DB_GetZNS"
                    , CommandType.StoredProcedure , "@ClientID", SqlDbType.NVarChar, ClientID);
            }
            return dt;
        }

        // Get Realtime noti flag
        public async Task<DataTable> RealTimeNoti(string db)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_DB_Detect_Realtime_Noti", CommandType.StoredProcedure, "@db", SqlDbType.NVarChar, db);
            }
            return dt;
        }

        // Get client option
        public async Task<DataSet> Option(string db)
        {
            DataSet ds = new DataSet();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("MLU_sp_DB_DetectOption", CommandType.StoredProcedure, "@db", SqlDbType.NVarChar, db);
                dt.TableName = "DB-Option";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("MLU_sp_DetectOption", CommandType.StoredProcedure);
                dt.TableName = "Option";
                ds.Tables.Add(dt);
            }
            return ds;
        }

        // Get client option expand
        //public async Task<DataTable> OptionExpand(string db)
        //{
        //    DataTable dt = new DataTable();
        //    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //    {
        //        dt = await confunc.ExecuteDataTable("MLU_sp_DB_Detect_OptionExtension", CommandType.StoredProcedure, "@db", SqlDbType.NVarChar, db);
        //    }
        //    return dt;
        //}



        // Get Require Validation
        public async Task<DataTable> RequireValidate()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Require_Validation_Get]", CommandType.StoredProcedure);
            }
            return dt;
        }

        // Get list control block
        public async Task<DataTable> ListBlockControl(int ClientID)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Per_GetTabControl]", CommandType.StoredProcedure
                        , "@ClientID", SqlDbType.NVarChar, ClientID);
            }
            return dt;
        }

        // Get client initialize data from start time
        public async Task<DataTable> StartClient()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("MLU_sp_DB_AuthorizeDBName", CommandType.StoredProcedure);
            }
            return dt;
        }


        // Get initialize info
        public async Task<DataTable> InitializeInfo()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Option_Get]", CommandType.StoredProcedure);
            }
            return dt;
        }
         public async Task<DataTable> FolderSystem()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Option_GetFolder]", CommandType.StoredProcedure);
            }
            return dt;
        }
        public async Task<DataTable> Getwebhook()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_GetGlobal_Webhook]", CommandType.StoredProcedure);
            }
            return dt;
        }
    }
}
