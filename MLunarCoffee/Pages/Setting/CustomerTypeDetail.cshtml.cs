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

namespace MLunarCoffee.Pages.Setting
{
    public class CustomerTypeDetailModel : PageModel
    {
        public string _CustomerDetailCurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _CustomerDetailCurrentID = curr.ToString();
            }
            else
            {
                _CustomerDetailCurrentID = "0";
            }
        }
         public async Task<IActionResult> OnPostLoadata(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt =await  confunc.ExecuteDataTable("MLU_sp_Customer_Type_LoadDetail", CommandType.StoredProcedure,
                      "@ID", SqlDbType.Int, Convert.ToInt32(id == 0 ? 0 : id));
                }
                if (dt != null)
                {
                    return Content(JsonConvert.SerializeObject(dt));
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

         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                CustomerTypeDetail DataMain = JsonConvert.DeserializeObject<CustomerTypeDetail>(data);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Customer_Type_Insert]", CommandType.StoredProcedure,
                       "@Name ", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                       "@Note ", SqlDbType.Int, DataMain.Note.Replace("'", "").Trim(),
                       "@Created_By", SqlDbType.Int, user.sys_userid



                   );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tên Hoặc Mã Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt =await connFunc.ExecuteDataTable("[MLU_sp_Customer_Type_Update]", CommandType.StoredProcedure,
                            "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@currentID ", SqlDbType.Int, CurrentID,
                            "@Note ", SqlDbType.Int, DataMain.Note.Replace("'", "").Trim()

                        );
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString() != "0")
                            {
                                return Content("Tên Hoặc Mã Đã Tồn Tại Vui Lòng Kiểm Tra Lại");
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

            }
            catch (Exception ex)
            {
               return Content("0");
            }


        }
        public class CustomerTypeDetail
        {
            public string Name { get; set; }
            public string Note { get; set; }
        }
    }
}
