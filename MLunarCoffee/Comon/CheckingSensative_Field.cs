using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MLunarCoffee.Comon
{
    public static class CheckingSensative_Field
    {
        /// <summary>
        /// 0:  tim thay cho update, 1 Khong Tim thay filed co trong phan quyen. Khong update field nay
        /// </summary>
        /// <returns></returns>

        public static int CheckSensetive_phone_customer( HttpContext httpContext )
        {
            try
            {
                string sensetive = "phone_customer";
                var user = Session.Session.GetUserSession (httpContext);
                DataTable dt = user.sys_PermissionTabControl;
                DataRow result = (from row in dt.AsEnumerable ()
                                  where row.Field<string> ("Name") == sensetive
                                  select row).SingleOrDefault ();
                if (result == null) return 1;
                else return 0;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
    }
}