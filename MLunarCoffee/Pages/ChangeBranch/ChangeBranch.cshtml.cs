using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.ChangeBranch
{
    public class ChangeBranchModel : PageModel
    {
        public string _CurrentBranch { get; set; }
        public string _CurrentFloor { get; set; }
        public string _CurrentRoom { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _CurrentBranch = (user.sys_branchID).ToString();
            _CurrentFloor = (user.sys_floorID).ToString();
            _CurrentRoom = (user.sys_roomID).ToString();


        }

        public async Task<IActionResult> OnPostInitializeComboType()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load_Change" ,CommandType.StoredProcedure
                        ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                        ,"@currentBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                        ,"@areaBranch" ,SqlDbType.NVarChar ,user.sys_AreaBranch);

                }
                return Content(Comon.DataJson.Datatable(dt));


            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadRoom(int BranchID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("YYY_sp_Room_Load" ,CommandType.StoredProcedure
                        ,"@Branchid" ,SqlDbType.Int ,BranchID);
                    dt.TableName = "Room";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Floor_Load]" ,CommandType.StoredProcedure
                     ,"@Branchid" ,SqlDbType.Int ,BranchID);
                    dt.TableName = "Floor";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));


            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string branch ,string floorid ,string roomid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                string branhcode = "";
                string branchshortname = "";
                string branchname = "";
                string mcn = "";
                string mcn_doc = "";
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_GetGlobal_GetCurrentBranch" ,CommandType.StoredProcedure
                    ,"@ID" ,SqlDbType.Int ,branch
                    ,"@userid" ,SqlDbType.Int ,user.sys_userid
                    ,"@floorid" ,SqlDbType.Int ,floorid
                    ,"@roomid" ,SqlDbType.Int ,roomid);
                }
                if (ds != null && ds != null)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        DataTable dt = ds.Tables[1];
                        branhcode = dt.Rows[0]["ShortName"].ToString();
                        branchshortname = dt.Rows[0]["ShortName"].ToString();
                        branchname = dt.Rows[0]["BranchName"].ToString();
                        mcn = dt.Rows[0]["mcn"].ToString();
                        mcn_doc = dt.Rows[0]["mcn_doc"].ToString();
                        Comon.Comon.UpdateUserBranch_AtGlobal(HttpContext ,branch ,branhcode ,branchshortname ,branchname ,mcn ,mcn_doc ,floorid ,roomid);
                        return Content("1");
                    }
                    else return Content("0");
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
    }
}
