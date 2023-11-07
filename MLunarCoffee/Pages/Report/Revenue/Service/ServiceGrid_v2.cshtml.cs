using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace SourceMain.Pages.Report.RevenuePayment.Service
{
    public class ServiceGrid_v2Model : PageModel
    {
        public string _dateFrom { get; set; }
        public string _dateTo { get; set; }
        public string _branch { get; set; }
        public string _SheetID { get; set; }
        public void OnGet()
        {
            //_dateFrom = Request.QueryString["dateFrom"].ToString();
            //_dateTo = Request.QueryString["dateTo"].ToString();
            _branch = Request.Query["branch"].ToString();
            _SheetID = Request.Query["sheet"].ToString();
        }

         public async Task<IActionResult> OnPostLoadata(int branchID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {

                    ds = await confunc.ExecuteDataSet("[ZZZ_sp_Report_Revenue_NewOld_Customer_Service_By_Month]", CommandType.StoredProcedure,
                        "@datefrom", SqlDbType.DateTime, Comon.Comon.GetDateTimeNow()
                        , "@BranchID", SqlDbType.NVarChar, Convert.ToInt32(branchID));

                }
                if (ds != null)
                {
                    string dateFrom = Comon.Comon.DateTimeYMD_To_DMY(Comon.Comon.GetDateTimeNow());
                    DataTable dtMain = ds.Tables[0].Copy();
                    DataTable dtService = ds.Tables[1].Copy();
                    DataTable dtDebtDays = ds.Tables[2].Copy();
                    DataTable dtAllDayofMonth = new DataTable();

                    dtAllDayofMonth.Columns.Add("Name", typeof(string));
                    dtAllDayofMonth.Columns.Add("Tháng " + Comon.Comon.DateTimeDMY_To_Month(dateFrom), typeof(decimal));
                    DataRow drr = dtAllDayofMonth.NewRow();
                    DataRow drr1 = dtAllDayofMonth.NewRow();
                    DataRow drr2 = dtAllDayofMonth.NewRow();
                    DataRow drr3 = dtAllDayofMonth.NewRow();
                    DataRow drr4 = dtAllDayofMonth.NewRow();
                    DataRow drr5 = dtAllDayofMonth.NewRow();
                    drr["Name"] = "Tổng";
                    drr1["Name"] = "Tiền Mặt";
                    drr2["Name"] = "Chuyển Khoản";
                    drr3["Name"] = "Pos";
                    drr4["Name"] = "Khác";
                    drr5["Name"] = "Nợ Ngày";

                    decimal Sum = 0;
                    decimal SumCash = 0;
                    decimal SumTransfer = 0;
                    decimal SumPos = 0;
                    decimal SumOther = 0;
                    decimal SumPrice = 0;

                    //Created column all day in month
                    for (var date = (DateTime)(Comon.Comon.DateTimeDMY_To_YMD_FirstDay(dateFrom)); date.Month == Comon.Comon.DateTimeDMY_To_Month(dateFrom); date = date.AddDays(1))
                    {
                        string day = Comon.Comon.DateTimeYMD_To_DMY(date);
                        dtAllDayofMonth.Columns.Add(day, typeof(decimal));
                    }

                    decimal TotalAmount = 0;
                    for (var j = 0; j < dtService.Rows.Count; j++)
                    {
                        DataRow dr = dtAllDayofMonth.NewRow();
                        int ServiceTypeID = Convert.ToInt32(dtService.Rows[j]["ServiceTypeID"].ToString());
                        int Type = Convert.ToInt32(dtService.Rows[j]["Type"].ToString());
                        dr["Name"] = dtService.Rows[j]["ServiceTypeName"].ToString();

                        DataTable dtt = new DataTable();
                        dtt = GetDataTableService(dtMain, ServiceTypeID, Type);

                        for (var m = 0; m < dtt.Rows.Count; m++)
                        {
                            string created = dtt.Rows[m]["Created"].ToString();
                            decimal amount = Convert.ToDecimal(dtt.Rows[m]["Paid"].ToString());
                            decimal Price = 0;
                            decimal TotalPaid = 0;

                            foreach (DataColumn column in dtAllDayofMonth.Columns)
                            {
                                if (column.ColumnName == created)
                                {

                                    if (dr[column].ToString() != "")
                                    {
                                        dr[column] = Convert.ToDecimal(dr[column].ToString()) + amount;
                                    }
                                    else
                                    {
                                        dr[column.ColumnName] = amount;
                                    }
                                    Sum += amount;
                                    if (drr[column].ToString() != "")
                                    {
                                        drr[column] = Convert.ToDecimal(drr[column].ToString()) + amount;
                                    }
                                    else
                                    {
                                        drr[column] = amount;
                                    }
                                    switch (dtt.Rows[m]["Method"].ToString())
                                    {
                                        case "3":
                                            drr1[column] = Convert.ToDecimal(((drr1[column].ToString() != "") ? Convert.ToDecimal(drr1[column].ToString()) : (0))) + amount;
                                            SumCash = SumCash + amount;
                                            break;
                                        case "4":
                                            drr2[column] = Convert.ToDecimal(((drr2[column].ToString() != "") ? Convert.ToDecimal(drr2[column].ToString()) : (0))) + amount;
                                            SumTransfer = SumTransfer + amount;
                                            break;
                                        case "5":
                                            drr3[column] = Convert.ToDecimal(((drr3[column].ToString() != "") ? Convert.ToDecimal(drr3[column].ToString()) : (0))) + amount;
                                            SumPos = SumPos + amount;
                                            break;
                                        default:
                                            drr4[column] = Convert.ToDecimal(((drr5[column].ToString() != "") ? Convert.ToDecimal(drr4[column].ToString()) : (0))) + amount;
                                            SumOther = SumOther + amount;
                                            break;
                                    }
                                    for (var n = 0; n < dtDebtDays.Rows.Count; n++)
                                    {
                                        if (column.ColumnName == dtDebtDays.Rows[n]["Created"].ToString())
                                        {
                                            Price = Convert.ToDecimal(dtDebtDays.Rows[n]["Price"].ToString());
                                            TotalPaid = Convert.ToDecimal(dtDebtDays.Rows[n]["Paid"].ToString());
                                            if (drr5[column].ToString() == "")
                                            {
                                                SumPrice = SumPrice + (Price - TotalPaid);
                                            }
                                            drr5[column] = Convert.ToDecimal(Price - TotalPaid);

                                        }
                                    }
                                    TotalAmount = TotalAmount + amount;
                                }
                                else if (column.ColumnName != "Name")
                                {
                                    if (dr[column].ToString() == "")
                                        dr[column.ColumnName] = 0;
                                }
                            }
                        }
                        dr["Tháng " + Comon.Comon.DateTimeDMY_To_Month(dateFrom)] = Sum;
                        Sum = 0;
                        dtAllDayofMonth.Rows.Add(dr);
                    }
                    drr[1] = TotalAmount;
                    drr1["Tháng " + Comon.Comon.DateTimeDMY_To_Month(dateFrom)] = SumCash;
                    drr2["Tháng " + Comon.Comon.DateTimeDMY_To_Month(dateFrom)] = SumTransfer;
                    drr3["Tháng " + Comon.Comon.DateTimeDMY_To_Month(dateFrom)] = SumPos;
                    drr4["Tháng " + Comon.Comon.DateTimeDMY_To_Month(dateFrom)] = SumOther;
                    drr5["Tháng " + Comon.Comon.DateTimeDMY_To_Month(dateFrom)] = SumPrice;
                    dtAllDayofMonth.Rows.Add(drr1);
                    dtAllDayofMonth.Rows.Add(drr2);
                    dtAllDayofMonth.Rows.Add(drr3);
                    dtAllDayofMonth.Rows.Add(drr4);
                    dtAllDayofMonth.Rows.Add(drr5);
                    dtAllDayofMonth.Rows.InsertAt(drr, 0);


                    ds.Tables.AddRange(new DataTable[] { dtAllDayofMonth });
                    return Content(JsonConvert.SerializeObject(ds, Formatting.Indented));
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
        private static DataTable GetDataTableService(DataTable dt, int ServiceTypeID, int Type)
        {
            try
            {
                var resultRow = from myRow in dt.AsEnumerable()
                                where myRow.Field<int>("ServiceTypeID") == ServiceTypeID && myRow.Field<int>("Type") == Type
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                return dtResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
