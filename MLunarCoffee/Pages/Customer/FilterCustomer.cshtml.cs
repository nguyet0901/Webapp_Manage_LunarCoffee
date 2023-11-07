using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SourceMain.Comon;
using SourceMain.Comon.Session;
using Microsoft.AspNetCore.Http.Extensions;

namespace SourceMain.Pages.Customer
{
    public class FilterCustomerModel : PageModel
    {
        public int sys_Limit_LoadList { get; set; }
        public void OnGet()
        {
            sys_Limit_LoadList = 999;
        }

        public async Task<IActionResult> OnPostLoadataCustomer(string dateFromRelationship, string dateToRelationship, string isdateRelationship, string tokenRelationshipBranch
           , string isCreateCust, string dateToCreateCust, string dateFromCreateCust
           , string isdateUsingService, string dateFrom, string dateTo, string tokengroupservice, string tokenSrouce
           , string tokenInsurance, string tokenServiceStatus
           , string tokenMember, string tokenCarree, string tokenBranch, string tokenGender, string tokenCustomerGroup, string offset, string limit
           , string AgeFrom, string AgeTo
           , string TreatmentFrom, string TreatmentTo, string isTreatmentDate
           )
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[YYY_sp_Customer_Filter]", CommandType.StoredProcedure,
                          "@isdateRelationship", SqlDbType.Int, isdateRelationship
                        , "@dateFromRelationship", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFromRelationship)
                        , "@dateToRelationship", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateToRelationship)
                        , "@tokenRelationshipBranch", SqlDbType.NVarChar, tokenRelationshipBranch

                        , "@isdateService", SqlDbType.Int, isdateUsingService
                        , "@dateFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom)
                        , "@dateTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo)
                        , "@isCreateCust", SqlDbType.Int, isCreateCust
                        , "@dateToCreateCust", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateToCreateCust)
                        , "@dateFromCreateCust", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFromCreateCust)

                        , "@isTreatmentDate", SqlDbType.Int, isTreatmentDate
                        , "@treatmentFrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(TreatmentFrom)
                        , "@treatmentTo", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(TreatmentTo)

                        , "@tokenSrouce", SqlDbType.NVarChar, tokenSrouce
                        , "@tokenGender", SqlDbType.NVarChar, tokenGender
                        , "@tokenBranch", SqlDbType.NVarChar, tokenBranch
                        , "@tokenCarree", SqlDbType.NVarChar, tokenCarree
                        , "@tokenMember", SqlDbType.NVarChar, tokenMember
                        , "@tokenCustomerGroup", SqlDbType.NVarChar, tokenCustomerGroup
                        , "@tokengroupservice", SqlDbType.NVarChar, tokengroupservice
                        , "@tokenInsurance", SqlDbType.NVarChar, tokenInsurance
                        , "AgeFrom", SqlDbType.Int, Convert.ToInt32(AgeFrom)
                        , "AgeTo", SqlDbType.Int, Convert.ToInt32(AgeTo)
                        , "@limit", SqlDbType.Int, Convert.ToInt32(limit)
                        , "@offset", SqlDbType.Int, Convert.ToInt32(offset));
                }
                if (ds != null)
                {
                    DataSet dsResult = new DataSet();
                    DataTable dtTotalRow = new DataTable();
                    dtTotalRow = ds.Tables[4].Copy();
                    DataTable dtMember = ds.Tables[2];
                    DataTable dtInsurance = ds.Tables[3];
                    DataTable dtDetail = ds.Tables[1];
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0].Copy();
                    if (tokengroupservice == "" && tokenMember == "" && tokenInsurance == "" && tokenServiceStatus == "")
                    {

                        dtTotalRow.TableName = "dtTotalRow";
                        dt.TableName = "dtResult";
                        dsResult.Tables.Add(dt);
                        dsResult.Tables.Add(dtTotalRow);

                        return Content(Comon.DataJson.Dataset(dsResult));
                    }

                    if (dt != null && dt.Rows.Count != 0)
                    {
                        bool detectServiceType = false;
                        bool noShowServiceType = false;

                        bool detectMember = false;
                        bool noShowMember = false;

                        bool detectInsurance = false;
                        bool noShowInsurance = false;

                        bool detectServiceStatus = false;
                        bool noShowServiceStatus = false;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            detectServiceType = false;
                            detectMember = false;
                            noShowServiceType = false;
                            noShowMember = false;
                            string member = "";
                            if (tokenMember != "")
                            {
                                detectMember = true;
                                decimal TotalAmount = Convert.ToDecimal(dt.Rows[i]["TotalAmount"]);
                                member = DectectMember(dtMember, tokenMember, TotalAmount);

                                if (member != "")
                                {
                                    dt.Rows[i]["Member"] = member;
                                }
                                else
                                {
                                    noShowMember = true;
                                }
                            }

                            if (tokengroupservice != "")
                            {
                                detectServiceType = true;
                                string result = DectectServiceType(dtDetail, Convert.ToInt32(dt.Rows[i]["CusID"]));
                                if (result != "")
                                {

                                    dt.Rows[i]["ServiceCatName"] = result;
                                }
                                else
                                {
                                    noShowServiceType = true;
                                }
                            }
                            if (tokenInsurance != "")
                            {
                                detectInsurance = true;
                                string result = DectectInsurance(dtInsurance, Convert.ToInt32(dt.Rows[i]["CusID"]));
                                if (result != "")
                                {

                                    dt.Rows[i]["InsuranceName"] = result;
                                }
                                else
                                {
                                    noShowInsurance = true;
                                }
                            }
                            if (tokenServiceStatus == "")
                            {
                                detectServiceStatus = false;
                            }
                            else if (tokenServiceStatus != "1")
                            {
                                detectServiceStatus = true;
                                if (Convert.ToInt32(dt.Rows[i]["IsTratment"].ToString()) > 0)
                                {
                                    noShowServiceStatus = true;
                                }
                            }
                            else if (tokenServiceStatus != "1")
                            {
                                detectServiceStatus = true;
                                if (Convert.ToInt32(dt.Rows[i]["IsTratment"].ToString()) <= 0)
                                {
                                    noShowServiceStatus = true;
                                }
                            }
                            if ((detectServiceType == false && detectMember == false && detectInsurance == false && detectServiceStatus == false) || (detectServiceType == true && noShowServiceType == false) || (detectMember == true && noShowMember == false) || (detectInsurance == true && noShowInsurance == false) || (detectServiceStatus == true && noShowServiceStatus == false))
                            {
                                dt.Rows[i]["State"] = 1;
                            }
                        }
                    }
                    DataRow[] resultRow = dt.Select("State = 1");
                    DataTable dtResult = resultRow.CopyToDataTable();

                    dtTotalRow.TableName = "dtTotalRow";
                    dtResult.TableName = "dtResult";
                    dsResult.Tables.Add(dtResult);
                    dsResult.Tables.Add(dtTotalRow);
                    return Content(Comon.DataJson.Dataset(dsResult));
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
        private static string DectectMember(DataTable dtMember, string tokenMember, decimal TotalAmount)
        {
            try
            {
                string result = "";
                string[] token = tokenMember.Split(',');
                foreach (var t in token)
                {
                    int id = Convert.ToInt32(t);
                    result = result + DetectSpecificMember(dtMember, id, TotalAmount);
                    if (result != "") break;
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }


        }
        private static string DetectSpecificMember(DataTable dtMember, int id, decimal TotalAmount)
        {
            try
            {
                string result = "";
                var resultRow = from myRow in dtMember.AsEnumerable()
                                where myRow.Field<int>("ID") == id
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    decimal AmountFrom = Convert.ToDecimal(dtResult.Rows[i]["AmountFrom"]);
                    decimal AmountTo = Convert.ToDecimal(dtResult.Rows[i]["AmountTo"]);
                    if (TotalAmount >= AmountFrom && TotalAmount <= AmountTo)
                    {
                        result = result + dtResult.Rows[i]["Name"].ToString();
                        break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private static string DectectServiceType(DataTable dt, int CusID)
        {
            try
            {
                string result = "";
                var resultRow = from myRow in dt.AsEnumerable()
                                where myRow.Field<int>("CusID") == CusID
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    result = result + dtResult.Rows[i]["ServiceTypeName"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }


        }
        private static string DectectInsurance(DataTable dt, int CusID)
        {
            try
            {
                string result = "";
                var resultRow = from myRow in dt.AsEnumerable()
                                where myRow.Field<int>("CusID") == CusID
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    result = result + dtResult.Rows[i]["InsuranceName"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }


        }


        public IActionResult OnPostCheckContent(string content)
        {
            try
            {
                if (content == "" || content.Length < 5) return Content("Tin Nhắn Rổng Hoặc Số Ký Tự Quá Ít (Lớn Hơn 5)");
                return Content(Comon.Comon.CheckContentSMS(content));
            }
            catch (Exception ex)
            {
                return Content("-1");
            }
        }
        public async Task<IActionResult> OnPostSendSMSMulti(string data, string content)
        {
            try
            {
                if (content == "" || content.Length < 5) return Content("Tin Nhắn Rổng Hoặc Số Ký Tự Quá Ít (Lớn Hơn 5)");
                if (Comon.Comon.CheckContentSMS(content) != "1") return Content("-2"); //Content khong chap nhan
                var user = Session.GetUserSession(HttpContext);
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                DataTable dt = new DataTable();
                dt.Columns.Add("CusID");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Status");
                string resulf = "0";
                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    string phone = dataDetail.Rows[i]["Phone"].ToString();
                    string CusID = dataDetail.Rows[i]["CusID"].ToString();
                    resulf = "0";// Comon.Comon.SendSMS(content, phone);
                    DataRow dr = dt.NewRow();
                    dr["CusID"] = CusID;
                    dr["Phone"] = phone;
                    if (Convert.ToInt32(resulf) < 3 && Convert.ToInt32(resulf) > 0)
                    {
                        dr["Status"] = 1;
                    }
                    else
                    {
                        dr["Status"] = 0;
                    }
                    dt.Rows.Add(dr);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("YYY_sp_Customer_SMS_Multi", CommandType.StoredProcedure,
                           "@Content", SqlDbType.NVarChar, content,
                           "@Created_By", SqlDbType.Int, user.sys_userid,
                           "@table_data", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                       );
                }

                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            DataSet ds = new DataSet();

            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("YYY_sp_Customer_Type_LoadList", CommandType.StoredProcedure);
                dt.TableName = "Source";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[YYY_sp_Customer_Group_ComboLoad]", CommandType.StoredProcedure);
                dt.TableName = "CustomerGroup";
                ds.Tables.Add(dt);

            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("YYY_sp_Service_Type", CommandType.StoredProcedure);
                dt.TableName = "Servicetype";
                ds.Tables.Add(dt);

            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("YYY_sp_Career_Load", CommandType.StoredProcedure);
                dt.TableName = "Carree";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.LoadPara("Gender");
                dt.TableName = "Gender";
                ds.Tables.Add(dt);
            }
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("YYY_sp_Member_Load", CommandType.StoredProcedure);
                dt.TableName = "Member";
                ds.Tables.Add(dt);
            }

            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                dt = await confunc.ExecuteDataTable("YYY_sp_Branch_Load", CommandType.StoredProcedure
                  , "@UserID", SqlDbType.Int, user.sys_userid
                  , "@LoadNormal", SqlDbType.Int, 1);
                dt.TableName = "Branch";
                ds.Tables.Add(dt);
            }

            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                DataTable dt = new DataTable();
                dt = await confunc.ExecuteDataTable("[YYY_sp_Insurance_LoadComBo]", CommandType.StoredProcedure);
                dt.TableName = "Insurance";
                ds.Tables.Add(dt);
            }

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("ID", typeof(int));
            dt1.Columns.Add("Name", typeof(String));
            DataRow dr2 = dt1.NewRow();
            dr2["ID"] = 1;
            dr2["Name"] = "Còn Điều Trị";
            dt1.Rows.Add(dr2);
            DataRow dr3 = dt1.NewRow();
            dr3["ID"] = 2;
            dr3["Name"] = "Không Còn  Điều Trị";
            dt1.Rows.Add(dr3);
            dt1.TableName = "Status";
            ds.Tables.Add(dt1);

            return Content(Comon.DataJson.Datatable(dt1));

        }
    }
}
