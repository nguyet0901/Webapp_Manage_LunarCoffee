using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MLunarCoffee.Comon.Option_Extension
{
    public class Extension
    {

        public static async Task<bool> CheckCashierTime(int customerid, int branchid,string ruleinvoice)
        {
            try
            {
                if (ruleinvoice == "0") return true;
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_Extension_Check_Cashier]", CommandType.StoredProcedure
                        , "@CustomerID", SqlDbType.Int, customerid
                         , "@BranchID", SqlDbType.Int, branchid
                         , "@Time", SqlDbType.Int, ruleinvoice);
                }
                if (dt != null && dt.Rows.Count != 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["IsCashier"].ToString()) == 1)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["IsOutRange"].ToString()) == 1)
                            return false;
                        else return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else return true;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        //public static bool Check_Rule_Before_16_Allowed()
        //{
        //    try
        //    {
        //        if (Global.sys_AskRepresntPerson_InsertService_Lower16 == 0) return true;
        //        else return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return true;
        //    }
        //}

    }
}