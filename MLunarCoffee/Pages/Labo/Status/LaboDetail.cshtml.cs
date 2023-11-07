using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Labo.Status
{
    public class LaboDetailModel : PageModel
    {
        private readonly IHubContext<NotiHub> hubContext;
        public LaboDetailModel(IHubContext<NotiHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLoadData(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[YYY_sp_Labo_LoadDetail_Info]" ,CommandType.StoredProcedure
                        ,"@CurrentID" ,SqlDbType.Int ,CurrentID
                        );

                }
                if (ds != null)
                {
                    return Content(Comon.DataJson.Dataset(ds));
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
        public async Task<IActionResult> OnPostLoadCommand(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);

                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    DataSet ds = new DataSet();
                    ds = await confunc.ExecuteDataSet("[YYY_sp_print_v2_Labo]" ,CommandType.StoredProcedure
                         ,"@id" ,SqlDbType.Int ,Convert.ToInt32(CurrentID));
                    return Content(Comon.DataJson.Dataset(ds));

                }

            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostLoadData_SameService(int CurrentID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_LoadDetail_SaveService]" ,CommandType.StoredProcedure
                        ,"@CurrentID" ,SqlDbType.Int ,CurrentID
                        );

                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

        public async Task<IActionResult> OnPostLoadData_Task(int LaboID ,int valueid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_LoadDetail_Value]" ,CommandType.StoredProcedure
                        ,"@LaboID" ,SqlDbType.Int ,LaboID
                         ,"@valueid" ,SqlDbType.Int ,valueid
                        );

                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostLoadData_LabStatus(int LaboID ,int valueid)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_LoadDetail_LaboStatus]" ,CommandType.StoredProcedure
                        ,"@LaboID" ,SqlDbType.Int ,LaboID
                         ,"@valueid" ,SqlDbType.Int ,valueid
                        );

                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }
        public async Task<IActionResult> OnPostLoadData_Upload(int LaboID)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    dt = await confunc.ExecuteDataTable("[YYY_sp_Labo_LoadDetailLoadUpload]" ,CommandType.StoredProcedure
                        ,"@LaboID" ,SqlDbType.Int ,LaboID
                        );

                }
                if (dt != null) return Content(Comon.DataJson.Datatable(dt));
                else return Content("[]");
            }
            catch (Exception ex)
            {
                return Content("[]");
            }

        }

    }
}
