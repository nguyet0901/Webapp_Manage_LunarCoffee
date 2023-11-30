using Microsoft.AspNetCore.Http;

namespace MLunarCoffee.Comon.SyntaxCode
{
    public static class SyntaxCode
    {
        public static string GetSyntax(HttpContext httpContext, string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                GlobalUser _globaluser = (GlobalUser)Session.Session.GetUserSession(httpContext);
                code = code.Replace("'" ,"").Trim().ToLower();
                code = code.Replace("[sn]" ,_globaluser.syntax_sn);
                code = code.Replace("[ss]" ,_globaluser.syntax_ss);
                code = code.Replace("[mcn_doc]" ,_globaluser.syntax_scmcn_doc);
                code = code.Replace("[mcn]" ,_globaluser.syntax_scmcn);
            }
            return code;
        }
    }
}
