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

    public partial class LaboCustomerListModel : PageModel
    {
        public string _dtPermissionCurrentPage { get; set; }
        public void OnGet()
        {

            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostInitialize()
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
                        dt = await confunc.ExecuteDataTable("YYY_sp_LaboSupplier_Load" ,CommandType.StoredProcedure);
                        dt.TableName = "Supplier";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
               {
                   using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                   {
                       DataTable dt = new DataTable();
                       dt = await confunc.ExecuteDataTable("YYY_sp_LaboColor" ,CommandType.StoredProcedure);
                       dt.TableName = "Color";
                       return dt;
                   }
               }));
                tasks.Add(Task.Run(async () =>
               {
                   using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                   {
                       DataTable dt = new DataTable();
                       dt = await confunc.ExecuteDataTable("YYY_sp_LaboDimension" ,CommandType.StoredProcedure);
                       dt.TableName = "Dimesion";
                       return dt;
                   }
               }));
                tasks.Add(Task.Run(async () =>
               {
                   using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                   {
                       DataTable dt = new DataTable();
                       dt = await confunc.ExecuteDataTable("YYY_sp_LaboMaterial" ,CommandType.StoredProcedure);
                       dt.TableName = "Material";
                       return dt;
                   }
               }));
                tasks.Add(Task.Run(async () =>
              {
                  using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                  {
                      DataTable dt = new DataTable();
                      dt = await confunc.ExecuteDataTable("YYY_sp_LaboTeethLayout" ,CommandType.StoredProcedure);
                      dt.TableName = "Layout";
                      return dt;
                  }
              }));
                tasks.Add(Task.Run(async () =>
               {
                   using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                   {
                       DataTable dt = new DataTable();
                       dt = await confunc.ExecuteDataTable("YYY_sp_LaboProperties" ,CommandType.StoredProcedure);
                       dt.TableName = "Properties";
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
            catch
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadataLaboCusList(string CustomerID ,string LaboID ,int limit ,int beginID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_Customer_LoadList]" ,CommandType.StoredProcedure ,
                       "@Customer_ID" ,SqlDbType.Int ,CustomerID
                      ,"@LaboID" ,SqlDbType.Int ,LaboID
                      ,"@limit" ,SqlDbType.Int ,limit
                      ,"@beginID" ,SqlDbType.Int ,beginID
                      ,"@IsTiny" ,SqlDbType.Int ,0
                      ,"@UserId" ,SqlDbType.Int ,user.sys_userid ,
                      "@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate
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
