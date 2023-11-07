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

namespace MLunarCoffee.Pages.Guide
{
    public class DocumentationModel : PageModel
    {
        public static string _dataDocument { get; set; }
        public void OnGet()
        {
        }
         public IActionResult OnPostLoadDataDocument()
        {
            _dataDocument = JsonConvert.SerializeObject(Comon.Global.sys_Document);
            if (_dataDocument == "" || _dataDocument == "[]")
            {
                Response.Redirect("~/index.html" + "?ver=" + DateTime.Now.Second);
                return Content("");
            }
            else
            return Content(_dataDocument);
        }

        
         public IActionResult OnPostGetDocument(string DocumentID)
        {

            try
            {
                DataRow[] rows = Comon.Global.sys_Document.Select("DocID=" + DocumentID);
                if (rows != null && rows.Count() == 1)
                {
                    string textHeader = rows[0]["DocName"].ToString();
                    string textBody = "";
                    string link = rows[0]["DocUrlHTTP"].ToString();
                    if (link != "#")
                    {
                        string content = Comon.Comon.Readfile(link);
                        if (content == "") return Content("");
                        else textBody = Comon.Comon.Readfile(link);
                    }
                    else
                    {
                        textBody = "";
                    }
                    var obj = new string[] { textHeader, textBody };
                   return Content(JsonConvert.SerializeObject(obj));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }

        
         public  IActionResult OnPostSaveDocument(string DocumentID, string content, string PlaitText)
        {

            try
            {
                DataRow[] rows = Comon.Global.sys_Document.Select("DocID=" + DocumentID);
                if (rows != null && rows.Count() == 1)
                {
                    string link = rows[0]["DocUrl"].ToString();

                    string[] sp_dot = link.Split('.');
                    string linkPlainText = sp_dot[0] + "___plaintText" + '.' + sp_dot[1];
                    string linkPlainTextNoSign = sp_dot[0] + "___plaintTextNoSign" + '.' + sp_dot[1];

                    if (link != "#")
                    {
                        int resulfWriteFile = Comon.Comon.Writefile(link, content, linkPlainText, PlaitText, linkPlainTextNoSign, Comon.Comon.ConvertToUnsign(PlaitText));
                        if (resulfWriteFile == 0) return Content("");
                    }
                    return Content("1");
                }
                else
                {
                    return Content("");
                }


            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
        
         public IActionResult OnPostSearching(string searching)
        {

            try
            {
                if (Comon.Comon.ConvertToUnsign(searching).Length < 4) return Content("0");
                DataTable dt = Comon.Comon.SeachingDocument(searching.Trim());
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow[] rows = Comon.Global.sys_Document.Select("DocUrl='" + dt.Rows[i]["FileName"].ToString() + "'");
                        dt.Rows[i]["ID"] = rows[0]["DocID"].ToString();
                        dt.Rows[i]["Name"] = rows[0]["DocName"].ToString();
                    }
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else return Content("");
            }
            catch (Exception ex)
            {
                return Content("");
            }

        }
    }
}
