using AffiliateCrawlers.Models;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;

namespace AffiliateCrawlers.Pages
{
    public abstract class CrawlPageBase
    {
        public string Host = "";
        public string FileName = "";
        public RemoteWebDriver Driver;

        public virtual List<ProductInfoModel> Start(string url, int quantity) => new List<ProductInfoModel>();
    }
}