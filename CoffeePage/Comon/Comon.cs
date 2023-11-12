using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkiaSharp;

namespace MLunarCoffee.Comon
{
    public static class Comon
    {

        public static string StreatmToString64(Stream inputstream ,ImageFormat format ,int contentlength)
        {
            string base64String = "";
            using (var binaryReader = new BinaryReader(
                               Comon.ConvertImage(inputstream ,format)))
            {
                byte[] fileData = null;
                fileData = binaryReader.ReadBytes(contentlength);
                base64String = Comon.ResizeImage(fileData ,100 ,100);
            }
            return base64String;
        }

        public static Stream ConvertImage(Stream originalStream ,ImageFormat format)
        {
            var image = Image.FromStream(originalStream);
            var stream = new MemoryStream();
            image.Save(stream ,format);
            stream.Position = 0;
            return stream;
        }
        public static Stream ConvertImageWebp(Stream originalStream)
        {
            var memStream = new MemoryStream();
            using (var image = new ImageMagick.MagickImage(originalStream))
            {
                image.Format = ImageMagick.MagickFormat.WebP;
                image.Quality = 100;
                image.Write(memStream);
            }
            return memStream;
        }
        public static Stream ConvertImageIphone(Stream originalStream)
        {
            var memStream = new MemoryStream();
            using (var image = new ImageMagick.MagickImage(originalStream))
            {
                image.Format = ImageMagick.MagickFormat.Jpg;
                image.Quality = 60;
                image.Write(memStream);
            }
            return memStream;
        }

        // Resize Image
        public static string ResizeImage(byte[] byteArrayIn ,int width ,int height)
        {
            byte[] byteImage = ResizeImage_Byte(byteArrayIn ,width ,height);
            var SigBase64 = Convert.ToBase64String(byteImage);
            return SigBase64;
        }
        public static byte[] ResizeImage_Byte(byte[] byteArrayIn ,int width ,int height)
        {
            try
            {
                using (var ms = new MemoryStream(byteArrayIn))
                {

                    var srcBitmap = SKBitmap.Decode(ms);
                    int resizedWidth = width, resizedHeight = height;
                    if ((double)srcBitmap.Width > (double)resizedWidth && (double)srcBitmap.Height > (double)resizedHeight)
                    {


                        double dbl = (double)srcBitmap.Width / (double)srcBitmap.Height;
                        int newheight = 0; int newwidth = 0;
                        if ((int)((double)resizedHeight * dbl) <= resizedWidth)
                        {
                            newwidth = (int)((double)resizedHeight * dbl);
                            newheight = resizedHeight;
                        }
                        else
                        {
                            newheight = (int)((double)resizedWidth / dbl);
                            newwidth = resizedWidth;
                        }

                        using (var surface = SKSurface.Create(new SKImageInfo(newwidth ,newheight ,SKImageInfo.PlatformColorType ,SKAlphaType.Premul)))
                        using (var paint = new SKPaint())
                        {
                            paint.IsAntialias = true;
                            paint.FilterQuality = SKFilterQuality.High;
                            surface.Canvas.DrawBitmap(srcBitmap ,new SKRectI(0 ,0 ,newwidth ,newheight) ,paint);
                            surface.Canvas.Flush();
                            using (var newImg = surface.Snapshot())
                            using (SKData data = newImg.Encode(SKEncodedImageFormat.Jpeg ,100))
                            using (Stream imgStream = data.AsStream())
                            {
                                var memoryStream = new MemoryStream();
                                imgStream.CopyTo(memoryStream);
                                byte[] byteImage = memoryStream.ToArray();

                                return byteImage;
                            }
                        }
                    }
                    else return byteArrayIn;
                }
            }
            catch (Exception ex)
            {
                return byteArrayIn;
            }


        }

