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

namespace MLunarCoffee.Pages.Guide
{
    public class FirstGuideSlideModel : PageModel
    {
        public void OnGet()
        {
        }
        //private void InitializeComboType()
        //{
        //    _DataComboReceiptype = "";
        //    _DataComboMethod = "";
        //    _DataBranch = "";
        //    DataTable dt = new DataTable();
        //    var user = Session.GetUserSession(HttpContext);
        //    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //    {
        //        dt = connFunc.LoadPara("ReceiptType");
        //    }
        //    DataTable dt1 = new DataTable();
        //    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //    {
        //        dt1 = connFunc.LoadPara("Method");
        //    }

        //    _DataComboReceiptype = JsonConvert.SerializeObject(dt);
        //    _DataComboMethod = JsonConvert.SerializeObject(dt1);

        //    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //    {
        //        dt = new DataTable();
        //        dt =await  confunc.ExecuteDataTable("MLU_sp_Branch_Load", CommandType.StoredProcedure
        //            , "@UserID", SqlDbType.Int, user.sys_userid
        //            , "@LoadNormal", SqlDbType.Int, 1);
        //        if (dt != null && dt.Rows.Count != 0)
        //            _DataBranch = JsonConvert.SerializeObject(dt);
        //        else _DataBranch = "";
        //    }

        //}
        //private void Loadata(int id)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
        //        {
        //            dt =await  confunc.ExecuteDataTable("[MLU_sp_Account_LoadDetail]", CommandType.StoredProcedure,
        //              "@Current", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
        //        }
        //        _AccountMainDetail = JsonConvert.SerializeObject(dt);
        //    }
        //    catch (Exception)
        //    {
        //        _AccountMainDetail = "";
        //    }


        //}
        //
        // public async Task<IActionResult> OnPostExcuteData(string data,string CurrentID)
        //{

        //    try
        //    {
        //        AccountDetail DataMain = JsonConvert.DeserializeObject<AccountDetail>(data);
        //        var user = Session.GetUserSession(HttpContext);
        //        if (CurrentID == "0")
        //        {
        //            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //            {
        //                var dt =await connFunc.ExecuteDataTable("[MLU_sp_Account_Insert]", CommandType.StoredProcedure,
        //             "@ReceiptsDate", SqlDbType.NVarChar, Comon.Comon.GetDateTimeNow(),
        //             "@ReceiptsType", SqlDbType.Int, DataMain.ReceiptsType,
        //             "@Recipient", SqlDbType.NVarChar, DataMain.Recipient.Replace("'", "").Trim(),
        //             "@ReceiptsContent", SqlDbType.NVarChar, DataMain.ReceiptsContent.Replace("'", "").Trim(),
        //             "@Amount", SqlDbType.Decimal, DataMain.Amount,
        //             "@Transfer_Type", SqlDbType.Int, DataMain.Transfer_Type,
        //              "@Branch_ID", SqlDbType.Int, DataMain.Branch_ID,
        //             "@Created_By", SqlDbType.Int, user.sys_userid,
        //                "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
        //           );
        //            }
        //        }
        //        else
        //        {
        //            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
        //            {
        //                connFunc.ExecuteDataTable("[MLU_sp_Account_Update]", CommandType.StoredProcedure,
        //              "@ReceiptsType", SqlDbType.Int, DataMain.ReceiptsType,
        //              "@Recipient", SqlDbType.NVarChar, DataMain.Recipient.Replace("'", "").Trim(),
        //              "@ReceiptsContent", SqlDbType.NVarChar, DataMain.ReceiptsContent.Replace("'", "").Trim(),
        //              "@Transfer_Type", SqlDbType.Int, DataMain.Transfer_Type,
        //              "@Branch_ID", SqlDbType.Int, DataMain.Branch_ID,
        //              "@amount", SqlDbType.Decimal, DataMain.Amount,
        //              "@Modified_By", SqlDbType.Int, user.sys_userid,
        //                 "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
        //                 "@CurrentID", SqlDbType.Int, CurrentID
        //            );
        //            }
        //        }
        //        return Content("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return Content("0");
        //    }
        //}
    }
}
