using OpenQA.Selenium.Remote;
using System.Collections.Generic;

namespace AffiliateCrawlers.Pages
{
    public abstract class CrawlPageBase : ICrawlPage
    {
        public string Host = "";
        public string FileExtension = "txt";
        public string FileName = "";
        public RemoteWebDriver Driver;

        public virtual List<string> Start(int numberOfItems)
        {
            return new List<string>();
        }
        public virtual List<string> GetAllItems(int numberOfItems)
        {
            return new List<string>();
        }
    }
}