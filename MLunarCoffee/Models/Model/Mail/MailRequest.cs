using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MLunarCoffee.Models.Model.Mail
{
    public class MailInfo
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string CCEmail { get; set; }
        public string BCCEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        //public string NameDownload { get; set; }
        //public string LinkDownload { get; set; }
        public string Link { get; set; }
        public Dictionary<string, string> TemplateValues { get; set; }
    }
    
    public class MailTemplate
    {

        public string ToEmail { get; set; }
        public string CCEmail { get; set; }
        public string BCCEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public Dictionary<string, string> TemplateValues { get; set; }
    }
    //public class MailRequest
    //{
    //    public string ToEmail { get; set; }
    //    public string CCEmail { get; set; }
    //    public string BCCEmail { get; set; }
    //    public string Subject { get; set; }
    //    public string Body { get; set; }
    //    public string Link { get; set; }
    //    public string Mail { get; set; }
    //    public string DisplayName { get; set; }
    //    public string Password { get; set; }
    //    public string Host { get; set; }
    //    public int Port { get; set; }
    //    public int Is2nd { get; set; }
    //    public Dictionary<string, string> TemplateValues { get; set; }
    //}
}
