using AffiliateCrawlers.Models;
using HtmlAgilityPack;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;

namespace AffiliateCrawlers.Pages
{
    public abstract class CrawlPageBase
    {
        public string Host = "";
        public string FileName = "";
        public RemoteWebDriver Driver;

        public virtual List<ProductInfoModel> Start(string url, int numberOfItems)
        {
            return new List<ProductInfoModel>();
        }

        public virtual IEnumerable<string> GetAllProductLink(HtmlNode document, int quantity)
        {
            return null;
        }

        public virtual List<string> GetAllItems(int numberOfItems)
        {
            return new List<string>();
        }
    }
}