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
    public class HistoryCallByPhoneModel : PageModel
    {
        public string _dataConnectCall { get; set; }
        public string _Phone1 { get; set; }
        public string _Phone2 { get; set; }
        public string _Type_LinkJS { get; set; }
        public void OnGet()
        {
            _dataConnectCall = Comon.Global.sys_DtDetectCallCenter;

            var phone1 = Request.Query["phone1"].ToString();
            var phone2 = Request.Query["phone2"].ToString();
            _Phone1 = phone1 != "" ? phone1 : "";
            _Phone2 = phone2 != "" ? phone2 : "";
            var type = Comon.Global.sys_CallCenterType;
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

        public async Task<IActionResult> OnPostLoadExtension()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Call_Extension_LoadCombo]", CommandType.StoredProcedure);
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));

                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }



        //public async Task<IActionResult> OnPostLoadata(string date, string page, string extension)
        //{
        //    try
        //    {
        //        var user = Session.GetUserSession(HttpContext);
        //        string userUsingCallCenter = user.sys_UsingCallCenter.ToString();
        //        string checkingCallCenter = Comon.Global.sys_ClientIsUsingCall.ToString();

        //        string resulf = "0";
        //        if (checkingCallCenter == "1")/*userUsingCallCenter == "1" && */
        //        {
        //            resulf = Comon.CallCenter.GetOutboundLog_All(date, page, extension);
        //        }
        //        else if (checkingCallCenter == "2")/*userUsingCallCenter == "1" && */
        //        {
        //            resulf = await Comon.CallCenterOutsite.GetOutboundLog_All(date);
        //        }

        //        return Content(resulf);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("");
        //    }
        //}
    }
}
