using AffiliateCrawlers.Models;
using System.Collections.Generic;

namespace AffiliateCrawlers.Pages
{
    public abstract class CrawlPageBase
    {
        public string Host = "";
        public string FileName = "";

        public virtual List<ProductInfoModel> Start(string url, int quantity) => new List<ProductInfoModel>();
    }
}