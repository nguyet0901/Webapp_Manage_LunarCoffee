
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MLunarCoffee.Comon.Crypt;
using MLunarCoffee.Models;

namespace MLunarCoffee.Comon
{

    public class GlobalUser
    {

        #region // General
        public int sys_userid { get; set; }
        public string sys_username { get; set; }
        public string sys_BranchName { get; set; }
        public int sys_branchID { get; set; }
        public int sys_AllBranchID { get; set; }
    }
    #endregion
    public class Global
    {

        #region // Client
        public static string SQLIP;
        public static string SQLNAME;
        public static string SQLUSER;
        public static string SQLPASSWORD;
        public static string ROOTCODE = "mlunarcofeeeefocranulmeefocranul";

        #endregion

        public static async Task System_Start(IConfiguration _config)
        {
            try
            {
                SQLNAME = Encrypt.DecryptString(_config.GetValue<string>("CLIENT:SQLNAME").ToString() ,ROOTCODE);
                SQLUSER = Encrypt.DecryptString(_config.GetValue<string>("CLIENT:SQLUSER").ToString() ,ROOTCODE);
                SQLPASSWORD = Encrypt.DecryptString(_config.GetValue<string>("CLIENT:SQLPASSWORD").ToString() ,ROOTCODE);


                //var task_startclient = client.StartClient(keycode);


                //await Task.WhenAll(task_startclient ,task_api
                //    ,task_callcenter);

            }
            catch
            {

            }

        }

    }

}

