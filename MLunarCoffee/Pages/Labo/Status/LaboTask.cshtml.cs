using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Labo.Status
{
    public class LaboTask : PageModel
    {
        public string _CurrentID { get; set; }
         public string _Laboid { get; set; }
        public void OnGet()
        {
            var curr = Request.Query["CurrentID"];
            _CurrentID = curr.ToString();
             var laboid = Request.Query["LaboID"];
            _Laboid = laboid.ToString();
        }

        public async Task<IActionResult> OnPostLoadData(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[MLU_sp_Labo_LoadDetail_Task]", CommandType.StoredProcedure
                        , "@CurrentID", SqlDbType.Int, CurrentID);
                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostSave(int CurrentID, int LaboID, string emp, string task, string note)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                if (CurrentID == 0)
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {

                        dt = await confunc.ExecuteDataTable("[MLU_sp_Labo_TaskInsert]", CommandType.StoredProcedure
                             , "@LaboID", SqlDbType.Int, LaboID
                             , "@emp", SqlDbType.Int, emp
                             , "@task", SqlDbType.Int, task
                             , "@note", SqlDbType.NVarChar , note != null ? note.ToString() : ""
                             , "@CreatedBy", SqlDbType.Int, user.sys_userid
                            );

                    }
                }
                else
                {
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {

                        dt = await confunc.ExecuteDataTable("[MLU_sp_Labo_TaskUpdate]", CommandType.StoredProcedure
                             , "@LaboID", SqlDbType.Int, LaboID
                             , "@emp", SqlDbType.Int, emp
                             , "@task", SqlDbType.Int, task
                             , "@note", SqlDbType.NVarChar, note != null ? note.ToString() : ""
                             , "@CreatedBy", SqlDbType.Int, user.sys_userid
                             , "@CurrentID", SqlDbType.Int, CurrentID
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
}
