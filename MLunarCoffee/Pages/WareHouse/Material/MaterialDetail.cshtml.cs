using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.WareHouse.Material
{
    public class MaterialDetailModel : PageModel
    {
        public string _ProductDetailCurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _ProductDetailCurrentID = curr.ToString();
            }
            else
            {
                _ProductDetailCurrentID = "0";
            }

        }

        public async Task<IActionResult> OnPostMaterial_Load_Combo()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_Product_Combo_Unit", CommandType.StoredProcedure);
                    dt.TableName = "Unit";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_Product_Combo_UnitFull", CommandType.StoredProcedure);
                    dt.TableName = "UnitFull";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt =await connFunc.ExecuteDataTable("MLU_sp_Product_Combo_Product_Type", CommandType.StoredProcedure);
                    dt.TableName = "Type";
                    ds.Tables.Add(dt);
                }
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("[MLU_sp_v2_Material_Get_Syntax_Code]", CommandType.StoredProcedure);
                    dt.TableName = "SyntaxCode";
                    ds.Tables.Add(dt);
                }
                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostLoadUnit()
        {
            try
            {

                DataTable dt = new DataTable();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    dt = await connFunc.ExecuteDataTable("MLU_sp_Product_Combo_UnitFull", CommandType.StoredProcedure);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception)
            {
                return Content("0");
            }
        }



        public async Task<IActionResult> OnPostLoad_Data(string Currentid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                {
                    ds =await connFunc.ExecuteDataSet("[MLU_sp_v2_Material_LoadDetail]"
                        , CommandType.StoredProcedure, "@Currentid", SqlDbType.NVarChar, Currentid);
                }

                return Content(Comon.DataJson.Dataset(ds));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public async Task<IActionResult> OnPostExcuteData(string data, string syntax_code, string CurrentID)
        {
            try
            {
                DataProduct DataMain = JsonConvert.DeserializeObject<DataProduct>(data);
                DataTable DataUnitChange = new DataTable();
                DataTable dt = new DataTable();
                DataUnitChange = Get_Table_In_UnitChange(DataMain.UnitChange);
                var user = Session.GetUserSession(HttpContext);
                if (CurrentID == "0")
                {
                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_v2_Material_Insert", CommandType.StoredProcedure,
                            "@Avatar", SqlDbType.NVarChar, DataMain.Avatar,
                            "@ProductCode", SqlDbType.Int, DataMain.ProductCode.Replace("'", "").Trim(),
                            "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                            "@Type", SqlDbType.Int, DataMain.Type,
                            "@Unit", SqlDbType.Int, DataMain.DefaultUnit,
                            "@N1", SqlDbType.Decimal, DataMain.N1,
                            "@N2", SqlDbType.Decimal, DataMain.N2,
                            "@N3", SqlDbType.Decimal, DataMain.N3,
                            "@Price", SqlDbType.Decimal, DataMain.Price,
                            "@Property", SqlDbType.NVarChar, DataMain.Property,
                            "@Description", SqlDbType.NVarChar, DataMain.Description,
                            "@DataUnitChange", SqlDbType.Structured, DataUnitChange.Rows.Count > 0 ? DataUnitChange : null,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@syntax_code", SqlDbType.NVarChar, syntax_code,
                            "@SalePrice", SqlDbType.Int, DataMain.SalePrice
                        );
                    }
                }
                else
                {

                    using (Models.ExecuteDataBase connFunc = new Models.ExecuteDataBase())
                    {
                        dt = await connFunc.ExecuteDataTable("MLU_sp_v2_Material_Update", CommandType.StoredProcedure,
                            "@Avatar", SqlDbType.NVarChar, DataMain.Avatar,
                            "@ProductCode", SqlDbType.Int, DataMain.ProductCode.Replace("'", "").Trim(),
                            "@Name", SqlDbType.Int, DataMain.Name.Replace("'", "").Trim(),
                            "@Type", SqlDbType.Int, DataMain.Type,
                            "@Unit", SqlDbType.Int, DataMain.DefaultUnit,
                            "@N1", SqlDbType.Decimal, DataMain.N1,
                            "@N2", SqlDbType.Decimal, DataMain.N2,
                            "@N3", SqlDbType.Decimal, DataMain.N3,
                            "@Price", SqlDbType.Decimal, DataMain.Price,
                            "@Property", SqlDbType.NVarChar, DataMain.Property,
                            "@Description", SqlDbType.NVarChar, DataMain.Description,
                            "@DataUnitChange", SqlDbType.Structured, DataUnitChange.Rows.Count > 0 ? DataUnitChange : null,
                            "@Created_By", SqlDbType.Int, user.sys_userid,
                            "@syntax_code", SqlDbType.NVarChar, syntax_code,
                            "@Currentid", SqlDbType.Int, CurrentID,
                            "@SalePrice" ,SqlDbType.Int ,DataMain.SalePrice
                        );
                    }
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        private static DataTable Get_Table_In_UnitChange(string dtunit)
        {
            try
            {
                DataTable newDataTable = new DataTable();
                newDataTable.Columns.Add("UnitID", typeof(string));
                newDataTable.Columns.Add("Number", typeof(decimal));

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(dtunit);
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = newDataTable.NewRow();
                    dr["UnitID"] = Convert.ToInt32(dt.Rows[i]["ID"]);
                    dr["Number"] = Convert.ToDecimal(dt.Rows[i]["Number"].ToString());
                    newDataTable.Rows.Add(dr);
                }
                return newDataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public class DataProduct
    {
        public string Avatar { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string DefaultUnit { get; set; }
        public string N1 { get; set; }
        public string N2 { get; set; }
        public string N3 { get; set; }
        public Decimal Price { get; set; }
        public string Property { get; set; }
        public string Description { get; set; }
        public string UnitChange { get; set; }
        public Decimal SalePrice { get; set; }
    }
}
