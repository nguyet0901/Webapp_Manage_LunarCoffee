using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing.Call
{
    public class MyExtensionCallModel : PageModel
    {
        public string _dataConnectCall { get; set; }
        public string _userCurrentExt { get; set; }
        public string _Type_LinkJS { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            var type = Comon.Global.sys_CallCenterType; 
            var user = Session.GetUserSession(HttpContext);
            string _layout = Request.Query["layout"];
            layout = _layout != null ? _layout : "";

            _dataConnectCall = Comon.Global.sys_DtDetectCallCenter;
            _userCurrentExt = user.sys_CalLExtension;
            _Type_LinkJS = LinkTypeJS(type);
             
        }

        private string LinkTypeJS(int Type)
        {
            String result = "";
            switch (Type)
            {
                case 1:
                    result = "";
                    break;
                case 2:
                    result = "/js/LogCallCenter/LogCall_CMC_V1.js";
                    break;
                case 3:
                    result = "/js/LogCallCenter/LogCall_CMC_V2.js";
                    break; 
                case 5:
                    result = "/js/LogCallCenter/LogCall_TACA.js";
                    break;
                case 6:
                    result = "/js/LogCallCenter/LogCall_Voicecloud.js";
                    break;
                case 7:
                    result = "/js/LogCallCenter/LogCall_CloudFone.js";
                    break;
                case 8:
                    result = "/js/LogCallCenter/LogCall_Voip24h.js";
                    break;
                case 10:
                    result = "/js/LogCallCenter/LogCall_FTP_Oncall.js";
                    break;
                case 11:
                    result = "/js/LogCallCenter/LogCall_ETelecom.js";
                    break;
                case 12:
                    result = "/js/LogCallCenter/LogCall_Omicall.js";
                    break;
                case 13:
                    result = "/js/LogCallCenter/LogCall_Incall.js";
                    break;
                case 14:
                    result = "/js/LogCallCenter/LogCall_Callio.js";
                    break; 
                case 15:
                    result = "/js/LogCallCenter/LogCall_3CX.js";
                    break;
                case 16:
                    result = "/js/LogCallCenter/LogCall_YCall.js";
                    break;
                default:
                    result = "#";
                    break;
            }
            return result;
        } 
    }
}
