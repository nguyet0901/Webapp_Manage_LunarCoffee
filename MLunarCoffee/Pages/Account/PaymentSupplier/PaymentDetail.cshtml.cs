using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Account.PaymentSupplier
{
    public class PaymentDetail : PageModel
    {
        public string _ChooseDateCustomer { get; set; }
        public string DetailID { get; set; }
        public string SupID { get; set; }
        public void OnGet()
        {
            var user = Session.GetUserSession(HttpContext);
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            DetailID = Request.Query["DetailID"].ToString();
            SupID = Request.Query["SupID"].ToString();
        }

        public async Task<IActionResult> OnPostLoadInitialize()
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
                        dt = await confunc.LoadPara("Method");
                        dt.TableName = "Method";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
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
                        dt = await confunc.ExecuteDataTable("[MLU_sp_v2_WareHouse_LoadCombo]" ,CommandType.StoredProcedure
                      ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                      ,"@LoadNormal" ,SqlDbType.Int ,1);
                        dt.TableName = "Warehouse";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dtMed = new DataTable();
                        dtMed = await confunc.ExecuteDataTable("[MLU_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                        dtMed.TableName = "MethodType";
                        return dtMed;
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

        public async Task<IActionResult> OnPostLoadGeneral(int supid ,int currentid ,string wareid ,string wareToken)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Account_SupplierFormGeneral]" ,CommandType.StoredProcedure
                        ,"@SupID" ,SqlDbType.Int ,Convert.ToInt32(supid)
                        ,"@Current" ,SqlDbType.Int ,Convert.ToInt32(currentid == 0 ? 0 : currentid)
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate
                        ,"@wareid" ,SqlDbType.Int ,Convert.ToInt32(wareid)
                        ,"@wareToken" ,SqlDbType.NVarChar ,wareToken != null ? wareToken : "");
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

        public async Task<IActionResult> OnPostLoadToPay(int supid ,int beginid ,string wareid ,string wareToken)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Account_SupplierFormToPay]" ,CommandType.StoredProcedure
                        ,"@SupID" ,SqlDbType.Int ,Convert.ToInt32(supid)
                        ,"@beginID" ,SqlDbType.Int ,Convert.ToInt32(beginid)
                        ,"@wareid" ,SqlDbType.Int ,Convert.ToInt32(wareid)
                        ,"@wareToken" ,SqlDbType.NVarChar ,wareToken != null ? wareToken : ""
                     );

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
            catch
            {
                return Content("");
            }

        }

        public async Task<IActionResult> OnPostLoadToUpdate(int detailid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_Account_SupplierFormView]" ,CommandType.StoredProcedure
                       ,"@detailid" ,SqlDbType.Int ,Convert.ToInt32(detailid));
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

        public async Task<IActionResult> OnPostExecuteData(string CurrentID ,string data ,string supid
            ,string method ,string TransferTypeID ,string PosTypeID ,string MedthodDetail
            ,string dateCreated ,string content ,string amount ,string sign ,int branchID ,string datadeposit)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                DataTable DataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable DataDetailDeposit = JsonConvert.DeserializeObject<DataTable>(datadeposit);

                DataTable Data = new DataTable();
                Data.Columns.Add("ID" ,typeof(Int32));
                Data.Columns.Add("Amount" ,typeof(Decimal));
                for (int i = 0; i < DataDetail.Rows.Count; i++)
                {
                    DataRow dr = Data.NewRow();
                    dr["ID"] = Convert.ToInt32(DataDetail.Rows[i]["ID"].ToString());
                    dr["Amount"] = Convert.ToDecimal(DataDetail.Rows[i]["Amount"].ToString());
                    Data.Rows.Add(dr);
                }

                DataTable DataDeposit = new DataTable();
                DataDeposit.Columns.Add("ID" ,typeof(Int32));
                DataDeposit.Columns.Add("RecieptID" ,typeof(Int32));
                DataDeposit.Columns.Add("Amount" ,typeof(Decimal));
                for (int i = 0; i < DataDetailDeposit.Rows.Count; i++)
                {
                    DataRow dr = DataDeposit.NewRow();
                    dr["ID"] = Convert.ToInt32(DataDetailDeposit.Rows[i]["ID"].ToString());
                    dr["RecieptID"] = Convert.ToInt32(DataDetailDeposit.Rows[i]["RecieptID"].ToString());
                    dr["Amount"] = Convert.ToDecimal(DataDetailDeposit.Rows[i]["Amount"].ToString());
                    DataDeposit.Rows.Add(dr);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    if (CurrentID == "0")
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Account_SupplierPay" ,CommandType.StoredProcedure ,
                              "@Supid" ,SqlDbType.Int ,supid ,
                              "@PaymentMethod_ID" ,SqlDbType.Int ,method ,
                              "@TransferType" ,SqlDbType.Int ,TransferTypeID ,
                              "@PosType" ,SqlDbType.Int ,PosTypeID ,
                              "@MedthodDetail" ,SqlDbType.Int ,MedthodDetail ,
                              "@Branch_ID" ,SqlDbType.Int ,branchID != 0 ? branchID : user.sys_branchID ,
                              "@Content" ,SqlDbType.NVarChar ,content != null ? content.Replace("'" ,"") : "" ,
                              "@Amount" ,SqlDbType.Decimal ,Convert.ToDecimal(amount != null ? amount : 0) ,
                              "@Sign" ,SqlDbType.NVarChar ,sign != null ? sign : "" ,
                              "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                              "@ChooseDateCustomer" ,SqlDbType.Int ,user.sys_ChooseDateCustomer.ToString() ,
                              "@dateCreated" ,SqlDbType.NVarChar ,Comon.Comon.DateTimeDMY_To_YMDHHMM(dateCreated.ToString()) ,
                              "@data" ,SqlDbType.Structured ,Data.Rows.Count > 0 ? Data : null ,
                              "@dataDep" ,SqlDbType.Structured ,DataDeposit.Rows.Count > 0 ? DataDeposit : null
                          );
                    }
                    else
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Account_SupplierUpdate" ,CommandType.StoredProcedure ,
                             "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                              "@Content" ,SqlDbType.NVarChar ,content != null ? content.Replace("'" ,"") : "" ,
                              "@Sign" ,SqlDbType.NVarChar ,sign != null ? sign : "" ,
                              "@Created_By" ,SqlDbType.Int ,user.sys_userid
                          );
                    }
                }

                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

        public async Task<IActionResult> OnPostDelete(int detailid)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Account_SupplierDelete]" ,CommandType.StoredProcedure
                       ,"@ModifiedBy" ,SqlDbType.Int ,user.sys_userid
                       ,"@detailid" ,SqlDbType.Int ,Convert.ToInt32(detailid));
                }
                if (dt != null)
                {
                    return Content(Comon.DataJson.GetFirstValue(dt));
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

    }

}
