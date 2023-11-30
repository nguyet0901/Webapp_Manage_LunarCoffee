using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Setting.AccountEntry
{
    public class AccountEntryDetailModel : PageModel
    {
        public string _AccountEntryDetailID { get; set; }
        public string _AccountEntryTypeID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            string type = Request.Query["Type"];
            _AccountEntryDetailID = curr != null ? curr.ToString() : "0";
            _AccountEntryTypeID = type != null ? type.ToString() : "0";
        }


        public async Task<IActionResult> OnPostLoadDetail(string CurrentID)
        {
            var user = Session.GetUserSession(HttpContext);
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Setting_AccountEntry_LoadDetail]" ,CommandType.StoredProcedure ,
                    "@ID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID));
            }
            if (dt != null)
            {
                return Content(Comon.DataJson.Datatable(dt));
            }
            else
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data ,string CurrentID ,string CurrentType)
        {
            try
            {
                AccountEntry DataMain = JsonConvert.DeserializeObject<AccountEntry>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0" || CurrentType == "1")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_AccountEntry_Insert]" ,CommandType.StoredProcedure ,
                              "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"").Trim() ,
                              "@CreditAccount" ,SqlDbType.Int ,DataMain.CreditAccount ,
                              "@DebitAccount" ,SqlDbType.Int ,DataMain.DebitAccount ,
                              "@IsCreditBranch" ,SqlDbType.Int ,DataMain.IsCreditBranch ,
                              "@IsDebitBranch" ,SqlDbType.Int ,DataMain.IsDebitBranch ,
                              "@TypeID" ,SqlDbType.Int ,DataMain.TypeID ,
                              "@ReasonID" ,SqlDbType.Int ,DataMain.ReasonID ,
                              "@MethodID" ,SqlDbType.Int ,DataMain.MethodID ,
                              "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                              "@IsProduct " ,SqlDbType.Int ,DataMain.IsProduct
                          );

                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[MLU_sp_Setting_AccountEntry_Update]" ,CommandType.StoredProcedure ,
                              "@Note" ,SqlDbType.NVarChar ,DataMain.Note.Replace("'" ,"").Trim() ,
                              "@CreditAccount" ,SqlDbType.Int ,DataMain.CreditAccount ,
                              "@DebitAccount" ,SqlDbType.Int ,DataMain.DebitAccount ,
                              "@IsCreditBranch" ,SqlDbType.Int ,DataMain.IsCreditBranch ,
                              "@IsDebitBranch" ,SqlDbType.Int ,DataMain.IsDebitBranch ,
                              "@TypeID" ,SqlDbType.Int ,DataMain.TypeID ,
                              "@ReasonID" ,SqlDbType.Int ,DataMain.ReasonID ,
                              "@MethodID" ,SqlDbType.Int ,DataMain.MethodID ,
                              "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                              "@currentID " ,SqlDbType.Int ,CurrentID ,
                              "@IsProduct " ,SqlDbType.Int ,DataMain.IsProduct
                        );
                        return Content(Comon.DataJson.GetFirstValue(dt));
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public class AccountEntry
        {
            public string Note { get; set; }
            public int CreditAccount { get; set; }
            public int DebitAccount { get; set; }
            public int IsCreditBranch { get; set; }
            public int IsDebitBranch { get; set; }
            public int TypeID { get; set; }
            public int ReasonID { get; set; }
            public int MethodID { get; set; }
            public int IsProduct { get; set; }
        }
    }
}
