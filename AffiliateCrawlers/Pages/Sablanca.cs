using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AffiliateCrawlers.Pages
{
    public class Sablanca : CrawlPageBase
    {
        private readonly List<string> _listSourceLink = new List<string>
        {
            "https://sablanca.vn/pages/tui-xach-moi-nhat",
            "https://sablanca.vn/pages/giay-dep-moi-nhat",
            "https://sablanca.vn/pages/giay-cao-got",
            "https://sablanca.vn/tui-xach/danh-muc/backpack",
            "https://sablanca.vn/vi-cam-tay",
        };
        
        public string sourceLink = "";

        public Sablanca(RemoteWebDriver driver)
        {
            Host = "https://sablanca.vn/";
            FileExtension = "txt";
            FileName = "sablanca_";
            Driver = driver;
        }

        public override List<string> Start(int numberOfItems)
        {
            Driver.Url = sourceLink;
            Driver.Navigate();
            Utilities.ScrollDown(Driver);

            return GetAllItems(1);
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