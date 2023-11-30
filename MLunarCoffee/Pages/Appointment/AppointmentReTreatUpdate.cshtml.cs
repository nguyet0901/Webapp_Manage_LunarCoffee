using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Appointment
{
    public class AppointmentReTreatUpdateModel : PageModel
    { 
        public string _CustomerAppID { get; set; }

        public void OnGet()
        {
            string cus = Request.Query["CustomerID"];  
            if (cus != null)
            {
                _CustomerAppID = cus.ToString(); 
            }
            else
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }
 
         public async Task<IActionResult> OnPostLoadDataDetail(string CustomerID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("[MLU_sp_Customer_Appointment_ReTreat_Load]", CommandType.StoredProcedure
                        , "@Customer_ID", SqlDbType.Int, Convert.ToInt32(CustomerID)                        
                    );
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
            catch (Exception ex)
            {
                return Content("");
            }

        }
         
         public async Task<IActionResult> OnPostExcuteData(string data, string Customer_ID)
        {
            try
            {
                DataTable dataDetail = new DataTable();
                dataDetail = JsonConvert.DeserializeObject<DataTable>(data);
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("date", typeof(DateTime));
                dt.Columns.Add("timeSpan", typeof(string));

                for (int i = 0; i < dataDetail.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = dataDetail.Rows[i]["ID"];
                    var dateFrom = DateTime.Parse(dataDetail.Rows[i]["DateFrom"].ToString());
                    dr["date"] = dateFrom;
                    dr["timeSpan"] = dataDetail.Rows[i]["RetreatCode"];

                    dt.Rows.Add(dr);
                }

                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    DataTable dt1 =await connFunc.ExecuteDataTable("MLU_sp_Customer_App_ReTreat_Update", CommandType.StoredProcedure,
                        "@Modified_By", SqlDbType.Int, user.sys_userid,
                        "@Customer_ID", SqlDbType.Int, Convert.ToInt32(Customer_ID),
                        "@data_table", SqlDbType.Structured, dt.Rows.Count > 0 ? dt : null
                    );
                    if (dt1 != null)
                    {
                        return Content(Comon.DataJson.Datatable(dt1));
                    }
                    else
                    {
                        return Content("");
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
    }
}
