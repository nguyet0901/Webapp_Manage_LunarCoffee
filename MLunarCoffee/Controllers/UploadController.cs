using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UploadController : ControllerBase
    {

        //#region //File accept

        List<string> ExtFileImageIphone = new List<string> { ".heif" ,".heic" };
        List<string> ExtAttachmentFile = new List<string> { ".pdf", ".doc", ".docx", ".xlsx", ".xls"
                                 , ".mp3", ".flac", ".aac", ".ogg"};
        List<string> ExtAttachmentFile_Lib = new List<string> { ".pdf", ".doc", ".docx", ".xlsx", ".xls"
                                 , ".png", ".jpg", ".jpeg", ".heif", ".svg"
                                , ".mp3", ".flac", ".aac", ".ogg" };
        List<string> ExtAttachmentFile_Student = new List<string> { ".pdf" ,".doc" ,".docx" ,".xlsx" ,".xls" };
        List<string> ExtAttachmentFile_Service = new List<string> { ".pdf" };

        List<string> ExtAttachmentVideo = new List<string> {   ".mp4", ".wmv", ".mkv", ".flv"
                                        , ".mpg", ".wma", ".wav"
                                        , ".mov" };
        //#endregion

        [Authorize]
        [HttpPost("Upload")]
        public async Task<IActionResult> Index(IList<IFormFile> uploadfiles)
        {
            try
            {
                if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count != 0)
                {
                    var Serverpath = Path.Combine(Directory.GetCurrentDirectory() ,"wwwroot" ,"UploadedFiles");
                    string customerID = HttpContext.Request.Query["CustomerID"];
                    string folderName = HttpContext.Request.Query["FolderName"];
                    string Type = HttpContext.Request.Query["Type"];
                    string ConvertToFormat = HttpContext.Request.Query["ToFormat"];
                    string Token = HttpContext.Request.Query["Token"];
                    string folderNameChild = HttpContext.Request.Query["FolderNameChild"];
                    string folderID = HttpContext.Request.Query["FolderID"];

                    if (Comon.Global.sys_AllowUploadVideo == 1)
                    {
                        ExtAttachmentFile.AddRange(ExtAttachmentVideo);
                        ExtAttachmentFile_Lib.AddRange(ExtAttachmentVideo);
                    }

                    if (Type == null || Type == "undefined")
                    {
                        return Content("0");
                    }
                    if (Type == "Image" && (customerID == null || folderName == null || folderName == "undefined" || customerID == "undefined"))
                    {
                        return Content("0");
                    }
                    foreach (IFormFile source in HttpContext.Request.Form.Files)
                    {

                        var postedFile = source;
                        string file = postedFile.FileName;


                        if (!Directory.Exists(Serverpath))
                            Directory.CreateDirectory(Serverpath);

                        string fileDirectory = Serverpath;
                        string fullpath = (Path.GetFileName(fileDirectory + "\\" + file)).ToLower();
                        if (Comon.Comon.CheckExtensionFileInBlacklist(fullpath))
                            return Content("0");

                        if (HttpContext.Request.Query["fileName"].ToString() != "")
                        {
                            file = HttpContext.Request.Query["fileName"];
                            if (System.IO.File.Exists(fileDirectory + "\\" + file))
                            {
                                System.IO.File.Delete(fileDirectory + "\\" + file);
                            }
                        }

                        string ext = (Path.GetExtension(fileDirectory + "\\" + file)).ToLower();

                        if ((ext != ".jpg" && ext != ".png" && ext != ".svg" && ext != ".webp" && ext != ".jpeg" && ext != ".gif" && ext != ".heif" && ext != ".heic")
                            && Type != "Attachment"
                            && Type != "AttachmentFile"
                            && Type != "Library"
                            && Type != "ServiceFile"
                            && Type != "StudentLibrary"
                            && Type != "MailTemplate")
                        {
                            return Content("0");
                        }
                        var fileLength = HttpContext.Request.ContentLength;

                        if (Type != "Library" && Type != "StudentLibrary" && Comon.Global.sys_IsOutside == 0)
                        {
                            if ((Type == "Image" || Type == "AttachmentFile") && fileLength > 5242880)
                            {
                                return Content("0");
                            }
                            else if (fileLength > 6000000)
                            {
                                return Content("0");
                            }
                        }
                        string filePath = Path.Combine(Serverpath ,source.FileName);
                        Stream fileNew = new MemoryStream();
                        fileNew = HttpContext.Request.Form.Files[0].OpenReadStream();
                        string realFilename = file;

                        bool isFileIphone = Comon.Comon.CheckExtensionFileValid(ExtFileImageIphone ,ext); // Kiểm tra định dạng hình iphone
                        if (isFileIphone == true)
                        {
                            if (Comon.Global.sys_IsOutside == 1)
                            {
                                fileNew = Comon.Comon.ConvertImageIphone(fileNew); // Convert to JPG
                                realFilename = realFilename.ToLower().Replace(ext.ToLower() ,".jpg");
                                ext = ".jpg";
                            }
                            else
                            {
                                fileNew = Comon.Comon.ConvertImageWebp(fileNew); // Convert to webp
                                realFilename = realFilename.ToLower().Replace(ext.ToLower() ,".webp");
                                ext = ".webp";
                            }
                        }

                        
                        var Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                        file = Time + Guid.NewGuid() + ext;
                        //if (Type == "Avatar")
                        //{
                        //    using (var stream = fileNew)
                        //    {
                        //        byte[] fileData = null;
                        //        MemoryStream ms = new MemoryStream();
                        //        stream.CopyTo(ms);
                        //        fileData = ms.ToArray();
                        //        string base64String = Comon.Comon.ResizeImage(fileData, 200, 200);
                        //        return Content(base64String);
                        //    }
                        //}
                        //else
                        if (Type == "AvatarCustomer")
                        {
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilenameResize = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                            string linkFeauture = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"_AvatarCustomer" ,_realFilenameResize);
                            string resulfResize = await Update_Image(ext ,linkFeauture ,fileNew ,filePath ,true ,ConvertToFormat);
                            if (resulfResize == "") return Content("0");
                            return Content(Comon.Global.sys_HTTPImageRoot + "_AvatarCustomer/" + _realFilenameResize);
                        }
                        else if (Type == "bigAvatar")
                        {
                            using (var stream = fileNew)
                            {
                                byte[] fileData = null;
                                MemoryStream ms = new MemoryStream();
                                stream.CopyTo(ms);
                                fileData = ms.ToArray();
                                string base64String = Comon.Comon.ResizeImage(fileData ,300 ,100);
                                return Content(base64String);
                            }
                        }
                        else if (Type == "SoftwareImage")
                        {
                            using (var stream = fileNew)
                            {
                                byte[] fileData = null;
                                MemoryStream ms = new MemoryStream();
                                stream.CopyTo(ms);
                                fileData = ms.ToArray();
                                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                                int height = image.Height;
                                int width = image.Width;
                                string base64String = Comon.Comon.ResizeImage(fileData ,width ,height);
                                return Content(base64String);
                            }
                        }
                        else if (Type == "Image")
                        {
                            if(Comon.Global.sys_IsOutside == 0 && ext != ".webp")
                            {
                                fileNew = Comon.Comon.ConvertImageWebp(fileNew);
                                ext = ".webp";
                            }
                           

                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilename = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                            string _realFilenameFeauture = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + "(fea)" + ext.ToLower();
                            string link = String.Format(@"{0}{1}/{2}/{3}" ,Comon.Global.sys_ServerImageFolder ,customerID ,folderName ,_realFilename);
                            string linkFeauture = String.Format(@"{0}{1}/{2}/{3}" ,Comon.Global.sys_ServerImageFolder ,customerID ,folderName ,_realFilenameFeauture);
                            string resulf = await Update_Image(ext ,link ,fileNew ,filePath ,false ,"");
                            string resulfFeauture = await Update_Image(ext ,linkFeauture ,fileNew ,filePath ,true ,"");
                            string realFileID = folderID == null ? "0" : folderID;
                            if (resulf == "" || resulfFeauture == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(_realFilename + "@image@" + realFilename + "@image@" + _realFilenameFeauture + "@image@" + realFileID);
                            }
                        }
                        else if (Type == "AvatarUser")
                        {
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilenameResize = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + "(fea)" + ext.ToLower();
                            string linkFeauture = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"_AvatarUser" ,_realFilenameResize);
                            string resulfFeauture = await Update_Image(ext ,linkFeauture ,fileNew ,filePath ,true ,"");
                            if (resulfFeauture == "") return Content("0");
                            else return Content(Comon.Global.sys_HTTPImageRoot + "_AvatarUser/" + _realFilenameResize);
                        }
                        else if (Type == "Logo")
                        {
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilename = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                            string link = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"_Logo" ,_realFilename);
                            string resulf = await Update_Image(ext ,link ,fileNew ,filePath ,false ,"");
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(Comon.Global.sys_HTTPImageRoot + "_Logo/" + _realFilename);
                            }

                        }
                        else if (Type == "ServiceImage")
                        {
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilenameResize = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + "(fea)" + ext.ToLower();
                            string linkFeauture = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"_Sysimage" ,_realFilenameResize);
                            string resulfFeauture = await Update_Image(ext ,linkFeauture ,fileNew ,filePath ,true ,"");
                            if (resulfFeauture == "") return Content("0");
                            else return Content(Comon.Global.sys_HTTPImageRoot + "_Sysimage/" + _realFilenameResize);
                        } 
                        else if (Type == "AttachmentFile")
                        {

                            string ftpFolder = String.Format(@"{0}{1}/{2}/{3}" ,Comon.Global.sys_ServerImageFolder ,customerID ,folderName ,folderNameChild);
                            string resulf = await Update_Attachment_File(ext ,Convert.ToDouble(fileLength) ,realFilename ,ftpFolder ,customerID ,folderName ,folderNameChild ,HttpContext.Request.Form.Files[0]);
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(resulf);
                            }
                        }
                        else if (Type == "StudentLibrary")
                        {

                            string ftpFolder = String.Format(@"{0}Sys_Student/{1}" ,Comon.Global.sys_ServerImageFolder ,folderName);
                            string resulf = await Update_StudentLibrary(ext ,Convert.ToDouble(fileLength) ,ftpFolder ,realFilename ,HttpContext.Request.Form.Files[0]);
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(resulf);
                            }
                        }
                        else if (Type == "ServiceFile")
                        {

                            string ftpFolder = String.Format(@"{0}_Sys_Service/{1}" ,Comon.Global.sys_ServerImageFolder ,folderName);
                            string resulf = await Update_ServiceFile(ext ,Convert.ToDouble(fileLength) ,ftpFolder ,realFilename ,HttpContext.Request.Form.Files[0]);
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(resulf);
                            }
                        }
                        else if (Type == "Library")
                        {
                            string ftpFolder = "", ftpuser = "", ftppassword = "";
                            if (Comon.Global.sys_FLibUrl != null)
                            {
                                if (Comon.Global.sys_FLibUrl == "")
                                {
                                    if (Comon.Global.sys_FLib != "")
                                    {
                                        ftpuser = Comon.Global.sys_ServerImageUserName;
                                        ftppassword = Comon.Global.sys_ServerImagePassword;
                                        ftpFolder = String.Format(@"{0}_Library/{1}" ,Comon.Global.sys_ServerImageFolder ,Comon.Global.sys_FLib);
                                        string resulf = await Update_Library(ext ,Convert.ToDouble(fileLength) ,ftpFolder ,realFilename ,HttpContext.Request.Form.Files[0] ,ftpuser ,ftppassword);
                                        if (resulf == "")
                                        {
                                            return Content("0");
                                        }
                                        else
                                        {
                                            return Content(resulf);
                                        }
                                    }
                                    else return Content("0");
                                }
                                else
                                {
                                    if (Comon.Global.sys_FLib != "")
                                    {
                                        ftpuser = Comon.Global.sys_FLibUrlUser;
                                        ftppassword = Comon.Global.sys_FLibUrlPass;
                                        ftpFolder = String.Format(@"{0}{1}" ,Comon.Global.sys_FLibUrl ,Comon.Global.sys_FLib);
                                        string resulf = await Update_Library(ext ,Convert.ToDouble(fileLength) ,ftpFolder ,realFilename ,HttpContext.Request.Form.Files[0] ,ftpuser ,ftppassword);
                                        if (resulf == "")
                                        {
                                            return Content("0");
                                        }
                                        else
                                        {
                                            return Content(resulf);
                                        }
                                    }
                                    else return Content("0");

                                }



                            }
                            else return Content("0");
                        }

                        else if (Type == "MailTemplate")
                        {

                            string ftpFolder = String.Format(@"{0}{1}" ,Comon.Global.sys_ServerImageFolder ,"_Template");
                            string resulf = await Update_Html(ext ,Convert.ToDouble(fileLength) ,realFilename ,ftpFolder ,HttpContext.Request.Form.Files[0]);
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(resulf);
                            }
                        }
                        else if (Type == "ClientApp")
                        {
                            string link = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"App" ,file);
                            string resulf = await Update_Image(ext ,link ,fileNew ,filePath ,false ,"");
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(Comon.Global.sys_HTTPImageRoot + "App/" + file);
                            }
                        }
                        else if (Type == "Membership")
                        {
                            string link = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"MemberShip" ,file);
                            string resulf = await Update_Image(ext ,link ,fileNew ,filePath ,false ,"");
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(Comon.Global.sys_HTTPImageRoot + "MemberShip/" + file);
                            }

                        }
                        else if (Type == "CustomerPage")
                        {
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilename = realFilename.ToLower();
                            string link = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"_Web" ,_realFilename);
                            string resulf = await Update_Image(ext ,link ,fileNew ,filePath ,false ,"");
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(Comon.Global.sys_HTTPImageRoot + "_Web/" + _realFilename);
                            }
                        }
                        else if (Type == "StudentManagement")
                        {
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilename = realFilename.ToLower();
                            string link = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"_StudentManagement" ,_realFilename);
                            string resulf = await Update_Image(ext ,link ,fileNew ,filePath ,false ,"");
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(Comon.Global.sys_HTTPImageRoot + "_StudentManagement/" + _realFilename);
                            }
                        }
                        else if (Type == "AvatarStudent")
                        {
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilenameResize = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                            string linkFeauture = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"_StudentManagement/_AvatarStudent" ,_realFilenameResize);
                            string resulfResize = await Update_Image(ext ,linkFeauture ,fileNew ,filePath ,true ,"");
                            if (resulfResize == "") return Content("0");
                            return Content(Comon.Global.sys_HTTPImageRoot + "_StudentManagement/_AvatarStudent/" + _realFilenameResize);
                        }
                        else if (Type == "SystemImage")
                        {
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilename = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                            string link = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"_Sysimage" ,_realFilename);
                            string resulf = await Update_Image(ext ,link ,fileNew ,filePath ,false ,"");
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(Comon.Global.sys_HTTPImageRoot + "_Sysimage/" + _realFilename);
                            }
                        }
                        else if (Type == "Disease")
                        {
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilename = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                            string link = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"Sys_Disease_Feature" ,_realFilename);
                            string resulf = await Update_Image(ext ,link ,fileNew ,filePath ,false ,"");
                            if (resulf == "")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(_realFilename);
                            }
                        }
                        else return Content("0");

                    }
                    return Content("1");
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
        private async Task<string> Update_Image(string ext ,string link ,Stream stream ,string filePath ,bool resize ,string ConvertToFormat)
        {
            try
            {

                byte[] fileData = null;
                using (var ms = new MemoryStream())
                {
                    if (ConvertToFormat != null && ConvertToFormat.ToLower().ToString() != "" && ConvertToFormat.ToLower().ToString() != ext)
                    {
                        if (ConvertToFormat == "jpg")
                        {
                            stream = Comon.Comon.ConvertImage(stream ,System.Drawing.Imaging.ImageFormat.Jpeg);
                            ext = "." + ConvertToFormat;
                        }
                        if (ConvertToFormat == "png")
                        {
                            stream = Comon.Comon.ConvertImage(stream ,System.Drawing.Imaging.ImageFormat.Png);
                            ext = "." + ConvertToFormat;
                        }
                        if (ConvertToFormat == "webp")
                        {
                            stream = Comon.Comon.ConvertImageWebp(stream);
                            ext = "." + ConvertToFormat;
                        }
                    }
                    stream.Position = 0;
                    stream.CopyTo(ms);
                    fileData = ms.ToArray();
                }
                if (resize && ext != ".svg")
                {
                    fileData = Comon.Comon.ResizeImage_Byte(fileData ,300 ,300); // RESIZE IMAGE
                }
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(link);
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.Credentials = new NetworkCredential(Comon.Global.sys_ServerImageUserName ,Comon.Global.sys_ServerImagePassword);
                req.ContentLength = fileData.Length;
                Stream reqStream = req.GetRequestStream();
                await reqStream.WriteAsync(fileData ,0 ,fileData.Length);
                reqStream.Close();
                return "1";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private async Task<string> Update_Html(string ext ,double fileLength ,string realFilename ,string ftpFolder ,Microsoft.AspNetCore.Http.IFormFile file)
        {
            try
            {
                string currentRealFileName = realFilename;
                long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                realFilename = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                if ((ext != ".html") && (ext != ".htm"))
                {
                    return "";
                }
                if (Convert.ToDouble(fileLength) > Convert.ToDouble("177790"))
                {
                    return "";
                }
                ext = ext.ToString();
                string type = ext.Remove(0 ,1);
                using (var stream = file.OpenReadStream())
                {
                    byte[] fileData = null;
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    fileData = ms.ToArray();
                    FtpWebRequest req = (FtpWebRequest)WebRequest.Create(String.Format(@"{0}/{1}" ,ftpFolder ,realFilename));
                    req.UseBinary = true;
                    req.Method = WebRequestMethods.Ftp.UploadFile;
                    req.Credentials = new NetworkCredential(Comon.Global.sys_ServerImageUserName ,Comon.Global.sys_ServerImagePassword);
                    req.ContentLength = fileData.Length;
                    Stream reqStream = req.GetRequestStream();
                    await reqStream.WriteAsync(fileData ,0 ,fileData.Length);
                    reqStream.Close();
                }
                string resulf = realFilename + "@" + type + "@" + currentRealFileName;
                return resulf;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private async Task<string> Update_Attachment_File(string ext ,double fileLength
            ,string realFilename ,string ftpFolder ,string customerID ,string folderName ,string folderNameChild
            ,Microsoft.AspNetCore.Http.IFormFile file)
        {
            try
            {
                string currentRealFileName = realFilename;
                long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                realFilename = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                if (!Comon.Comon.CheckExtensionFileValid(ExtAttachmentFile ,ext))
                {
                    return "";
                }
                if (Convert.ToDouble(fileLength) > Convert.ToDouble("10971520"))
                {
                    return "";
                }
                ext = ext.ToString();
                string type = ext.Remove(0 ,1);
                // Tao folder File trong Customer, chưa có thì tạo
                Check_Folder_Is_Exist(customerID + "/" + folderName + "/" + folderNameChild);
                // name da tồn tại thì thay name
                if (!Checking_Exist_File_Name(String.Format(@"{0}/{1}" ,ftpFolder ,realFilename)))
                {
                    realFilename = realFilename.Replace(ext ,"") + "(1)" + ext;
                }
                using (var stream = file.OpenReadStream())
                {
                    byte[] fileData = null;
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    fileData = ms.ToArray();
                    FtpWebRequest req = (FtpWebRequest)WebRequest.Create(String.Format(@"{0}/{1}" ,ftpFolder ,realFilename));
                    req.UseBinary = true;
                    req.Method = WebRequestMethods.Ftp.UploadFile;
                    req.Credentials = new NetworkCredential(Comon.Global.sys_ServerImageUserName ,Comon.Global.sys_ServerImagePassword);
                    req.ContentLength = fileData.Length;
                    Stream reqStream = req.GetRequestStream();
                    await reqStream.WriteAsync(fileData ,0 ,fileData.Length);
                    reqStream.Close();
                }
                string resulf = realFilename + "@" + type + "@" + currentRealFileName;
                return resulf;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private async Task<string> Update_StudentLibrary(string ext ,double fileLength
                    ,string ftpFolder ,string realFilename
                    ,IFormFile file)
        {
            try
            {
                string currentRealFileName = realFilename;
                long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                realFilename = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                if (!Comon.Comon.CheckExtensionFileValid(ExtAttachmentFile_Student ,ext))
                {
                    return "";
                }

                if (Convert.ToDouble(fileLength) > Convert.ToDouble("11000000"))
                {
                    return "";
                }
                ext = ext.ToString();
                string type = ext.Remove(0 ,1); ;
                if (!Checking_Exist_File_Name(String.Format(@"{0}/{1}" ,ftpFolder ,realFilename)))
                {
                    realFilename = realFilename.Replace(ext ,"") + "(1)" + ext;
                }
                using (var stream = file.OpenReadStream())
                {
                    byte[] fileData = null;
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    fileData = ms.ToArray();
                    FtpWebRequest req = (FtpWebRequest)WebRequest.Create(String.Format(@"{0}/{1}" ,ftpFolder ,realFilename));
                    req.UseBinary = true;
                    req.Method = WebRequestMethods.Ftp.UploadFile;
                    req.Credentials = new NetworkCredential(Comon.Global.sys_ServerImageUserName ,Comon.Global.sys_ServerImagePassword);
                    req.ContentLength = fileData.Length;
                    Stream reqStream = req.GetRequestStream();
                    await reqStream.WriteAsync(fileData ,0 ,fileData.Length);
                    reqStream.Close();
                }
                string resulf = realFilename + "@" + type + "@" + currentRealFileName;
                return resulf;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private async Task<string> Update_ServiceFile(string ext ,double fileLength
                    ,string ftpFolder ,string realFilename
                    ,IFormFile file)
        {
            try
            {
                string currentRealFileName = realFilename;
                long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                realFilename = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                if (!Comon.Comon.CheckExtensionFileValid(ExtAttachmentFile_Service ,ext))
                {
                    return "";
                }

                if (Convert.ToDouble(fileLength) > Convert.ToDouble("11000000"))
                {
                    return "";
                }
                ext = ext.ToString();
                string type = ext.Remove(0 ,1); ;
                if (!Checking_Exist_File_Name(String.Format(@"{0}/{1}" ,ftpFolder ,realFilename)))
                {
                    realFilename = realFilename.Replace(ext ,"") + "(1)" + ext;
                }
                using (var stream = file.OpenReadStream())
                {
                    byte[] fileData = null;
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    fileData = ms.ToArray();
                    FtpWebRequest req = (FtpWebRequest)WebRequest.Create(String.Format(@"{0}/{1}" ,ftpFolder ,realFilename));
                    req.UseBinary = true;
                    req.Method = WebRequestMethods.Ftp.UploadFile;
                    req.Credentials = new NetworkCredential(Comon.Global.sys_ServerImageUserName ,Comon.Global.sys_ServerImagePassword);
                    req.ContentLength = fileData.Length;
                    Stream reqStream = req.GetRequestStream();
                    await reqStream.WriteAsync(fileData ,0 ,fileData.Length);
                    reqStream.Close();
                }
                string resulf = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_HTTPImageRoot ,"_Sys_Service" ,realFilename);

                return resulf;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private async Task<string> Update_Library(string ext ,double fileLength
                  ,string ftpFolder ,string realFilename
                  ,IFormFile file ,string ftpuser ,string ftppassword)
        {
            try
            {
                string currentRealFileName = realFilename;
                long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                realFilename = realFilename.ToLower().Replace(ext.ToLower() ,"") + "(" + n + ")" + ext.ToLower();
                if (!Comon.Comon.CheckExtensionFileValid(ExtAttachmentFile_Lib ,ext))
                {
                    return "";
                }

                if (Convert.ToDouble(fileLength) > Convert.ToDouble("11000000"))
                {
                    return "";
                }
                ext = ext.ToString();
                string type = ext.Remove(0 ,1);
                using (var stream = file.OpenReadStream())
                {
                    byte[] fileData = null;
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    fileData = ms.ToArray();
                    FtpWebRequest req = (FtpWebRequest)WebRequest.Create(String.Format(@"{0}/{1}" ,ftpFolder ,realFilename));
                    req.UseBinary = true;
                    req.Method = WebRequestMethods.Ftp.UploadFile;
                    req.Credentials = new NetworkCredential(ftpuser ,ftppassword);
                    req.ContentLength = fileData.Length;
                    Stream reqStream = req.GetRequestStream();
                    await reqStream.WriteAsync(fileData ,0 ,fileData.Length);
                    reqStream.Close();
                }
                string resulf = realFilename + "@" + type + "@" + currentRealFileName;
                return resulf;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private bool Checking_Exist_File_Name(string name)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(name);
                request.Credentials = new NetworkCredential(Comon.Global.sys_ServerImageUserName ,Comon.Global.sys_ServerImagePassword);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                try
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode ==
                        FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void Check_Folder_Is_Exist(string name)
        {
            try
            {
                List<string> currrentdirectories = Comon.Comon.GetAllDirectoryInFolder(name);
                if (currrentdirectories == null)
                {
                    Comon.Comon.CreateDirectoryFTP(name);
                }
            }
            catch (Exception ex)
            {

            }
        }


        // UploadFace
        [Authorize]
        [HttpPost("UploadFace")]
        public async Task<IActionResult> UploadFace(IList<IFormFile> uploadface)
        {
            try
            {
                const float _maximum_file = 11000000; // 10M
                string Method = HttpContext.Request.Query["Method"];
                string Type = HttpContext.Request.Query["Type"];
                string TimeSpan = HttpContext.Request.Query["TimeSpan"];
                if (Method.ToLower() == "upload")
                {
                    if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count != 0)
                    {
                        var Serverpath = Path.Combine(Directory.GetCurrentDirectory() ,"wwwroot/UploadedFiles");
                        foreach (IFormFile source in HttpContext.Request.Form.Files)
                        {
                            var postedFile = source;
                            string filename = postedFile.FileName;
                            string ext = (Path.GetExtension(filename)).ToLower();
                            var fileLength = HttpContext.Request.ContentLength;
                            string random = DateTime.Now.ToString("HH-mm-ss");
                            filename = filename.Replace(ext ,"") + "(" + random + ")" + ext;
                            string savedFileName = Serverpath + "/" + filename;

                            if (Type.ToLower() == "image")
                            {
                                if (ext != ".jpg" && ext != ".png" && ext != ".jpeg" && ext != ".heif")
                                {
                                    return Content("0");
                                }
                                if (fileLength > _maximum_file)
                                {
                                    return Content("0");
                                }
                                filename = Convert_Png_To_Jpg(filename);
                                string ftplink = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"Attachment" ,filename);
                                if (UploadFile(postedFile ,ftplink))
                                {
                                    return Content(filename + "||" + TimeSpan + "||" + Type.ToLower());
                                }
                                return Content("0");

                            }
                            else if (Type.ToLower() == "video")
                            {
                                if (ext != ".mp4")
                                {
                                    return Content("0");
                                }
                                if (fileLength > _maximum_file)
                                {
                                    return Content("0");
                                }

                                string ftplink = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"Attachment" ,filename);
                                if (UploadFile(postedFile ,ftplink))
                                {
                                    return Content(filename + "||" + TimeSpan + "||" + Type.ToLower());
                                }
                                return Content("0");
                            }

                            else if (Type.ToLower() == "file")
                            {
                                if (ext != ".xlsx" && ext != ".xls" && ext != ".doc" && ext != ".docx" && ext != ".pdf" && ext != ".mp3")
                                {
                                    return Content("0");
                                }
                                if (fileLength > _maximum_file)
                                {
                                    return Content("0");
                                }
                                string ftplink = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"Attachment" ,filename);
                                if (UploadFile(postedFile ,ftplink))
                                {
                                    return Content(filename + "||" + TimeSpan + "||" + Type.ToLower());
                                }
                                return Content("0");


                            }
                            return Content("0");
                        }
                    }
                    return Content("0");
                }
                else if (Method.ToLower() == "delete")
                {
                    if (HttpContext.Request.Form.Keys != null && HttpContext.Request.Form.Count != 0)
                    {
                        foreach (string key in HttpContext.Request.Form.Keys)
                        {
                            string filename = key;
                            string ftplink = String.Format(@"{0}{1}/{2}" ,Comon.Global.sys_ServerImageFolder ,"Attachment" ,filename);
                            if (DeleteFile(ftplink))
                                return Content("1");
                            else
                                return Content("0");
                        }
                    }
                    return Content("0");
                }
                else return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }
        private string Convert_Png_To_Jpg(string file)
        {
            try
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ".png")
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    return Path.GetFileNameWithoutExtension(file) + ".jpg";
                }
                return file;
            }
            catch (Exception ex)
            {
                return file;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private bool UploadFile(Microsoft.AspNetCore.Http.IFormFile file ,string linkfile)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    byte[] fileData = null;
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    fileData = ms.ToArray();
                    FtpWebRequest req = (FtpWebRequest)WebRequest.Create(linkfile);
                    req.UseBinary = true;
                    req.Method = WebRequestMethods.Ftp.UploadFile;
                    req.Credentials = new NetworkCredential(Comon.Global.sys_ServerImageUserName ,Comon.Global.sys_ServerImagePassword);
                    req.ContentLength = fileData.Length;
                    Stream reqStream = req.GetRequestStream();
                    reqStream.Write(fileData ,0 ,fileData.Length);
                    reqStream.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool DeleteFile(string des)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(des);
                request.Credentials = new NetworkCredential(Comon.Global.sys_ServerImageUserName ,Comon.Global.sys_ServerImagePassword);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }

}
