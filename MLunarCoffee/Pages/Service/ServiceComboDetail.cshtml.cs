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


namespace MLunarCoffee.Pages.Service
{
    public class ServiceComboDetailModel : PageModel
    {
        public int _Dentistcosmetic { get; set; }
        public int _ServiceComboID { get; set; }
        public void OnGet()
        {
            try
            {
                string v = Request.Query["ServiceComboID"];
                if (v != null)
                {
                    _ServiceComboID = Convert.ToInt32(v.ToString());
                }
                else
                {
                    _ServiceComboID = 0;
                }
                _Dentistcosmetic = Global.sys_DentistCosmetic;
            }
            catch (Exception ex)
            {
                Response.Redirect("/assests/Error/index.html");
            }
        }
        public async Task<IActionResult> OnPostLoadInit()
        {
            try
            {
                DataSet ds = new DataSet();
                var user = Session.GetUserSession(HttpContext);
                var tasks = new List<Task<DataTable>>();
                tasks.Add(Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await confunc.ExecuteDataTable("YYY_sp_ServiceCombo_LoadService" ,CommandType.StoredProcedure
                        );
                        dt.TableName = "service";
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
                return Content("0");
            }
        }
        public async Task<IActionResult> OnPostDeleteItem(int id)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[YYY_sp_ServiceCombo_Delete]" ,CommandType.StoredProcedure ,
                         "@UserID" ,SqlDbType.Int ,user.sys_userid ,
                         "@ComboID" ,SqlDbType.Int ,id);
                }
                return Content(Comon.DataJson.GetFirstValue(dt));
            }

            catch (Exception ex)
            {
                return Content("0");
            }

        }

        public async Task<IActionResult> OnPostLoadataDetail(int ID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("[YYY_sp_Service_Combo_LoadDetail]", CommandType.StoredProcedure
                        , "@ID", SqlDbType.Int, ID);
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
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
         public async Task<IActionResult> OnPostExcuteData(int ComboServiceID, string ComboName, string Code, string Note, string Data,string ServiceFree, string Ruler,int ComboType)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (ComboServiceID == 0)
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_ServiceCombo_Insert]", CommandType.StoredProcedure,
                            "@ComboName", SqlDbType.NVarChar, ComboName,
                            "@ComboType" , SqlDbType.Int,ComboType ,
                            "@InfoService" , SqlDbType.NVarChar, Data,
                            "@Code", SqlDbType.NVarChar, Code,
                            "@Note", SqlDbType.NVarChar, Note,
                            "@ServiceFree" , SqlDbType.NVarChar,ServiceFree,
                            "@Ruler" , SqlDbType.Int,Ruler,
                            "@CreatedBy" , SqlDbType.Int, user.sys_userid
                         );
                    }
                }
                else
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("[YYY_sp_ServiceCombo_Update]", CommandType.StoredProcedure,
                            "@CurrentID", SqlDbType.Int, ComboServiceID,
                            "@Code", SqlDbType.NVarChar, Code,
                            "@ComboName", SqlDbType.NVarChar, ComboName,
                            "@ComboType" ,SqlDbType.Int ,ComboType ,
                            "@InfoService" , SqlDbType.NVarChar, Data,
                            "@Note", SqlDbType.NVarChar, Note,
                            "@ServiceFree" ,SqlDbType.NVarChar ,ServiceFree,
                            "@Ruler" ,SqlDbType.Int ,Ruler,
                            "@ModifiedBy" , SqlDbType.Int, user.sys_userid
                         );
                    }
                }

                return Content(Comon.DataJson.GetFirstValue(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }


        }
    }
}
