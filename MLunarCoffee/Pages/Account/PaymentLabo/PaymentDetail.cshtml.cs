using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Account.PaymentLabo
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
        public async Task<IActionResult> OnPostLoadInitialize(int supid ,int currentid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.LoadPara("Method");
                    dt.TableName = "Method";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_Branch_Load" ,CommandType.StoredProcedure
                        ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                        ,"@LoadNormal" ,SqlDbType.Int ,1);
                    dt.TableName = "Branch";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_MethodPaymentType_LoadCombo_All]" ,CommandType.StoredProcedure);
                    dt.TableName = "MethodType";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Account_LaboFormGeneral]" ,CommandType.StoredProcedure
                         ,"@SupID" ,SqlDbType.Int ,Convert.ToInt32(supid)
                         ,"@Current" ,SqlDbType.Int ,Convert.ToInt32(currentid == 0 ? 0 : currentid)
                         ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                         ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate
                    );
                    dt.TableName = "General";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public async Task<IActionResult> OnPostLoadToPay(int supid ,int beginid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {

                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Account_LaboFormToPay]" ,CommandType.StoredProcedure
                        ,"@SupID" ,SqlDbType.Int ,Convert.ToInt32(supid)
                        ,"@beginID" ,SqlDbType.Int ,Convert.ToInt32(beginid)
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
                    ds = await connFunc.ExecuteDataSet("[MLU_sp_Account_LaboFormView]" ,CommandType.StoredProcedure
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
        public async Task<IActionResult> OnPostExcuteData(string CurrentID ,string data ,string supid
            ,string method ,string TransferTypeID ,string PosTypeID ,string MedthodDetail
            ,string dateCreated ,string content ,string amount ,string sign ,int branchID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                DataTable DataDetail = new DataTable();
                DataDetail = JsonConvert.DeserializeObject<DataTable>(data);

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
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    if (CurrentID == "0")
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Account_LaboPay" ,CommandType.StoredProcedure ,
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
                              "@data" ,SqlDbType.Structured ,Data.Rows.Count > 0 ? Data : null
                          );
                    }
                    else
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_Account_LaboUpdate" ,CommandType.StoredProcedure ,
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
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Account_LaboDelete]" ,CommandType.StoredProcedure
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
