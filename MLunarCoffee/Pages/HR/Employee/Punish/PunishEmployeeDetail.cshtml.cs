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

namespace MLunarCoffee.Pages.HR.Employee.Punish
{
    public class PunishEmployeeDetailModel : PageModel
    {
        public string _DataCombo { get; set; }
        public string _PunishDetailCurrentID { get; set; }
        public string _dataEmployeePunish { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _PunishDetailCurrentID = curr.ToString();
            }
            else
            {
                _PunishDetailCurrentID = "0";
            }
        }

         public async Task<IActionResult> OnPostLoadInitialize(int CurrentID)
        {
            try
            {
                DataSet ds = new DataSet();
                //LoadUserEmployee
                var user = Session.GetUserSession(HttpContext);
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("MLU_sp_User_Employee_LoadCombo", CommandType.StoredProcedure);
                    dt.TableName = "ComboUserEmployee";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("MLU_sp_HR_Combo_Type_LoadType", CommandType.StoredProcedure);
                    dt.TableName = "ComboHRType";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    DataTable dt = new DataTable();
                    dt =await  confunc.ExecuteDataTable("MLU_sp_HR_Employee_Punish_LoadDetail", CommandType.StoredProcedure,
                     "@CurrentID", SqlDbType.Int, Convert.ToInt32(CurrentID == 0 ? 0 : CurrentID));
                    dt.TableName = "dataDetail";
                    ds.Tables.Add(dt);
                }

                return Content(Comon.DataJson.Dataset(ds));

            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
         public async Task<IActionResult> OnPostExcuteData(string data, string CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                punishDetail DataMain = JsonConvert.DeserializeObject<punishDetail>(data);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("MLU_sp_Employee_Punish_Insert", CommandType.StoredProcedure,
                            "@Employee_ID", SqlDbType.Int, DataMain.Employee_ID,
                            "@IsPunishMoney", SqlDbType.Int, DataMain.typeIsPunishMoney,
                            "@PunishTypeDetail", SqlDbType.Int, DataMain.PunishTypeDetail,
                            "@PunishAmount", SqlDbType.Decimal, DataMain.PunishAmount,
                            "@PunishDate", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.PunishDate),
                            "@Created_By", SqlDbType.Int, user.sys_userid
                        );

                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        await connFunc.ExecuteDataTable("MLU_sp_Employee_Punish_Update", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, CurrentID,
                            "@Modified_By", SqlDbType.Int, user.sys_userid,
                            "@Modified", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow(),
                            "@Employee_ID", SqlDbType.Int, DataMain.Employee_ID,
                            "@IsPunishMoney", SqlDbType.Int, DataMain.typeIsPunishMoney,
                            "@PunishTypeDetail", SqlDbType.Int, DataMain.PunishTypeDetail,
                            "@PunishAmount", SqlDbType.Decimal, DataMain.PunishAmount,
                            "@PunishDate", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(DataMain.PunishDate)
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
    public class punishDetail
    {
        public int Employee_ID { get; set; }
        public int typeIsPunishMoney { get; set; }
        public int PunishTypeDetail { get; set; }
        public decimal PunishAmount { get; set; }
        public string PunishDate { get; set; }
    }
}