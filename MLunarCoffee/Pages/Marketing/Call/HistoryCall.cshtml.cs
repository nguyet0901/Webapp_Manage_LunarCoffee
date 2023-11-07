using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Marketing.Call
{
    public class HistoryCallModel : PageModel
    {
        public string _dataExt { get; set; }
        public string _dataConnectCall { get; set; }
        public string _Type_LinkJS { get; set; }
        public int _LogClient { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";
            _dataConnectCall = Comon.Global.sys_DtDetectCallCenter;
            _LogClient = Comon.Global.sys_CallLogClient;
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
                case 4:
                    result = "/js/LogCallCenter/LogCall_ThienKhue.js";
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
                    break;
            }
            return result;
        }

        public async Task<IActionResult> OnPostLoadata(string date/*, string page, string extension*/)
        {
            try
            {
                //var user = Session.GetUserSession(HttpContext);
                //string userUsingCallCenter = user.sys_UsingCallCenter.ToString();
                //string checkingCallCenter = Comon.Global.sys_ClientIsUsingCall.ToString();

                //string resulf = "0";
                //if (checkingCallCenter == "1")
                //{
                //    resulf = Comon.CallCenter.GetOutboundLog_All(date, page, extension);
                //}
                //else if (checkingCallCenter == "2")
                //{
                //    resulf = await Comon.CallCenterOutsite.GetOutboundLog_All(date);
                //}
                string resulf = "0";
                if (Comon.Global.sys_CallOnlyHTTP == "1")
                {
                    resulf = await Comon.CallCenterOutsite.GetOutboundLog_All(date);
                }
                return Content(resulf);


            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadEmpExtension(String UserID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Call_Extension_Load]", CommandType.StoredProcedure, "@UserID", SqlDbType.Int, UserID);
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

        public async Task<IActionResult> OnPostLoadLogs(string dateFrom, string dateTo, string extension)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_LogCall_LoadList]", CommandType.StoredProcedure,
                        "@dateFrom", SqlDbType.NVarChar, dateFrom
                        , "@dateTo", SqlDbType.NVarChar, dateTo
                        , "@extension", SqlDbType.NVarChar, extension
                    );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
