using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MLunarCoffee.Models;

namespace MLunarCoffee.Comon.AfterStatus
{
    public static class AfterStatus
    {
        public async static Task<SMSTemplateResultMulti> getTreatInDay(string CustomerID)
        {
            try
            {
                List<SMSMareach> items = new List<SMSMareach>();
                int IsZNS = 0;
                await Task.Run(async () =>
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        DataTable dt = new DataTable();
                        dt = await connFunc.ExecuteDataTable("YYY_sp_Auto_AfterStatus_LoadTreatInDayTemp" ,CommandType.StoredProcedure
                           ,"@CustID" ,SqlDbType.Int ,CustomerID);
                        if (dt != null && dt.Rows.Count != 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var dr = dt.Rows[i];
                                Dictionary<string ,string> mainKey = new Dictionary<string ,string>();
                                mainKey["CustCode"] = Convert.ToString(dr["CustCode"]);
                                mainKey["CustName"] = Convert.ToString(dr["CustName"]);
                                mainKey["Date"] = Convert.ToString(dr["DateNow"]);
                                mainKey["AppNextDate"] = !Convert.ToString(dr["AppNextDate"]).Contains("1900") ? Convert.ToString(dr["AppNextDate"]) : "CSKH sẽ liên hệ";
                                mainKey["ServiceName"] = Convert.ToString(dr["ServiceName"]);
                                mainKey["TreatProcess"] = getTreatProcess(Convert.ToDouble(dr["TreatPercentDetail"]) ,Convert.ToDouble(dr["PercentOfService"]) ,Convert.ToString(dr["TeethChoosing"]) ,Convert.ToInt32(dr["Quantity"]) ,Convert.ToInt32(dr["TreatIndex"]) ,Convert.ToInt32(dr["TimeToTreatment"]));
                                mainKey["PriceDiscounted"] = Convert.ToString(dr["PriceDiscounted"]) != "" ? Convert.ToString(dr["PriceDiscounted"]) : "0"; ; // Service Price 
                                mainKey["PayInday"] = Convert.ToString(dr["PayInday"]) != "" ? Convert.ToString(dr["PayInday"]) : "0"; ; // Paid in day
                                mainKey["ContentSMS"] = Convert.ToString(dr["ContentSMS"]); ; // Content

                                string contenttemplate = mainKey["ContentSMS"];
                                string dataSend = Comon.GetRepTemp(mainKey ,contenttemplate);
                                if (dataSend != "")
                                {
                                    string content = dataSend;
                                    string phone = Convert.ToString(dr["Phone"]);
                                    IsZNS = Convert.ToInt16(dr["IsZNS"]);
                                    if (phone.Length >= 9 && phone.Length <= 11)
                                    {
                                        items.Add(new SMSMareach()
                                        {
                                            Phone = phone ,
                                            Content = content
                                        });
                                    }
                                }
                            }

                        }
                    }

                });
                var dataResult = new SMSTemplateResultMulti
                {
                    SmsItem = items.ToArray() ,
                    IsZNS = IsZNS
                };
                return dataResult;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string getTreatProcess(double Treat_Percent_Detail ,double PercentOfService ,string TeethChoosing ,int Quantity ,int Treat_Index ,int TimeToTreatment)
        {
            try
            {
                string result = "";
                int DentistCosmetic = Convert.ToInt16(Global.sys_DentistCosmetic);
                if (DentistCosmetic == 1)

                {
                    if (Treat_Percent_Detail == 0)
                    {
                        result = PercentOfService.ToString() + "%";
                    }
                    else
                    {
                        float deno = TeethChoosing != "" ? Quantity : 1;
                        result = deno != 0 ? (Treat_Percent_Detail / deno).ToString() + "%" : "0%";
                    }

                }
                else
                {
                    result = $"{Treat_Index.ToString()}/{TimeToTreatment.ToString()}";
                }
                return result;
            }
            catch (Exception e)
            {
                return "";
            }
        }

    }
}
