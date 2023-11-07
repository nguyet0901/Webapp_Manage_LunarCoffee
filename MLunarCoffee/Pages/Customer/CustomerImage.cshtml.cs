using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Customer
{
    public class CustomerImageModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }
        public string _Clicktoshow { get; set; }
       
       
        public void OnGet()
        {

            _dtPermissionCurrentPage = HttpContext.Request.Path;
            _Clicktoshow = Global.sys_Clicktoshow != null ? Global.sys_Clicktoshow.ToString() : "-1";
         
            
        }

        public  async Task<IActionResult> OnPostLoadAllFolder(string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =  await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_GetList]", CommandType.StoredProcedure,
                        "@CustomerID", SqlDbType.Int, CustomerID
                    );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }


        public async Task<IActionResult> OnPostLoadImageByFolder(string currentFolderID
            , string idetail
            , string type)
        {
            try
            {
                DataSet ds = new DataSet();


                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds =await connFunc.ExecuteDataSet("[YYY_sp_CustomerImage_GetDetail]", CommandType.StoredProcedure,
                        "@currentFolderID", SqlDbType.Int, currentFolderID
                        , "@idetail", SqlDbType.Int, idetail
                         , "@type", SqlDbType.Int, type
                    );
                }
                return Content(Comon.DataJson.Dataset(ds));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        //public  async Task<IActionResult> OnPostCheckLimit()
        //{
        //    if (Comon.Comon.CheckLimitUpLoad(HttpContext))
        //    {
        //        return Content("1");
        //    }
        //    else
        //    {
        //        return Content("0");
        //    }
        //}


        public  async Task<IActionResult> OnPostInsertImage(string type, string folderid, string name, string realname,string namefeature, string sizebyte)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_Create_Image]", CommandType.StoredProcedure,
                   "@FolderID", SqlDbType.Int, folderid
                   , "@RealName", SqlDbType.NVarChar, realname.Replace("'", "")
                   , "@Type", SqlDbType.NVarChar, type
                   , "@Name", SqlDbType.NVarChar, name.Replace("'", "")
                   , "@Namefeature", SqlDbType.NVarChar,namefeature!=null ? namefeature.Replace("'", ""):""
                   , "@CreatedBy", SqlDbType.Int, user.sys_userid
                   , "@BranchID", SqlDbType.Int, user.sys_branchID
                   , "@sizebyte", SqlDbType.Int, Convert.ToInt32(sizebyte)
                   );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

        public  async Task<IActionResult> OnPostDeleteFolder(string customer, string id, string name)
        {
            try
            {
                DataTable dtRes = new DataTable();

                if (Comon.Comon.DeleteDirectoryFTP(customer, name))
                {
                    var user = Session.GetUserSession(HttpContext);

                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtRes = await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_Delete_Folder]", CommandType.StoredProcedure,
                           "@currentID", SqlDbType.Int, id
                           , "@Createdby", SqlDbType.Int, user.sys_userid
                       );
                    }
                    return Content(Comon.DataJson.GetFirstValue(dtRes));
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }



        public  async Task<IActionResult> OnPostDeleteImage(string data, string cusID, string folder, string type)
        {
            try
            {
                if (type == "1")
                {
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(data);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string des = Comon.Global.sys_ServerImageFolder + cusID + '/' + folder + '/' + dt.Rows[i]["value"].ToString();
                            string desfeaure= Comon.Global.sys_ServerImageFolder + cusID + '/' + folder + '/' + dt.Rows[i]["feaure"].ToString();
                            Comon.Comon.DeleteFile(des);
                            if(desfeaure!= des && dt.Rows[i]["feaure"].ToString()!="")
                            {
                                Comon.Comon.DeleteFile(desfeaure);
                            }
                            var user = Session.GetUserSession(HttpContext);
                            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                            {
                                await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_Delete_Image]", CommandType.StoredProcedure,
                                   "@currentID", SqlDbType.Int, dt.Rows[i]["id"].ToString()
                                   , "@Createdby", SqlDbType.Int, user.sys_userid
                               );
                            }

                        }
                        return Content("1");
                    }
                    else return Content("0");
                }
                else
                {
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(data);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string des = Comon.Global.sys_ServerImageFolder + cusID + '/' + folder + '/' + "File" + '/' + dt.Rows[i]["value"].ToString();
                            Comon.Comon.DeleteFile(des);
                            var user = Session.GetUserSession(HttpContext);
                            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                            {
                                await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_Delete_Image]", CommandType.StoredProcedure,
                                   "@currentID", SqlDbType.Int, dt.Rows[i]["id"].ToString()
                                   , "@Createdby", SqlDbType.Int, user.sys_userid
                               );
                            }

                        }
                        return Content("1");
                    }
                    else return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }





        public  async Task<IActionResult> OnPostLoadTemplateForm()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_GetTemplate]", CommandType.StoredProcedure
                    );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        public  async Task<IActionResult> OnPostLoadImgByForm(int FolderID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_GetByForm]", CommandType.StoredProcedure,
                        "@FolderID", SqlDbType.Int, FolderID
                    );
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        public  async Task<IActionResult> OnPostInsertImgForm(string folderid, string templateid, string data)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_UpdateImgForm]", CommandType.StoredProcedure,
                        "@FolderID", SqlDbType.Int, folderid
                        , "@TemplateID", SqlDbType.Int, templateid
                        , "@Data", SqlDbType.NVarChar, data.Replace("'", "")
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
        public async Task<IActionResult> OnPostExeSH(string imgid)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                     dt = await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_ExeSH]", CommandType.StoredProcedure,
                        "@ImageID", SqlDbType.Int, imgid
                        ,"@UserID", SqlDbType.Int, user.sys_userid
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostGetSizeInDay()
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_GetSizeInDay]" ,CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));

            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
    }
}
