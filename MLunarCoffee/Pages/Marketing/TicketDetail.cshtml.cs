using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Marketing
{
    public class TicketDetailModel : PageModel
    {
        public string _TicketDetailID { get; set; }
        public string _Customer_ID { get; set; }
        public string CurrentPath { get; set; }
        public void OnGet()
        {
            // FROM TICKET ACTION
            string CurrentID = Request.Query["CurrentID"];
            string CustomerID = Request.Query["CustomerID"];

            // FROM TICKET STORE
            if (CurrentID != null || CustomerID != null)
            {
                _TicketDetailID = CurrentID.ToString();
                _Customer_ID = CustomerID.ToString();
            }
            else
            {
                _TicketDetailID = "0";
                _Customer_ID = "0";
            }

            CurrentPath = HttpContext.Request.Path;
        }


        public async Task<IActionResult> OnPostInitialize(string TicketID)
        {
            try
            {
                DataSet ds = new DataSet();
                var tasks = new List<Task<DataTable>>();
                var user = Session.GetUserSession(HttpContext);
                tasks.Add(Task.Run(async () =>
                {

                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Type_ComboLoad]", CommandType.StoredProcedure);
                        dt.TableName = "TicketSource";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.LoadPara("Gender");
                        dt.TableName = "Gender";
                        return dt;
                    }
                }));

                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("[YYY_sp_ServiceCare_LoadCombo]", CommandType.StoredProcedure);
                        dt.TableName = "ServiceCare";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Customer_Group_ComboLoad]", CommandType.StoredProcedure);
                        dt.TableName = "TicketGroup";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_EmployeeMarketing_LoadCombo]", CommandType.StoredProcedure);
                        dt.TableName = "EmployeeMar";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_TicketTag_LoadTele]", CommandType.StoredProcedure
                            , "@UserID", SqlDbType.Int, user.sys_userid
                            );
                        dt.TableName = "TeleFollow";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Check_Tele_Permission]", CommandType.StoredProcedure
                            , "@UserID", SqlDbType.Int, user.sys_userid
                            );
                        dt.TableName = "PerTele";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_GetStaffID]", CommandType.StoredProcedure
                            , "@ID", SqlDbType.Int, Convert.ToInt32(TicketID));
                        dt.TableName = "Staff";
                        return dt;
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Combo_Tag]" ,CommandType.StoredProcedure);
                        dt.TableName = "ComboTag";
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


        public async Task<IActionResult> OnPostLoadataTicketDetail(int TicketID)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Ticket_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(TicketID == 0 ? 0 : TicketID)
                      );
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID, string CustomerID, string exist)
        {
            try
            {
                DataTable dt = new DataTable();
                TicketDetail DataMain = JsonConvert.DeserializeObject<TicketDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Insert]", CommandType.StoredProcedure,
                           "@Email", SqlDbType.NVarChar, DataMain.Email.Replace("'", "").Trim(),
                           "@Phone1", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                           "@Phone2", SqlDbType.NVarChar, DataMain.Phone2.Replace("'", "").Trim(),
                           "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                           "@NameNoSign", SqlDbType.NVarChar, DataMain.NameNoSign.Replace("'", "").Trim(),
                           "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                           "@GenderID", SqlDbType.Int, DataMain.Gender,
                           "@SourceID", SqlDbType.Int, DataMain.Source,
                           "@UrlInstgram", SqlDbType.NVarChar, DataMain.instgramurl.Replace("'", "").Trim(),
                           "@UrlFacebook", SqlDbType.NVarChar, DataMain.facebookurl.Replace("'", "").Trim(),
                           "@UrlZalo", SqlDbType.NVarChar, DataMain.zalomurl.Replace("'", "").Trim(),
                           "@UrlViber", SqlDbType.NVarChar, DataMain.viberurl.Replace("'", "").Trim(),
                           "@Gclid", SqlDbType.NVarChar, DataMain.gclid.Replace("'", "").Trim(),
                           "@CreatedBy", SqlDbType.Int, user.sys_userid,
                           "@IsExistPhone", SqlDbType.Int, exist,
                           "@ServiceCareToken", SqlDbType.NVarChar, DataMain.ServiceCareToken,
                           "@ServiceCareID", SqlDbType.Int, DataMain.SerCareID,
                           "@Address", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                           "@CustomerGroupID", SqlDbType.Int, DataMain.CustomerGroup,
                           "@CityID", SqlDbType.Int, DataMain.City,
                           "@DistID", SqlDbType.Int, DataMain.District,
                           "@EmployeeMar", SqlDbType.Int, DataMain.EmployeeMar,
                           "@TeleFollow", SqlDbType.Int, DataMain.TeleFollow,
                           "@TagID", SqlDbType.Int, DataMain.TagID
                       );
                    }
                }
                else
                {
                    if (CustomerID == "0")
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Update]", CommandType.StoredProcedure,
                                "@Email", SqlDbType.NVarChar, DataMain.Email.Replace("'", "").Trim(),
                                "@Phone1", SqlDbType.NVarChar, DataMain.Phone.Replace("'", "").Trim(),
                                "@Phone2", SqlDbType.NVarChar, DataMain.Phone2.Replace("'", "").Trim(),
                                "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                                "@NameNoSign", SqlDbType.NVarChar, DataMain.NameNoSign.Replace("'", "").Trim(),
                                "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                                "@GenderID", SqlDbType.Int, DataMain.Gender,
                                "@SourceID", SqlDbType.Int, DataMain.Source,
                                "@UrlInstgram", SqlDbType.NVarChar, DataMain.instgramurl.Replace("'", "").Trim(),
                                "@UrlFacebook", SqlDbType.NVarChar, DataMain.facebookurl.Replace("'", "").Trim(),
                                "@UrlZalo", SqlDbType.NVarChar, DataMain.zalomurl.Replace("'", "").Trim(),
                                "@UrlViber", SqlDbType.NVarChar, DataMain.viberurl.Replace("'", "").Trim(),
                                "@Gclid", SqlDbType.NVarChar, DataMain.gclid.Replace("'", "").Trim(),
                                "@IsExistPhone", SqlDbType.Int, exist,
                                "@ServiceCareToken", SqlDbType.NVarChar, DataMain.ServiceCareToken,
                                "@ServiceCareID", SqlDbType.Int, DataMain.SerCareID,
                                "@Address", SqlDbType.NVarChar, DataMain.Address.Replace("'", "").Trim(),
                                "@CustomerGroupID", SqlDbType.Int, DataMain.CustomerGroup,
                                "@CityID", SqlDbType.Int, DataMain.City,
                                "@DistID", SqlDbType.Int, DataMain.District,
                                "@EmployeeMar", SqlDbType.Int, DataMain.EmployeeMar,
                                "@TeleFollow", SqlDbType.Int, DataMain.TeleFollow,
                                "@CurrentID", SqlDbType.Int, CurrentID,
                                "@ModifiedBy", SqlDbType.Int, user.sys_userid,
                                "@UpdatePhone", SqlDbType.Int, Comon.CheckingSensative_Field.CheckSensetive_phone_customer(HttpContext),
                                 "@TagID" ,SqlDbType.Int ,DataMain.TagID
                            );
                        }
                    }
                    else
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            dt = await connFunc.ExecuteDataTable("[YYY_sp_Ticket_Customer_Update]", CommandType.StoredProcedure,
                                "@Content", SqlDbType.NVarChar, DataMain.Content.Replace("'", "").Trim(),
                                "@Email", SqlDbType.NVarChar, DataMain.Email.Replace("'", "").Trim(),
                                "@UrlInstgram", SqlDbType.NVarChar, DataMain.instgramurl.Replace("'", "").Trim(),
                                "@UrlFacebook", SqlDbType.NVarChar, DataMain.facebookurl.Replace("'", "").Trim(),
                                "@UrlZalo", SqlDbType.NVarChar, DataMain.zalomurl.Replace("'", "").Trim(),
                                "@UrlViber", SqlDbType.NVarChar, DataMain.viberurl.Replace("'", "").Trim(),
                                "@ModifiedBy", SqlDbType.Int, user.sys_userid,
                                "@CurrentID", SqlDbType.Int, CurrentID,
                                "@CustomerID", SqlDbType.Int, CustomerID,
                                "@ServiceCareToken", SqlDbType.NVarChar, DataMain.ServiceCareToken,
                                "@ServiceCareID", SqlDbType.Int, DataMain.SerCareID,
                                "@TagID" ,SqlDbType.Int ,DataMain.TagID,
                                "@Gclid" ,SqlDbType.NVarChar ,DataMain.gclid.Replace("'" ,"").Trim()
                            );
                        }

                    }

                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadDuplicateData(string phone)
        {
            try
            {
                phone = (phone != null ? phone : "");
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_Ticket_List_Duplicate_V1]", CommandType.StoredProcedure,
                        "@Phone", SqlDbType.NVarChar, phone
                    );
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
            catch (Exception)
            {
                return Content("[]");
            }
        }
    }

    public class TicketDetail
    {
        public string Email { get; set; }
        public string Content { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Name { get; set; }
        public string NameNoSign { get; set; }
        public int Gender { get; set; }
        public int Source { get; set; }
        public string instgramurl { get; set; }
        public string facebookurl { get; set; }
        public string zalomurl { get; set; }
        public string viberurl { get; set; }
        public string gclid { get; set; }
        public string ServiceCareToken { get; set; }
        public int SerCareID { get; set; }
        public string Address { get; set; }
        public string CustomerGroup { get; set; }
        public int City { get; set; }
        public int District { get; set; }
        public int EmployeeMar { get; set; }
        public int TeleFollow { get; set; }
        public int TagID { get; set; }
    }
}
