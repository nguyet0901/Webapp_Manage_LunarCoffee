using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Models.GlobalPer;
using MLunarCoffee.Comon.Crypt;

namespace MLunarCoffee.Models
{
    public class AsyncPageFilter : IAsyncPageFilter
    {
        private readonly IConfiguration _config;
        public AsyncPageFilter(IConfiguration config)
        {
            _config = config;
        }
        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context ,PageHandlerExecutionDelegate next)
        {

            var isPass = true;
            string IsMinify = Session.GetSession(context.HttpContext.Session ,"Minify");
            if (context.HttpContext.Request.Method == "GET")
            {
                var path = context.HttpContext.Request.Path;
                path = path.ToString().Trim().ToLower();

                if (path != "/vi/login/login" && path != "/login/login" && path != "/shared/_layout" && path != "/master/master_top")
                {
                    if (Session.GetSession(context.HttpContext.Session ,"Token") == null
                        || Session.GetSession(context.HttpContext.Session ,"Token") == "")
                    {
                        isPass = false;
                    }
                    else
                    {
                        string ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
                        if (CheckPermission(path ,context.HttpContext ,ip))
                        {
                            isPass = false;
                        }
                    }
                }
                else
                {
                    var Idleminute = Session.GetSession(context.HttpContext.Session ,"Idleminute");
                    if (Idleminute != "0")
                    {
                        if (path == "/master/master_top")
                        {
                            var Timeidle = Session.GetSession(context.HttpContext.Session ,"Idletimeout");
                            if (Timeidle != null && Timeidle != "" && double.TryParse(Timeidle ,out double timeidle))
                            {
                                if (DateTime.Now.AddMinutes(0 - Convert.ToInt32(Idleminute)).Ticks > Convert.ToInt64(Timeidle.ToString()))
                                {
                                    Session.SetSession(context.HttpContext.Session ,"Token" ,"");
                                    isPass = false;
                                }
                                else Session.SetSession(context.HttpContext.Session ,"Idletimeout" ,DateTime.Now.Ticks.ToString());
                            }
                        }
                    }
                }
            }
            else
            {

                if (Session.GetSession(context.HttpContext.Session ,"Token") == null
                || Session.GetSession(context.HttpContext.Session ,"Token") == "")
                {
                    isPass = false;
                }
            }
            if (isPass) await next.Invoke();
            else
            {
                context.Result = new RedirectToPageResult("/error");
                return;
            }
        }
        private bool CheckPermission(string path ,HttpContext httpContext ,string ip)
        {
            if (path != "" && path.Length > 1)
            {
                path = path.TrimEnd('/');

            }

            List<PageUser> permenu = Session.GetUserSession(httpContext).sys_Pageuser;
            List<PageClass> sys_Page = Global.sys_Page;
            List<PageMushide> sys_Pagemusthide = Global.sys_Pagemusthide;
            bool result;
            var per = new PermissionPage(sys_Page ,sys_Pagemusthide ,permenu);
            result = per.CheckPermissionPageByMenu(path ,ip);
            //using (var per =new PermissionPage(sys_Page,sys_Pagemusthide, permenu)) {
            //    result= per.CheckPermissionPageByMenu(path,ip );

            //}
            // permenu.Clear();
            // permenu = null;
            return result;
        }
    }
}

