using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LunarCoffee
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<Models.ApplicationDbContext>(null);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["HomNay"] = 0;
            Application["HomQua"] = 0;
            Application["TuanNay"] = 0;
            Application["TuanTruoc"] = 0;
            Application["ThangNay"] = 0;
            Application["ThangTruoc"] = 0;
            Application["TatCa"] = 0;
            Application["visitors_online"] = 0;
        }
        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 150;
            Application.Lock();
            Application["visitors_online"] = Convert.ToInt32(Application["visitors_online"]) + 1;
            Application.UnLock();
            try
            {
                var item = LunarCoffee.Models.Common.ThongKeTruyCap.ThongKe();
                if (item != null)
                {
                    Application["HomNay"] = long.Parse("0" + item.HomNay.ToString("#,###"));
                    Application["HomQua"] = long.Parse("0" + item.HomQua.ToString("#,###"));
                    Application["TuanNay"] = long.Parse("0" + item.TuanNay.ToString("#,###"));
                    Application["TuanTruoc"] = long.Parse("0" + item.TuanTruoc.ToString("#,###"));
                    Application["ThangNay"] = long.Parse("0" + item.ThangNay.ToString("#,###"));
                    Application["ThangTruoc"] = long.Parse("0" + item.ThangTruoc.ToString("#,###"));
                    Application["TatCa"] = int.Parse(item.TatCa.ToString()).ToString("#,###");
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }
        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["visitors_online"] = Convert.ToUInt32(Application["visitors_online"]) - 1;
            Application.UnLock();
        }
    }
}
