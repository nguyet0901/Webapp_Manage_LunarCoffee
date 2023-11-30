using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer
{

    public class LaboCusDetailModel : PageModel
    {
        public string _CustomerID { get; set; }
        public string _CurrentLaboID { get; set; }
        public string _EmplyeeeID { get; set; }
        public string _dataLaboCus { get; set; }
        public string _typeHistory { get; set; }
        public string _dtPermissionCurrentPage { get; set; }

        private readonly IHubContext<NotiHub> hubContext;
        public LaboCusDetailModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _EmplyeeeID = user.sys_employeeid.ToString();
            var cus = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];
            if (cus.ToString() != String.Empty)
            {
                _CustomerID = cus.ToString();
                if (curr.ToString() != String.Empty)
                {
                    _CurrentLaboID = curr.ToString();
                }
                else
                {
                    _CurrentLaboID = "0";
                }
            }
            else
            {
                _CustomerID = null;
                Response.Redirect("/assests/Error/index.html");
            }

            _dtPermissionCurrentPage = HttpContext.Request.Path;

        }


        public async Task<IActionResult> OnPostLoadInitialize(string CustomerID, string CurrentID)
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
                        dt = await confunc.LoadEmployee(14, user.sys_branchID, user.sys_AllBranchID);
                        dt.TableName = "Doctor";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Labo_Service_Supplier]", CommandType.StoredProcedure);
                        dt.TableName = "ServiceSupplier";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Labo_Combo_Supplier]", CommandType.StoredProcedure);
                        dt.TableName = "Supplier";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Labo_LoadComboService]", CommandType.StoredProcedure,
                            "@Customer_ID", SqlDbType.Int, CustomerID);
                        dt.TableName = "Service";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_TabService_Load_Teeth]", CommandType.StoredProcedure);
                        dt.TableName = "Teeth";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_LaboTemplate_LoadCombo]", CommandType.StoredProcedure);
                        dt.TableName = "Template";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_LaboWarantly_LoadCombo]", CommandType.StoredProcedure);
                        dt.TableName = "Warantly";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Labo_Customer_LoadDetail]", CommandType.StoredProcedure
                            ,"@ID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                            ,"@UserId", SqlDbType.Int, user.sys_userid
                            ,"@EditCustomerPassingDate", SqlDbType.Int, Convert.ToInt32(user.sys_EditCustomerPassingDate)
                            );
                        dt.TableName = "CurrentDetail";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
               {
                   using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                   {
                       DataTable dt = new DataTable();
                       dt = await confunc.ExecuteDataTable("[MLU_sp_Labo_Customer_LoadProperties]", CommandType.StoredProcedure,
                         "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID));
                       dt.TableName = "PropertiesDetail";
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
        public async Task<IActionResult> OnPostLoadDataValue(int commandid, int currentID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_print_v2_ExeCommand]", CommandType.StoredProcedure,
                      "@commandid", SqlDbType.Int, Convert.ToInt32(commandid)
                      , "@id", SqlDbType.Int, Convert.ToInt32(currentID)
                      , "@idstring", SqlDbType.NVarChar, "");
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
            catch (Exception ex)
            {
                return Content("");
            }
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string properties, string CurrentID, string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                LaboCusDetail DataMain = JsonConvert.DeserializeObject<LaboCusDetail>(data);
                DataTable dtProperty = new DataTable();
                dtProperty.Columns.Add("LaboProID" , typeof(Int32));
                dtProperty.Columns.Add("Teeth", typeof(Int32));
                dtProperty.Columns.Add("Color", typeof(Int32));
                dtProperty.Columns.Add("Dimension", typeof(Int32));
                dtProperty.Columns.Add("Properties", typeof(Int32));
                dtProperty.Columns.Add("Material", typeof(Int32));
                dtProperty.Columns.Add("UnitPrice", typeof(Decimal));
                dtProperty.Columns.Add("Content", typeof(string));
                DataTable iProperty = JsonConvert.DeserializeObject<DataTable>(properties);
                for (int j = 0; j < iProperty.Rows.Count; j++)
                {
                    if (iProperty.Rows[j]["Selected"].ToString() == "1")
                    {
                        DataRow dr = dtProperty.NewRow();
                        dr["LaboProID"] = Convert.ToInt32(iProperty.Rows[j]["LaboProID"]);
                        dr["Teeth"] = Convert.ToInt32(iProperty.Rows[j]["Teeth"]);
                        dr["Color"] = Convert.ToInt32(iProperty.Rows[j]["Color"]);
                        dr["Dimension"] = Convert.ToInt32(iProperty.Rows[j]["Dimension"]);
                        dr["Properties"] = Convert.ToInt32(iProperty.Rows[j]["Properties"]);
                        dr["Material"] = Convert.ToInt32(iProperty.Rows[j]["Material"]);
                        dr["UnitPrice"] = Convert.ToDecimal(iProperty.Rows[j]["UnitPrice"]);
                        dr["Content"] = iProperty.Rows[j]["Content"].ToString();
                        dtProperty.Rows.Add(dr);
                    }
                }
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Customer_Insert]", CommandType.StoredProcedure,
                            "@Customer_ID", SqlDbType.Int, CustomerID,
                            "@tabID", SqlDbType.Int, DataMain.TabID,
                            "@branch_ID", SqlDbType.Int, user.sys_branchID,
                            "@Doctor_ID", SqlDbType.Int, DataMain.IDDoctor,
                            "@LaboLayout", SqlDbType.Int, DataMain.LaboLayout,
                            "@IDWarantly", SqlDbType.Int, DataMain.IDWarantly,
                            "@UserTo", SqlDbType.Int, DataMain.IDDeliver,
                            "@Receipt_Date", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Receipt_Date),
                            "@Price", SqlDbType.Decimal, Convert.ToDecimal(DataMain.Price),
                            "@DiscountAmount", SqlDbType.Decimal, Convert.ToDecimal(DataMain.DiscountAmount),
                            "@DiscountPercent", SqlDbType.Decimal, Convert.ToDecimal(DataMain.DiscountPercent),
                            "@PriceRoot", SqlDbType.Decimal, Convert.ToDecimal(DataMain.PriceRoot),
                            "@UnitPrice", SqlDbType.Decimal, Convert.ToDecimal(DataMain.UnitPrice),
                            "@Quantity", SqlDbType.Int, Convert.ToDecimal(DataMain.Quantity),
                            "@Teeth", SqlDbType.NVarChar, DataMain.TeethChoosing,
                            "@SupID", SqlDbType.Int, DataMain.IDSupplier,
                            "@LaboServiceID", SqlDbType.Int, DataMain.LaboServiceID,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@DataTemplate", SqlDbType.NVarChar, DataMain.DataTemplate.Replace("'", "").Trim(),
                            "@table_data", SqlDbType.Structured, (dtProperty.Rows.Count > 0) ? dtProperty : null
                        );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Customer_Update]", CommandType.StoredProcedure,
                            "@tabID", SqlDbType.Int, DataMain.TabID,
                            "@Doctor_ID", SqlDbType.Int, DataMain.IDDoctor,
                            "@LaboLayout", SqlDbType.Int, DataMain.LaboLayout,
                            "@IDWarantly", SqlDbType.Int, DataMain.IDWarantly,
                            "@UserTo", SqlDbType.Int, DataMain.IDDeliver,
                            "@Receipt_Date", SqlDbType.NVarChar, Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Receipt_Date),
                            "@Price", SqlDbType.Decimal, Convert.ToDecimal(DataMain.Price),
                            "@DiscountAmount", SqlDbType.Decimal, Convert.ToDecimal(DataMain.DiscountAmount),
                            "@DiscountPercent", SqlDbType.Decimal, Convert.ToDecimal(DataMain.DiscountPercent),
                            "@PriceRoot", SqlDbType.Decimal, Convert.ToDecimal(DataMain.PriceRoot),
                            "@UnitPrice", SqlDbType.Decimal, Convert.ToDecimal(DataMain.UnitPrice),
                            "@Quantity", SqlDbType.Int, Convert.ToDecimal(DataMain.Quantity),
                            "@Teeth", SqlDbType.NVarChar, DataMain.TeethChoosing,
                            "@SupID", SqlDbType.Int, DataMain.IDSupplier,
                            "@LaboServiceID", SqlDbType.Int, DataMain.LaboServiceID,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@DataTemplate", SqlDbType.NVarChar, DataMain.DataTemplate.Replace("'", "").Trim(),
                            "@table_data", SqlDbType.Structured, (dtProperty.Rows.Count > 0) ? dtProperty : null,
                            "@EditCustomerPassingDate" ,SqlDbType.Int ,Convert.ToInt32(user.sys_EditCustomerPassingDate)
                        );
                    }
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
        public async Task<IActionResult> OnPostUpdate(string data ,string properties ,string CurrentID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                LaboCusDetail DataMain = JsonConvert.DeserializeObject<LaboCusDetail>(data);
                DataTable dtProperty = new DataTable();
                dtProperty.Columns.Add("LaboProID" ,typeof(Int32));
                dtProperty.Columns.Add("Teeth" ,typeof(Int32));
                dtProperty.Columns.Add("Color" ,typeof(Int32));
                dtProperty.Columns.Add("Dimension" ,typeof(Int32));
                dtProperty.Columns.Add("Properties" ,typeof(Int32));
                dtProperty.Columns.Add("Material" ,typeof(Int32));
                dtProperty.Columns.Add("UnitPrice" ,typeof(Decimal));
                dtProperty.Columns.Add("Content" ,typeof(string));
                DataTable iProperty = JsonConvert.DeserializeObject<DataTable>(properties);
                for (int j = 0; j < iProperty.Rows.Count; j++)
                {
                    if (iProperty.Rows[j]["Selected"].ToString() == "1")
                    {
                        DataRow dr = dtProperty.NewRow();
                        dr["LaboProID"] = Convert.ToInt32(iProperty.Rows[j]["LaboProID"]);
                        dr["Teeth"] = Convert.ToInt32(iProperty.Rows[j]["Teeth"]);
                        dr["Color"] = Convert.ToInt32(iProperty.Rows[j]["Color"]);
                        dr["Dimension"] = Convert.ToInt32(iProperty.Rows[j]["Dimension"]);
                        dr["Properties"] = Convert.ToInt32(iProperty.Rows[j]["Properties"]);
                        dr["Material"] = Convert.ToInt32(iProperty.Rows[j]["Material"]);
                        dr["UnitPrice"] = Convert.ToDecimal(iProperty.Rows[j]["UnitPrice"]);
                        dr["Content"] = iProperty.Rows[j]["Content"].ToString();
                        dtProperty.Rows.Add(dr);
                    }
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Customer_UpdateValue]" ,CommandType.StoredProcedure
                        ,"@IDWarantly" ,SqlDbType.Int ,DataMain.IDWarantly
                        ,"@Doctor_ID" ,SqlDbType.Int ,DataMain.IDDoctor
                        ,"@LaboLayout" ,SqlDbType.Int ,DataMain.LaboLayout
                        ,"@UserTo" ,SqlDbType.Int ,DataMain.IDDeliver
                        ,"@Receipt_Date" ,SqlDbType.NVarChar ,Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Receipt_Date)
                        ,"@SupID" ,SqlDbType.Int ,DataMain.IDSupplier
                        ,"@Modified_By" ,SqlDbType.Int ,user.sys_userid
                        ,"@Content" ,SqlDbType.NVarChar ,DataMain.Content.Replace("'" ,"").Trim()
                        ,"@DataTemplate" ,SqlDbType.NVarChar ,DataMain.DataTemplate.Replace("'" ,"").Trim()
                        ,"@table_data" ,SqlDbType.Structured ,(dtProperty.Rows.Count > 0) ? dtProperty : null
                        ,"@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID)
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
        public async Task<IActionResult> OnPostDelete(int id)
        {

            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Labo_Customer_Delete]" ,CommandType.StoredProcedure ,
                        "@CurrentID" ,SqlDbType.Int ,id ,
                        "@userID" ,SqlDbType.Int ,user.sys_userid,
                        "@EditCustomerPassingDate" ,SqlDbType.Int ,Convert.ToInt32(user.sys_EditCustomerPassingDate)
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
    public class LaboCusDetail
    {
        public int TabID { get; set; }
        public string Content { get; set; }
        public int IDDoctor { get; set; }
        public int LaboLayout { get; set; }
        public int IDSupplier { get; set; }
        public int LaboServiceID { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal PriceRoot { get; set; }
        public string Receipt_Date { get; set; }

        public string IDWarantly { get; set; }
        public string IDDeliver { get; set; }
        public string Quantity { get; set; }
        public string TeethChoosing { get; set; }
        public string TeethProperties { get; set; }
        public string UnitPrice { get; set; }
        public string DataTemplate { get; set; }
    }

}