        #region // Convert Num to String ( Money)
        private static string ConvertChu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "Không";
                    break;
                case "1":
                    result = "Một";
                    break;
                case "2":
                    result = "Hai";
                    break;
                case "3":
                    result = "Ba";
                    break;
                case "4":
                    result = "Bốn";
                    break;
                case "5":
                    result = "Năm";
                    break;
                case "6":
                    result = "Sáu";
                    break;
                case "7":
                    result = "Bảy";
                    break;
                case "8":
                    result = "Tám";
                    break;
                case "9":
                    result = "Chín";
                    break;
            }
            return result;
        }

        private static string ConvertDonvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }

        private static string ConvertTach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0 ,1).ToString().Trim();
                string ch = tach3.Trim().Substring(1 ,1).ToString().Trim();
                string dv = tach3.Trim().Substring(2 ,1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + ConvertChu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = ConvertChu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = ConvertChu(tr.ToString().Trim()).Trim() + " trăm lẻ " + ConvertChu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + ConvertChu(ch.Trim()).Trim() + " mươi " + ConvertChu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + ConvertChu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + ConvertChu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + ConvertChu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = ConvertChu(tr.Trim()).Trim() + " trăm " + ConvertChu(ch.Trim()).Trim() + " mươi " + ConvertChu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = ConvertChu(tr.Trim()).Trim() + " trăm " + ConvertChu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = ConvertChu(tr.Trim()).Trim() + " trăm " + ConvertChu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = ConvertChu(tr.Trim()).Trim() + " trăm mười " + ConvertChu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = ConvertChu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = ConvertChu(tr.Trim()).Trim() + " trăm mười lăm ";

            }

            return Ktach;

        }

        public static string ConvertNumToString(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";
            if (gNum < 0)
            {
                gNum = gNum * -1;

            }
            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum ,0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0 ,1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0 ,2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod ,Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = ConvertTach(tach_mod).ToString().Trim() + " " + ConvertDonvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0 ,3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + ConvertTach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + ConvertDonvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3 ,tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0 ,1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10 ,lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0 ,1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2 ,lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0 ,1).Trim().ToUpper() + lso_chu.Trim().Substring(1 ,lso_chu.Trim().Length - 1).Trim() + " đồng.";

            return lso_chu.ToString().Trim().Replace("mươi Một" ,"mươi mốt");

        }
        #endregion

        public static System.Data.DataTable GetControlPermissionByGroup(System.Data.DataTable dt ,string groupID)
        {
            try
            {
                if (dt != null)
                {
                    System.Data.DataRow[] dr = dt.Select("PermissionGroupID=" + groupID);
                    return dr.CopyToDataTable();
                }
                else return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string Readfile(string link)
        {
            try
            {
                string content = "";
                WebClient client = new WebClient();
                using (Stream stream = client.OpenRead(link))
                {
                    StreamReader reader = new StreamReader(stream);
                    content = reader.ReadToEnd();

                }
                return content;

            }
            catch (Exception ex)
            {
                return "";
            }



        }
        //private static int WritefileDetail(string link ,string content)
        //{
        //    try
        //    {
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Global.sys_ALLFTP_FTPDocument + '/' + link);
        //        request.Method = WebRequestMethods.Ftp.UploadFile;

        //        request.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER ,Global.sys_ALLFTP_PASSWORD);

        //        // convert contents to byte.
        //        byte[] fileContents = Encoding.UTF8.GetBytes(content);

        //        request.ContentLength = fileContents.Length;

        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(fileContents ,0 ,fileContents.Length);
        //        }

        //        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
        //        {
        //            Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
        //        }
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}
        //public static int Writefile(string link ,string content ,string linkPlainText ,string PlaitText ,string linkPlainTextNoSign ,string PlaitTextNoSign)
        //{
        //    try
        //    {
        //        if (WritefileDetail(link ,content) == 1 && WritefileDetail(linkPlainText ,PlaitText.ToLower()) == 1 && WritefileDetail(linkPlainTextNoSign ,PlaitTextNoSign.ToLower()) == 1) return 1;
        //        else return 0;

        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}


 


        public static DateTime DateTimeDMY_To_YMD(string date)
        {
            try
            {

                date = date.Trim();
                string[] A = date.Split(' ');
                string[] B = A[0].Split('-');
                return new DateTime(Convert.ToInt32(B[2]) ,Convert.ToInt32(B[1]) ,Convert.ToInt32(B[0]) ,0 ,0 ,0);
            }
            catch (Exception ex)
            {
                return new DateTime(1900 ,01 ,01);
            }


        }
        public static DateTime DateTimeDMY_To_YMD_FirstDay(string date)
        {
            try
            {

                date = date.Trim();
                string[] A = date.Split(' ');
                string[] B = A[0].Split('-');
                return new DateTime(Convert.ToInt32(B[2]) ,Convert.ToInt32(B[1]) ,1 ,0 ,0 ,0);
            }
            catch (Exception ex)
            {
                return new DateTime(1900 ,01 ,01);
            }


        }
        public static String DateTimeYMD_To_DMY(DateTime date)
        {
            try
            {
                string DMY = date.ToString("dd-MM-yyyy");

                return DMY;
            }
            catch (Exception ex)
            {
                return "01-01-1990";
            }
        }
        public static int DateTimeDMY_To_Month(string date)
        {
            try
            {

                date = date.Trim();
                string[] A = date.Split(' ');
                string[] B = A[0].Split('-');
                return Convert.ToInt32(B[1]);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static DateTime DateTimeMY_To_YM(string date)
        {
            try
            {

                date = date.Trim();
                string[] A = date.Split(' ');
                string[] B = A[0].Split('-');
                return new DateTime(Convert.ToInt32(B[1]) ,Convert.ToInt32(B[0]) ,01 ,0 ,0 ,0);
            }
            catch (Exception ex)
            {
                return new DateTime(1900 ,01 ,01);
            }


        }
        public static DateTime DateTimeDMY_To_YMDHHMM(string date)
        {
            try
            {
                date = date.Trim();
                string[] A = date.Split(' ');
                string[] C = A[1].Split(':');
                string[] B = A[0].Split('-');
                return new DateTime(Convert.ToInt32(B[2]) ,Convert.ToInt32(B[1]) ,Convert.ToInt32(B[0]) ,Convert.ToInt32(C[0]) ,Convert.ToInt32(C[1])
                    ,(C.Length > 2) ? Convert.ToInt32(C[2]) : 0
                    );
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }


        }
        public static DateTime DateTimeDMY_To_YMDHHMMSS(string date)
        {
            try
            {
                date = date.Trim();
                string[] A = date.Split(' ');
                string[] C = A[1].Split(':');
                string[] B = A[0].Split('-');
                return new DateTime(Convert.ToInt32(B[2]) ,Convert.ToInt32(B[1]) ,Convert.ToInt32(B[0]) ,Convert.ToInt32(C[0]) ,Convert.ToInt32(C[1]) ,Convert.ToInt32(C[2]));
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }


        }
        public static DateTime StringDMY_To_DateTime(string date)
        {
            try
            {
                date = date.Trim();
                string[] A = date.Split('-');
                return new DateTime(Convert.ToInt32(A[2]) ,Convert.ToInt32(A[1]) ,Convert.ToInt32(A[0]) ,0 ,0 ,0);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime StringYMDHM_To_DateTime(string date)
        {
            try
            {
                date = date.Trim();
                string[] A = date.Split(' ');
                string[] B = A[0].Split('-');
                if (A.Length == 2)
                {
                    string[] C = A[1].Split(':');
                    return new DateTime(Convert.ToInt32(B[2]) ,Convert.ToInt32(B[1]) ,Convert.ToInt32(B[0]) ,Convert.ToInt32(C[0]) ,Convert.ToInt32(C[1]) ,Convert.ToInt32(C[2]));
                }
                else
                {
                    return new DateTime(Convert.ToInt32(B[2]) ,Convert.ToInt32(B[1]) ,Convert.ToInt32(B[0]) ,0 ,0 ,0);
                }
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }

 

        // URL TO BASE64
        public static String ConvertImageURLToBase64(String url)
        {
            try
            {
                StringBuilder _sb = new StringBuilder();

                Byte[] _byte = GetImage(url);

                _sb.Append(Convert.ToBase64String(_byte ,0 ,_byte.Length));

                return _sb.ToString();
            }
            catch (Exception)
            {
                return "";
            }

        }

        private static byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }

            return (buf);
        }


        #region // sort array
        public static string Accending_String(string str)
        {
            try
            {
                string result = "";
                string[] strArray = str.Split(',');
                int[] numArray = new int[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (int.TryParse(strArray[i] ,out _))
                        numArray[i] = Convert.ToInt32(strArray[i]);
                    else numArray[i] = 0;
                }
                Array.Sort(numArray);

                for (int i = 0; i < numArray.Length; i++)
                {
                    if (numArray[i] != 0)
                        result = result + numArray[i].ToString() + ",";
                }
                return "," + result;
            }
            catch (Exception ex)
            {
                return str;
            }
        }
        #endregion
        // Convert List To DataTable
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            System.ComponentModel.PropertyDescriptorCollection properties =
               System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (System.ComponentModel.PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name ,Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (System.ComponentModel.PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

 
        public static Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes ,0 ,imageBytes.Length))
            {
                Image image = Image.FromStream(ms ,true);
                return image;
            }
        }

        public static string GetRepTemp(Dictionary<string ,string> data ,string template ,string signKey = "#")
        {
            try
            {
                string result = template;
                for (int i = 0; i < data.Count; i++)
                {
                    string keyReplace = signKey + data.ElementAt(i).Key + signKey;
                    result = result.Replace(keyReplace ,data.ElementAt(i).Value);
                }
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string GetRepTemp(DataTable data ,string template ,string signKey = "#")
        {
            try
            {
                string result = template;
                if(data != null && data.Rows.Count > 0)
                {
                    DataRow row = data.Rows[0];
                    foreach (DataColumn column in data.Columns)
                    {
                        string keyReplace = signKey + column.ColumnName + signKey;
                        result = result.Replace(keyReplace ,row[column.ColumnName].ToString());

                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static Dictionary<string ,string> ConvertRowsToDictionary(DataTable table , DataRow row)
        {
            Dictionary<string ,string> rowDict = new Dictionary<string ,string>();
            foreach (DataColumn column in table.Columns)
            {
                rowDict[column.ColumnName] = row[column].ToString();
            }
            return rowDict;
        }
    }
}
