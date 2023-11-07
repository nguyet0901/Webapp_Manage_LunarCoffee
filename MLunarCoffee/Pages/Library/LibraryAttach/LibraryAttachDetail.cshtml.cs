using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Library.LibraryAttach
{
    public class LibraryAttachDetailModel : PageModel
    {
        public string _OriginCode { get; set; }
        public string _CreatedCode { get; set; }
        public string _OriginID { get; set; }
        public string _IsView { get; set; }
        public void OnGet()
        {
            string OriginCode = Request.Query["OriginCode"];
            string CreatedCode = Request.Query["CreatedCode"];
            string OriginID = Request.Query["OriginID"];
            string IsView = Request.Query["IsView"];
            _OriginCode = OriginCode != null ? OriginCode.ToString() : "";
            _CreatedCode = CreatedCode != null ? CreatedCode.ToString() : "";
            _OriginID = OriginID != null ? OriginID.ToString() : "0";
            _IsView = IsView != null ? IsView.ToString() : "0";
        }

        public async Task<IActionResult> OnPostLoadFile(string code, string createdcode, string id)
        {
            try
            {
                DataTable dt = new DataTable();

                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_Library_Attachment_LoadDetail_ByOrigin]", CommandType.StoredProcedure
                         , "@code", SqlDbType.NVarChar, code?.ToString().Trim()
                         , "@CreatedCode", SqlDbType.DateTime, createdcode
                         , "@id", SqlDbType.Int, Convert.ToInt32(id.ToString())
                         , "@UserID", SqlDbType.Int, user.sys_userid
                         , "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate
                    );
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("[]");


            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostInsert(string code, string createdcode, string name, string realname, string filetype)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                filetype = filetype.ToLower().Trim();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Library_Attachment_Insert]", CommandType.StoredProcedure
                         , "@code", SqlDbType.NVarChar, code.ToString().Trim()
                         , "@CreatedCode", SqlDbType.DateTime, createdcode
                         , "@name", SqlDbType.NVarChar, Path.GetFileNameWithoutExtension(name)
                         , "@realName", SqlDbType.NVarChar, Path.GetFileNameWithoutExtension(realname)
                         , "@fileType", SqlDbType.NVarChar, filetype
                         , "@userID", SqlDbType.Int, user.sys_userid
                    );
                }
                if (dt != null && dt.Rows.Count == 1)
                {
                    return Content(dt.Rows[0]["ID"].ToString());
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostDeleteItem(string id, string filename)
        {
            try
            {
                string url = Comon.Global.sys_FLibUrl == ""
                                              ? $"{Comon.Global.sys_ServerImageFolder}_Library/{Comon.Global.sys_FLib}"
                                              : $"{Comon.Global.sys_FLibUrl}/{Comon.Global.sys_FLib}";

                string des = url + '/' + filename.ToString();
                Comon.Comon.DeleteFile(des);
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[YYY_Library_Attachment_Delete]", CommandType.StoredProcedure,
                       "@currentID", SqlDbType.Int, Convert.ToInt32(id)
                       , "@UserID", SqlDbType.Int, user.sys_userid
                   );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

        public async Task<IActionResult> OnPostLoadTotal(string code, string createdcode)
        {
            try
            {
                DataTable dt = new DataTable();


                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_Library_Attachment_LoadTotal_ByCode]", CommandType.StoredProcedure
                         , "@code", SqlDbType.NVarChar, code?.ToString().Trim()
                         , "@CreatedCode", SqlDbType.DateTime, createdcode
                    );
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("[]");


            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
