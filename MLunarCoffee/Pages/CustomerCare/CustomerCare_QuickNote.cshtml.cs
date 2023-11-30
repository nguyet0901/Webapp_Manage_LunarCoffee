using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.CustomerCare
{
    public class CustomerCare_QuickNoteModel : PageModel
    {
        public int _MasterID { get; set; }
        public int _AppID { get; set; }
        public int _CustomerID { get; set; }
        public int _IsDone { get; set; }
        public int _Type { get; set; }
        public string _Title { get; set; }
        public int _Typeinput { get; set; }
        public int _StatusCallID { get; set; }
        public int _StatusCallDetail { get; set; }
        public int _TreatmentID { get; set; }
        public string _TreatmentDate { get; set; }
        public int _CurrentEmp { get; set; }
        public int _NextEmp { get; set; }
        public void OnGet()
        {
            string MasterID = Request.Query["MasterID"];
            string AppID = Request.Query["AppID"];
            string CustomerID = Request.Query["CustomerID"];
            string IsDone = Request.Query["IsDone"];
            string Type = Request.Query["Type"];
            string Title = Request.Query["Title"];
            string Typeinput = Request.Query["Typeinput"];
            string StatusCallID = Request.Query["StatusCallID"];
            string StatusCallDetail = Request.Query["StatusCallDetail"];
            string TreatmentID = Request.Query["TreatmentID"];
            string TreatmentDate = Request.Query["TreatmentDate"];
            string CurrentEmp = Request.Query["CurrentEmp"];
            string NextEmp = Request.Query["NextEmp"];
            if (MasterID != "" && MasterID != null)
            {
                _MasterID = Convert.ToInt32(MasterID == null ? "0" : MasterID.ToString());
                _AppID = Convert.ToInt32(AppID == null ? "0" : AppID.ToString());
                _CustomerID = Convert.ToInt32(CustomerID == null ? "0" : CustomerID.ToString());
                _IsDone = Convert.ToInt32(IsDone == null ? "0" : IsDone.ToString());
                _Type = Convert.ToInt32(Type == null ? "0" : Type.ToString());
                _Title = Title == null ? "" : Title.ToString();
                _Typeinput = Convert.ToInt32(Typeinput == null ? "0" : Typeinput.ToString());
                _StatusCallID = Convert.ToInt32(StatusCallID == null ? "0" : StatusCallID.ToString());
                _StatusCallDetail = Convert.ToInt32(StatusCallDetail == null ? "0" : StatusCallDetail.ToString());
                _TreatmentID = Convert.ToInt32(TreatmentID == null ? "0" : TreatmentID.ToString());
                _TreatmentDate = (TreatmentDate == null ? "0" : TreatmentDate.ToString());
                _CurrentEmp = Convert.ToInt32(CurrentEmp == null ? "0" : CurrentEmp.ToString());
                _NextEmp = Convert.ToInt32(NextEmp == null ? "0" : NextEmp.ToString());
            }
        }



         public async Task<IActionResult> OnPostExcuteData(string Data, int Type_Care)
        {
            try
            {
                CustomerCareInput DataMain = JsonConvert.DeserializeObject<CustomerCareInput>(Data);
                var user = Session.GetUserSession(HttpContext);
                switch (Type_Care)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 9:
                    case 10:
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            await connFunc.ExecuteDataTable("MLU_sp_CustomerCare_InsertValue", CommandType.StoredProcedure,
                                "@MasterID", SqlDbType.Int, DataMain.MasterID,
                                "@CustomerID", SqlDbType.Int, DataMain.CustomerID,
                                "@AppID", SqlDbType.Int, DataMain.AppID,
                                "@IsDone", SqlDbType.Int, DataMain.IsDone,
                                "@Type", SqlDbType.Int, Type_Care,
                                "@Created_By", SqlDbType.Int, user.sys_userid,
                                "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                                "@Title", SqlDbType.NVarChar, DataMain.Title.Replace("'", "").Trim(),
                                "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                                "@Typeinput", SqlDbType.Int, DataMain.Typeinput,
                                "@TimeCallBack", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                                "@StatusCallID", SqlDbType.Int, DataMain.StatusCall,
                                "@StatusCallDetail", SqlDbType.Int, DataMain.StatusCallDetail
                            );
                        }
                        break;
 
                    case 6:
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            await connFunc.ExecuteDataTable("MLU_sp_CustomerCare_InsertValue_Complaint", CommandType.StoredProcedure,
                                "@MasterID", SqlDbType.Int, DataMain.MasterID,
                                "@CustomerID", SqlDbType.Int, DataMain.CustomerID,
                                "@AppID", SqlDbType.Int, DataMain.AppID,
                                "@IsDone", SqlDbType.Int, DataMain.IsDone,
                                "@Type", SqlDbType.Int, Type_Care,
                                "@Created_By", SqlDbType.Int, user.sys_userid,
                                "@CurrentEmp", SqlDbType.Int, DataMain.CurrentEmp,
                                "@NextEmp", SqlDbType.Int, DataMain.NextEmp,
                                "@Created", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                                "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                                "@Typeinput", SqlDbType.Int, DataMain.Typeinput,
                                "@TimeCallBack", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                                "@StatusCallID", SqlDbType.Int, DataMain.StatusCall,
                                "@StatusCallDetail", SqlDbType.Int, DataMain.StatusCallDetail
                            );
                        }
                        break;
                    default: break;

                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }



         public async Task<IActionResult> OnPostLoadNoteTeamplate(int Type)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("MLU_sp_CustomerCare_LoadQuickTeamplate_ByType", CommandType.StoredProcedure,
                        "@Type", SqlDbType.Int, Type
                    );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        public class CustomerCareInput
        {
            public int MasterID { get; set; }
            public int CustomerID { get; set; }
            public int AppID { get; set; }
            public int IsDone { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public int Typeinput { get; set; }
            public int StatusCall { get; set; }
            public int CurrentEmp { get; set; }
            public int NextEmp { get; set; }
            public string StatusCallDetail { get; set; }
            public int TreatID { get; set; }
        }

    }
}
