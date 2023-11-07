using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Customer.Treatment
{

    public class TreatMultiv2Model : PageModel
    {
        public string _CurrentID { get; set; }
        public string _TabID { get; set; }
        public string _View { get; set; }
        public string _PatientRecordID { get; set; }
        public string sys_DentistCosmetic { get; set; }
        public string _ChooseDateCustomer { get; set; }
        public string sys_Showallsteptreatment { get; set; }
        public string _dtPermissionCurrentPage { get; set; }

        public void OnGet()
        {
            sys_DentistCosmetic = Comon.Global.sys_DentistCosmetic.ToString();
            sys_Showallsteptreatment = Comon.Global.sys_Showallsteptreatment.ToString();

            var user = Session.GetUserSession(HttpContext);
            _ChooseDateCustomer = user.sys_ChooseDateCustomer.ToString();
            var CurrentID = Request.Query["CurrentID"];
            var pat = Request.Query["PatientRecordID"];
            var tab = Request.Query["TabID"];
            var type = Request.Query["Type"];
            _CurrentID = (CurrentID.ToString() != String.Empty) ? CurrentID : "0";
            _TabID = (tab.ToString() != String.Empty) ? tab : "0";
            _PatientRecordID = (pat.ToString() != String.Empty) ? pat : "0";
            _View = (type.ToString() != String.Empty) ? type : "";
            _dtPermissionCurrentPage = HttpContext.Request.Path;
        }
        public async Task<IActionResult> OnPostLoadComboMain(string CustomerID ,string PatientRecordID ,string TabID ,string TreatID ,string dencos)
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
                        dt = await confunc.ExecuteDataTable("YYY_sp_v3_Customer_Treatment_LoadService" ,CommandType.StoredProcedure ,
                            "@Customer_ID" ,SqlDbType.Int ,CustomerID
                            ,"@DentistCosmetic" ,SqlDbType.Int ,Convert.ToInt32(dencos)
                            ,"@PatientRecordID" ,SqlDbType.Int ,Convert.ToInt32(PatientRecordID)
                            ,"@TabID" ,SqlDbType.Int ,Convert.ToInt32(TabID)
                            ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                            ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate
                         );
                        dt.TableName = "Service";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_v3_GetTreatmentStage" ,CommandType.StoredProcedure);
                        dt.TableName = "Stage";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_v3_Customer_Treatment_LoadNote]" ,CommandType.StoredProcedure);
                        dt.TableName = "ServiceNote";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_ContentTemplateTreat_LoadCombo]" ,CommandType.StoredProcedure);
                        dt.TableName = "ContentTemplate";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
               {
                   using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                   {
                       DataTable dt = new DataTable();
                       dt = await confunc.ExecuteDataTable("[YYY_sp_Treat_LoadPercent]" ,CommandType.StoredProcedure);
                       dt.TableName = "PercentSelect";
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
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostLoadTab(string Tab ,string TreatID ,string dencos)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_v3_Customer_Treatment_LoadTab" ,CommandType.StoredProcedure ,
                       "@TabID" ,SqlDbType.Int ,Tab
                       ,"@DentistCosmetic" ,SqlDbType.Int ,Convert.ToInt32(dencos)
                       ,"@TreatID" ,SqlDbType.Int ,Convert.ToInt32(TreatID)
                       ,"@UserID" ,SqlDbType.Int ,user.sys_userid
                       ,"@EditCustomerPassingDate" ,SqlDbType.Int ,user.sys_EditCustomerPassingDate);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadDataUpdate(string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("YYY_sp_v3_Customer_Treatment_LoadDetail" ,CommandType.StoredProcedure ,
                       "@ID" ,SqlDbType.Int ,CurrentID);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostExecuteData(string data ,string CurrentID ,string CustomerID
           ,string UsingDateCustomer)
        {
            try
            {
                #region // DATAMAIN
                DataTable dt = new DataTable();
                dt.Columns.Add("Percent" ,typeof(int));
                dt.Columns.Add("Doc1" ,typeof(int));
                dt.Columns.Add("Doc2" ,typeof(int));
                dt.Columns.Add("Doc3" ,typeof(int));
                dt.Columns.Add("Doc4" ,typeof(int));
                dt.Columns.Add("Ass1" ,typeof(int));
                dt.Columns.Add("Ass2" ,typeof(int));
                dt.Columns.Add("Ass3" ,typeof(int));
                dt.Columns.Add("Ass4" ,typeof(int));
                dt.Columns.Add("Tech1" ,typeof(int));
                dt.Columns.Add("Tech2" ,typeof(int));
                dt.Columns.Add("teethChoosing" ,typeof(string));
                dt.Columns.Add("Content" ,typeof(string));
                dt.Columns.Add("ContentNext" ,typeof(string));
                dt.Columns.Add("Note" ,typeof(string));
                dt.Columns.Add("Symptoms" ,typeof(string));
                dt.Columns.Add("dateTreatNext" ,typeof(DateTime));
                dt.Columns.Add("StageContentTemplate" ,typeof(string));
                dt.Columns.Add("TabID" ,typeof(int));
                dt.Columns.Add("ServiceID" ,typeof(int));
                dt.Columns.Add("dateCreated" ,typeof(DateTime));
                dt.Columns.Add("StageNum" ,typeof(int));
                dt.Columns.Add("IsFinish" ,typeof(Int32));
                dt.Columns.Add("ManualAmount" ,typeof(Decimal));
                #endregion

                #region // DetailTeeth
                DataTable teethdetail = new DataTable();
                teethdetail.Columns.Add("TabID" ,typeof(string));
                teethdetail.Columns.Add("TeethID" ,typeof(string));
                teethdetail.Columns.Add("StageID" ,typeof(string));
                teethdetail.Columns.Add("Completeper" ,typeof(decimal));
                #endregion


                var DataMain = JsonConvert.DeserializeObject<DataTable>(data);

                for (int i = 0; i < DataMain.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Percent"] = DataMain.Rows[i]["Percent"];
                    dr["Doc1"] = DataMain.Rows[i]["Doc1"];
                    dr["Doc2"] = DataMain.Rows[i]["Doc2"];
                    dr["Doc3"] = DataMain.Rows[i]["Doc3"];
                    dr["Doc4"] = DataMain.Rows[i]["Doc4"];
                    dr["Ass1"] = DataMain.Rows[i]["Ass1"];
                    dr["Ass2"] = DataMain.Rows[i]["Ass2"];
                    dr["Ass3"] = DataMain.Rows[i]["Ass3"];
                    dr["Ass4"] = DataMain.Rows[i]["Ass4"];
                    dr["Tech1"] = DataMain.Rows[i]["Tech1"];
                    dr["Tech2"] = DataMain.Rows[i]["Tech2"];
                    dr["teethChoosing"] = DataMain.Rows[i]["teethChoosing"];
                    dr["Content"] = DataMain.Rows[i]["Content"];
                    dr["ContentNext"] = DataMain.Rows[i]["ContentNext"].ToString().Replace("'" ,"").Trim();
                    dr["Note"] = DataMain.Rows[i]["Note"].ToString().Replace("'" ,"").Trim();
                    dr["Symptoms"] = DataMain.Rows[i]["Symptoms"].ToString().Replace("'" ,"").Trim();
                    dr["dateTreatNext"] = Convert.ToDateTime(Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Rows[i]["dateTreatNext"].ToString()));
                    dr["StageContentTemplate"] = DataMain.Rows[i]["StageContentTemplate"].ToString().Replace("'" ,"").Trim();
                    dr["TabID"] = DataMain.Rows[i]["Tab_ID"];
                    dr["ServiceID"] = DataMain.Rows[i]["ServiceID"];
                    dr["dateCreated"] = Convert.ToDateTime(Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Rows[i]["dateCreated"].ToString()));
                    dr["IsFinish"] = Convert.ToInt32(DataMain.Rows[i]["IsFinish"]);
                    dr["ManualAmount"] = Convert.ToDecimal(DataMain.Rows[i]["ManualAmount"]);
                    var detail_teeth = JsonConvert.DeserializeObject<DataTable>(DataMain.Rows[i]["DetailTeeth"].ToString());
                    dt.Rows.Add(dr);
                    if (detail_teeth != null)
                        for (int j = 0; j < detail_teeth.Rows.Count; j++)
                        {
                            DataRow teeth = teethdetail.NewRow();
                            teeth["TabID"] = detail_teeth.Rows[j]["tabid"];
                            teeth["TeethID"] = detail_teeth.Rows[j]["teeth"];
                            teeth["StageID"] = detail_teeth.Rows[j]["stageid"];
                            teeth["Completeper"] = Convert.ToDecimal(detail_teeth.Rows[j]["completeper"]);
                            teethdetail.Rows.Add(teeth);
                        }
                }
                var user = Session.GetUserSession(HttpContext);
                DataTable dtres = new DataTable();
       
                if (CurrentID == "0")
                {

                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dtres = await connFunc.ExecuteDataTable("YYY_sp_v3_Customer_TreatMulti_Insert" ,CommandType.StoredProcedure ,
                             "@Customer_ID" ,SqlDbType.Int ,CustomerID ,
                             "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                             "@Branch_ID" ,SqlDbType.Int ,user.sys_branchID ,
                             "@data" ,SqlDbType.Structured ,((DataTable)dt).Rows.Count > 0 ? (DataTable)dt : null ,
                             "@teethdetail" ,SqlDbType.Structured ,((DataTable)teethdetail).Rows.Count > 0 ? (DataTable)teethdetail : null ,
                             "@ChooseDateCustomer" ,SqlDbType.Int ,UsingDateCustomer ,
                             "@dencos" ,SqlDbType.Int ,Comon.Global.sys_DentistCosmetic
                         );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        var DataMainUpdate = JsonConvert.DeserializeObject<DataTable>(data);
                        dtres = await connFunc.ExecuteDataTable("YYY_sp_v3_Customer_TreatMulti_Update" ,CommandType.StoredProcedure ,
                              "@CurrentID" ,SqlDbType.Int ,CurrentID ,
                              "@Customer_ID" ,SqlDbType.Int ,CustomerID ,
                              "@BS1" ,SqlDbType.Int ,DataMain.Rows[0]["Doc1"] ,
                              "@BS2" ,SqlDbType.Int ,DataMain.Rows[0]["Doc2"] ,
                              "@BS3" ,SqlDbType.Int ,DataMain.Rows[0]["Doc3"] ,
                              "@BS4" ,SqlDbType.Int ,DataMain.Rows[0]["Doc4"] ,
                              "@PT1" ,SqlDbType.Int ,DataMain.Rows[0]["Ass1"] ,
                              "@PT2" ,SqlDbType.Int ,DataMain.Rows[0]["Ass2"] ,
                              "@PT3" ,SqlDbType.Int ,DataMain.Rows[0]["Ass3"] ,
                              "@PT4" ,SqlDbType.Int ,DataMain.Rows[0]["Ass4"] ,
                              "@Tech1" ,SqlDbType.Int ,DataMain.Rows[0]["Tech1"] ,
                              "@Tech2" ,SqlDbType.Int ,DataMain.Rows[0]["Tech2"] ,
                              "@Content" ,SqlDbType.NVarChar ,DataMain.Rows[0]["Content"].ToString().Replace("'" ,"").Trim() ,
                              "@Content_Next" ,SqlDbType.NVarChar ,DataMain.Rows[0]["ContentNext"].ToString().Replace("'" ,"").Trim() ,
                              "@Note" ,SqlDbType.NVarChar ,DataMain.Rows[0]["Note"].ToString().Replace("'" ,"").Trim() ,
                              "@Symptoms" ,SqlDbType.NVarChar ,DataMain.Rows[0]["Symptoms"].ToString().Replace("'" ,"").Trim() ,
                              "@dateTreatNext" ,SqlDbType.DateTime ,Convert.ToDateTime(Comon.Comon.DateTimeDMY_To_YMDHHMM(DataMain.Rows[0]["dateTreatNext"].ToString())) ,
                              "@StageContentTemplate" ,SqlDbType.NVarChar ,DataMain.Rows[0]["StageContentTemplate"].ToString().Replace("'" ,"").Trim() ,
                              "@teethChoosing" ,SqlDbType.NVarChar ,DataMain.Rows[0]["teethChoosing"] ,
                              "@Created_By" ,SqlDbType.Int ,user.sys_userid ,
                              "@Percent" ,SqlDbType.Int ,DataMain.Rows[0]["Percent"] ,
                              "@ManualAmount" ,SqlDbType.Decimal ,Convert.ToDecimal(DataMain.Rows[0]["ManualAmount"]) ,
                              "@StageNum" ,SqlDbType.Int ,DataMain.Rows[0]["StageNum"] ,
                              "@dencos" ,SqlDbType.Int ,Comon.Global.sys_DentistCosmetic
                         );
                    }
                }
                return Content(Comon.DataJson.Datatable(dtres));
            }
            catch (Exception ex)
            {
                return Content("-1");
            }
        }
    }
}
