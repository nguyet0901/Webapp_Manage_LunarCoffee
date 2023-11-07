using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.GlobalStore;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UploadChattingController : ControllerBase
    {
        [Authorize]
        [HttpPost("Upload")]
        public async Task<IActionResult> Index(IList<IFormFile> uploadfiles)
        {
            try
            {
                if(HttpContext.Request.Form.Files!=null&&HttpContext.Request.Form.Files.Count!=0)
                {
                    var user = Session.GetUserSession(HttpContext);
                    var Serverpath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/UploadedFiles");
                    string conver = HttpContext.Request.Query["Conver"];
                    string Type = HttpContext.Request.Query["Type"];

                    if(conver==null||conver==""||Type==null||Type=="undefined"||conver==null)
                    {
                        return Content("0");
                    }
                    foreach(IFormFile source in HttpContext.Request.Form.Files)
                    {

                        var postedFile = source;
                        string file = postedFile.FileName;
                        if(!Directory.Exists(Serverpath)) Directory.CreateDirectory(Serverpath);
                        string fileDirectory = Serverpath;
                        if(HttpContext.Request.Query["fileName"].ToString()!="")
                        {
                            file=HttpContext.Request.Query["fileName"];
                            if(System.IO.File.Exists(fileDirectory+"\\"+file))
                            {
                                System.IO.File.Delete(fileDirectory+"\\"+file);
                            }
                        }

                        string ext = (Path.GetExtension(fileDirectory+"\\"+file)).ToLower();
                        if(ext!=".jpg"&&ext!=".png"&&ext!=".jpeg"&&Type=="Image")
                        {
                            return Content("0");
                        }
                        if(ext!=".pdf"&&ext!=".docx"&&ext!=".doc"&&ext!=".csv"&&ext!=".xls"&&ext!=".xlsx"&&Type=="File")
                        {
                            return Content("0");
                        }
                        var fileLength = HttpContext.Request.ContentLength;
                        if(fileLength>6000000)
                        {
                            return Content("0");
                        }

                        string realFilename = file;
                        string time = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                        file=time+Guid.NewGuid()+ext;
                        if(Type=="Image")
                        {
                            string foldername = "_Chatting/"+conver;
                            Check_Folder_Is_Exist(foldername,Comon.Global.sys_ServerImageFolder);
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilename = realFilename.ToLower().Replace(ext.ToLower(),"")+"("+n+")"+ext.ToLower();
                            string _realFilenameFeauture = realFilename.ToLower().Replace(ext.ToLower(),"")+"("+n+")"+"(fea)"+ext.ToLower();
                            string link = String.Format(@"{0}{1}/{2}",Comon.Global.sys_ServerImageFolder,foldername,_realFilename);
                            string linkFeauture = String.Format(@"{0}{1}/{2}",Comon.Global.sys_ServerImageFolder,foldername,_realFilenameFeauture);
                            string resulf = await Update_Image(ext,link,HttpContext.Request.Form.Files[0],false);
                            string resulfFeauture = await Update_Image(ext,linkFeauture,HttpContext.Request.Form.Files[0],true);
                            if(resulf==""||resulfFeauture=="")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(Comon.Global.sys_HTTPImageRoot+foldername+"/"+_realFilename+"##"+Comon.Global.sys_HTTPImageRoot+foldername+"/"+_realFilenameFeauture);
                            }
                        }
                        if(Type=="File")
                        {
                            string foldername = "_Chatting/"+conver;
                            Check_Folder_Is_Exist(foldername,Comon.Global.sys_ServerImageFolder);
                            long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            string _realFilename = realFilename.ToLower().Replace(ext.ToLower(),"")+"("+n+")"+ext.ToLower();
                            string link = String.Format(@"{0}{1}/{2}",Comon.Global.sys_ServerImageFolder,foldername,_realFilename);
                            string resulf = await Update_Image(ext,link,HttpContext.Request.Form.Files[0],false);
                            if(resulf=="")
                            {
                                return Content("0");
                            }
                            else
                            {
                                return Content(Comon.Global.sys_HTTPImageRoot+foldername+"/"+_realFilename);
                            }
                        }
                        else return Content("0");
                    }
                    return Content("1");
                }
                else return Content("0");
            }
            catch(Exception)
            {
                return Content("0");
            }
        }

        private async Task<string> Update_Image(string ext,string link,Microsoft.AspNetCore.Http.IFormFile file,bool resize)
        {
            try
            {
                using(var stream = file.OpenReadStream())
                {

                    byte[] fileData = null;
                    // byte[] fileDataFeature = null;
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    fileData=ms.ToArray();
                    if(resize)
                    {
                        fileData=Comon.Comon.ResizeImage_Byte(fileData,70,70);
                    }
                    // fileDataFeature = Comon.Comon.ResizeImage_Byte(fileData, 100, 100);
                    FtpWebRequest req = (FtpWebRequest)WebRequest.Create(link);
                    req.UseBinary=true;
                    req.Method=WebRequestMethods.Ftp.UploadFile;
                    req.Credentials=new NetworkCredential(Comon.Global.sys_ServerImageUserName,Comon.Global.sys_ServerImagePassword);
                    req.ContentLength=fileData.Length;
                    Stream reqStream = req.GetRequestStream();
                    await reqStream.WriteAsync(fileData,0,fileData.Length);
                    reqStream.Close();
                }
                return "1";
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        private bool Checking_Exist_File_Name(string name)
        {
            try
            {
                var user = Session.GetUserSession(HttpContext);
                var request = (FtpWebRequest)WebRequest.Create(name);
                request.Credentials=new NetworkCredential(Comon.Global.sys_ServerImageUserName,Comon.Global.sys_ServerImagePassword);
                request.Method=WebRequestMethods.Ftp.GetFileSize;

                try
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                }
                catch(WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if(response.StatusCode==
                        FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch(Exception)
            {
                return false;
            }
        }
        private void Check_Folder_Is_Exist(string name,string folder)
        {
            try
            {
                List<string> currrentdirectories = Comon.Comon.GetAllDirectoryInFolder(folder+name);
                if(currrentdirectories==null)
                {
                    Comon.Comon.CreateDirectoryFTP(name);
                }
            }
            catch(Exception ex)
            {

            }
        }

    }

}
