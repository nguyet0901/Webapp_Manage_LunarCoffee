using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Comon
{
    public class FormPrint
    {
        public static string ConnectDataToJson(DataTable dtRow, DataTable dtElement)
        {
            Random rand = new Random();
            List<PaymentDesignRow> Rows = new List<PaymentDesignRow>();
            for (int i = 0; i < dtRow.Rows.Count; i++)
            {
                PaymentDesignRow row = new PaymentDesignRow();

                row.ID = dtRow.Rows[i]["ID"].ToString();
                row.RowRandom = dtRow.Rows[i]["RowRandom"].ToString();
                row.Column = dtRow.Rows[i]["Column"].ToString();
                row.IsTable = dtRow.Rows[i]["IsTable"].ToString();
                row.Margin = dtRow.Rows[i]["Margin"].ToString();
                row.Padding = dtRow.Rows[i]["Padding"].ToString();
                row.Border = dtRow.Rows[i]["Border"].ToString();
                row.BorderTop = dtRow.Rows[i]["BorderTop"].ToString();
                row.BorderLeft = dtRow.Rows[i]["BorderLeft"].ToString();
                row.BorderBottom = dtRow.Rows[i]["BorderBottom"].ToString();
                row.BorderRight = dtRow.Rows[i]["BorderRight"].ToString();
                row.ClassName = dtRow.Rows[i]["Class"].ToString();
                row.Cells = GetCells(dtElement, dtRow.Rows[i]["Value"].ToString(), dtRow.Rows[i]["IsTable"].ToString(), Convert.ToDouble(dtRow.Rows[i]["RowRandom"].ToString()));

                Rows.Add(row);
            }
            return JsonConvert.SerializeObject(Rows);
        }
        private static PaymentDesignCells[] GetCells(DataTable dtElement, string strdata, string isTable, double idRow)
        {
            var Value = JToken.Parse(strdata);
            var ListCell = Value.ToList<object>();
            PaymentDesignCells[] cells = new PaymentDesignCells[ListCell.Count];
            Random rand = new Random();
            for (int i = 0; i < ListCell.Count; i++)
            {
                PaymentDesignCells cell = new PaymentDesignCells();
                int Random = rand.Next(1, 1000);
                double timeStamp = Convert.ToDouble(DateTime.Now.ToString("HHmmssffff"));
                double numRandom = Random * timeStamp + idRow;

                cell.ID = i.ToString();
                cell.Random = numRandom.ToString();
                cell.IsTable = isTable;
                var style = Value[i.ToString()]["style"].ToString();
                string[] liststyle = style.Split(';');
                string padding = liststyle[0].Split(':')[1];
                string margin = liststyle[1].Split(':')[1];
                string width = liststyle[2].Split(':')[1];
                string bodertop = ((liststyle[3] != null) ? liststyle[3].Split(':')[1] : "");
                string boderleft = ((liststyle[4] != null) ? liststyle[4].Split(':')[1] : "");
                string boderbottom = ((liststyle[5] != null) ? liststyle[5].Split(':')[1] : "");
                string boderright = ((liststyle[6] != null) ? liststyle[6].Split(':')[1] : "");
                cell.Padding = padding;
                cell.Margin = margin;
                cell.Width = width;
                cell.BorderTop = bodertop;
                cell.BorderLeft = boderleft;
                cell.BorderBottom = boderbottom;
                cell.BorderRight = boderright;
                cell.Style = style.Replace(("Padding:" + padding + ";Margin:" + margin + ";Width:" + width 
                    + ";Border-top:" + bodertop
                    + ";Border-left:" + boderleft
                    + ";Border-bottom:" + boderbottom
                    + ";Border-right:" + boderright
                    + ";")
                    , "");
                cell.Attribute = GetElements(dtElement, Value[i.ToString()]["data"].ToString(), numRandom);
                cells[i] = cell;
            }
            return cells;
        }
        private static PaymentDesignAttribute[] GetElements(DataTable dtElement, string strdata, double idCell)
        {
            string[] listElementID = strdata.Split(',');
            PaymentDesignAttribute[] attributes = new PaymentDesignAttribute[listElementID.Length - 1];
            if (listElementID.Length > 0)
            {
                Random rand = new Random();
                for (int i = 0; i < listElementID.Length - 1; i++)
                {
                    string ID = listElementID[i];
                    DataTable Elm = GetElementDB(dtElement, ID);
                    if (Elm.Rows.Count > 0)
                    {

                        PaymentDesignAttribute attribute = new PaymentDesignAttribute();
                        attribute.Attribute = Elm.Rows[0]["Attribute"].ToString();
                        attribute.AttributeRandom = Elm.Rows[0]["AttributeRandom"].ToString();
                        attribute.Margin = Elm.Rows[0]["Margin"].ToString();
                        attribute.FontSize = Elm.Rows[0]["FontSize"].ToString();
                        attribute.FontStyle = Elm.Rows[0]["FontStyle"].ToString();
                        attribute.TextTransform = Elm.Rows[0]["TextTransform"].ToString();
                        attribute.Height = Elm.Rows[0]["Height"].ToString();
                        attribute.Width = Elm.Rows[0]["Width"].ToString();
                        attribute.TextAlign = Elm.Rows[0]["TextAlign"].ToString();
                        attribute.Border = Elm.Rows[0]["Border"].ToString();
                        attribute.BorderTop = Elm.Rows[0]["BorderTop"].ToString();
                        attribute.BorderLeft = Elm.Rows[0]["BorderLeft"].ToString();
                        attribute.BorderBottom = Elm.Rows[0]["BorderBottom"].ToString();
                        attribute.BorderRight = Elm.Rows[0]["BorderRight"].ToString();
                        attribute.Padding = Elm.Rows[0]["Padding"].ToString();
                        attribute.Display = Elm.Rows[0]["Display"].ToString();
                        attribute.VerticalAlign = Elm.Rows[0]["VerticalAlign"].ToString();
                        attribute.FontWeight = Elm.Rows[0]["FontWeight"].ToString();
                        attribute.Colspan = Elm.Rows[0]["Colspan"].ToString();
                        attribute.Type = Elm.Rows[0]["Type"].ToString();
                        attribute.Text = Elm.Rows[0]["Text"].ToString();
                        attribute.IsText = Elm.Rows[0]["IsText"].ToString();
                        attribute.FormContainer = Convert.ToInt32(Elm.Rows[0]["FormContainer"].ToString());
                        attributes[i] = attribute;
                    }
                }
            }
            return attributes;
        }
        private static DataTable GetElementDB(DataTable dt, string ID)
        {
            try
            {
                var resultRow = from myRow in dt.AsEnumerable()
                                where myRow.Field<int>("AttributeRandom") == Convert.ToInt32(ID)
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                return dtResult;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static DataSet GenerateExcuteDataDesign(List<PaymentDesignRow> data)
        {
            DataSet ds = new DataSet();
            DataTable dtDesign = new DataTable();
            dtDesign.Columns.Add("ID", typeof(String));
            dtDesign.Columns.Add("Column", typeof(String));
            dtDesign.Columns.Add("IsTable", typeof(String));
            dtDesign.Columns.Add("Margin", typeof(String));
            dtDesign.Columns.Add("Padding", typeof(String));
            dtDesign.Columns.Add("Border", typeof(String));
            dtDesign.Columns.Add("BorderTop", typeof(String));
            dtDesign.Columns.Add("BorderLeft", typeof(String));
            dtDesign.Columns.Add("BorderBottom", typeof(String));
            dtDesign.Columns.Add("BorderRight", typeof(String));
            dtDesign.Columns.Add("Class", typeof(String));
            dtDesign.Columns.Add("Index", typeof(String));
            dtDesign.Columns.Add("RowDesignID", typeof(String));

            DataTable dtCells = new DataTable();
            dtCells.Columns.Add("ID", typeof(String));
            dtCells.Columns.Add("RowID", typeof(String));
            dtCells.Columns.Add("Style", typeof(String));
            dtCells.Columns.Add("IsTable", typeof(String));
            dtCells.Columns.Add("Margin", typeof(String));
            dtCells.Columns.Add("Padding", typeof(String));
            dtCells.Columns.Add("Width", typeof(String));
            dtCells.Columns.Add("BorderTop", typeof(String));
            dtCells.Columns.Add("BorderLeft", typeof(String));
            dtCells.Columns.Add("BorderBottom", typeof(String));
            dtCells.Columns.Add("BorderRight", typeof(String));

            DataTable dtAttribute = new DataTable();
            dtAttribute.Columns.Add("Attribute", typeof(String));
            dtAttribute.Columns.Add("Margin", typeof(String));
            dtAttribute.Columns.Add("FontSize", typeof(String)); 
            dtAttribute.Columns.Add("FontStyle", typeof(String));
            dtAttribute.Columns.Add("TextTransform", typeof(String));
            dtAttribute.Columns.Add("Height", typeof(String));
            dtAttribute.Columns.Add("Width", typeof(String));
            dtAttribute.Columns.Add("TextAlign", typeof(String));
            dtAttribute.Columns.Add("Border", typeof(String));
            dtAttribute.Columns.Add("BorderTop", typeof(String));
            dtAttribute.Columns.Add("BorderLeft", typeof(String));
            dtAttribute.Columns.Add("BorderBottom", typeof(String));
            dtAttribute.Columns.Add("BorderRight", typeof(String));
            dtAttribute.Columns.Add("Padding", typeof(String));
            dtAttribute.Columns.Add("Display", typeof(String));
            dtAttribute.Columns.Add("VerticalAlign", typeof(String));
            dtAttribute.Columns.Add("FontWeight", typeof(String));
            dtAttribute.Columns.Add("Colspan", typeof(String));
            dtAttribute.Columns.Add("Type", typeof(String)); 
            dtAttribute.Columns.Add("FormContainer", typeof(Int32));
            dtAttribute.Columns.Add("Text", typeof(String));
            dtAttribute.Columns.Add("CellsID", typeof(String));

            for (int i = 0; i < data.Count; i++)
            {
                DataRow dr = dtDesign.NewRow();
                dr["ID"] = data[i].RowRandom;
                dr["Column"] = data[i].Column;
                dr["IsTable"] = data[i].IsTable;
                dr["Margin"] = data[i].Margin;
                dr["Padding"] = data[i].Padding;
                dr["Border"] = data[i].Border;
                dr["BorderTop"] = data[i].BorderTop;
                dr["BorderLeft"] = data[i].BorderLeft;
                dr["BorderBottom"] = data[i].BorderBottom;
                dr["BorderRight"] = data[i].BorderRight;
                dr["Class"] = data[i].ClassName;
                dr["Index"] = i.ToString();
                dr["RowDesignID"] = data[i].ID;
                dtDesign.Rows.Add(dr);

                DataSet dttemp = new DataSet();
                dttemp = GenerateExcuteDataCells(data[i].Cells, dtCells, dtAttribute, data[i].RowRandom);
                for (int j = 0; j < dttemp.Tables[0].Rows.Count; j++)
                {
                    dtCells.ImportRow(dttemp.Tables[0].Rows[j]);
                }
                for (int j = 0; j < dttemp.Tables[1].Rows.Count; j++)
                {
                    dtAttribute.ImportRow(dttemp.Tables[1].Rows[j]);
                }
            }
            ds.Tables.Add(dtDesign);
            ds.Tables.Add(dtCells);
            ds.Tables.Add(dtAttribute);
            return ds;
        }
        private static DataSet GenerateExcuteDataCells(PaymentDesignCells[] data, DataTable dtCells, DataTable dtAttribute, string RowID)
        {
            DataSet ds = new DataSet();
            DataTable dtC = dtCells.Clone();
            DataTable dtA = dtAttribute.Clone();
            for (int i = 0; i < data.Length; i++)
            {
                DataRow dr = dtC.NewRow();
                dr["ID"] = data[i].Random;
                dr["RowID"] = RowID;
                dr["Style"] = data[i].Style;
                dr["IsTable"] = data[i].IsTable;
                dr["Margin"] = data[i].Margin;
                dr["Padding"] = data[i].Padding;
                dr["Width"] = data[i].Width;
                dr["BorderTop"] = data[i].BorderTop;
                dr["BorderLeft"] = data[i].BorderLeft;
                dr["BorderBottom"] = data[i].BorderBottom;
                dr["BorderRight"] = data[i].BorderRight;
                dtC.Rows.Add(dr);

                DataTable dt = GenerateExcuteDataAttribute(data[i].Attribute, dtAttribute, data[i].Random);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dtA.ImportRow(dt.Rows[j]);
                }
            }
            ds.Tables.Add(dtC);
            ds.Tables.Add(dtA);

            return ds;
        }
        private static DataTable GenerateExcuteDataAttribute(PaymentDesignAttribute[] data, DataTable dtAttribute, string CellsID)
        {
            DataTable dtA = dtAttribute.Clone();
            for (int i = 0; i < data.Length; i++)
            {
                DataRow dr = dtA.NewRow();
                dr["Attribute"] = data[i].Attribute;
                dr["Margin"] = data[i].Margin;
                dr["FontSize"] = data[i].FontSize; 
                dr["FontStyle"] = data[i].FontStyle;
                dr["TextTransform"] = data[i].TextTransform;
                dr["Height"] = data[i].Height;
                dr["Width"] = data[i].Width;
                dr["TextAlign"] = data[i].TextAlign;
                dr["Border"] = data[i].Border;
                dr["BorderTop"] = data[i].BorderTop;
                dr["BorderLeft"] = data[i].BorderLeft;
                dr["BorderBottom"] = data[i].BorderBottom; 
                dr["BorderRight"] = data[i].BorderRight;
                dr["Padding"] = data[i].Padding;
                dr["Display"] = data[i].Display;
                dr["VerticalAlign"] = data[i].VerticalAlign;
                dr["FontWeight"] = data[i].FontWeight;
                dr["Colspan"] = data[i].Colspan;
                dr["Type"] = data[i].Type; 
                dr["FormContainer"] = Convert.ToInt32(data[i].FormContainer);
                dr["Text"] = data[i].Text;
                dr["CellsID"] = CellsID;
                dtA.Rows.Add(dr);
            }
            return dtA;
        }
    }
    public class PaymentDesignRow
    {
        public string ID { get; set; }
        public string RowRandom { get; set; }
        public string Column { get; set; }
        public string IsTable { get; set; }
        public string Margin { get; set; }
        public string Padding { get; set; }  
        public string Border { get; set; }
        public string BorderTop { get; set; }
        public string BorderLeft { get; set; }
        public string BorderBottom { get; set; }
        public string BorderRight { get; set; }
        public string ClassName { get; set; }
 
        public PaymentDesignCells[] Cells { get; set; }
    }
    public class PaymentDesignCells
    {
        public string ID { get; set; }
        public string Random { get; set; }
        public string IsTable { get; set; }
        public string Width { get; set; }
        public string Margin { get; set; }
        public string Padding { get; set; }
        public string BorderTop { get; set; }
        public string BorderLeft { get; set; }
        public string BorderBottom { get; set; }
        public string BorderRight { get; set; }
        public string Style { get; set; }
        public PaymentDesignAttribute[] Attribute { get; set; }
    }
    public class PaymentDesignAttribute
    {
        public string Attribute { get; set; }
        public string AttributeRandom { get; set; }
        public string Margin { get; set; }
        public string FontSize { get; set; }
        public string FontStyle { get; set; }
        public string TextTransform { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string TextAlign { get; set; }
        public string Border { get; set; } 
        public string BorderTop { get; set; }  
        public string BorderLeft { get; set; } 
        public string BorderBottom { get; set; } 
        public string BorderRight { get; set; }
        public string Padding { get; set; }
        public string Display { get; set; }
        public string VerticalAlign { get; set; }
        public string FontWeight { get; set; }
        public string Colspan { get; set; }
        public string Type { get; set; }
        public int FormContainer { get; set; } 
        public string Text { get; set; } 
        public string IsText { get; set; }
    }
}