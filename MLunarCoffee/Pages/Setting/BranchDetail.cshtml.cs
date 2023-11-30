using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting
{
    public class BranchDetailModel : PageModel
    {
        public int sys_isShowLogoBranch { get; set; }
        public string _DataDetailID { get; set; }
        public string _LinkRoot { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _DataDetailID = curr.ToString();
            }
            else
            {
                _DataDetailID = "0";
            }
            sys_isShowLogoBranch = Comon.Global.sys_isShowLogoBranch;
            _LinkRoot = Comon.Global.sys_HTTPImageRoot;
        }
        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBranch = new DataTable();
                        dtBranch = await confunc.ExecuteDataTable("MLU_sp_LoadCombo_Area" ,CommandType.StoredProcedure);
                        dtBranch.TableName = "Area";
                        return dtBranch;
                    }

                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_LoadCombo_Bank" ,CommandType.StoredProcedure);
                        dt.TableName = "Bank";
                        return dt;
                    }

                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBranch = new DataTable();
                        dtBranch = await confunc.ExecuteDataTable("[MLU_sp_BranchMailServer]" ,CommandType.StoredProcedure);
                        dtBranch.TableName = "BranchMail";
                        return dtBranch;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Branch_LoadDetail]" ,CommandType.StoredProcedure ,
                      "@ID" ,SqlDbType.Int ,Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("[]");
            }
            catch
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID ,string ImageDelete)
        {
            try
            {
                Branch DataMain = JsonConvert.DeserializeObject<Branch>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                string Bank_LinkQR = String.Format("https://api.vietqr.io/image/{2}-{0}-4UkW1dT.jpg?accountName={1}" ,DataMain.Bank_Num ,DataMain.Bank_Name.ToUpper().Replace(" " ,"%20") ,DataMain.Bank);
                if (CurrentID == "0")
                {

                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Branch_Insert]" ,CommandType.StoredProcedure ,
                            "@BranchCode" ,SqlDbType.NVarChar ,DataMain.BranchCode.Replace("'" ,"").Trim() ,
                            "@Name " ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@ShortName " ,SqlDbType.NVarChar ,DataMain.ShortName.Replace("'" ,"").Trim() ,
                            "@Tel " ,SqlDbType.NVarChar ,DataMain.Tel ,
                            "@Target_Amount " ,SqlDbType.Decimal ,DataMain.Target ,
                            "@Address " ,SqlDbType.NVarChar ,DataMain.Address.Replace("'" ,"").Trim() ,
                            "@IP" ,SqlDbType.NVarChar ,DataMain.IP ,
                            "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Lat" ,SqlDbType.Float ,DataMain.Lat ,
                            "@Long" ,SqlDbType.Float ,DataMain.Long ,
                            "@TimeOpen" ,SqlDbType.NVarChar ,DataMain.TimeOpen ,
                            "@IsShowApp" ,SqlDbType.Int ,DataMain.IsShowApp ,
                            "@IsTraining" ,SqlDbType.Int ,DataMain.IsTraining ,
                            "@ImgJson" ,SqlDbType.NVarChar ,DataMain.ImgJson ,
                            "@Image" ,SqlDbType.NVarChar ,DataMain.Image ,
                            "@Image1" ,SqlDbType.NVarChar ,DataMain.Image1 ,
                            "@Email" ,SqlDbType.NVarChar ,DataMain.Mail ,
                            "@BranchCodeDoc" ,SqlDbType.NVarChar ,DataMain.BranchCodeDoc ,
                            "@Represent" ,SqlDbType.NVarChar ,DataMain.Represent ,
                            "@RepresentPosition" ,SqlDbType.NVarChar ,DataMain.RepresentPosition ,
                            "@SMSBrandName" ,SqlDbType.NVarChar ,DataMain.SMSBrandName != null ? DataMain.SMSBrandName : "" ,
                            "@ServerMailID" ,SqlDbType.NVarChar ,DataMain.ServerMailID != null ? Convert.ToInt32(DataMain.ServerMailID) : 0 ,
                            "@CityCode" ,SqlDbType.NVarChar ,DataMain.CityCode ,
                            "@DistrictID" ,SqlDbType.NVarChar ,DataMain.DistrictID ,
                            "@CommuneCode" ,SqlDbType.NVarChar ,DataMain.CommuneCode ,
                            "@AreaID" ,SqlDbType.Int ,DataMain.AreaID ,
                            "@NationalID" ,SqlDbType.NVarChar ,DataMain.NationalID ,
                            "@ThirdParty_BranchID" ,SqlDbType.NVarChar ,DataMain.ThirdPartyBranchID ,
                            "@Bank_Num" ,SqlDbType.NVarChar ,DataMain.Bank_Num ,
                            "@Bank_Name" ,SqlDbType.NVarChar ,DataMain.Bank_Name ,
                            "@Bank" ,SqlDbType.NVarChar ,DataMain.Bank ,
                            "@Bank_LinkQR" ,SqlDbType.NVarChar ,Bank_LinkQR ,
                            "@BranchAlias" ,SqlDbType.Int ,DataMain.BranchAlias
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Branch_Update]" ,CommandType.StoredProcedure ,
                            "@BranchCode" ,SqlDbType.NVarChar ,DataMain.BranchCode.Replace("'" ,"").Trim() ,
                            "@Name " ,SqlDbType.NVarChar ,DataMain.Name.Replace("'" ,"").Trim() ,
                            "@ShortName " ,SqlDbType.NVarChar ,DataMain.ShortName.Replace("'" ,"").Trim() ,
                            "@Target_Amount " ,SqlDbType.Int ,DataMain.Target ,
                            "@Address " ,SqlDbType.NVarChar ,DataMain.Address ,
                            "@Modified_By" ,SqlDbType.Int ,user.sys_userid ,
                            "@Tel " ,SqlDbType.NVarChar ,DataMain.Tel ,
                            "@IP" ,SqlDbType.NVarChar ,DataMain.IP ,
                            "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                            "@Lat" ,SqlDbType.Float ,DataMain.Lat ,
                            "@Long" ,SqlDbType.Float ,DataMain.Long ,
                            "@TimeOpen" ,SqlDbType.NVarChar ,DataMain.TimeOpen ,
                            "@IsShowApp" ,SqlDbType.Int ,DataMain.IsShowApp ,
                            "@IsTraining" ,SqlDbType.Int ,DataMain.IsTraining ,
                            "@ImgJson" ,SqlDbType.NVarChar ,DataMain.ImgJson ,
                            "@Image" ,SqlDbType.NVarChar ,DataMain.Image ,
                            "@Image1" ,SqlDbType.NVarChar ,DataMain.Image1 ,
                            "@Email" ,SqlDbType.NVarChar ,DataMain.Mail ,
                            "@BranchCodeDoc" ,SqlDbType.NVarChar ,DataMain.BranchCodeDoc ,
                            "@Represent" ,SqlDbType.NVarChar ,DataMain.Represent ,
                            "@RepresentPosition" ,SqlDbType.NVarChar ,DataMain.RepresentPosition ,
                            "@SMSBrandName" ,SqlDbType.NVarChar ,DataMain.SMSBrandName != null ? DataMain.SMSBrandName : "" ,
                            "@ServerMailID" ,SqlDbType.NVarChar ,DataMain.ServerMailID != null ? Convert.ToInt32(DataMain.ServerMailID) : 0 ,
                            "@CityCode" ,SqlDbType.NVarChar ,DataMain.CityCode ,
                            "@DistrictID" ,SqlDbType.NVarChar ,DataMain.DistrictID ,
                            "@CommuneCode" ,SqlDbType.NVarChar ,DataMain.CommuneCode ,
                            "@AreaID" ,SqlDbType.Int ,DataMain.AreaID ,
                            "@NationalID" ,SqlDbType.NVarChar ,DataMain.NationalID ,
                            "@ThirdParty_BranchID" ,SqlDbType.NVarChar ,DataMain.ThirdPartyBranchID ,
                            "@Bank_Num" ,SqlDbType.NVarChar ,DataMain.Bank_Num ,
                            "@Bank_Name" ,SqlDbType.NVarChar ,DataMain.Bank_Name ,
                            "@Bank" ,SqlDbType.NVarChar ,DataMain.Bank ,
                            "@Bank_LinkQR" ,SqlDbType.NVarChar ,Bank_LinkQR ,
                            "@BranchAlias" ,SqlDbType.Int ,DataMain.BranchAlias
                        );
                    }
                }
                if (ImageDelete != null) DeleteImg(ImageDelete);
                if (dt.Rows.Count > 0)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
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
        public async Task<IActionResult> OnPostDeleteItem(int CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Branch_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostCheckPassword(string password)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_User_CheckPassword]" ,CommandType.StoredProcedure ,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                if (dt.Rows.Count != 0 && Comon.DataJson.GetFirstValue(dt) == password)
                    return Content("1");
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("");
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
        public string IP { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string BranchCode { get; set; }
        public string DistrictID { get; set; }
        public string CommuneCode { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public int Target { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string TimeOpen { get; set; }
        public string IsShowApp { get; set; }
        public string IsTraining { get; set; }
        public string ImgJson { get; set; }
        public string Image { get; set; }
        public string Image1 { get; set; }
        public string Mail { get; set; }
        public string BranchCodeDoc { get; set; }
        public string SMSBrandName { get; set; }
        public string ServerMailID { get; set; }
        public string CityCode { get; set; }
        public string AreaID { get; set; }
        public string NationalID { get; set; }
        public string Represent { get; set; }
        public string RepresentPosition { get; set; }
        public string ThirdPartyBranchID { get; set; }

        public string Bank_Num { get; set; }
        public string Bank_Name { get; set; }
        public string Bank { get; set; }
        public string BranchAlias { get; set; }


    }
}
