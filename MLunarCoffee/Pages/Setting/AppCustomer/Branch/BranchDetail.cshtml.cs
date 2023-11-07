using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Setting.AppCustomer.Branch
{
    public class BranchDetailModel : PageModel
    {
        public string _ABranchDetailID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            _ABranchDetailID = curr != null ? curr.ToString() : "0";
        }

        public async Task<IActionResult> OnPostInit()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Branch_LoadCombo]", CommandType.StoredProcedure);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadata(int CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Setting_AC_Branch_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, string ImageDelete)
        {
            try
            {
                DataTable dt = new DataTable();
                Branch DataMain = JsonConvert.DeserializeObject<Branch>(data);

                DataTable DataImage = JsonConvert.DeserializeObject<DataTable>(DataMain.ImgJson);
                DataTable dtImage = new DataTable();
                dtImage.Columns.Add("Image", typeof(String));
                for (int i = 0; i < DataImage.Rows.Count; i++)
                {
                    DataRow dr = dtImage.NewRow();
                    dr["Image"] = DataImage.Rows[i]["Image"];
                    dtImage.Rows.Add(dr);
                }

                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Setting_AC_Branch_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@TimeOpen ", SqlDbType.NVarChar, DataMain.TimeOpen,
                            "@Address ", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                            "@Lat", SqlDbType.Float, DataMain.Lat,
                            "@Lon", SqlDbType.Float, DataMain.Lon,
                            "@Zalo", SqlDbType.NVarChar, DataMain.Zalo,
                            "@Facebook", SqlDbType.Int, DataMain.Facebook,
                            "@ImgTable", SqlDbType.Structured, (dtImage.Rows.Count > 0) ? dtImage : null,
                            "@Mess", SqlDbType.NVarChar, DataMain.Mess,
                            "@Website", SqlDbType.NVarChar, DataMain.Website,
                            "@Description", SqlDbType.NVarChar, DataMain.Description,
                            "@MainBranchID", SqlDbType.Int, DataMain.MainBranchID,
                            "@UserID", SqlDbType.Int, user.sys_userid
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {

                                return Content("Trùng Tên Chi Nhánh");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Setting_AC_Branch_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@TimeOpen ", SqlDbType.NVarChar, DataMain.TimeOpen,
                            "@Address ", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                            "@Lat", SqlDbType.Float, DataMain.Lat,
                            "@Lon", SqlDbType.Float, DataMain.Lon,
                            "@Zalo", SqlDbType.NVarChar, DataMain.Zalo,
                            "@Facebook", SqlDbType.Int, DataMain.Facebook,
                            "@ImgTable", SqlDbType.Structured, (dtImage.Rows.Count > 0) ? dtImage : null,
                            "@Mess", SqlDbType.NVarChar, DataMain.Mess,
                            "@Website", SqlDbType.NVarChar, DataMain.Website,
                            "@Description", SqlDbType.NVarChar, DataMain.Description,
                            "@MainBranchID", SqlDbType.Int, DataMain.MainBranchID,
                            "@UserID", SqlDbType.Int, user.sys_userid,
                            "@currentID ", SqlDbType.Int, CurrentID
                        );
                    }
                }
                if (ImageDelete != null) DeleteImg(ImageDelete);
                return Content("1");

            }
            catch (Exception ex)
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
    public class Branch
    {
        public string Name { get; set; }
        public string TimeOpen { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Zalo { get; set; }
        public string Facebook { get; set; }
        public string ImgJson { get; set; }
        public string Mess { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public int MainBranchID { get; set; }

    }
}