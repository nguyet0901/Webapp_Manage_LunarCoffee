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

namespace MLunarCoffee.Pages.Marketing
{
    public class CallDetailInputModel : PageModel
    {
        public int CurrentID { get; set; } // HO tro update , chi moi ho tro cho  history trong customer
        public int CustomerID { get; set; }
        public int TikcetID { get; set; }
        public int Type_Care { get; set; }
        public int AppID { get; set; }
        public int MasterID { get; set; }
        public string TreatmentDate { get; set; }
        public int _CurrentEmployeeExe { get; set; }
        public int TreatID { get; set; }
        public int IsEditMaster { get; set; }
        public string CurrentPath { get; set; }
        public int _IsNeedToCallBack { get; set; }
        public void OnGet()
        {
            CurrentPath = HttpContext.Request.Path;
            string cusID = Request.Query["CustomerID"];
            if (cusID != "" && cusID != null)
            {
                string curr = Request.Query["CurrentID"];
                string ticID = Request.Query["TicketID"];
                string typeid = Request.Query["Type"];
                string appid = Request.Query["AppID"];
                string masterid = Request.Query["MasterID"];
                string currentEmp = Request.Query["CurrentEmp"];
                string treatmentDate = Request.Query["TreatmentDate"];
                string treatID = Request.Query["TreatID"];
                string isEditMaster = Request.Query["IsEditMaster"];

                CurrentID = Convert.ToInt32(curr == null ? "0" : curr.ToString());
                CustomerID = Convert.ToInt32(cusID == null ? "0" : cusID.ToString());
                TikcetID = Convert.ToInt32(ticID == null ? "0" : ticID.ToString());
                Type_Care = Convert.ToInt32(typeid == null ? "0" : typeid.ToString());
                AppID = Convert.ToInt32(appid == null ? "0" : appid.ToString());
                MasterID = Convert.ToInt32(masterid == null ? "0" : masterid.ToString());
                TreatmentDate = (treatmentDate == null ? "" : treatmentDate.ToString());
                _CurrentEmployeeExe = Convert.ToInt32(currentEmp == null ? "0" : currentEmp.ToString());
                TreatID = Convert.ToInt32(treatID == null ? "0" : treatID.ToString());
                IsEditMaster = Convert.ToInt32(isEditMaster == null ? "0" : isEditMaster.ToString());
                _IsNeedToCallBack = Global.sys_CustCare_NeedToCallBack;
            }
            else
            {
                CustomerID = 0;
                Type_Care = 0;
                AppID = 0;
                MasterID = 0;
                CurrentID = 0;
                _CurrentEmployeeExe = 0;
                TreatID = 0;
                IsEditMaster = 0;
            }
        }

