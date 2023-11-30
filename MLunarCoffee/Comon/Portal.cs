using System;
using System.Data;
using System.Threading.Tasks;
using MLunarCoffee.Models.Portal;

namespace MLunarCoffee.Comon
{
    public class Portal
    {
        public async static Task<DataTable> excuteRating(Ratingus rat)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtDetail = new DataTable();
                dtDetail.Columns.Add("Type" ,typeof(Int32));
                dtDetail.Columns.Add("EmpID" ,typeof(Int32));
                dtDetail.Columns.Add("Note" ,typeof(String));
                dtDetail.Columns.Add("Star" ,typeof(Decimal));
                for (int i = 0; i < rat.dtItem.Length; i++)
                {
                    DataRow dr = dtDetail.NewRow();
                    RatingusItem item = rat.dtItem[i];
                    dr["Type"] = Convert.ToInt32(item.type.ToString());
                    dr["EmpID"] = Convert.ToInt32(item.empid.ToString());
                    dr["Star"] = Convert.ToDecimal(item.star.ToString());
                    dr["Note"] = item.note.ToString();
                    dtDetail.Rows.Add(dr);
                }
                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt = await confunc.ExecuteDataTable("MLU_sp_PortalRatingInsert" ,CommandType.StoredProcedure ,
                         "@table_data" ,SqlDbType.Structured ,dtDetail.Rows.Count > 0 ? dtDetail : null ,
                         "@Customer" ,SqlDbType.Int ,rat.customer ,
                         "@BranchID" ,SqlDbType.Int ,rat.branchid
                        );
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
