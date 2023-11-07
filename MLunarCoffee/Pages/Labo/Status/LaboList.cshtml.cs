using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Labo.Status
{
    public class LaboListModel : PageModel
    {
        public int _LaboID { get; set; }
        public string layout { get; set; }

        public void OnGet()
        {
            string laboid = Request.Query["detail"];
            _LaboID = Convert.ToInt32((laboid != null ? laboid : "0"));
            string _layout = Request.Query["layout"];
            if (_layout != null)
            {
                layout = _layout;
            }
            else layout = "";

        }

        public async Task<IActionResult> OnPostInitialize()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load" ,CommandType.StoredProcedure
                          ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                          ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dt.TableName = "Branch";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_LoadCombo_LaboEmp" ,CommandType.StoredProcedure);
                        dt.TableName = "LaboEmp";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_LoadCombo_Task" ,CommandType.StoredProcedure);
                        dt.TableName = "LaboTask";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_Supplier_LoadCombo]" ,CommandType.StoredProcedure);
                        dt.TableName = "Supplier";
                        //DataRow dr = dt.NewRow();
                        //dr[0] = 0;
                        //dr[1] = "All Supplier";
                        //dt.Rows.InsertAt(dr ,0);
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_Labo_Status_LoadCombo" ,CommandType.StoredProcedure);
                        dt.TableName = "AllStatus";
                        DataRow dr = dt.NewRow();
                        dr[0] = 0;
                        dr[1] = "All Status";
                        dt.Rows.InsertAt(dr ,0);
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
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadData(string CurrentID ,string BranchID ,int limit
            ,string idbegin ,string textsearch ,string txt_searchroot ,string DateFrom ,string DateTo ,int LaboSupID ,int LaboStatusID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_LoadList_ByUser]" ,CommandType.StoredProcedure
                        ,"@limit" ,SqlDbType.Int ,limit
                        ,"@idbegin" ,SqlDbType.Int ,idbegin
                        ,"@BranchID" ,SqlDbType.Int ,BranchID
                        ,"@SupID" ,SqlDbType.Int ,LaboSupID
                        ,"@LaboStatusID" ,SqlDbType.Int ,LaboStatusID
                        ,"@DetailID" ,SqlDbType.Int ,CurrentID
                        ,"@DateFromNum" ,SqlDbType.Int ,DateFrom
                        ,"@DateToNum" ,SqlDbType.Int ,DateTo
                          ,"@textsearch" ,SqlDbType.NVarChar ,textsearch != null ? textsearch : ""
                          ,"@textroot" ,SqlDbType.NVarChar ,txt_searchroot != null ? txt_searchroot : ""
                        );

                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }


    }
}
