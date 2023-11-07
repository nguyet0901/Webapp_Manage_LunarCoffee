using MLunarCoffee.Comon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MLunarCoffee.Models
{
    class ExecuteDataBaseRoot : IDisposable
    {

        private SqlConnection _conn;

        public SqlConnection Conn
        {
            get { return _conn; }
            set { _conn = value; }
        }
        public void Dispose()
        {

        }


        public ExecuteDataBaseRoot()
        {
            CreateConnect();
        }
        private void CreateConnect()
        {
            try
            {
                string connectionString = String.Format(@"Server={0}; " + "Initial Catalog={1}; User ID={2};Password={3};Trusted_Connection=false; "
                   , Global.ROOTIP
                   , Global.ROOTNAME
                   , Global.ROOTUSER
                   , Global.ROOTPASSWORD);

                _conn = new SqlConnection(connectionString);
                if (_conn.State == ConnectionState.Closed) _conn.Open();
            }
            catch (Exception ex)
            {
                // Connect Local
            }
        }
        public async Task<DataTable> ExecuteDataTable(string sql,
            CommandType commandType,
            params object[] pars)
        {
            try
            {
                if (_conn.State == ConnectionState.Closed) _conn.Open();
                SqlCommand com = new SqlCommand(sql, _conn);
                com.CommandType = commandType;
                com.CommandTimeout = 10000;
                for (int i = 0; i < pars.Length; i += 3)
                {
                    SqlParameter par = new SqlParameter(pars[i].ToString(), pars[i + 1]);
                    par.Value = pars[i + 2];
                    com.Parameters.Add(par);
                }

                SqlDataAdapter dad = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                await Task.Run(() => dad.Fill(dt));
                if (_conn.State == ConnectionState.Open) _conn.Close();
                return dt;
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
            SqlCommand com = new SqlCommand(sql, _conn);
            com.CommandType = commandType;
            com.CommandTimeout = 10000;
            for (int i = 0; i < pars.Length; i += 3)
            {
                SqlParameter par = new SqlParameter(pars[i].ToString(), pars[i + 1]);
                par.Value = pars[i + 2];
                com.Parameters.Add(par);
            }

            SqlDataAdapter dad = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            await Task.Run(() => dad.Fill(ds));
            if (_conn.State == ConnectionState.Open) _conn.Close();
            return ds;
        }
    }
}