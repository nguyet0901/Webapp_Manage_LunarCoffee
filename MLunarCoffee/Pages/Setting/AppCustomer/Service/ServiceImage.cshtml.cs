using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.AppCustomer.Service
{
    public class ServiceImageModel : PageModel
    {
        public string _Type { get; set; }
        public void OnGet()
        {
            string type = Request.Query["Type"];
            _Type = type != null ? type.ToString() : "";
        }

        public async Task<IActionResult> OnPostLoadData(string ImageType)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_App_SlideImage]", CommandType.StoredProcedure,
                        "@type", SqlDbType.NVarChar, ImageType);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
            }
            catch
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExecute(string data, string type, string ImageDelete)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable DataMain = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("Image", typeof(String));
                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dtMain.NewRow();
                    dr["Image"] = DataMain.Rows[i]["Image"];
                    dtMain.Rows.Add(dr);
                }
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_AC_Image_Insert]", CommandType.StoredProcedure,
                        "@table", SqlDbType.Structured, (dtMain.Rows.Count > 0) ? dtMain : null,
                        "@UserID", SqlDbType.Int, user.sys_userid,
                        "@type", SqlDbType.NVarChar, type);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
                if (ImageDelete != null) DeleteImg(ImageDelete);
                return Content("1");
            }
            catch
            {
                return Content("0");
            }
        }

        private static void DeleteImg(string ImgList)
        {
            try
            {
                string[] words = ImgList.Split(',');
                foreach (string word in words)
                {
                    if (word != "")
                    {
                        string des = Comon.Global.sys_ServerImageFolder + "App/" + word;
                        Comon.Comon.DeleteFile(des);
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }
    }
}
