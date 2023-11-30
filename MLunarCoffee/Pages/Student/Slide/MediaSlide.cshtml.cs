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

namespace MLunarCoffee.Pages.Student.Settings.Media
{
    public class MediaSlideModel : PageModel
    {
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            if (_layout != null) layout = _layout;
            else layout = "";
        }
        public async Task<IActionResult> OnPostLoadCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSlideType = new DataTable();
                        dtSlideType = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_TrainSlideType_LoadCombo]", CommandType.StoredProcedure);
                        dtSlideType.TableName = "SlideType";
                        return dtSlideType;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSlideTag = new DataTable();
                        dtSlideTag = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_TrainSlideTag_LoadCombo]", CommandType.StoredProcedure);
                        dtSlideTag.TableName = "SlideTag";
                        return dtSlideTag;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSlideType = new DataTable();
                        dtSlideType = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_MediaAttachmentType_LoadCombo]", CommandType.StoredProcedure);
                        dtSlideType.TableName = "AttachmentType";
                        return dtSlideType;
                    }
                }));
                var resutl = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var i in resutl)
                {
                    ds.Tables.Add(i);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecutedSlide(string data, int CurrentID)
        {
            try
            {
                DataSlideInit DataMain = JsonConvert.DeserializeObject<DataSlideInit>(data);
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == 0)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_TrainSlide_Insert]", CommandType.StoredProcedure
                            , "@SlideTypeID", SqlDbType.Int, Convert.ToInt32(DataMain.SlideTypeID)
                            , "@Code", SqlDbType.NVarChar, DataMain.Code
                            , "@Name", SqlDbType.NVarChar, DataMain.Name
                            , "@TagID", SqlDbType.Int, Convert.ToInt32(DataMain.TagID)
                            , "@Content", SqlDbType.NVarChar, DataMain.Content
                            , "@Note", SqlDbType.NVarChar, DataMain.Note
                            , "@Created_By", SqlDbType.Int, user.sys_userid
                            );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_TrainSlide_Update]", CommandType.StoredProcedure
                            , "@SlideTypeID", SqlDbType.Int, Convert.ToInt32(DataMain.SlideTypeID)
                            , "@Code", SqlDbType.NVarChar, DataMain.Code
                            , "@Name", SqlDbType.NVarChar, DataMain.Name
                            , "@TagID", SqlDbType.Int, Convert.ToInt32(DataMain.TagID)
                            , "@Content", SqlDbType.NVarChar, DataMain.Content
                            , "@Note", SqlDbType.NVarChar, DataMain.Note
                            , "@Modify_By", SqlDbType.Int, user.sys_userid
                            , "@CurrentID", SqlDbType.Int, CurrentID
                            );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostInsertFiles(string Name, string URL, string SlideID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_MediaAttachment_Insert]", CommandType.StoredProcedure
                        , "@Name", SqlDbType.NVarChar, Name
                        , "@URL", SqlDbType.NVarChar, URL
                        , "@Created_By", SqlDbType.Int, user.sys_userid
                        , "@SlideID", SqlDbType.Int,Convert.ToInt32(SlideID)
                        );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteFile(int IDMediaAttach,int IDSlideAttach, string URL) {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                string des = Comon.Global.sys_ServerImageFolder + "Sys_Student" + '/' + "Folder" + '/' + "File" + '/' + URL.ToString();
                Comon.Comon.DeleteFile(des);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_MediaAttachment_Delete]", CommandType.StoredProcedure
                        , "@IDMediaAttach", SqlDbType.NVarChar, IDMediaAttach
                        , "@IDSlideAttach", SqlDbType.NVarChar, IDSlideAttach
                        );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExecutedContent(string slideid, string data)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dataContent = JsonConvert.DeserializeObject<DataTable>(data);

                DataTable dtContent = new DataTable();
                dtContent.Columns.Add("IDLayout", typeof(Int32));
                dtContent.Columns.Add("title", typeof(string));
                dtContent.Columns.Add("content", typeof(string));
                dtContent.Columns.Add("numcol", typeof(Int32));
                for (int i = 0; i < dataContent.Rows.Count; i++)
                {
                    DataRow dr = dtContent.NewRow();
                    dr["IDLayout"] = Convert.ToInt32(dataContent.Rows[i]["IDLayout"]);
                    dr["title"] = Convert.ToString(dataContent.Rows[i]["title"]);
                    dr["content"] = Convert.ToString(dataContent.Rows[i]["content"]);
                    dr["numcol"] = Convert.ToInt32(dataContent.Rows[i]["numcol"]);
                    dr["PackageNumber"] = "";
                    dtContent.Rows.Add(dr);
                }
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_ST_Settings_TrainContent_Insert]", CommandType.StoredProcedure
                        , "@SlideID", SqlDbType.NVarChar, Convert.ToInt32(slideid)
                        , "@DataTable", SqlDbType.Structured, dtContent.Rows.Count > 0 ? dtContent : null
                        ); ;
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class DataSlideInit
    {
        public string SlideTypeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TagID { get; set; }
        public string Content { get; set; }
        public string IsDisabled { get; set; }
        public string Note { get; set; }
    }
}
