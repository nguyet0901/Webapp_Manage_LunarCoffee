using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.GlobalStore;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Models;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CommissionController : ControllerBase
    {
        [Authorize]
        [HttpPost("RevenueSettings")]
        public async Task<IActionResult> RevenueSettings()
        {
            try
            {
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("[sp_v2_ReportVer1_Revenue_Prepare]", CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }

        #region // REVENUE DOCTOR
        [Authorize]
        [HttpPost("RevDoctor_LoadData")]
        public async Task<IActionResult> RevDoctor_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_doctortreat_total]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@branchID", SqlDbType.Int, comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        [Authorize]
        [HttpPost("RevDoctor_LoadDetail")]
        public async Task<IActionResult> RevDoctor_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_doctortreat]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@empid", SqlDbType.Int, comm.empid,
                            "@limit", SqlDbType.Int, comm.limit,
                            "@branchID", SqlDbType.Int, comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        #endregion

        #region // REVENUE DOCTOR TREATMENT
        [Authorize]
        [HttpPost("RevDoctorTreat_LoadData")]
        public async Task<IActionResult> RevDoctorTreat_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_docnomoney_total]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@branchID", SqlDbType.Int, comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        [Authorize]
        [HttpPost("RevDoctorTreat_LoadDetail")]
        public async Task<IActionResult> RevDoctorTreat_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_docnomoney]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@empid", SqlDbType.Int, comm.empid,
                            "@limit", SqlDbType.Int, comm.limit,
                            "@branchID", SqlDbType.Int, comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        #endregion

        #region // REVENUE DOCTOR INVOICE TREATMENT
        [Authorize]
        [HttpPost("RevDoctorInvoiceTreat_LoadData")]
        public async Task<IActionResult> RevDoctorInvoiceTreat_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to " ,"@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_doctorInvoicetreat_total]" ,CommandType.StoredProcedure ,
                            "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                            "@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo) ,
                            "@branchID" ,SqlDbType.Int ,comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        [Authorize]
        [HttpPost("RevDoctorInvoiceTreat_LoadDetail")]
        public async Task<IActionResult> RevDoctorInvoiceTreat_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to " ,"@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_doctorInvoicetreat]" ,CommandType.StoredProcedure ,
                            "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                            "@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo) ,
                            "@empid" ,SqlDbType.Int ,comm.empid ,
                            "@limit" ,SqlDbType.Int ,comm.limit ,
                            "@branchID" ,SqlDbType.Int ,comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        #endregion

        #region // REVENUE ASSISTANT
        [Authorize]
        [HttpPost("RevAssistant_LoadData")]
        public async Task<IActionResult> RevAssistant_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);

                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_assistreatmoney_total]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@branchID", SqlDbType.Int, comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
        [Authorize]
        [HttpPost("RevAssistant_LoadDetail")]
        public async Task<IActionResult> RevAssistant_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);

                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_assistreatmoney]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@empid", SqlDbType.Int, comm.empid,
                            "@limit", SqlDbType.Int, comm.limit,
                            "@branchID", SqlDbType.Int, comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        #endregion

        #region // REVENUE ASSISTANT TREAT
        [Authorize]
        [HttpPost("RevAssistantTreat_LoadData")]
        public async Task<IActionResult> RevAssistantTreat_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_assisttreat_total]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@branchID", SqlDbType.Int, comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        [Authorize]
        [HttpPost("RevAssistantTreat_LoadDetail")]
        public async Task<IActionResult> RevAssistantTreat_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_assisttreat]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@empid", SqlDbType.Int, comm.empid,
                            "@limit", SqlDbType.Int, comm.limit,
                            "@branchID", SqlDbType.Int, comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        #endregion

        #region // REVENUE CONSULT
        [Authorize]
        [HttpPost("RevConsult_LoadData")]
        public async Task<IActionResult> RevConsult_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_consult_total]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@branchID", SqlDbType.Int, comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        [Authorize]
        [HttpPost("RevConsult_LoadDetail")]
        public async Task<IActionResult> RevConsult_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_consult]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@empid", SqlDbType.Int, comm.empid,
                            "@limit", SqlDbType.Int, comm.limit,
                            "@branchID", SqlDbType.Int, comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        #endregion

        #region // REVENUE CSKH
        [Authorize]
        [HttpPost("RevCSKH_LoadData")]
        public async Task<IActionResult> RevCSKH_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_cskh_total]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@branchID", SqlDbType.Int, comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        [Authorize]
        [HttpPost("RevCSKH_LoadDetail")]
        public async Task<IActionResult> RevCSKH_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_cskh]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@empid", SqlDbType.Int, comm.empid,
                            "@limit", SqlDbType.Int, comm.limit,
                            "@branchID", SqlDbType.Int, comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        #endregion

        #region //REVENUE TELESALE
        [Authorize]
        [HttpPost("RevTele_LoadData")]
        public async Task<IActionResult> RevTele_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_telesale_total]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@branchID", SqlDbType.Int, comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        [Authorize]
        [HttpPost("RevTele_LoadDetail")]
        public async Task<IActionResult> RevTele_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_telesale]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@empid", SqlDbType.Int, comm.empid,
                            "@limit", SqlDbType.Int, comm.limit,
                            "@branchID", SqlDbType.Int, comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }

        #endregion

        #region // REVENUE SUPPORT
        [Authorize]
        [HttpPost("RevSupport_LoadData")]
        public async Task<IActionResult> RevSupport_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_supporttreat_total]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@branchID", SqlDbType.Int, comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        [Authorize]
        [HttpPost("RevSupport_LoadDetail")]
        public async Task<IActionResult> RevSupport_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to ", "@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_supporttreat]", CommandType.StoredProcedure,
                            "@datefrom", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateFrom),
                            "@dateto", SqlDbType.DateTime, Comon.Comon.DateTimeDMY_To_YMD(dateTo),
                            "@empid", SqlDbType.Int, comm.empid,
                            "@limit", SqlDbType.Int, comm.limit,
                            "@branchID", SqlDbType.Int, comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        #endregion

        #region // REVENUE MANUAL
        [Authorize]
        [HttpPost("RevManual_LoadData")]
        public async Task<IActionResult> RevManual_LoadData(CommissionLoad comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to " ,"@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_commissionmanual_total]" ,CommandType.StoredProcedure ,
                            "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                            "@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo) ,
                            "@branchID" ,SqlDbType.Int ,comm.branch);

                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        [Authorize]
        [HttpPost("RevManual_LoadDetail")]
        public async Task<IActionResult> RevManual_LoadDetail(CommissionDetail comm)
        {
            try
            {
                string dateFrom = "";
                string dateTo = "";
                if (comm.date.Contains(" to "))
                {
                    comm.date = comm.date.Replace(" to " ,"@");
                    dateFrom = comm.date.Split('@')[0];
                    dateTo = comm.date.Split('@')[1];
                }
                else
                {
                    dateFrom = comm.date;
                    dateTo = comm.date;
                }
                var date_From = Comon.Comon.DateTimeDMY_To_YMD(dateFrom);
                var date_To = Comon.Comon.DateTimeDMY_To_YMD(dateTo);
                var totalDate = (date_To - date_From).TotalDays;
                if (totalDate > comm.maxdate)
                {
                    return Content("0");
                }
                else
                {
                    DataSet ds = new DataSet();
                    var user = Session.GetUserSession(HttpContext);
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {
                        ds = await confunc.ExecuteDataSet("[sp_reportv3_commissionmanual]" ,CommandType.StoredProcedure ,
                            "@datefrom" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateFrom) ,
                            "@dateto" ,SqlDbType.DateTime ,Comon.Comon.DateTimeDMY_To_YMD(dateTo) ,
                            "@empid" ,SqlDbType.Int ,comm.empid ,
                            "@limit" ,SqlDbType.Int ,comm.limit ,
                            "@branchID" ,SqlDbType.Int ,comm.branch);
                    }
                    return Content(Comon.DataJson.Dataset(ds));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        #endregion

    }
}
