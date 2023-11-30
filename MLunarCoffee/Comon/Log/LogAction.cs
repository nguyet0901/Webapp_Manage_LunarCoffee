namespace MLunarCoffee.Comon.Log
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using MLunarCoffee.Models;
    public static class LogAction
    {
        public static async Task<string> InsertLog(Log log)
        {
            try
            {
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataTable("MLU_sp_InsertLog" ,CommandType.StoredProcedure
                         ,"@BranchID" ,SqlDbType.Int ,log.BranchID
                         ,"@UserID" ,SqlDbType.Int ,log.UserID
                         ,"@CustomerID" ,SqlDbType.Int ,log.CustomerID
                         ,"@Type" ,SqlDbType.VarChar ,log.Type
                         ,"@Content" ,SqlDbType.NVarChar ,log.Content
                          ,"@Page" ,SqlDbType.NVarChar ,log.Page
                         ,"@Action" ,SqlDbType.VarChar ,log.Action
                         ,"@Value" ,SqlDbType.NVarChar ,log.Value
                         ,"@JsonContent" ,SqlDbType.NVarChar ,log.JsonContent
                         );
                }
                return "1";
            }
            catch (Exception)
            {
                return "0";
            }

        }
    }
}
