using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace MLunarCoffee
{
    internal class ResourceGroup
    {
        public string Name { get; set; }
        public IEnumerable<LocalizedString> Entries { get; set; }
    }

    internal static class ResourceGroupExtensions
    {
        /// <summary>
        /// Converts the source data to a Javascript variable
        /// </summary>
        /// <param name="fields">The record to convert</param>
        /// <returns>A valid Javascript object</returns>
        internal static string ToJavascript(this IEnumerable<ResourceGroup> fields)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("var Resources = ");

            ExpandoObject uiCaptions = new ExpandoObject();

            // Get the fields
            foreach (ResourceGroup fieldGroup in fields)
                ((IDictionary<string, object>)uiCaptions)[fieldGroup.Name] =
                    fieldGroup.Entries.ToDictionary(x => x.Name.ToString(), x => x.Value);

            string serialized = JsonSerializer.Serialize(uiCaptions);
            sb.Append(serialized);
            sb.Append(";");

            return sb.ToString();
        }
    }

}