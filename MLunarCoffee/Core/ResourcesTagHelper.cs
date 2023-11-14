using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Reflection;

namespace MLunarCoffee
{
    [HtmlTargetElement("resources")]
    public class ResourcesTagHelper : TagHelper
    {
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public ResourcesTagHelper(IStringLocalizerFactory stringLocalizerFactory)
        {
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        [HtmlAttributeName("names")]
        public string[] Resources { get; set; }

        /// <summary>
        /// Execute script only once document is loaded.
        /// </summary>
        public bool OnContentLoaded { get; set; } = false;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (OnContentLoaded)
                await base.ProcessAsync(context, output);
            else
            {                
                IEnumerable<ResourceGroup> groupedResources = Resources.Select(x =>
                {
                    IStringLocalizer localizer = _stringLocalizerFactory.Create(x, Assembly.GetEntryAssembly().FullName);
                    return new ResourceGroup { Name = x, Entries = localizer.GetAllStrings(true).ToList() };
                });

                StringBuilder sb = new StringBuilder();
                sb.Append(groupedResources.ToJavascript());

                TagHelperContent content = await output.GetChildContentAsync();
                sb.Append(content.GetContent());

                output.TagName = "script";
                output.Content.AppendHtml(sb.ToString());
            }
        }
    }
}