using System.Collections.Generic;

namespace AffiliateCrawlers.Models
{
    public class ProductInfoModel
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
        public string OriginalPrice { get; set; } = "";
        public string SalePrice { get; set; } = "";
        public string Content { get; set; } = "";
        public List<string> ImageLinks { get; set; } = new List<string>();
    }

    public class ProductInfoViewModel
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
        public string OriginalPrice { get; set; } = "";
        public string SalePrice { get; set; } = "";
        public string Content { get; set; } = "";
        public string ImageLinks { get; set; } = "";
    }
}