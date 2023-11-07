using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Marketing.TicketFile
{
    public class TicketFile_ImportModel : PageModel
    {
        public int _sys_MaxImportTicket { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            _sys_MaxImportTicket = Comon.Global.sys_MaxImportTicket;
            CurrentPath = HttpContext.Request.Path;
        }

        public async Task<IActionResult> OnPostInitializeCombo()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Type_ComboLoad]" ,CommandType.StoredProcedure);
                    dt.TableName = "Source";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_EmployeeMarketing_LoadCombo]" ,CommandType.StoredProcedure);
                    dt.TableName = "Employee";
                    ds.Tables.Add(dt);
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
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostGetTeamplate(string type)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Import_GetTemplate]" ,CommandType.StoredProcedure ,
                        "@type" ,SqlDbType.NVarChar ,@type
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string sourceID ,string fileName ,int userMar)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Import_Info_Insert]" ,CommandType.StoredProcedure ,
                        "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                        "@Source" ,SqlDbType.Int ,Convert.ToInt32(sourceID) ,
                        "@UserMar" ,SqlDbType.Int ,Convert.ToInt32(userMar) ,
                        "@Name" ,SqlDbType.NVarChar ,fileName.Replace("'" ,"").Trim()
                    );
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteDataItem(string data ,string source ,string importID ,int userMar, int isDuplicate)
        {
            try
            {
                DataTable DataImportDetail = new DataTable();
                DataImportDetail = JsonConvert.DeserializeObject<DataTable>(data);
                //DataImportDetail = DataImportDetail.Select("Status = ''").CopyToDataTable();
                var user = Session.GetUserSession(HttpContext);

                DataTable dt = new DataTable();
                dt.Columns.Add("Name" ,typeof(string));
                dt.Columns.Add("NameNoSign" ,typeof(string));
                dt.Columns.Add("Phone" ,typeof(string));
                dt.Columns.Add("Phone2" ,typeof(string));
                dt.Columns.Add("Facebook" ,typeof(string));
                dt.Columns.Add("ServiceCare" ,typeof(string));
                dt.Columns.Add("Area" ,typeof(string));
                dt.Columns.Add("TimeSlot" ,typeof(string));
                dt.Columns.Add("Symbol" ,typeof(string));
                dt.Columns.Add("Campaign" ,typeof(string));
                dt.Columns.Add("ADS" ,typeof(string));
                dt.Columns.Add("Note" ,typeof(string));
                dt.Columns.Add("Created" ,typeof(string));
                dt.Columns.Add("Email" ,typeof(string));
                dt.Columns.Add("Address" ,typeof(string));
                dt.Columns.Add("Birthday" ,typeof(string));
                dt.Columns.Add("GenderID" ,typeof(string));
                dt.Columns.Add("Gclid" ,typeof(string));
                dt.Columns.Add("TempCustCodeOld" ,typeof(string));
                dt.Columns.Add("TempIDIssuenum" ,typeof(string));
                dt.Columns.Add("TempIDIssueplace" ,typeof(string));
                dt.Columns.Add("TempIDIssuedate" ,typeof(string));

                for (int i = 0; i < DataImportDetail.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Name"] = DataImportDetail.Rows[i]["Name"].ToString();
                    dr["NameNoSign"] = DataImportDetail.Columns.Contains("NameNoSign") ? DataImportDetail.Rows[i]["NameNoSign"].ToString() : "";
                    dr["Phone"] = DataImportDetail.Rows[i]["Phone"].ToString();
                    dr["Phone2"] = DataImportDetail.Columns.Contains("Phone2") ? DataImportDetail.Rows[i]["Phone2"].ToString() : "";
                    dr["Facebook"] = DataImportDetail.Columns.Contains("Facebook") ? DataImportDetail.Rows[i]["Facebook"].ToString() : "";
                    dr["ServiceCare"] = DataImportDetail.Columns.Contains("ServiceCare") ? DataImportDetail.Rows[i]["ServiceCare"].ToString() : "";
                    dr["Area"] = DataImportDetail.Columns.Contains("Area") ? DataImportDetail.Rows[i]["Area"].ToString() : "";
                    dr["TimeSlot"] = DataImportDetail.Columns.Contains("TimeSlot") ? DataImportDetail.Rows[i]["TimeSlot"].ToString() : "";
                    dr["Symbol"] = DataImportDetail.Columns.Contains("Symbol") ? DataImportDetail.Rows[i]["Symbol"].ToString() : "";
                    dr["Campaign"] = DataImportDetail.Columns.Contains("Campaign") ? DataImportDetail.Rows[i]["Campaign"].ToString() : "";
                    dr["ADS"] = DataImportDetail.Columns.Contains("ADS") ? DataImportDetail.Rows[i]["ADS"].ToString() : "";
                    dr["Note"] = DataImportDetail.Columns.Contains("Note") ? DataImportDetail.Rows[i]["Note"].ToString() : "";
                    dr["Created"] = DataImportDetail.Columns.Contains("Created") ? DataImportDetail.Rows[i]["Created"].ToString() : "";
                    dr["Email"] = DataImportDetail.Columns.Contains("Email") ? DataImportDetail.Rows[i]["Email"].ToString() : "";
                    dr["Address"] = DataImportDetail.Columns.Contains("Address") ? DataImportDetail.Rows[i]["Address"].ToString() : "";
                    dr["Birthday"] = DataImportDetail.Columns.Contains("Birthday") ? DataImportDetail.Rows[i]["Birthday"].ToString() : "";
                    dr["GenderID"] = DataImportDetail.Rows[i]["GenderID"].ToString();
                    dr["Gclid"] = DataImportDetail.Columns.Contains("Gclid") ? DataImportDetail.Rows[i]["Gclid"].ToString() : "";
                    dr["TempCustCodeOld"] = DataImportDetail.Columns.Contains("CustCodeOld") ? DataImportDetail.Rows[i]["CustCodeOld"].ToString() : "";
                    dr["TempIDIssuenum"] = DataImportDetail.Columns.Contains("IDIssuenum") ? DataImportDetail.Rows[i]["IDIssuenum"].ToString() : "";
                    dr["TempIDIssueplace"] = DataImportDetail.Columns.Contains("IDIssueplace") ? DataImportDetail.Rows[i]["IDIssueplace"].ToString() : "";
                    dr["TempIDIssuedate"] = DataImportDetail.Columns.Contains("IDIssuedate") ? DataImportDetail.Rows[i]["IDIssuedate"].ToString() : "";
                    dt.Rows.Add(dr);
                }

                DataTable dtResult = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dtResult = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Import_Insert]" ,CommandType.StoredProcedure ,
                        "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                        "@ImportID" ,SqlDbType.Int ,Convert.ToInt32(importID) ,
                        "@Source" ,SqlDbType.Int ,Convert.ToInt32(source) ,
                        "@UserMar" ,SqlDbType.Int ,userMar ,
                        "@IsAllowDuplicate" ,SqlDbType.Int ,isDuplicate ,
                        "@table_data" ,SqlDbType.Structured ,dt.Rows.Count > 0 ? dt : null
                    );
                }
                return Content(Comon.DataJson.Datatable(dtResult));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
