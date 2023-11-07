using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MLunarCoffee.Pages.Customer
{
    public class ImageNewFolderModel : PageModel
    {

        public string _ImageFolderCustomerID { get; set; }
        public string _CurrentFolderID { get; set; }
        public string _CurrentFolderName { get; set; }
        public string _TreatmentPlanID { get; set; }

        public void OnGet()
        {
            var cus = Request.Query["CustomerID"];
            var currenrid = Request.Query["CurrentID"];
            var currenrname = Request.Query["CurrentName"];
            var treatmentplantid = Request.Query["TreatmentPlanID"];

            if (cus.ToString() != "")
            {
                _ImageFolderCustomerID = cus.ToString();
                _CurrentFolderID = (currenrid.ToString() != "") ? currenrid.ToString() : "0";
                _CurrentFolderName = (currenrname.ToString() != "") ? currenrname.ToString() : "";
                _TreatmentPlanID = (treatmentplantid.ToString() != "") ? treatmentplantid.ToString() : "0";
            }
            else
            {
                _ImageFolderCustomerID = "0";
                Response.Redirect("/assests/Error/index.html");
            }

        }
        public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_CustomerImage_Folder_LoadDetail]", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
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
        public async Task<IActionResult> OnPostExcuteData(string data, string CustomerID, string CurrentID, string oldName, string TreatmentPlanId)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                FolderName DataMain = JsonConvert.DeserializeObject<FolderName>(data);
                string FolderName = DataMain.Content.Trim();
                FolderName = Comon.Comon.ConvertToUnsign(FolderName).ToLower().Replace(" ", "").Trim();
                if (CurrentID == "0")
                {
                    if (Comon.Comon.CheckDirectoryFTP(CustomerID, FolderName))
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_Create_Folder]", CommandType.StoredProcedure,
                                "@CustomerID", SqlDbType.Int, CustomerID
                                , "@CreatedBy", SqlDbType.Int, user.sys_userid
                                , "@FolderName", SqlDbType.NVarChar, FolderName
                                , "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'", "").Trim()
                                , "@Branch", SqlDbType.Int, user.sys_branchID
                                , "@TreatmentPlanId", SqlDbType.Int, TreatmentPlanId
                            );
                        }
                        return Content("1");
                    }
                    else return Content("0");
                }
                else
                {
                    if (Comon.Comon.UpdateDirectoryFTP(CustomerID, oldName, FolderName))
                    {
                        using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                        {
                            await connFunc.ExecuteDataTable("[YYY_sp_CustomerImage_Update_Folder]", CommandType.StoredProcedure,
                                "@CurrentID", SqlDbType.Int, CurrentID
                                , "@CreatedBy", SqlDbType.Int, user.sys_userid
                                , "@FolderName", SqlDbType.NVarChar, FolderName
                                , "@Note", SqlDbType.NVarChar, DataMain.Note.Replace("'" , "").Trim()
                            );
                        }
                        return Content("1");
                    }
                    else return Content("Something wrongs");

                }
            }
            catch (Exception ex)
            {
                return Content("Something wrongs");
            }
        }

    }
    public class FolderName
    {
        public string Content { get; set; }
        public string Note { get; set; }
    }
}
