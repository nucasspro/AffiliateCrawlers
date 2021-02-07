using AffiliateCrawlers.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AffiliateCrawlers.Pages
{
    public class Sablanca : CrawlPageBase
    {
        public Sablanca(RemoteWebDriver driver)
        {
            Host = "https://sablanca.vn/";
            FileExtension = "txt";
            FileName = "sablanca_";
            Driver = driver;
        }

        public override async Task<List<ProductInfoModel>> Start(string url, int numberOfItems)
        {
            Driver.Url = url;
            Driver.Navigate();
            Utilities.ScrollDown(Driver);

             GetAllItems(1);
            return null;
        }

        public override List<string> GetAllItems(int numberOfItems)
        {
            var itemsXPath = "//*[@id=\"5051HB011317857\"]";
            var itemData = Driver.FindElementByXPath(itemsXPath);

            var data = GetAnItem();
            return data;
        }

        private List<string> GetAnItem()
        {
            return new List<string>();
        }

      
    }
}