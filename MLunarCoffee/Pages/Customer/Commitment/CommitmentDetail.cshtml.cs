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

namespace MLunarCoffee.Pages.Customer.Commitment
{

    public class CommitmentDetailModel : PageModel
    {
        public string _Commitment_CustomerID { get; set; }
        public string _Commitment_CurrentID { get; set; }
        public string _Commitment_HTTPRoot { get; set; }
        public void OnGet()
        {
            _Commitment_HTTPRoot = Comon.Global.sys_HTTPImageRoot;
            var cust = Request.Query["CustomerID"];
            var curr = Request.Query["CurrentID"];
            if (cust.ToString() != String.Empty)
            {
                _Commitment_CustomerID = cust.ToString();
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
            if (curr.ToString() != String.Empty)
            {
                _Commitment_CurrentID = curr.ToString();
            }
            else
            {
                _Commitment_CurrentID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadComboMain()
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Commitment_Form_LoadCombo]", CommandType.StoredProcedure);
                    dt.TableName = "dtForm";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public IActionResult OnPostReadFormCommitment(string Link)
        {
            try
            {
                string url = String.Format(@"{0}{1}", Comon.Global.sys_HTTPImageRoot, Link);
                string content = Comon.Comon.Readfile(url);
                return Content(content);
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        public async Task<IActionResult> OnPostLoadataUpdate(string CustomerID, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds = await confunc.ExecuteDataSet("[MLU_sp_Customer_Commitment_Detail]", CommandType.StoredProcedure,
                        "@customerID", SqlDbType.Int, CustomerID,
                        "@currentID", SqlDbType.Int, CurrentID
                        );
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }

        public async Task<IActionResult> OnPostUpdateSign(int id, string sign)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    await connFunc.ExecuteDataTable("[MLU_sp_Customer_Commitment_Update_Sign]", CommandType.StoredProcedure,
                        "@CurrentID", SqlDbType.Int, id,
                        "@userID", SqlDbType.Int, user.sys_userid,
                        "@Sign", SqlDbType.NVarChar, sign
                    );
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostExcuteData(string CurrentID, string CustomerID, string FormID, string Data, string CustSign)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Session.GetUserSession(HttpContext);

                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Commitment_Insert]", CommandType.StoredProcedure,
                            "@customerID", SqlDbType.Int, CustomerID,
                            "@formID", SqlDbType.Int, FormID,
                            "@data", SqlDbType.NVarChar, Data,
                            "@cust_sign", SqlDbType.NVarChar, CustSign,
                            "@created_by", SqlDbType.Int, user.sys_userid
                        );
                    }

                }
                else
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        dt = await confunc.ExecuteDataTable("[MLU_sp_Customer_Commitment_Update]", CommandType.StoredProcedure,
                            "@currentID", SqlDbType.Int, CurrentID,
                            "@data", SqlDbType.NVarChar, Data,
                            "@cust_sign", SqlDbType.NVarChar, CustSign,
                            "@modified_by", SqlDbType.Int, user.sys_userid
                        );
                    }

                }
                if (dt.Rows.Count > 0)
                    return Content(Comon.DataJson.GetFirstValue(dt));
                else
                    return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
    }
}
