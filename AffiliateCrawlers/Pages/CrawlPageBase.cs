using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AffiliateCrawlers.Pages
{
    public abstract class CrawlPageBase
    {
        public string Host = "";
        public string FileExtension = "txt";
        public string FileName = "";
        public RemoteWebDriver Driver;

        public virtual async Task<List<string>> Start(int numberOfItems)
        {
            await Task.Run(()=> { });
            return new List<string>();
        }
        public virtual List<string> GetAllItems(int numberOfItems)
        {
            return new List<string>();
        }
    }
}