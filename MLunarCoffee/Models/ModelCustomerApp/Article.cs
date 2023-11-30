using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class Article
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public DateTime Created { get; set; }
        public string Image { get; set; }
        public int IsFeature { get; set; }
    }
    public class ArticleDetail
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime Created { get; set; }
        public string Image { get; set; }
    }
}
