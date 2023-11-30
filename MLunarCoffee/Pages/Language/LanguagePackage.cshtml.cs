using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;

namespace MLunarCoffee.Pages.Language
{

    public class LanguagePackageModel : PageModel
    {
        static IDictionary<string, string> fileName = new Dictionary<string, string>() {
            { "en-dyn", "wwwroot/Language/ENG/dynamic.xml" },
            { "en-sta", "wwwroot/Language/ENG/static.xml" },
            { "vn-sta", "wwwroot/Language/VN/static.xml" }
        };
        public void OnGet()
        {

        }
        public IActionResult OnPostLoadata(string type)
        {
            try
            {
                int total = (type != null &&type != "") ? 1: fileName.Count;
                PackageData[] packageData = new PackageData[total];
                int i = 0;
                foreach (KeyValuePair<string, string> kvp in fileName)
                {
                    if (type == null || type == "" || kvp.Key == type)
                    {
                        XDocument doc = XDocument.Load(kvp.Value);
                        var elements = doc.Descendants().Where(x => x.Name == "root");
                        packageData[i] = new PackageData()
                        {
                            type = kvp.Key,
                            node = elements,
                        };
                        i++;
                    }
                }
                return Content(JsonConvert.SerializeObject(packageData));

            }
            catch (Exception ex)
            {
                return Content("[]");
            }
        }
        public IActionResult OnPostExcuteData(string data, string key, string name
            , string note, string currentkey, string currenttype)
        {
            try
            {
                XElement newElement;
                XDocument doc = XDocument.Load(fileName[currenttype]);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(data);
                if (currenttype == "en-dyn")
                {
                    newElement = new XElement(key
                   , new XAttribute("name", name != null ? name : "")
                   , new XAttribute("note", note != null ? note : ""));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        XElement child = new XElement("value", new XAttribute("vn", dt.Rows[i]["@vn"].ToString()));
                        child.Value = dt.Rows[i]["#text"].ToString();
                        newElement.Add(child);
                    }
                }
                else
                {
                    newElement = new XElement(key
                     , new XAttribute("note", note != null ? note : ""));
                    newElement.Value = name;
                }

                if (currentkey == key)
                {
                    XElement alt = doc.Descendants(key).Where(x => x.Name == key).FirstOrDefault();
                    alt.ReplaceWith(newElement);
                }
                else
                {

                    XElement alt = doc.Descendants().Where(x => x.Name == "root").FirstOrDefault();
                    alt.Add(newElement);
                }
                doc.Save(fileName[currenttype]);
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
        public IActionResult OnPostDelete(string key, string currenttype)
        {

            try
            {
                XDocument doc = XDocument.Load(fileName[currenttype]);
                XElement alt = doc.Descendants(key).Where(x => x.Name == key).FirstOrDefault();
                alt.ReplaceWith(null);
                doc.Save(fileName[currenttype]);
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }

        }


    }

    public class PackageData
    {
        public string type { get; set; }
        public IEnumerable<XElement> node { get; set; }

    }

}
