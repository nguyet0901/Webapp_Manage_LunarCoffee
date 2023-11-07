using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;

namespace MLunarCoffee.Comon
{
    public class DataToJson : IDisposable
    {
        public StringBuilder jsonResult { get; protected set; }
        public DataTable dtRef { get; protected set; }
        public DataSet dsRef { get; protected set; }
        public List<Dictionary<string, object>> listData { get; protected set; }
        private string TABLENAME = "Table";
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dtRef != null)
                {
                    dtRef.Clear();
                    dtRef.Dispose();
                }
                if (dsRef != null)
                {
                    dsRef.Clear();
                    dsRef.Dispose();
                }
                listData = null;
                jsonResult = null;
            }
        }
        public DataToJson(ref DataTable dataTable)
        {
            dtRef = dataTable;
            dataTable = null;
        }
        public DataToJson(ref DataSet dataSet)
        {
            dsRef = dataSet;
            dataSet = null;
        }
        public string DataTableToJSON()
        {
            try
            {
                listData = new List<Dictionary<string, object>>();
                foreach (DataRow row in dtRef.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn col in dtRef.Columns)
                    {
                        dict[col.ColumnName] = row[col].GetType().FullName != "System.DBNull" ? row[col] : "";
                    }
                    listData.Add(dict);
                }
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                {
                    IncludeFields = true
                };
                return JsonSerializer.Serialize(listData, jsonSerializerOptions);
            }
            catch (Exception)
            {
                return "[]";
            }
        }
        private string DataTableToJSON(DataTable dt)
        {
            try
            {
                listData = new List<Dictionary<string, object>>();
                foreach (DataRow row in dt.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        dict[col.ColumnName] = row[col].GetType().FullName != "System.DBNull" ? row[col] : "";
                    }
                    listData.Add(dict);
                }
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                {
                    IncludeFields = true
                };
                return JsonSerializer.Serialize(listData, jsonSerializerOptions);
            }
            catch (Exception)
            {
                return "[]";
            }
        }
        public string DataSetToJSON()
        {
            try
            {
                if (dsRef.Tables.Count == 0) return "[]";
                jsonResult = new StringBuilder();
                jsonResult.Append("{");
                for (int i = 0; i < dsRef.Tables.Count; i++)
                {
                    var tableDataSet = dsRef.Tables[i];
                    string tbName = tableDataSet.TableName != null
                        ? (tableDataSet.TableName).ToString()
                        : i == 0 ? TABLENAME : TABLENAME + (i + 1).ToString();
                    jsonResult.Append("\"" + tbName + "\":");
                    jsonResult.Append(DataTableToJSON(tableDataSet));
                    if (i + 1 != dsRef.Tables.Count) jsonResult.Append(",");
                }
                jsonResult.Append("}");
                return jsonResult.ToString();
            }
            catch (Exception)
            {
                return "[]";
            }
        }
        public string GetFirstValue()
        {
            try
            {
                if (dtRef == null) return "";
                return dtRef.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        public string GetValueRowProperty(int row, string column)
        {
            try
            {
                if (dtRef == null) return "";
                return dtRef.Rows[row][column].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        public string GetValueRowProperty(int row, int column)
        {
            try
            {
                if (dtRef == null) return "";
                return dtRef.Rows[row][column].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    public static class DataJson
    {
        public static string Datatable(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return "[]";
            using (var jsonConvert = new DataToJson(ref dt))
            {
                return jsonConvert.DataTableToJSON();
            }
        }
        public static string Dataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return "[]";
            using (var jsonConvert = new DataToJson(ref ds))
            {
                return jsonConvert.DataSetToJSON();
            }
        }
        public static string GetFirstValue(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return "[]";
            using (var jsonConvert = new DataToJson(ref dt))
            {
                return jsonConvert.GetFirstValue();
            }
        }
        public static string GetValueRowProperty(DataTable dt, int row, string column)
        {
            if (dt == null || dt.Rows.Count == 0) return "[]";
            using (var jsonConvert = new DataToJson(ref dt))
            {
                return jsonConvert.GetValueRowProperty(row, column);
            }
        }
        public static string GetValueRowProperty(DataTable dt, int row, int column)
        {
            if (dt == null || dt.Rows.Count == 0) return "[]";
            using (var jsonConvert = new DataToJson(ref dt))
            {
                return jsonConvert.GetValueRowProperty(row, column);
            }
        }
    }
}

