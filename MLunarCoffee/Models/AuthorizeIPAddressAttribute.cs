using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MLunarCoffee.Comon;
using System.Net;
using System.Text.RegularExpressions;

namespace MLunarCoffee.Models
{
    public class AuthorizeIPAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Get users IP Address 
            var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
            string _safelist = Global.TrickIP != null ? Global.TrickIP : "";
            var ip = Regex.Matches(_safelist, @"\[(.*?)\]");
            var badIp = true;

            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }

            foreach (var item in ip)
            {
                string address = Regex.Replace(item.ToString(), @"\[|\]", "");
                var testIp = IPAddress.Parse(address);

                if (testIp.Equals(remoteIp))
                {
                    badIp = false;
                    break;
                }
            }

            if (badIp)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
