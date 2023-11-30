using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Report
{
    public class ReportGeneralModel : PageModel
    {
        public int _AllowPermission { get; set; }
        public string _AllowGroup { get; set; }
        public string layout { get; set; }
        public void OnGet()
        {
            string _layout = Request.Query["layout"];
            layout = (_layout != null) ? _layout : "";
            _AllowPermission = Comon.Global.sys_DB_AllowPermission;
            _AllowGroup = Comon.Global.sys_groupreport;
        }

        public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                var user = Session.GetUserSession(HttpContext);
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtBranch = new DataTable();
                        dtBranch = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                            ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dtBranch.TableName = "Branch";
                        return dtBranch;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtWarehouse = new DataTable();
                        dtWarehouse = await confunc.ExecuteDataTable("MLU_sp_v2_WareHouse_LoadCombo" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dtWarehouse.TableName = "Warehouse";
                        return dtWarehouse;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtServiceCare = new DataTable();
                        dtServiceCare = await confunc.ExecuteDataTable("[MLU_sp_Customer_ServiceCare_ReportFilter_ComboLoad]" ,CommandType.StoredProcedure);
                        dtServiceCare.TableName = "ServiceCare";
                        return dtServiceCare;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSource = new DataTable();
                        dtSource = await confunc.ExecuteDataTable("[MLU_sp_Customer_Type_ReportFilter_ComboLoad]" ,CommandType.StoredProcedure);
                        dtSource.TableName = "Source";
                        return dtSource;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtSourceCat = new DataTable();
                        dtSourceCat = await confunc.ExecuteDataTable("[MLU_sp_Customer_TypeCat_ComboLoad]" ,CommandType.StoredProcedure);
                        dtSourceCat.TableName = "SourceCat";
                        return dtSourceCat;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGroupCustomer = new DataTable();
                        dtGroupCustomer = await confunc.ExecuteDataTable("[MLU_sp_Customer_Group_Load]" ,CommandType.StoredProcedure);
                        dtGroupCustomer.TableName = "GroupCustomer";
                        return dtGroupCustomer;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtServiceCat = new DataTable();
                        dtServiceCat = await confunc.ExecuteDataTable("[MLU_sp_ServiceCate_LoadList]" ,CommandType.StoredProcedure);
                        dtServiceCat.TableName = "ServiceCat";
                        return dtServiceCat;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtServiceSource = new DataTable();
                        dtServiceSource = await confunc.ExecuteDataTable("[YYY_LoadCombo_Source_CustService]" ,CommandType.StoredProcedure);
                        dtServiceSource.TableName = "ServiceSource";
                        return dtServiceSource;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtService = new DataTable();
                        dtService = await confunc.ExecuteDataTable("[MLU_sp_Service_LoadList]" ,CommandType.StoredProcedure);
                        dtService.TableName = "Service";
                        return dtService;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtCard = new DataTable();
                        dtCard = await confunc.ExecuteDataTable("[MLU_sp_Card_LoadList]" ,CommandType.StoredProcedure);
                        dtCard.TableName = "Card";
                        return dtCard;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMedicine = new DataTable();
                        dtMedicine = await confunc.ExecuteDataTable("[MLU_sp_Medicine_LoadList]" ,CommandType.StoredProcedure);
                        dtMedicine.TableName = "Medicine";
                        return dtMedicine;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtStaff = new DataTable();
                        dtStaff = await confunc.ExecuteDataTable("[MLU_sp_Tele_LoadCombo]" ,CommandType.StoredProcedure
                                 ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                                 ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dtStaff.TableName = "Staff";
                        return dtStaff;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtGroupStaff = new DataTable();
                        dtGroupStaff = await confunc.ExecuteDataTable("[MLU_sp_Ticket_LoadGroupTele]" ,CommandType.StoredProcedure
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid);
                        dtGroupStaff.TableName = "GroupStaff";
                        return dtGroupStaff;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtEmployee = new DataTable();
                        dtEmployee = await confunc.ExecuteDataTable("[MLU_sp_rp_Employee]" ,CommandType.StoredProcedure);
                        dtEmployee.TableName = "Employee";
                        return dtEmployee;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtUser = new DataTable();
                        dtUser = await confunc.ExecuteDataTable("[MLU_sp_rp_User]" ,CommandType.StoredProcedure);
                        dtUser.TableName = "User";
                        return dtUser;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtProduct = new DataTable();
                        dtProduct = await confunc.ExecuteDataTable("[MLU_sp_Product_Get]" ,CommandType.StoredProcedure);
                        dtProduct.TableName = "Product";
                        return dtProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtProduct = new DataTable();
                        dtProduct = await confunc.ExecuteDataTable("[MLU_sp_LoadCombo_Para_Method]" ,CommandType.StoredProcedure);
                        dtProduct.TableName = "PaymentMethod";
                        return dtProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtProduct = new DataTable();
                        dtProduct = await confunc.ExecuteDataTable("[MLU_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                        dtProduct.TableName = "PaymentMethodDetail";
                        return dtProduct;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtProduct = new DataTable();
                        dtProduct = await confunc.ExecuteDataTable("[MLU_sp_CustomerCareStatus_LoadCombo_All]" ,CommandType.StoredProcedure);
                        dtProduct.TableName = "CusCareStatus";
                        return dtProduct;
                    }
                }));
                var result = await Task.WhenAll(tasks.ToList()).ConfigureAwait(false);
                foreach (var r in result)
                {
                    ds.Tables.Add(r);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception e)
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostLoadataReport()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Report_GetData]" ,CommandType.StoredProcedure
                           ,"@GroupID" ,SqlDbType.Int ,user.sys_RoleID);
                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("[]");
                }
            }
            catch (Exception)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostLoadDescription()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Report_Load_Description]" ,CommandType.StoredProcedure);
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
