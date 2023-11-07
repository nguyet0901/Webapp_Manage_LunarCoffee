using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using Microsoft.AspNetCore.Http;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.SyntaxCode;

namespace MLunarCoffee.Pages.Marketing.TicketFile
{
    public class TicketFile_File_ConvertCustomerModel : PageModel
    {
        public string _ImportID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["ImportID"];
            if (curr != null)
            {
                _ImportID = curr.ToString();
            }
            else
            {
                _ImportID = "0";
            }
        }


        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                //using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                //{
                //    DataTable dtSource = new DataTable();
                //    dtSource = await confunc.ExecuteDataTable("[YYY_sp_Customer_Type_ComboLoad]" ,CommandType.StoredProcedure);
                //    dtSource.TableName = "Source";
                //    ds.Tables.Add(dtSource);
                //}
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dtBranch = new DataTable();
                    dtBranch = await confunc.ExecuteDataTable("YYY_sp_Branch_Load_Change" ,CommandType.StoredProcedure
                        ,"@isAllBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                        ,"@currentBranch" ,SqlDbType.Int ,user.sys_AllBranchID
                        ,"@areaBranch" ,SqlDbType.NVarChar ,user.sys_AreaBranch);
                    dtBranch.TableName = "Branch";
                    ds.Tables.Add(dtBranch);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostLoadDetail(string ImportID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Ticket_File_ConvertCust_LoadDetail]" ,CommandType.StoredProcedure
                      ,"@ImportID" ,SqlDbType.Int ,Convert.ToInt32(ImportID));
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExecute(string data, string branchid, string allowduplicate)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                //GlobalUser _globaluser = (GlobalUser)Session.GetUserSession(HttpContext);
                //string customercode = !String.IsNullOrEmpty(_globaluser.syntax_customercode) ? SyntaxCode.GetSyntax(HttpContext ,_globaluser.syntax_customercode) : "";
                //string documentcode = !String.IsNullOrEmpty(_globaluser.syntax_documentcode) ? SyntaxCode.GetSyntax(HttpContext ,_globaluser.syntax_documentcode) : "";

                DataTable dt = new DataTable();
                DataTable dataMain = new DataTable();
                dataMain = JsonConvert.DeserializeObject<DataTable>(data);

                DataTable TableTicket = new DataTable();
                TableTicket.Columns.Add("TicketID" ,typeof(int));

                for (int i = 0; i < dataMain.Rows.Count; i++)
                {
                    DataRow drMain = dataMain.Rows[i]; 
                    DataRow drTicket = TableTicket.NewRow();
                    drTicket["TicketID"] = Convert.ToInt32(drMain["ID"].ToString());
                    TableTicket.Rows.Add(drTicket);
                }
               
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Ticket_File_ConvertCust_Insert]" ,CommandType.StoredProcedure
                      ,"@CreatedBy" ,SqlDbType.Int ,user.sys_userid
                      ,"@CustCode" ,SqlDbType.NVarChar ,"" //customercode
                      ,"@DocCode" ,SqlDbType.NVarChar ,"" //documentcode
                      ,"@BranchID" ,SqlDbType.Int ,Convert.ToInt32(branchid)
                      ,"@IsAllowDuplicate" ,SqlDbType.Int ,Convert.ToInt32(allowduplicate)
                      ,"@DataTable" ,SqlDbType.Structured ,TableTicket);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

    }
}
