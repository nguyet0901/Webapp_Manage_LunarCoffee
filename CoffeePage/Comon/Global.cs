
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CoffeePage.Comon.Crypt;
using CoffeePage.Models;

namespace CoffeePage.Comon
{
    public class Global
    {

        #region // Client
        public static string RootLink = "https://localhost:44303/";
        public static string PrivateKey = "mlunarcofeeeefocranulmeefocranul";

        #endregion

        //public static async Task InitSystem(IConfiguration _config)
        //{
        //    try
        //    {
        //        RootLink = "https://localhost:44303/";
        //    }
        //    catch
        //    {

        //    }

        //}

    }

}

