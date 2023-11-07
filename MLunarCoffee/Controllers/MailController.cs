using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Comon.TokenJWT;
using MLunarCoffee.Models.Model.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MLunarCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MailController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        const double Maxbyteattach = 2098152;
        const int Maxblocksend = 50;
        public MailController(IConfiguration config, ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

        [Authorize]
        [Route("Send")]
        [HttpPost]
        public async Task<IActionResult> Send(IFormCollection formdata)
        {
            try
            {

                MailInfo info = JsonConvert.DeserializeObject<MailInfo>(formdata["objArr"].ToString());
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session, "Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
                {
                    List<MailBrand> mailBrand = user.sys_MailBrand;
                    List<MailBrand> _mail = mailBrand.FindAll(e => e.sys_MailName.ToLower() == info.FromEmail.ToLower());
                    List<IFormFile> upload = new List<IFormFile>();

                    using (var multiContent = new MultipartFormDataContent())
                    {
                        multiContent.Headers.ContentType.MediaType = "multipart/form-data";
                        if (_mail != null && _mail.Count > 0)
                        {

                            using var content = new MultipartFormDataContent
                                        {
                                            { new StringContent(info.ToEmail), "MailRequest.ToEmail" },
                                            { new StringContent(info.CCEmail), "MailRequest.CCEmail" },
                                            { new StringContent(info.BCCEmail), "MailRequest.BCCEmail" },
                                            { new StringContent(info.Subject), "MailRequest.Subject" },
                                            { new StringContent(info.Body), "MailRequest.Body" },
                                            { new StringContent(info.Link), "MailRequest.Link" },
                                            { new StringContent(_mail[0].sys_MailName), "MailRequest.Mail" },
                                            { new StringContent(_mail[0].sys_MailDisplayName), "MailRequest.DisplayName" },
                                            { new StringContent(_mail[0].sys_MailPassword), "MailRequest.Password" },
                                            { new StringContent(_mail[0].sys_MailHost), "MailRequest.Host" },
                                            { new StringContent(_mail[0].sys_MailPort), "MailRequest.Port" },
                                            { new StringContent("0"), "Is2nd" }
                                        };
                            if (formdata.Files != null && formdata.Files.Count != 0)
                            {
                                foreach (IFormFile source in formdata.Files)
                                {
                                    if (source.Length > 0)
                                    {
                                        if (source.Length > Maxbyteattach || formdata.Files.Count > 5) return Content("-1");
                                        string ext = (Path.GetExtension(source.FileName)).ToLower();
                                        if (ext != ".jpg" && ext != ".png" && ext != ".jpeg" && ext != ".html" && ext != ".htm"
                                            && ext != ".xlsx" && ext != ".xls" && ext != ".doc"
                                            && ext != ".docx" && ext != ".pdf")
                                        {
                                            return Content("-1");
                                        }

                                        using (var ms = new MemoryStream())
                                        {
                                            source.CopyTo(ms);
                                            var fileBytes = ms.ToArray();
                                            var file = CreateFileContent(new MemoryStream(fileBytes), source.FileName, "application/octet-stream");
                                            content.Add(file);
                                        }
                                    }

                                }
                            }
                            #region // Download link
                            //string linkdownload = info.LinkDownload;
                            //if(linkdownload!=null && linkdownload!="" && linkdownload.Contains("vttechsolution.com"))
                            //{
                            //    WebClient wc = new WebClient();
                            //    byte[] bytes = wc.DownloadData(linkdownload);
                            //    var file = CreateFileContent(new MemoryStream(bytes)
                            //        , info.NameDownload!=null ? info.NameDownload:"Vttech File"
                            //        , "application/octet-stream");
                            //    content.Add(file);
                            //}
                            #endregion
                            using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                            {
                                clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.sys_TokenAPI);
                                var result = await clientautho.PostAsync("/api/Email/SendMail", content);
                                string responseBody = await result.Content.ReadAsStringAsync();
                                return Content(responseBody);
                            }
                        }
                    }

                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        [Authorize]
        [Route("SendMulti")]
        [HttpPost]
        public async Task<IActionResult> SendMulti(IFormCollection formdata)
        {
            try
            {
                List<MailInfo> info = JsonConvert.DeserializeObject<List<MailInfo>>(formdata["objArr"].ToString());
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session, "Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token) && info != null && info.Count > 0 && info.Count <= Maxblocksend)
                {
                    List<MailBrand> mailBrand = user.sys_MailBrand;
                    List<MailBrand> _mail = mailBrand.FindAll(e => e.sys_MailName.ToLower() == info[0].FromEmail.ToLower());
                    List<IFormFile> upload = new List<IFormFile>();

                    if (_mail == null || _mail.Count < 0) return Content("0"); // KIỂM TRA MAIL BRANCH

                    foreach (var item in info)
                    {
                        using (var multiContent = new MultipartFormDataContent())
                        {
                            multiContent.Headers.ContentType.MediaType = "multipart/form-data";
                            using var content = new MultipartFormDataContent
                                        {
                                            { new StringContent(item.ToEmail), "MailRequest.ToEmail" },
                                            { new StringContent(item.CCEmail), "MailRequest.CCEmail" },
                                            { new StringContent(item.BCCEmail), "MailRequest.BCCEmail" },
                                            { new StringContent(item.Subject), "MailRequest.Subject" },
                                            { new StringContent(item.Body), "MailRequest.Body" },
                                            { new StringContent(item.Link), "MailRequest.Link" },
                                            { new StringContent(_mail[0].sys_MailName), "MailRequest.Mail" },
                                            { new StringContent(_mail[0].sys_MailDisplayName), "MailRequest.DisplayName" },
                                            { new StringContent(_mail[0].sys_MailPassword), "MailRequest.Password" },
                                            { new StringContent(_mail[0].sys_MailHost), "MailRequest.Host" },
                                            { new StringContent(_mail[0].sys_MailPort), "MailRequest.Port" },
                                            { new StringContent("0"), "Is2nd" }
                                        };
                            if (formdata.Files != null && formdata.Files.Count != 0)
                            {
                                foreach (IFormFile source in formdata.Files)
                                {
                                    if (source.Length > 0)
                                    {
                                        if (source.Length > Maxbyteattach || formdata.Files.Count > 5) return Content("-1");
                                        string ext = (Path.GetExtension(source.FileName)).ToLower();
                                        if (ext != ".jpg" && ext != ".png" && ext != ".jpeg" && ext != ".html" && ext != ".htm"
                                            && ext != ".xlsx" && ext != ".xls" && ext != ".doc"
                                            && ext != ".docx" && ext != ".pdf")
                                        {
                                            return Content("-1");
                                        }

                                        using (var ms = new MemoryStream())
                                        {
                                            source.CopyTo(ms);
                                            var fileBytes = ms.ToArray();
                                            var file = CreateFileContent(new MemoryStream(fileBytes), source.FileName, "application/octet-stream");
                                            content.Add(file);
                                        }
                                    }

                                }
                            }

                            using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                            {
                                clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.sys_TokenAPI);
                                await clientautho.PostAsync("/api/Email/SendMail", content).ConfigureAwait(false);
                            }
                        }
                    }
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("SendMailTemplate")]
        [HttpPost]
        public async Task<IActionResult> SendMailTemplate(MailInfo info)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session, "Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(), token))
                {

                    List<MailBrand> mailBrand = user.sys_MailBrand;
                    List<MailBrand> _mail = mailBrand.FindAll(e => e.sys_MailName.ToLower() == info.FromEmail.ToLower());
                    List<IFormFile> upload = new List<IFormFile>();
                    if (_mail != null && _mail.Count > 0)
                    {
                        MailTemplate mailRequest = new MailTemplate()
                        {
                            ToEmail = info.ToEmail,
                            CCEmail = info.CCEmail,
                            BCCEmail = info.BCCEmail,
                            Subject = info.Subject,
                            Body = info.Body,
                            Link = info.Link,
                            Mail = _mail[0].sys_MailName,
                            DisplayName = _mail[0].sys_MailDisplayName,
                            Password = _mail[0].sys_MailPassword,
                            Host = _mail[0].sys_MailHost,
                            Port = Convert.ToInt32(_mail[0].sys_MailPort),
                            TemplateValues = info.TemplateValues
                        };
                        using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                        {
                            clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.sys_TokenAPI);
                            var content = new StringContent(JsonConvert.SerializeObject(mailRequest), Encoding.UTF8, "application/json");
                            var result = await clientautho.PostAsync("/api/Email/SendMailTemplate", content);
                            string responseBody = await result.Content.ReadAsStringAsync();
                            return Content(responseBody);
                        }
                    }
                    else return Content("0");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        [Authorize]
        [Route("CheckValid")]
        [HttpPost]
        public async Task<IActionResult> CheckValid(MailInfo info)
        {
            try
            {
                GlobalUser user = Session.GetUserSession(HttpContext);
                var token = Session.GetSession(HttpContext.Session, "Token");
                if (token == null)
                {
                    return Content("0");
                }
                if (_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(), token))
                {

                    List<MailBrand> mailBrand = user.sys_MailBrand;
                    List<MailBrand> _mail = mailBrand.FindAll(e => e.sys_MailName.ToLower() == info.FromEmail.ToLower());
                    if (_mail != null && _mail.Count > 0)
                    {
                        MailTemplate mailRequest = new MailTemplate()
                        {
                            ToEmail = "",
                            CCEmail = "",
                            BCCEmail = "",
                            Subject = "",
                            Body = "",
                            Link = "",
                            Mail = _mail[0].sys_MailName,
                            DisplayName = _mail[0].sys_MailDisplayName,
                            Password = _mail[0].sys_MailPassword,
                            Host = _mail[0].sys_MailHost,
                            Port = Convert.ToInt32(_mail[0].sys_MailPort),
                            TemplateValues = new Dictionary<string, string>()

                        };
                        using (HttpClient clientautho = new HttpClient() { BaseAddress = new Uri(Global.sys_APIClient.Url) })
                        {
                            clientautho.DefaultRequestHeaders.Add("Authorization", "Bearer " + user.sys_TokenAPI);
                            var content = new StringContent(JsonConvert.SerializeObject(mailRequest), Encoding.UTF8, "application/json");
                            var result = await clientautho.PostAsync("/api/Email/CheckValid", content);
                            string responseBody = await result.Content.ReadAsStringAsync();
                            return Content(responseBody);
                        }
                    }
                    else return Content("0");
                }
                return Content("0");
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }


        private StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"Attachments\"",
                FileName = "\"" + fileName + "\""
            }; // the extra quotes are key here
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }
    }
}
