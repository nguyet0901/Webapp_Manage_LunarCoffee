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
using Microsoft.AspNetCore.Http.Extensions;

namespace MLunarCoffee.Pages.Customer.Anamnesis
{

    public class CustomerAnamnesisDetailModel : PageModel
    {
        public string _listRegimen { get; set; }
        public string _dataRegimenDetail { get; set; }
        public string _customerID { get; set; }
        public string _patientHistoryID { get; set; }
        public string _dataPatientHistory { get; set; }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostLoadInitialize(int CurrentID,int CustomerID)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ////load combo
                //using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
                //    dt =await  confunc.ExecuteDataTable ("[YYY_sp_Anamnesis_LoadCombo]", CommandType.StoredProcedure);
                //    dt.TableName = "cbbAnamnesis";
                //    ds.Tables.Add (dt);
                //}

                //Load data detail

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_GetForm]", CommandType.StoredProcedure
                         , "@TYPE", SqlDbType.NVarChar, "prehistory");
                    dt.TableName = "Template";
                    ds.Tables.Add(dt);
                }
                //using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //{
                //    dt = await confunc.ExecuteDataTable("[YYY_sp_print_v2_TreatmentRecords]" ,CommandType.StoredProcedure
                //         ,"@id" ,SqlDbType.Int ,CustomerID);
                //    dt.TableName = "QuestionField";
                //    ds.Tables.Add(dt);
                //}
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Anamnesis_LoadDetail]", CommandType.StoredProcedure,
                          "@ID", SqlDbType.Int, Convert.ToInt32(CurrentID),
                          "@UserId", SqlDbType.Int, user.sys_userid,
                          "@EditCustomerPassingDate", SqlDbType.Int, user.sys_EditCustomerPassingDate);
                    dt.TableName = "AnamnesisDetail";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        
        public async Task<IActionResult> OnPostCheckStogeSign(string CustomerID ,string StogeRule)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_PrintForm_StogeRule_Check]" ,CommandType.StoredProcedure ,
                      "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID)
                      ,"@StogeRule" ,SqlDbType.Int ,Convert.ToInt32(StogeRule)
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostLoadCommand(int commandid ,int id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_print_v2_ExeCommand]" ,CommandType.StoredProcedure ,
                      "@commandid" ,SqlDbType.Int ,Convert.ToInt32(commandid)
                      ,"@id" ,SqlDbType.Int ,Convert.ToInt32(id)
                      ,"@idstring" ,SqlDbType.NVarChar ,"");
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

        public async Task<IActionResult> OnPostSaveStogeSign(string CustomerID ,string StogeSign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataSet("[YYY_sp_PrintForm_StogeRule_Insert]" ,CommandType.StoredProcedure ,
                      "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID)
                      ,"@StogeRule" ,SqlDbType.Int ,Convert.ToInt32(StogeSign)
                      ,"@CreatedBy" ,SqlDbType.Int ,user.sys_userid
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostExcuteDataPatienHis(string data, string CustomerID, string CurrentID,string TemplateID)
        {
            try
            {

                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Customer_Anamnesis_Insert]", CommandType.StoredProcedure,
                           "@CustomerID", SqlDbType.Int, CustomerID,
                           "@Data", SqlDbType.NVarChar, data.Replace("'", "").Trim(),
                           "@Created_By", SqlDbType.Int, user.sys_userid,
                           "@TemplateID" , SqlDbType.Int,TemplateID
                       );

                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("[YYY_sp_Customer_Anamnesis_Update]", CommandType.StoredProcedure,
                            "@Data", SqlDbType.NVarChar, data.Replace("'", "").Trim(),
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@TemplateID" ,SqlDbType.Int ,TemplateID
                        );
                    }
                }
                return Content("1");

            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
    }

}