        public async Task<IActionResult> OnPostLoadata(string TicketID ,string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_v2_sp_Ticket_Customer_LoadHistoryCare]" ,CommandType.StoredProcedure ,
                      "@Ticket" ,SqlDbType.Int ,Convert.ToInt32(TicketID) ,
                      "@CustomerID" ,SqlDbType.Int ,Convert.ToInt32(CustomerID));
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
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadUpdate(string currentID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Complaint_LoadValue]" ,CommandType.StoredProcedure ,
                      "@currentID" ,SqlDbType.Int ,Convert.ToInt32(currentID));
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
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostInitializeCombo(int CurrentID ,int Type ,int TicketID, string CustomerID)
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
                        dt = await confunc.LoadPara("TypeIntput");
                        dt.TableName = "TypeIntput";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_CustomerCare_LoadStatus" ,CommandType.StoredProcedure);
                        dt.TableName = "Status";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.LoadEmployeeFull(user.sys_branchID ,user.sys_AllBranchID);
                        dt.TableName = "Emp";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_History_LoadDetail]" ,CommandType.StoredProcedure ,
                            "@ID" ,SqlDbType.Int ,Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                        dt.TableName = "Data";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_CustomerCare_LoadQuickTeamplate_ByType" ,CommandType.StoredProcedure ,
                         "@Type" ,SqlDbType.Int ,Type);
                        dt.TableName = "QuickTemplate";
                        return dt;
                    }
                }));

                if (TicketID != '0')
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtTag = new DataTable();
                            dtTag = await confunc.ExecuteDataTable("[YYY_sp_Ticket_Combo_Tag]" ,CommandType.StoredProcedure);
                            dtTag.TableName = "Tag";
                            return dtTag;
                        }
                    }));
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtTagDetail = new DataTable();
                            dtTagDetail = await confunc.ExecuteDataTable("[YYY_sp_Ticket_TagByTicket]" ,CommandType.StoredProcedure ,
                                   "@TicketID" ,SqlDbType.Int ,TicketID);
                            dtTagDetail.TableName = "TagDetail";
                            return dtTagDetail;
                        }
                    }));
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtTagDetail = new DataTable();
                            dtTagDetail = await confunc.ExecuteDataTable("[YYY_sp_Branch_Load]" ,CommandType.StoredProcedure
                                ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                                ,"@LoadNormal" ,SqlDbType.Int ,1);
                            dtTagDetail.TableName = "Branch";
                            return dtTagDetail;
                        }
                    }));
                    tasks.Add(Task.Run(async () =>
                    {
                        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            DataTable dtTagDetail = new DataTable();
                            dtTagDetail = await confunc.ExecuteDataTable("[YYY_sp_NearestSchedule_Load]" ,CommandType.StoredProcedure
                                ,"@CustomerID" ,SqlDbType.Int ,CustomerID != null ? Convert.ToInt32(CustomerID) : 0
                                );
                            dtTagDetail.TableName = "NearestApp";
                            return dtTagDetail;
                        }
                    }));
                }
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

        public async Task<IActionResult> OnPostExcuteData(string Title ,string content ,int CustomerID ,int TicketID ,int Type_Care
           ,int AppID ,int IsDone ,int MasterID ,string data
           ,int CurrentID ,string TreatmentDate ,string TreatID ,string TagID ,int EmpID ,int IsEditMaster, int BranchComplain)
        {
            try
            {
                string result = "1";
                CustomerCareInput DataMain = JsonConvert.DeserializeObject<CustomerCareInput>(data);
                var user = Session.GetUserSession(HttpContext);
                Title = (Title != null ? Title : "");
                if (DataMain.Typeinput != 88)
                {
                    DataMain.TimeCallBack = "1900-01-01";
                }
                switch (Type_Care)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 6:
                    case 9:
                    case 10:
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            DataTable dt = new DataTable();
                            dt = await connFunc.ExecuteDataTable("YYY_sp_CustomerCare_InsertValue" ,CommandType.StoredProcedure ,
                                "@MasterID" ,SqlDbType.Int ,MasterID ,
                                "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                                "@AppID" ,SqlDbType.Int ,AppID ,
                                "@IsDone" ,SqlDbType.Int ,IsDone ,
                                "@Type" ,SqlDbType.Int ,Type_Care ,
                                "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                                "@Created" ,SqlDbType.DateTime ,Comon.Comon.GetDateTimeNow() ,
                                "@Title" ,SqlDbType.NVarChar ,Title.Replace("'" ,"").Trim() ,
                                "@Content" ,SqlDbType.NVarChar ,content.Replace("'" ,"").Trim() ,
                                "@Typeinput" ,SqlDbType.Int ,DataMain.Typeinput ,
                                "@TimeCallBack" ,SqlDbType.DateTime ,DataMain.TimeCallBack ,
                                "@StatusCallID" ,SqlDbType.Int ,DataMain.StatusCall ,
                                "@StatusCallDetail" ,SqlDbType.Int ,DataMain.StatusCallDetail ,
                                "@BranchComplain", SqlDbType.Int,BranchComplain,
                                "@IsEditMaster" ,SqlDbType.Int ,Convert.ToInt32(IsEditMaster)
                            );
                            result = (dt != null && dt.Rows.Count == 1) ? dt.Rows[0][0].ToString() : "0";
                        }
                        break;

                    case 5:
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            try
                            {
                                DataTable dt = new DataTable();
                                dt = await connFunc.ExecuteDataTable("YYY_sp_CustomerCare_InsertValue_Treatment" ,CommandType.StoredProcedure ,
                                 "@TreatmentDate" ,SqlDbType.Int ,(TreatmentDate != null && TreatmentDate != "") ? TreatmentDate : 0 ,
                                 "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                                  "@TreatID" ,SqlDbType.Int ,TreatID ,
                                 "@Type" ,SqlDbType.Int ,Type_Care ,
                                 "@IsDone" ,SqlDbType.Int ,IsDone ,
                                 "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                                 "@Title" ,SqlDbType.NVarChar ,Title.Replace("'" ,"").Trim() ,
                                 "@Content" ,SqlDbType.NVarChar ,content.Replace("'" ,"").Trim() ,
                                 "@Typeinput" ,SqlDbType.Int ,DataMain.Typeinput ,
                                 "@TimeCallBack" ,SqlDbType.DateTime ,DataMain.TimeCallBack ,
                                 "@StatusCallID" ,SqlDbType.Int ,DataMain.StatusCall ,
                                 "@StatusCallDetail" ,SqlDbType.Int ,DataMain.StatusCallDetail ,
                                 "@EPhysicalFacilities" ,SqlDbType.Int ,DataMain.EPhysicalFacilities ,
                                 "@EEmpAttitudes" ,SqlDbType.Int ,DataMain.EEmpAttitudes ,
                                 "@EDoctor" ,SqlDbType.Int ,DataMain.EDoctor
                             );
                                result = (dt != null && dt.Rows.Count == 1) ? dt.Rows[0][0].ToString() : "0";
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }

                        }
                        break;
                    case 12:
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            try
                            {
                                DataTable dt = new DataTable();
                                dt = await connFunc.ExecuteDataTable("YYY_sp_CustomerCare_InsertValue_Self" ,CommandType.StoredProcedure ,
                                 "@CustomerID" ,SqlDbType.Int ,CustomerID ,
                                 "@Type" ,SqlDbType.Int ,Type_Care ,
                                 "@IsDone" ,SqlDbType.Int ,IsDone ,
                                 "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                                 "@Title" ,SqlDbType.NVarChar ,Title.Replace("'" ,"").Trim() ,
                                 "@Content" ,SqlDbType.NVarChar ,content.Replace("'" ,"").Trim() ,
                                 "@Typeinput" ,SqlDbType.Int ,DataMain.Typeinput ,
                                 "@TimeCallBack" ,SqlDbType.DateTime ,DataMain.TimeCallBack ,
                                 "@StatusCallID" ,SqlDbType.Int ,DataMain.StatusCall ,
                                 "@StatusCallDetail" ,SqlDbType.Int ,DataMain.StatusCallDetail ,
                                 "@EPhysicalFacilities" ,SqlDbType.Int ,DataMain.EPhysicalFacilities ,
                                 "@EEmpAttitudes" ,SqlDbType.Int ,DataMain.EEmpAttitudes ,
                                 "@EDoctor" ,SqlDbType.Int ,DataMain.EDoctor
                             );
                                result = (dt != null && dt.Rows.Count == 1) ? dt.Rows[0][0].ToString() : "0";
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }
                        break;
                    case 7: // Tu Ticket
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            //DataTable dt = new DataTable();
                            await connFunc.ExecuteDataTable("YYY_sp_Ticket_Input_Insert" ,CommandType.StoredProcedure ,
                                "@Ticket" ,SqlDbType.Int ,Convert.ToInt32(TicketID) ,
                                "@Typeinput" ,SqlDbType.Int ,DataMain.Typeinput ,
                                "@StatusCall" ,SqlDbType.Int ,DataMain.StatusCall ,
                                "@StatusCallDetail" ,SqlDbType.Int ,DataMain.StatusCallDetail ,
                                "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                                "@TagID" ,SqlDbType.Int ,TagID ,
                                "@TimeCallBack" ,SqlDbType.DateTime ,DataMain.TimeCallBack ,
                                "@Content" ,SqlDbType.NVarChar ,content.Replace("'" ,"").Trim()
                             );
                            // Comon.PushNoti.Noti_Telesale_Execute(user.sys_userid.ToString(), TicketID.ToString(), CustomerID.ToString(), 1);
                        }
                        break;
                    case 11: // Customer History : Insert and update
                    case 8: // Customer History : Insert and update
                        if (CurrentID == 0)
                        {
                            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                            {
                                await connFunc.ExecuteDataTable("YYY_sp_Customer_History_Insert" ,CommandType.StoredProcedure ,
                                "@Customer_ID" ,SqlDbType.Int ,CustomerID ,
                                "@Content" ,SqlDbType.NVarChar ,content != null ? content.Replace("'" ,"").Trim() : "" ,
                                "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                                "@branch_ID" ,SqlDbType.Int ,user.sys_branchID ,
                                "@Type" ,SqlDbType.Int ,DataMain.StatusCallDetail ,
                                "@IsAdvice" ,SqlDbType.Int ,(Type_Care == 11) ? 1 : 0 ,
                                "@EmployeeID" ,SqlDbType.Int ,EmpID
                            );
                            }
                        }
                        else
                        {
                            using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                            {
                                await connFunc.ExecuteDataTable("YYY_sp_Customer_History_Update" ,CommandType.StoredProcedure ,
                                    "@Content" ,SqlDbType.NVarChar ,content != null ? content.Replace("'" ,"").Trim() : "" ,
                                    "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                                    "@Type" ,SqlDbType.Int ,DataMain.StatusCallDetail ,
                                    "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                                    "@EmployeeID" ,SqlDbType.Int ,EmpID
                            );
                            }
                        }
                        break;

                    default: break;

                }
                return Content(result);
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public class CustomerCareInput
        {
            public int Typeinput { get; set; }
            public int StatusCall { get; set; }
            public string TimeCallBack { get; set; }
            public string StatusCallDetail { get; set; }
            public int EPhysicalFacilities { get; set; }
            public int EEmpAttitudes { get; set; }
            public int EDoctor { get; set; }
            public int IsEditMaster { get; set; } = 0;
        }
    }
}
