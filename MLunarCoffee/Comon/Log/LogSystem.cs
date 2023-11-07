using System.Data;
using System.Threading.Tasks;
using System;
using MLunarCoffee.Models;

namespace MLunarCoffee.Comon.Log
{
    public class LogSystem
    {

        public static async Task LogSys(string Type ,string Action ,string Description ,string Exception)
        {
            try
            {
                await InsertLogSys(new LogSys()
                {
                    Action = Action ,
                    Type = Type ,
                    Exception = Exception ,
                    Description = Description ,
                    DateSystem = Comon.GetDateTimeNow()
                })
                .ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }
        }
        public static async Task InsertLogSys(LogSys log)
        {
            try
            {
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    await confunc.ExecuteDataTable("YYY_sp_SystemLog_Insert" ,CommandType.StoredProcedure
                         ,"@Type" ,SqlDbType.NVarChar ,log.Type
                         ,"@Action" ,SqlDbType.NVarChar ,log.Action
                         ,"@Exception" ,SqlDbType.NVarChar ,log.Exception
                         ,"@Description" ,SqlDbType.VarChar ,log.Description
                         ,"@DateSystem" ,SqlDbType.DateTime ,log.DateSystem);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
