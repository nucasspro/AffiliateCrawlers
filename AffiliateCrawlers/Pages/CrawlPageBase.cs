using AffiliateCrawlers.Models;
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

        public virtual async Task<List<ProductInfoModel>> Start(string url, int numberOfItems)
        {
            await Task.Run(()=> { });
            return null;
        }
        public virtual List<string> GetAllItems(int numberOfItems)
        {
            return new List<string>();
        }
    }
}