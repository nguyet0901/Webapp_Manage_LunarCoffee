using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MLunarCoffee.Models
{
    class ExecuteDataBase : IDisposable
    {

        private SqlConnection _conn;

        public SqlConnection Conn
        {
            get { return _conn; }
            set { _conn = value; }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _conn.Dispose();
            }
        }


        //private bool _disposed = false;
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_disposed)//    {
        //        if (disposing)//        {

        //        }
        //        _conn.Close();
        //    }                                                                                                                                         //}
        ~ExecuteDataBase()
        {
            //Dispose(false);
            _conn.Dispose();
        }
        public ExecuteDataBase()
        {
            CreateConnect();
        }
        public ExecuteDataBase(string[] loc)
        {
            CreateConnect(loc);
        }
        private void CreateConnect()
        {
            try
            {

                string connectionString = String.Format(@"Server={0}; " + "Initial Catalog={1}; User ID={2};Password={3};Trusted_Connection=false; ", Comon.Global.sys_DB_IP
                    , Comon.Global.sys_DB_Name
                    , Comon.Global.sys_DB_User
                    , Comon.Global.sys_DB_Pass);
                _conn = new SqlConnection(connectionString);
                if (_conn.State == ConnectionState.Closed) _conn.Open();
            }
            catch (Exception ex)
            {
                // Connect Local
            }
        }

        private void CreateConnect(string[] loc)
        {
            try
            {
                string connectionString = String.Format(@"Server={0}; " + "Initial Catalog={1}; User ID={2};Password={3};Trusted_Connection=false; ", loc[0]
                    , loc[1]
                    , loc[2]
                    , loc[3]);
                _conn = new SqlConnection(connectionString);
                if (_conn.State == ConnectionState.Closed) _conn.Open();
            }
            catch (Exception ex)
            {
                // Connect Local
            }
        }



        public async Task<DataTable> LoadPara(string paraTypeName)
        {
            try
            {
                if (_conn.State == ConnectionState.Closed) _conn.Open();
                DataTable table = new DataTable("myTable");
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    table = await confunc.ExecuteDataTable("YYY_sp_LoadCombo_Para", CommandType.StoredProcedure,
                      "@paraTypeName", SqlDbType.NVarChar, paraTypeName.Trim());
                }
                if (_conn.State == ConnectionState.Open) _conn.Close();
                return table;
            }
            catch
            {
                return null;
            }
        }


        public async Task<DataTable> ExecuteDataTable(string sql, CommandType commandType, params object[] pars)
        {
            try
            {
                if (_conn.State == ConnectionState.Closed) _conn.Open();
                using (SqlCommand com = new SqlCommand(sql, _conn))
                {
                    com.CommandType = commandType;
                    com.CommandTimeout = 120;
                    for (int i = 0; i < pars.Length; i += 3)
                    {
                        SqlParameter par = new SqlParameter(pars[i].ToString(), pars[i + 1]);
                        par.Value = pars[i + 2];
                        com.Parameters.Add(par);
                    }
                    using (SqlDataAdapter dad = new SqlDataAdapter(com))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            await Task.Run(() => dad.Fill(dt));
                            if (_conn.State == ConnectionState.Open)
                            {
                                _conn.Close();
                                _conn.Dispose();
                            }
                            return dt;
                        }
                        //DataTable dt = new DataTable();

                    }
                    //SqlDataAdapter dad = new SqlDataAdapter(com);

                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<DataSet> ExecuteDataSet(string sql,
           CommandType commandType,
           params object[] pars)
        {
            if (_conn.State == ConnectionState.Closed) _conn.Open();

            using (SqlCommand com = new SqlCommand(sql, _conn))
            {
                com.CommandType = commandType;
                com.CommandTimeout = 120;
                for (int i = 0; i < pars.Length; i += 3)
                {
                    SqlParameter par = new SqlParameter(pars[i].ToString(), pars[i + 1]);
                    par.Value = pars[i + 2];
                    com.Parameters.Add(par);
                }
                using (SqlDataAdapter dad = new SqlDataAdapter(com))
                {
                    using (DataSet ds = new DataSet())
                    {
                        await Task.Run(() => dad.Fill(ds));
                        if (_conn.State == ConnectionState.Open)
                        {
                            _conn.Close();
                            _conn.Dispose();
                        }
                        return ds;
                    }
                    //DataSet ds = new DataSet();

                }


            }



        }

        public async Task<DataTable> LoadEmployee(int GroupID, int Branch_ID, int isAllBranch)
        {
            try
            {
                //if (_conn.State == ConnectionState.Closed) _conn.Open();
                DataTable table = new DataTable("myTable");
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    table = await confunc.ExecuteDataTable("YYY_sp_LoadCombo_Employee", CommandType.StoredProcedure,
                      "@GroupID", SqlDbType.Int, GroupID,
                      "@Branch_ID", SqlDbType.Int, Branch_ID,
                      "@isAllBranch", SqlDbType.Int, isAllBranch
                      );
                }
                //if (_conn.State == ConnectionState.Open)
                //{
                //    _conn.Close();
                //    _conn.Dispose();
                //}
                return table;
            }
            catch
            {
                return null;
            }
        }
        public async Task<DataTable> LoadEmployeeFull(int Branch_ID, int isAllBranch)
        {
            try
            {
                if (_conn.State == ConnectionState.Closed) _conn.Open();
                DataTable table = new DataTable("myTable");
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    table = await confunc.ExecuteDataTable("YYY_sp_LoadCombo_EmployeeFull", CommandType.StoredProcedure,
                      "@Branch_ID", SqlDbType.Int, Branch_ID,
                      "@isAllBranch", SqlDbType.Int, isAllBranch
                      );
                }
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                    _conn.Dispose();
                }
                return table;
            }
            catch
            {
                return null;
            }
        }

    }
}
