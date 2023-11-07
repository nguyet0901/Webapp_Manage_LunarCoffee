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

namespace MLunarCoffee.Pages.Customer.ConsultationRegimen
{

    public class ConsultationRegimenDetailModel : PageModel
    {
        public string _CurrentDetailID { get; set; }

        public void OnGet()
        {
            var curr = Request.Query["CurrentID"];
            if (curr.ToString() != String.Empty)
            {
                _CurrentDetailID = curr.ToString();
            }
            else
            {
                _CurrentDetailID = "0";
            }
        }

        public async Task<IActionResult> OnPostLoadata(int id)
        {
            DataTable dt = new DataTable();
            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
            {
                dt = await confunc.ExecuteDataTable("[YYY_sp_Consultation_Anamnesis_Type_LoadDetail]", CommandType.StoredProcedure,
                  "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
            }
            return Content(Comon.DataJson.Datatable(dt));
        }
        public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                Consultation DataMain = JsonConvert.DeserializeObject<Consultation>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = await connFunc.ExecuteDataTable("[YYY_sp_Anamnesis_Type_Insert]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.NVarChar, DataMain.Name.Replace("'", "").Trim(),
                            "@Data", SqlDbType.NVarChar, DataMain.Data.Replace("'", "").Trim(),
                            "@Created_By", SqlDbType.Int, user.sys_userid
                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Trùng Tên Phác Đồ Điều Trị");
                            }
                            else
                            {
                                return Content("1");
                            }
                        }
                        else
                        {
                            return Content("1");
                        }
                    }
                }
                else {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase ()) {
                        await connFunc.ExecuteDataTable ("[YYY_sp_Anamnesis_Type_Update]", CommandType.StoredProcedure,
                            "@Name ", SqlDbType.NVarChar, DataMain.Name.Replace ("'", "").Trim (),
                            "@Data ", SqlDbType.NVarChar, DataMain.Data.Replace ("'", "").Trim (),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@CurrentID", SqlDbType.Int, CurrentID
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
    public class Consultation
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
