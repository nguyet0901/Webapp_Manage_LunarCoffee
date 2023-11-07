using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MLunarCoffee.Comon.Log;
using MLunarCoffee.Models.ThirdParty.Account;

namespace MLunarCoffee.Comon.Accounting
{
    public class AccountingAction
    {

        public static async Task<int> UpdateVoucher(string dataVoucher)
        {
            try
            {
                DataTable dtVoucher = JsonConvert.DeserializeObject<dtAccountVoucher>(dataVoucher);
                if (dtVoucher != null && dtVoucher.Rows.Count > 0)
                {
                    DataTable dtMain = new DataTable();
                    dtMain.Columns.Add("ID" ,typeof(Int32));
                    dtMain.Columns.Add("Type" ,typeof(Int32));
                    dtMain.Columns.Add("MasterGuid" ,typeof(string));
                    DataTable dt = new DataTable();
                    foreach (DataRow item in dtVoucher.Rows)
                    {
                        DataRow dr = dtMain.NewRow();
                        dr["ID"] = item["ID"] ?? 0;
                        dr["Type"] = item["Type"] ?? 0;
                        dr["MasterGuid"] = item["MasterGuid"] ?? "";
                        dtMain.Rows.Add(dr);
                    }
                    using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                    {

                        dt = await confunc.ExecuteDataTable("YYY_sp_API_Account_Update" ,CommandType.StoredProcedure
                             ,"@data" ,SqlDbType.Structured ,dtMain
                             );
                    }
                    return 1;
                }
                return 0;
            }
            catch { return 0; }

        }

        public static async Task<int> WriteLog(int BranchID ,int UserID ,int CustID ,string Code ,string page ,string jsonContent ,string content ,string Action = "i")
        {
            try
            {
                var log = new Models.Log()
                {
                    BranchID = BranchID ,
                    UserID = UserID ,
                    CustomerID = CustID ,
                    Type = "mis" ,
                    Content = content ,
                    Page = page ,
                    Action = Action ,
                    Value = Code ,
                    JsonContent = jsonContent
                };
                await LogAction.InsertLog(log).ConfigureAwait(false);
                return 1;
            }
            catch { return 0; }

        }

        public static string getActionLog(string action ,int isUpdate)
        {
            switch (action)
            {
                case "save":
                    return isUpdate == 1 ? "u" : "i";
                case "del":
                    return "d";
            }
            return "";
        }

        public static async Task<int> PushVoucher(string type ,string action ,string token ,string jsonPara ,string BaseLink)
        {
            try
            {
                switch (type.ToLower(), action)
                {
                    case ("misa", "save"):
                        {
                            using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(BaseLink) })
                            {
                                clientautho.DefaultRequestHeaders.Add("X-MISA-AccessToken" ,token);
                                var content = new StringContent(jsonPara ,Encoding.UTF8 ,"application/json");
                                await Task.Run(async () => await clientautho.PostAsync("/api/sync/actopen/save" ,content).ConfigureAwait(false)).ConfigureAwait(false);
                            }
                            break;
                        }
                    case ("misa", "del"):
                        {
                            using (HttpClient clientautho = new HttpClient())
                            {
                                clientautho.DefaultRequestHeaders.Add("X-MISA-AccessToken" ,token);
                                using (HttpRequestMessage request = new HttpRequestMessage()
                                {
                                    Method = HttpMethod.Delete ,
                                    RequestUri = new Uri(BaseLink + "/api/sync/actopen/delete") ,
                                    Content = new StringContent(jsonPara ,Encoding.UTF8 ,"application/json")
                                })
                                {
                                    await Task.Run(async () => await clientautho.SendAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
                                }
                            }
                            break;
                        }
                }

                return 1;
            }
            catch { return 0; }

        }
    }
}
