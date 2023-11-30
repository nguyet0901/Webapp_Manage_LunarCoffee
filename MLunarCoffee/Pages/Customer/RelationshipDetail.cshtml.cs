using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer
{

    public class RelationshipDetailModel : PageModel
    {
        public string _CustomerID { get; set; }
        public int _rela1vs1 { get; set; }
        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            _rela1vs1 = Comon.Global.sys_Relationship1v1;
            if (cus.ToString() != String.Empty)
            {
                _CustomerID = cus.ToString();
            }
            else
            {
                _CustomerID = "0";
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostInitialization()
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[MLU_sp_Relationship_LoadCombo]" ,CommandType.StoredProcedure);
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
        public async Task<IActionResult> OnPostLoadata(int custID)
        {
            try
            {

                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                   
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_Relationship_Load]" ,CommandType.StoredProcedure ,
                    "@CustomerID" ,SqlDbType.Int ,custID ,
                    "@descustid" ,SqlDbType.Int ,0);
 

                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
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
        public async Task<IActionResult> OnPostLoadIni()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Relationship_LoadCombo" ,CommandType.StoredProcedure);
                        dt.TableName = "Rela";
                        return dt;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));



            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadataRelationship(int CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Relationship_Load]" ,CommandType.StoredProcedure ,
                        "@CustomerID" ,SqlDbType.Int ,0 ,
                        "@descustid" ,SqlDbType.Int ,CustomerID);
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
            catch (Exception ex)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string custID ,string custselected ,string ralid ,string rela1)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt=await connFunc.ExecuteDataTable("MLU_sp_Customer_Relationship_Update" ,CommandType.StoredProcedure ,
                        "@custID" ,SqlDbType.Int ,custID ,
                        "@custselected" ,SqlDbType.Int ,custselected ,
                        "@ralid" ,SqlDbType.Int ,ralid ,
                         "@rela1" ,SqlDbType.Int ,rela1 ,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                        "@BranchID" ,SqlDbType.Int ,user.sys_branchID
                    );
                    return Content(dt.Rows[0][0].ToString());
                }

               
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteData(string currentid,string custid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("MLU_sp_Customer_Relationship_Delete" ,CommandType.StoredProcedure ,
                        "@currentid" ,SqlDbType.Int ,currentid ,
                         "@custid" ,SqlDbType.Int ,custid ,
                        "@UserID" ,SqlDbType.Int ,user.sys_userid
                    );
                }

                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        
        public async Task<IActionResult> OnPostSearchCustomer(string custID ,string keyword)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Searching_Customer_Relationship]" ,CommandType.StoredProcedure ,
                        "@searchText" ,SqlDbType.NVarChar ,keyword.Replace("'" ,"").Trim() ,
                        "@CustomerID" ,SqlDbType.Int ,custID
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }




    }
}
