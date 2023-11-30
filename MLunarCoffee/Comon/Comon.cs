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
using MLunarCoffee.Models.Model.AlrmScheduler;
using MLunarCoffee.Models.Model.WorkScheduler;

namespace MLunarCoffee.Comon
{
    public static class Comon
    {
        public static List<string> GetAllDirectoryInFolder(string directory)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Global.sys_ServerImageFolder + directory);
                ftpRequest.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                List<string> directories = new List<string>();
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    directories.Add(line);
                    line = streamReader.ReadLine();
                }

                streamReader.Close();
                return directories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string InsertMLunarCoffee(string Content)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://103.48.191.137:7071/IDPageKeyCode/IDPageCodeClient.txt");
                request.Method = WebRequestMethods.Ftp.AppendFile;
                request.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER ,Global.sys_ALLFTP_PASSWORD);

                using (StreamWriter requestStream = new StreamWriter(request.GetRequestStream()))
                {
                    requestStream.WriteLine(Content);
                    requestStream.Flush();
                    requestStream.Close();

                }
                return "1";
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static string UnsubscribePage(string Content ,string link)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://103.48.191.137:7071/IDPageKeyCode/IDPageCodeClient.txt");
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER ,Global.sys_ALLFTP_PASSWORD);

                using (StreamWriter requestStream = new StreamWriter(request.GetRequestStream()))
                {
                    requestStream.Write(Content);
                    requestStream.Flush();
                    requestStream.Close();

                }
                return "1";
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public static bool DeleteDirectoryFTP(string customer ,string directory)
        {
            List<string> currrentdirectories = GetAllDirectoryInFolder(customer);
            // Chua co folder customer
            if (currrentdirectories == null)
            {
                return false;
            }
            if (currrentdirectories.Count == 0)
            {
                return false;
            }
            else
            {
                bool resulf = false;
                foreach (var dr in currrentdirectories)
                {
                    if (dr.ToLower() == directory.ToLower())
                        resulf = true;
                }
                if (!resulf) return false; // Khong co folder
                else
                {

                    List<string> image_in_current = GetAllDirectoryInFolder(customer + "/" + directory);
                    List<string> file_in_current = GetAllDirectoryInFolder(customer + "/" + directory + "/" + "File");


                    if (
                            image_in_current == null
                            || image_in_current.Count == 0
                            || (image_in_current.Count == 1 && file_in_current.Count == 0)
                        )
                    {
                        DeleteDirectoryFTP(customer + "/" + directory + "/" + "File");
                        return DeleteDirectoryFTP(customer + "/" + directory);
                    }
                    return false;
                }
            }
        }
        private static bool DeleteDirectoryFTP(string directory)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Global.sys_ServerImageFolder + directory);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            request.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);
            try
            {
                request.GetResponse();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool UpdateDirectoryFTP(string customer ,string directoryOld ,string directoryNew)
        {
            List<string> currrentdirectories = GetAllDirectoryInFolder(customer);
            // Chua co folder customer
            if (currrentdirectories == null)
            {
                return false;
                //CreateDirectoryFTP(customer);
                //currrentdirectories = GetAllDirectoryInFolder(customer);
            }
            if (currrentdirectories.Count == 0)
            {
                return false;
            }
            else
            {
                bool resulf = false;
                foreach (var dr in currrentdirectories)
                {
                    if (dr == directoryOld)
                        resulf = true;
                }
                if (!resulf) return false; // Khong co folder can update
                else
                {
                    return ChangeNameDirectoryFTP(customer + "/" + directoryOld ,directoryNew);
                }
            }
        }
        public static bool CheckDirectoryFTP(string customer ,string directory)
        {
            List<string> currrentdirectories = GetAllDirectoryInFolder(customer);
            // Chua co folder customer
            if (currrentdirectories == null)
            {
                CreateDirectoryFTP(customer);
                currrentdirectories = GetAllDirectoryInFolder(customer);
            }
            // Co roi nhung chua co 1 folder hinh anh nao
            if (currrentdirectories.Count == 0)
            {
                CreateDirectoryFTP(customer + "/" + directory);
                return true;
            }
            else
            {
                bool resulf = false;
                foreach (var dr in currrentdirectories)
                {
                    if (dr.ToLower() == directory.ToLower())
                        resulf = true;
                }
                if (resulf) return false; // Trung folder
                else
                {
                    return CreateDirectoryFTP(customer + "/" + directory);
                }
            }
        }
        public static bool ChangeNameDirectoryFTP(string directoryOld ,string nameChanged)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Global.sys_ServerImageFolder + directoryOld);
            request.Method = WebRequestMethods.Ftp.Rename;
            request.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);
            request.RenameTo = nameChanged;
            try
            {
                request.GetResponse();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool CreateDirectoryFTP(string directory)
        {
            WebRequest request = WebRequest.Create(Global.sys_ServerImageFolder + directory);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);
            try
            {
                request.GetResponse();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static bool MoveDetailFolder(string CurrentFolder ,string dicfrom ,string dicto)
        {
            // Tao folder truoc, Kiem tra dicto da co folder hinh anh chua
            try
            {
                string newFolder = CurrentFolder + "(M)";
                CheckDirectoryFTP(dicto ,newFolder);
                List<string> allimage = GetAllDirectoryInFolder(dicfrom + "/" + CurrentFolder);
                if (allimage != null && allimage.Count > 0)
                {
                    foreach (var im in allimage)
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Global.sys_ServerImageFolder + dicfrom + '/' + CurrentFolder + '/' + im.ToString());
                        request.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);
                        request.Method = WebRequestMethods.Ftp.DownloadFile;

                        using (Stream ftpStream = request.GetResponse().GetResponseStream())
                        {
                            if (UploadStream(ftpStream ,Global.sys_ServerImageFolder + dicto + '/' + newFolder + '/' + im.ToString()))
                            {
                                DeleteFile(Global.sys_ServerImageFolder + dicfrom + '/' + CurrentFolder + '/' + im.ToString());
                            }
                        }
                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }



        }
        public static decimal MathRoundMoney(double number)
        {
            if (number >= 0 && number <= 100)
            {
                number = Math.Round(number);
            }
            else if (number > 100 && number <= 10000)
            {
                number = Math.Round(number / 100) * 100;
            }
            else if (number > 10000)
            {
                number = Math.Round(number / 1000) * 1000;
            }
            return (decimal)number;
        }
        public static bool DeleteFolder(string des)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(des);
                request.Method = WebRequestMethods.Ftp.RemoveDirectory;
                request.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);
                request.GetResponse().Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public static bool DeleteFile(string des)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(des);
                request.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);

                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Console.WriteLine("Delete status: {0}" ,response.StatusDescription);
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer ,0 ,buffer.Length)) > 0)
                {
                    ms.Write(buffer ,0 ,read);
                }
                return ms.ToArray();
            }
        }
        private static bool UploadStream(Stream stream ,string des)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(des);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(Global.sys_ServerImageUserName ,Global.sys_ServerImagePassword);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                byte[] fileContents = ReadFully(stream);


                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents ,0 ,fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public static bool MoveFolder(string dicFrom ,string dicTo)
        {
            try
            {
                List<string> currrentdirectories = GetAllDirectoryInFolder(dicFrom);
                // Csutomer From khong co hinh anh
                if (currrentdirectories == null)
                {
                    return true;
                }
                else
                {
                    // Co roi nhung chua co 1 folder hinh anh nao
                    if (currrentdirectories.Count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        foreach (var dr in currrentdirectories)
                        {
                            if (MoveDetailFolder(dr.ToString() ,dicFrom ,dicTo))
                            {
                                DeleteFolder(Global.sys_ServerImageFolder + dicFrom + '/' + dr.ToString());
                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // customer + foldername
        public static List<string> GetImageFromDirectory(string directory)
        {
            List<string> stringName = new List<string>();
            stringName = GetAllDirectoryInFolder(directory);
            if (stringName == null)
            {
                return null;
            }
            for (int i = 0; i < stringName.Count; i++)
            {
                stringName[i] = String.Format(@"{0}{1}/{2}" ,Global.sys_HTTPImageRoot ,directory ,stringName[i]);
            }
            return stringName;
        }
        public static DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
        #region // Type 2
        public static string UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            try
            {
                System.DateTime dtDateTime = new DateTime(1970 ,1 ,1 ,0 ,0 ,0 ,0 ,System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                return dtDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static DateTime UnixTimeStampToDateTime_ToDT(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            try
            {
                System.DateTime dtDateTime = new DateTime(1970 ,1 ,1 ,0 ,0 ,0 ,0 ,System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                return dtDateTime;
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }

        }
        #endregion
        #region // Type 1
        public static string UnixTimeStampToDateTimeMili(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            try
            {
                System.DateTime dtDateTime = new DateTime(1970 ,1 ,1 ,0 ,0 ,0 ,0 ,System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
                return dtDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static DateTime UnixTimeStampToDateTimeMili_ToDT(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            try
            {
                System.DateTime dtDateTime = new DateTime(1970 ,1 ,1 ,0 ,0 ,0 ,0 ,System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
                return dtDateTime;
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }

        }
        #endregion
        public static string ConvertToUnsign(string str)
        {
            str = str.Replace("'" ,"");
            string[] signs = new string[]
                {
                    "aAeEoOuUiIdDyY",
                    "áàạảãâấầậẩẫăắằặẳẵ",
                    "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                    "éèẹẻẽêếềệểễ",
                    "ÉÈẸẺẼÊẾỀỆỂỄ",
                    "óòọỏõôốồộổỗơớờợởỡ",
                    "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                    "úùụủũưứừựửữ",
                    "ÚÙỤỦŨƯỨỪỰỬỮ",
                    "íìịỉĩ",
                    "ÍÌỊỈĨ",
                    "đ",
                    "Đ",
                    "ýỳỵỷỹ",
                    "ÝỲỴỶỸ"
                };
            for (int i = 1; i < signs.Length; i++)
            {
                for (int j = 0; j < signs[i].Length; j++)
                {
                    str = str.Replace(signs[i][j] ,signs[0][i - 1]);
                }
            }
            return str;
        }

        // COnvert Image Format
        public static bool CheckExtensionFileValid(List<string> FileExts ,string ext)
        {
            try
            {
                bool isFileValid = false;
                foreach (var extfile in FileExts)
                {
                    if (ext.Contains(extfile))
                    {
                        isFileValid = true;
                        break;
                    }
                }
                return isFileValid;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static bool CheckExtensionFileInBlacklist(string fullpath)
        {
            try
            {
                //string blacklist = Convert.ToString(Global.sys_ALLFTP_Blacklist);
                //string[] FileExts = (Convert.ToString(blacklist)).Split('.');
                //for (int i = 0; i < FileExts.Length; i++)
                //{
                //    string extRestrict = Convert.ToString(FileExts[i]);
                //    if (extRestrict != "" && fullpath.Contains(extRestrict))
                //    {
                //        return true;
                //    }
                //}
                //return false;
                const int BeginChar = 3, LimitChar = 10;
                string blacklist = Convert.ToString(Global.sys_ALLFTP_Blacklist);
                string[] FileExts = Convert.ToString(fullpath).Split('.');
                for (int i = 1; i < FileExts.Length; i++)
                {
                    string extfile = "." + FileExts[i];
                    int j = BeginChar;

                    while (j < LimitChar && j <= extfile.Length)
                    {
                        string charExt = extfile.Substring(0 ,j);
                        int startIndex = blacklist.IndexOf(charExt);
                        if (startIndex > -1)
                        {
                            int endIndex = blacklist.IndexOf("." ,startIndex + 1);
                            var distance = (int)(endIndex - startIndex);
                            string extRestricted = blacklist.Substring(startIndex ,distance);
                            if (extfile.Equals(extRestricted))
                                return true;
                        }
                        j++;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
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


        //public static bool CheckPreventCusByBranch(Microsoft.AspNetCore.Http.HttpContext httpContext, int cusBranchID)
        //{
        //    var user = Session.Session.GetUserSession(httpContext);
        //    if (user.sys_AllBranchID == 1) return true;
        //    if (Global.sys_PreventCusByBranch == 0) return true;
        //    else
        //    {
        //        if (user.sys_branchID == cusBranchID) return true;
        //        return false;
        //    }
        //}
        //public static bool CheckLimitUpLoad( Microsoft.AspNetCore.Http.HttpContext httpContext ) {
        //    try {
        //        var user = Session.Session.GetUserSession (httpContext);
        //        if (user.sys_isLimit == 0) return true;
        //        DataTable dt = new DataTable ();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
        //            dt =await  confunc.ExecuteDataTable ("[MLU_sp_CheckLimitBranch_UpLoad]", CommandType.StoredProcedure,
        //              "@BranchID", SqlDbType.Int, user.sys_branchID);
        //        }
        //        if (dt != null && dt.Rows.Count != 0) {
        //            if (Convert.ToInt32 (dt.Rows[0][0].ToString ()) > 0) {
        //                return true;
        //            }
        //            else {
        //                return false;
        //            }
        //        }
        //        else {
        //            return true;
        //        }
        //    }
        //    catch (Exception ex) {
        //        return true;
        //    }
        //}

        //public static async Task<bool>  CheckLimitSMS ( Microsoft.AspNetCore.Http.HttpContext httpContext ) {
        //    try {
        //        var user = Session.Session.GetUserSession (httpContext);
        //       // if (user.sys_isLimit == 0) return true;
        //        DataTable dt = new DataTable ();
        //        using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase ()) {
        //            dt =await confunc.ExecuteDataTable ("[MLU_sp_CheckLimitBranch_SMS]", CommandType.StoredProcedure,
        //              "@BranchID", SqlDbType.Int, user.sys_branchID);
        //        }
        //        if (dt != null && dt.Rows.Count != 0) {
        //            if (Convert.ToInt32 (dt.Rows[0][0].ToString ()) > 0) {
        //                return true;
        //            }
        //            else {
        //                return false;
        //            }
        //        }
        //        else {
        //            return true;
        //        }
        //    }
        //    catch (Exception ex) {
        //        return true;
        //    }
        //}
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
        private static int WritefileDetail(string link ,string content)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Global.sys_ALLFTP_FTPDocument + '/' + link);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER ,Global.sys_ALLFTP_PASSWORD);

                // convert contents to byte.
                byte[] fileContents = Encoding.UTF8.GetBytes(content);

                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents ,0 ,fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int Writefile(string link ,string content ,string linkPlainText ,string PlaitText ,string linkPlainTextNoSign ,string PlaitTextNoSign)
        {
            try
            {
                if (WritefileDetail(link ,content) == 1 && WritefileDetail(linkPlainText ,PlaitText.ToLower()) == 1 && WritefileDetail(linkPlainTextNoSign ,PlaitTextNoSign.ToLower()) == 1) return 1;
                else return 0;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        private static List<string> GetDocumentNoSignInFolder()
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Global.sys_ALLFTP_FTPDocument);
                ftpRequest.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER ,Global.sys_ALLFTP_PASSWORD);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                List<string> directories = new List<string>();
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    if (line.Contains("___plaintTextNoSign"))
                    {
                        directories.Add(line);

                    }
                    line = streamReader.ReadLine();
                }

                streamReader.Close();
                return directories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private static string GetSomeTextShow(string document ,int index)
        {
            try
            {
                string content = "";
                WebClient client = new WebClient();
                using (Stream stream = client.OpenRead(Global.sys_ALLHTTPDocument + document))
                {
                    StreamReader reader = new StreamReader(stream);
                    content = reader.ReadToEnd();

                }
                int stringcount = content.Length - index;
                return content.Substring(index ,(stringcount > 200) ? 200 : stringcount);

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static DataTable SeachingDocument(string searchText)
        {
            try
            {
                searchText = ConvertToUnsign(searchText).ToLower();
                List<string> stringName = new List<string>();
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("Name");
                dt.Columns.Add("FileName");
                dt.Columns.Add("Text");
                dt.Columns.Add("Index");
                dt.Columns.Add("FileNameSign");
                // "lichhen1___plaintTextNoSign.txt"
                stringName = GetDocumentNoSignInFolder();
                if (stringName == null)
                {
                    return null;
                }
                for (int i = 0; i < stringName.Count; i++)
                {
                    string link = Global.sys_ALLHTTPDocument + stringName[i];
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(link);
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();
                        int index = content.IndexOf(searchText);
                        if (index != -1)
                        {
                            string[] sp_underSorce = stringName[i].Split(new string[] { "___" } ,StringSplitOptions.None);
                            string[] sp_dot = sp_underSorce[1].Split('.');
                            DataRow dr = dt.NewRow();
                            dr["FileName"] = sp_underSorce[0].ToString() + '.' + sp_dot[1].ToString();
                            dr["Index"] = index.ToString();
                            dr["FileNameSign"] = stringName[i].Replace("plaintTextNoSign" ,"plaintText");
                            dr["Text"] = GetSomeTextShow(dr["FileNameSign"].ToString() ,index);
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<string> GetDocumentInFolder(string name)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Global.sys_ALLFTP_FTPDocument);
                ftpRequest.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER ,Global.sys_ALLFTP_PASSWORD);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                List<string> directories = new List<string>();
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    if (line == name)
                    {
                        directories.Add(line);
                    }
                    line = streamReader.ReadLine();
                }

                streamReader.Close();
                return directories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int CreateFileDocument(string link)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Global.sys_ALLFTP_FTPDocument + '/' + link);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(Global.sys_ALLFTP_USER ,Global.sys_ALLFTP_PASSWORD);

                // convert contents to byte.
                byte[] fileContents = Encoding.UTF8.GetBytes("");

                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents ,0 ,fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static bool CheckEmptyFile(string url)
        {
            try
            {
                string link = Global.sys_ALLHTTPDocument + url;
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(link);
                using (StreamReader reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    if (content.Length == 0) return true;
                    else return false;
                }
            }
            catch (Exception)
            { return false; }
        }


        public static string CheckContentSMS(string content)
        {
            string resulf = "";
            DataTable dt = Global.sys_SMS_NotInBrandName;
            if (dt == null) return "1";
            content = ConvertToUnsign(content).ToLower();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string blockText = dt.Rows[i]["Text"].ToString().Trim().ToLower();
                // xu ly nhung thang co *
                if (blockText.Contains("*"))
                {
                    blockText = blockText.Replace(" " ,"");
                    string[] blockTextArray = blockText.Split(new string[] { "*" } ,StringSplitOptions.None);
                    if (blockTextArray.Length == 2) // Chi chap nhan dau * o giua 2 chu
                    {
                        string contentNoSpace = content.Replace(" " ,"");
                        if (contentNoSpace.Contains(blockTextArray[0].ToString()))
                        {
                            string[] arr = contentNoSpace.Split(new string[] { blockTextArray[0].ToString() } ,StringSplitOptions.None);
                            foreach (var x in arr)
                            {
                                if (x != null && x.Length != 0)
                                {
                                    if (x.IndexOf(blockTextArray[1].ToString()) < 4 && x.IndexOf(blockTextArray[1].ToString()) >= 0)
                                    {
                                        resulf = resulf + blockText + " ,";
                                    }
                                }
                            }

                        }
                    }

                }
                else // Khong co *
                {
                    if (content.Contains(blockText))
                    {
                        resulf = resulf + blockText + ",";
                    }
                }
            }
            if (resulf != "")
            {
                resulf = resulf.Substring(0 ,resulf.Length - 1);
                return (resulf);
            }
            else return "1";
        }


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
                return new DateTime(1900 ,01 ,01);
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
        //public static DateTime StringDMYHM_To_DateTime(string date)
        //{
        //    try
        //    {
        //        date = date.Trim();
        //        string[] A = date.Split(' ');
        //        string[] B = A[0].Split('-');
        //        string[] C = A[1].Split(':');
        //        return new DateTime(Convert.ToInt32(B[0]), Convert.ToInt32(B[1]), Convert.ToInt32(B[2]), Convert.ToInt32(C[0]), Convert.ToInt32(C[1]), Convert.ToInt32(C[2]));
        //    }
        //    catch (Exception ex)
        //    {
        //        return DateTime.MinValue;
        //    }
        //}


        public static void UpdateUserInfo_AtGlobal(Microsoft.AspNetCore.Http.HttpContext httpContext ,string avatar)
        {
            GlobalUser _globaluser = (GlobalUser)Session.Session.GetUserSession(httpContext);
            _globaluser.sys_userAvatar = avatar;
            Session.Session.SetSession(httpContext.Session ,"CurrentUserInfo" ,JsonConvert.SerializeObject(_globaluser));
        }


        public static void UpdateUserBranch_AtGlobal(Microsoft.AspNetCore.Http.HttpContext httpContext ,string branchid
            ,string branchcode ,string branchshortname ,string branchname ,string mcn ,string mcn_doc ,string floorid ,string roomid)
        {
            var _globaluser = Session.Session.GetUserSession(httpContext);
            _globaluser.sys_branchID = Convert.ToInt32(branchid);
            _globaluser.sys_floorID = Convert.ToInt32(floorid);
            _globaluser.sys_roomID = Convert.ToInt32(roomid);
            _globaluser.sys_branchCode = branchcode;
            _globaluser.sys_BranchShortName = branchshortname;
            _globaluser.sys_BranchName = branchname;
            _globaluser.syntax_scmcn = mcn;
            _globaluser.syntax_scmcn_doc = mcn_doc;
            Session.Session.SetSession(httpContext.Session ,"CurrentUserInfo" ,JsonConvert.SerializeObject(_globaluser));

        }
        public static void UpdateEmail_AtGlobal(Microsoft.AspNetCore.Http.HttpContext httpContext ,DataTable dtEmail)
        {
            try
            {
                var _globaluser = Session.Session.GetUserSession(httpContext);
                List<MailBrand> multimail = new List<MailBrand>();
                if (dtEmail != null && dtEmail.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dtEmail.Rows)
                    {
                        var r = new MailBrand();
                        r.sys_MailDisplayName = dataRow["DisplayName"].ToString();
                        r.sys_MailName = dataRow["Mail"].ToString();
                        r.sys_MailPassword = dataRow["Password"].ToString();
                        r.sys_MailHost = dataRow["Host"].ToString();
                        r.sys_MailPort = dataRow["Port"].ToString();
                        r.sys_MailIs2ndAuthen = dataRow["Is2ndAuthen"].ToString();
                        multimail.Add(r);
                    }
                }
                _globaluser.sys_MailBrand = multimail;
                Session.Session.SetSession(httpContext.Session ,"CurrentUserInfo" ,JsonConvert.SerializeObject(_globaluser));
            }
            catch (Exception e)
            {

            }



        }
        // WAREHOUSE

        public static DataTable ExecuteCountNumberWarehouse(DataTable BeginTerm ,DataTable BodyTerm)
        {
            try
            {
                for (int i = 0; i < BodyTerm.Rows.Count; i++)
                {
                    int productID = Convert.ToInt32(BodyTerm.Rows[i]["ID"]);
                    decimal Input = Convert.ToDecimal(BodyTerm.Rows[i]["Input"]);
                    decimal InputTransfer = Convert.ToDecimal(BodyTerm.Rows[i]["InputTransfer"]);
                    decimal Output = Convert.ToDecimal(BodyTerm.Rows[i]["Output"]);
                    decimal OutputTransfer = Convert.ToDecimal(BodyTerm.Rows[i]["OutputTransfer"]);
                    decimal OutputTreat = Convert.ToDecimal(BodyTerm.Rows[i]["OutputTreat"]);
                    decimal OutputSale = Convert.ToDecimal(BodyTerm.Rows[i]["OutputSale"]);
                    decimal beginterm = FindNumberWarehouse(BeginTerm ,productID);
                    decimal left = beginterm + Input + InputTransfer - Output - OutputTransfer - OutputTreat - OutputSale;
                    BodyTerm.Rows[i]["NumBeginTerm"] = beginterm;
                    BodyTerm.Rows[i]["NumLeft"] = left;
                    BodyTerm.Rows[i]["AmountTotalBegin"] = FindAmountWarehouse(BeginTerm ,productID);
                }

                return BodyTerm;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private static decimal FindNumberWarehouse(DataTable BeginTerm ,int ProductID)
        {
            try
            {
                decimal resulf = 0;
                var resultRow = from myRow in BeginTerm.AsEnumerable()
                                where myRow.Field<Int32>("ID") == ProductID
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    resulf = Convert.ToDecimal(dtResult.Rows[i]["NUM"].ToString());
                }
                return resulf;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // add colorcode by N1, N2, N3
        public static DataTable AddBackgroundByNumberLeft(DataTable BeginTerm ,DataTable BodyTerm)
        {
            try
            {
                for (int i = 0; i < BodyTerm.Rows.Count; i++)
                {
                    int NumberLeft = Convert.ToInt32(BodyTerm.Rows[i]["Num_Left"]);
                    int N1 = Convert.ToInt32(BodyTerm.Rows[i]["N1"]);
                    int N2 = Convert.ToInt32(BodyTerm.Rows[i]["N2"]);
                    int N3 = Convert.ToInt32(BodyTerm.Rows[i]["N3"]);
                    string ColorCodeN1 = "";
                    string ColorCodeN2 = "";
                    string ColorCodeN3 = "";

                    for (int j = 0; j < BeginTerm.Rows.Count; j++)
                    {
                        if (BeginTerm.Rows[j]["Name"].ToString() == "N3")
                        {
                            ColorCodeN3 = BeginTerm.Rows[j]["Color"].ToString();
                        }
                        else if (BeginTerm.Rows[j]["Name"].ToString() == "N2")
                        {
                            ColorCodeN2 = BeginTerm.Rows[j]["Color"].ToString();
                        }
                        else if (BeginTerm.Rows[j]["Name"].ToString() == "N1")
                        {
                            ColorCodeN1 = BeginTerm.Rows[j]["Color"].ToString();
                        }
                    }
                    if (NumberLeft <= N3)
                    {
                        BodyTerm.Rows[i]["ColorCode"] = ColorCodeN3;
                    }
                    else if (NumberLeft <= N2)
                    {
                        BodyTerm.Rows[i]["ColorCode"] = ColorCodeN2;
                    }
                    else if (NumberLeft <= N1)
                    {
                        BodyTerm.Rows[i]["ColorCode"] = ColorCodeN1;
                    }
                }

                return BodyTerm;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        // add unitChange for list product
        public static DataTable AddUnitChangeForProduct(DataTable BeginTerm ,DataTable BodyTerm)
        {
            for (int i = 0; i < BodyTerm.Rows.Count; i++)
            {
                int productID = Convert.ToInt32(BodyTerm.Rows[i]["ID"]);
                int productUnit = Convert.ToInt32(BodyTerm.Rows[i]["UnitID"]);
                int NumLeft = Convert.ToInt32(BodyTerm.Rows[i]["NumLeft"]);
                string Product_Unit = BodyTerm.Rows[i]["DVT"].ToString();
                string UnitChange = "";
                string NumLeftPerUnit = "";
                for (int j = 0; j < BeginTerm.Rows.Count; j++)
                {
                    if (productUnit != Convert.ToInt32(BeginTerm.Rows[j]["Unit_ID"]) && Convert.ToInt32(BeginTerm.Rows[j]["Product_ID"]) == productID)
                    {
                        UnitChange += BeginTerm.Rows[j]["UnitName"].ToString() + " = " + BeginTerm.Rows[j]["Number"].ToString() + " " + Product_Unit + "<br>";
                        int NumOfUnit = NumLeft / Convert.ToInt32(BeginTerm.Rows[j]["Number"].ToString());
                        int LeftOfUnit = NumLeft % Convert.ToInt32(BeginTerm.Rows[j]["Number"].ToString());
                        if (NumOfUnit != 0 || LeftOfUnit != 0)
                        {
                            if (NumOfUnit != 0)
                            {
                                if (LeftOfUnit == 0)
                                {
                                    NumLeftPerUnit += "</br> <span> ≈ " + NumOfUnit + " " + BeginTerm.Rows[j]["UnitName"].ToString() + "</span>";
                                }
                                else
                                {
                                    NumLeftPerUnit += "</br> <span> ≈ " + NumOfUnit + " " + BeginTerm.Rows[j]["UnitName"].ToString() + "  " + LeftOfUnit + " " + Product_Unit + "</span>";
                                }
                            }

                        }

                    }
                }
                BodyTerm.Rows[i]["UnitChange"] = UnitChange;
                BodyTerm.Rows[i]["NumLeftString"] = NumLeftPerUnit;
            }
            return BodyTerm;
        }

        public static long RoundingTo(long myNum ,long roundTo)
        {
            if (roundTo <= 0) return myNum;
            return (myNum + roundTo / 2) / roundTo * roundTo;
        }

        public static DataTable ExecuteCountAmountWarehouse(DataTable BeginTerm ,DataTable BodyTerm)
        {
            try
            {
                for (int i = 0; i < BodyTerm.Rows.Count; i++)
                {
                    int productID = Convert.ToInt32(BodyTerm.Rows[i]["ID"]);
                    int QuantityPrice = FindAmountWarehouse(BeginTerm ,productID);
                    BodyTerm.Rows[i]["QuantityPrice"] = QuantityPrice;

                    int left = Convert.ToInt32(BodyTerm.Rows[i]["NumLeft"]);
                    int totalInputItem = Convert.ToInt32(BodyTerm.Rows[i]["Input"]) + Convert.ToInt32(BodyTerm.Rows[i]["InputTransfer"]);
                    int totalOutputItem = Convert.ToInt32(BodyTerm.Rows[i]["Output"]) + Convert.ToInt32(BodyTerm.Rows[i]["OutputTransfer"])
                                    + Convert.ToInt32(BodyTerm.Rows[i]["OutputTreat"]) + Convert.ToInt32(BodyTerm.Rows[i]["OutputSale"]);

                    BodyTerm.Rows[i]["AmountTotalLeft"] = RoundingTo((left * QuantityPrice) ,500);
                    BodyTerm.Rows[i]["TotalInputAmountItem"] = RoundingTo((totalInputItem * QuantityPrice) ,500);
                    BodyTerm.Rows[i]["TotalOutputAmoutItem"] = RoundingTo((totalOutputItem * QuantityPrice) ,500);
                }

                return BodyTerm;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private static int FindAmountWarehouse(DataTable BeginTerm ,int ProductID)
        {
            try
            {
                int resulf = 0;
                var resultRow = from myRow in BeginTerm.AsEnumerable()
                                where myRow.Field<Int32>("ID") == ProductID
                                select myRow;
                DataTable dtResult = resultRow.CopyToDataTable();
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    resulf = Convert.ToInt32(dtResult.Rows[i]["AMOUNT"].ToString());
                }
                return resulf;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static DataTable ExecuteCountNumberWarhouseTicket(DataTable BeginTerm ,DataTable BodyTerm ,int ProductID)
        {
            try
            {
                decimal initialize = 0;
                decimal terminal = 0;
                decimal NUMDVC_EX = 0;
                for (int i = 0; i < BodyTerm.Rows.Count; i++)
                {
                    NUMDVC_EX = Convert.ToDecimal(BodyTerm.Rows[i]["NUMDVC_EX"]);
                    if (i == 0)
                    {
                        initialize = FindNumberWarehouse(BeginTerm ,ProductID);
                    }
                    else
                    {
                        initialize = terminal;
                    }
                    terminal = initialize + NUMDVC_EX;
                    BodyTerm.Rows[i]["InitializeNum"] = initialize;
                    BodyTerm.Rows[i]["TerminalNum"] = terminal;
                    initialize = terminal;
                }

                return BodyTerm;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static int DetectGenderIdByName(string name)
        {
            try
            {
                if (name == "") return 0;
                name = Comon.ConvertToUnsign(name).Trim().ToLower();
                if (name.Contains("name") || name.Contains("male"))
                {
                    return 60;
                }
                else
                {
                    return 61;
                }

            }
            catch (Exception ex)
            {
                return 0;
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

        #region // Work Scheduler
        /// <summary>
        /// COnvert dt to array work scheduler
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static WorkScheduler[] GetArrayTimeLine_Detail(DataTable dt)
        {
            try
            {
                var data = JToken.Parse(dt.Rows[0]["Data"].ToString()).ToArray();
                WorkScheduler[] ws = new WorkScheduler[data.Length];

                for (int k = 0; k < data.Length; k++)
                {
                    WorkScheduler w = new WorkScheduler();
                    w.dayofweek = (data[k]["dayofweek"] != null) ? data[k]["dayofweek"].ToString() : "";
                    w.off = Convert.ToBoolean(data[k]["off"]);
                    var datashifts = JToken.Parse(data[k]["shift"].ToString()).ToArray();
                    shifts[] _shifts = new shifts[datashifts.Length];
                    for (int z = 0; z < datashifts.Length; z++)
                    {
                        shifts shi = new shifts();
                        shi.shift = datashifts[z]["shift"].ToString();
                        shi.branch = datashifts[z]["branch"].ToString();
                        _shifts[z] = shi;
                    }
                    w.shift = _shifts;
                    ws[k] = w;
                }
                return ws;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static WorkScheduler[] GetArrayTimeLine_Detail_Empty()
        {
            try
            {
                WorkScheduler[] ws = new WorkScheduler[7];
                for (int k = 0; k < 7; k++)
                {
                    WorkScheduler w = new WorkScheduler();
                    w.dayofweek = k.ToString();
                    w.off = false;
                    w.shift = new shifts[] { new shifts { shift = "" ,branch = "" } };
                    ws[k] = w;
                }
                return ws;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// COnvert dt to array work scheduler
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static WorkScheduler_TimeLine[] GetArrayTimeLine(DataTable dt)
        {
            try
            {
                WorkScheduler_TimeLine[] works = new WorkScheduler_TimeLine[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WorkScheduler_TimeLine work = new WorkScheduler_TimeLine();
                    work.Date = Convert.ToDateTime(dt.Rows[i]["Date"]);
                    var data = JToken.Parse(dt.Rows[i]["Data"].ToString()).ToArray();
                    WorkScheduler[] ws = new WorkScheduler[data.Length];

                    for (int k = 0; k < data.Length; k++)
                    {
                        WorkScheduler w = new WorkScheduler();
                        w.dayofweek = (data[k]["dayofweek"] != null) ? data[k]["dayofweek"].ToString() : "";
                        w.off = Convert.ToBoolean(data[k]["off"]);
                        var datashifts = JToken.Parse(data[k]["shift"].ToString()).ToArray();
                        shifts[] _shifts = new shifts[datashifts.Length];
                        for (int z = 0; z < datashifts.Length; z++)
                        {
                            shifts shi = new shifts();
                            shi.shift = datashifts[z]["shift"].ToString();
                            shi.branch = datashifts[z]["branch"].ToString();
                            _shifts[z] = shi;
                        }
                        w.shift = _shifts;
                        ws[k] = w;
                    }
                    work.workScheduler = ws;
                    works[i] = work;
                }
                return works;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static WorkScheduler_TimeLine_Employee[] GetArrayTimeLineAllEmployee(DataTable dt)
        {
            try
            {
                WorkScheduler_TimeLine_Employee[] works = new WorkScheduler_TimeLine_Employee[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WorkScheduler_TimeLine_Employee work = new WorkScheduler_TimeLine_Employee();
                    work.Date = Convert.ToDateTime(dt.Rows[i]["Date"]);
                    work.EmployeeID = Convert.ToInt32(dt.Rows[i]["EmpID"]);
                    var data = JToken.Parse(dt.Rows[i]["Data"].ToString()).ToArray();
                    WorkScheduler[] ws = new WorkScheduler[data.Length];

                    for (int k = 0; k < data.Length; k++)
                    {
                        WorkScheduler w = new WorkScheduler();
                        w.dayofweek = (data[k]["dayofweek"] != null) ? data[k]["dayofweek"].ToString() : "";
                        w.off = Convert.ToBoolean(data[k]["off"]);
                        var datashifts = JToken.Parse(data[k]["shift"].ToString()).ToArray();
                        shifts[] _shifts = new shifts[datashifts.Length];
                        for (int z = 0; z < datashifts.Length; z++)
                        {
                            shifts shi = new shifts();
                            shi.shift = datashifts[z]["shift"].ToString();
                            shi.branch = datashifts[z]["branch"].ToString();
                            _shifts[z] = shi;
                        }
                        w.shift = _shifts;
                        ws[k] = w;
                    }
                    work.workScheduler = ws;
                    works[i] = work;
                }
                return works;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// string to object work scheduler
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static WorkScheduler_TimeLine GetTimeLine_FromString(string stringdata)
        {
            try
            {
                WorkScheduler_TimeLine work = new WorkScheduler_TimeLine();
                var data = JToken.Parse(stringdata).ToArray();
                WorkScheduler[] ws = new WorkScheduler[data.Length];

                for (int k = 0; k < data.Length; k++)
                {
                    WorkScheduler w = new WorkScheduler();
                    w.dayofweek = data[k]["dayofweek"].ToString();
                    w.off = Convert.ToBoolean(data[k]["off"]);
                    var datashifts = JToken.Parse(data[k]["shift"].ToString()).ToArray();

                    shifts[] _shifts = new shifts[datashifts.Length];
                    for (int z = 0; z < datashifts.Length; z++)
                    {
                        shifts shi = new shifts();
                        shi.shift = datashifts[z]["shift"].ToString();
                        shi.branch = datashifts[z]["branch"].ToString();
                        _shifts[z] = shi;
                    }
                    w.shift = _shifts;
                    ws[k] = w;
                }
                work.workScheduler = ws;
                return work;

            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        /// COnvert dt to array work scheduler
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static WorkScheduler_Extension[] GetArrayTimeLine_Detail_Particular(DataTable dt ,int dayofweek)
        {
            try
            {
                var data = JToken.Parse(dt.Rows[0]["Data"].ToString()).ToArray();
                WorkScheduler_Extension[] ws = new WorkScheduler_Extension[1];

                for (int k = 0; k < data.Length; k++)
                {
                    if (data[k]["dayofweek"] != null && data[k]["dayofweek"].ToString() == dayofweek.ToString())
                    {
                        WorkScheduler_Extension w = new WorkScheduler_Extension();
                        w.off = Convert.ToBoolean(data[k]["off"]);
                        var datashifts = JToken.Parse(data[k]["shift"].ToString()).ToArray();
                        shifts[] _shifts = new shifts[datashifts.Length];
                        for (int z = 0; z < datashifts.Length; z++)
                        {
                            shifts shi = new shifts();
                            shi.shift = datashifts[z]["shift"].ToString();
                            shi.branch = datashifts[z]["branch"].ToString();
                            _shifts[z] = shi;
                        }
                        w.shift = _shifts;
                        ws[0] = w;
                    }
                }
                return ws;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static WorkScheduler_Extension[] GetArrayTimeLine_Detail_Particular_Ex(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0) return null;
                var data = JToken.Parse(dt.Rows[0]["Data"].ToString()).ToArray();
                WorkScheduler_Extension[] ws = new WorkScheduler_Extension[data.Length];

                for (int k = 0; k < data.Length; k++)
                {
                    WorkScheduler_Extension w = new WorkScheduler_Extension();
                    w.off = Convert.ToBoolean(data[k]["off"]);
                    var datashifts = JToken.Parse(data[k]["shift"].ToString()).ToArray();
                    shifts[] _shifts = new shifts[datashifts.Length];
                    for (int z = 0; z < datashifts.Length; z++)
                    {
                        shifts shi = new shifts();
                        shi.shift = datashifts[z]["shift"].ToString();
                        shi.branch = datashifts[z]["branch"].ToString();
                        _shifts[z] = shi;
                    }
                    w.shift = _shifts;
                    ws[k] = w;

                }
                return ws;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static WorkScheduler_Extension[] GetArrayTimeLine_Detail_Empty_Particular()
        {
            try
            {
                WorkScheduler_Extension[] ws = new WorkScheduler_Extension[1];
                WorkScheduler_Extension w = new WorkScheduler_Extension();
                w.off = false;
                w.shift = new shifts[] { new shifts { shift = "" ,branch = "" } };
                ws[0] = w;

                return ws;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region // Alarm Scheduler

        public static AlrmScheduler[] GetArrayTimeLine_AlarmScheduler(DataTable dt)
        {
            try
            {
                AlrmScheduler[] works = new AlrmScheduler[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AlrmScheduler work = new AlrmScheduler();
                    var data = JToken.Parse(dt.Rows[i]["Data"].ToString()).ToArray();
                    work.id = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                    work.dayofweek = Convert.ToInt32(data[0]["dayofweek"].ToString());
                    work.type = Convert.ToInt32(data[0]["type"].ToString());
                    work.day = Convert.ToInt32(data[0]["day"].ToString());
                    work.hour = data[0]["hour"].ToString();
                    work.date = data[0]["date"].ToString();
                    work.checklist = dt.Rows[i]["DataCheckList"].ToString();
                    work.title = dt.Rows[i]["Title"].ToString();
                    work.content = dt.Rows[i]["Content"].ToString();
                    work.createdName = (dt.Rows[i]["CreatedName"] != null) ? dt.Rows[i]["CreatedName"].ToString() : "";
                    work.createdWorkName = (dt.Rows[i]["CreatedWorkName"] != null) ? dt.Rows[i]["CreatedWorkName"].ToString() : "";
                    work.dateCreated = Convert.ToDateTime(dt.Rows[i]["Cretead"]);
                    work.status = Convert.ToInt32(dt.Rows[i]["Status"].ToString());
                    work.empWork = Convert.ToInt32(dt.Rows[i]["EmpWork"].ToString());
                    work.empCreate = Convert.ToInt32(dt.Rows[i]["EmpCreate"].ToString());
                    work.editButton = Convert.ToInt32(dt.Rows[i]["EditButton"].ToString());
                    work.deleteButton = Convert.ToInt32(dt.Rows[i]["DeleteButton"].ToString());
                    works[i] = work;
                }
                return works;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AlrmScheduler_Extension[] GetArrayTimeLine_AlarmSchedulerExtension(DataTable dt)
        {
            try
            {
                AlrmScheduler_Extension[] works = new AlrmScheduler_Extension[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AlrmScheduler_Extension work = new AlrmScheduler_Extension();
                    work.alarmID = Convert.ToInt32(dt.Rows[i]["AlarmID"].ToString());
                    work.status = Convert.ToInt32(dt.Rows[i]["Status"].ToString());
                    work.date = Convert.ToDateTime(dt.Rows[i]["Date"]);
                    work.isNew = Convert.ToInt32(dt.Rows[i]["IsNew"].ToString());
                    work.countComment = Convert.ToInt32(dt.Rows[i]["CountComment"].ToString());
                    works[i] = work;
                }
                return works;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion



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
            try
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
            catch
            {
                return null;
            }
        }

        // CHuyen du lieu
        public static async Task Convert_Data_From_Folder()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Cus" ,typeof(String));
            dt.Columns.Add("Folder" ,typeof(String));
            dt.Columns.Add("ImageName" ,typeof(String));

            string none_change = "App,Disease,MemberShip,Sys_Media,Sys_Pdf_War,Teeth";
            List<string> parrentFolder = GetAllDirectoryInFolder("");
            if (parrentFolder == null || parrentFolder.Count == 0)
            {
                return;
            }
            else
            {
                foreach (var dr in parrentFolder)
                {
                    if (!none_change.Contains(dr))
                    {

                        string _cusid = dr;
                        List<string> folders = GetAllDirectoryInFolder(_cusid);
                        foreach (var drsub in folders)
                        {
                            string _sub = drsub;
                            List<string> images = GetAllDirectoryInFolder(_cusid + "/" + drsub);
                            DataTable dtFolder = new DataTable();
                            using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                            {
                                dtFolder = await confunc.ExecuteDataTable("[MLU_sp_DATA_INSERT_FOLDER]" ,CommandType.StoredProcedure ,
                                  "@CUS" ,SqlDbType.NVarChar ,_cusid
                                  ,"@FolderName" ,SqlDbType.NVarChar ,_sub);
                            }
                            int idCurrentFolder = Convert.ToInt32(dtFolder.Rows[0]["ID"].ToString());
                            foreach (var drimage in images)
                            {
                                using (Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                                {
                                    dtFolder = await confunc.ExecuteDataTable("[MLU_sp_DATA_INSERT_IMAGE]" ,CommandType.StoredProcedure ,
                                      "@ID" ,SqlDbType.NVarChar ,idCurrentFolder.ToString()
                                      ,"@ImageName" ,SqlDbType.NVarChar ,drimage);
                                }


                                //DataRow drow = dt.NewRow();
                                //drow["Cus"] = _cusid;
                                //drow["Folder"] = _sub;
                                //drow["ImageName"] = drimage;
                                //dt.Rows.Add(drow);
                            }

                        }

                    }
                }
                //if (!resulf) return false; // Khong co folder
                //else
                //{

                //    List<string> image_in_current = GetAllDirectoryInFolder(customer + "/" + directory);
                //    List<string> file_in_current = GetAllDirectoryInFolder(customer + "/" + directory + "/" + "File");
                //    if ((image_in_current == null || image_in_current.Count == 0) && (file_in_current == null || file_in_current.Count == 0))
                //    {
                //        return DeleteDirectoryFTP(customer + "/" + directory);
                //    }
                //    return false;
                //}
            }
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
                if (data != null && data.Rows.Count > 0)
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

        public static Dictionary<string ,string> ConvertRowsToDictionary(DataTable table ,DataRow row)
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
