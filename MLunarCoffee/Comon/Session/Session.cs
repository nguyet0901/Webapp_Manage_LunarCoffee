using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Comon.Session
{
    public class Session
    {

        public static void SetSession(ISession session,string key, string value)
        {
            session.SetString(Global.sys_Session_Client+ key, value);
        }
        public static string GetSession(ISession session, string key)
        {
            return session.GetString(Global.sys_Session_Client + key);
        }
        public static GlobalUser GetUserSession(HttpContext httpContext)
        {
            return JsonConvert.DeserializeObject<GlobalUser>(Session.GetSession(httpContext.Session, "CurrentUserInfo"));
        }
    }
}
