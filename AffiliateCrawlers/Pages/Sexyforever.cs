using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace AffiliateCrawlers.Pages
{
    public class Sexyforever : CrawlPageBase
    {
        private readonly List<string> _listSourceLink = new List<string>
        {
            "https://sexyforever.vn/collections/sleepwear",
            "https://sexyforever.vn/collections/ao-khoac-ngu",
            "https://sexyforever.vn/collections/lingeries-set",
            "https://sexyforever.vn/collections/body-suit"
        };

        public string sourceLink = "";

        public Sexyforever(RemoteWebDriver driver)
        {
            Host = "https://sexyforever.vn/";
            FileExtension = "txt";
            FileName = "sexyforever_";
            Driver = driver;
        }

        public override List<string> Start(int numberOfItems)
        {
            Driver.Url = _listSourceLink[0];
            Driver.Navigate();
            Utilities.ScrollDown(Driver);

            var allItemLinks = GetAllItemLinks();

            if (numberOfItems > allItemLinks.Count)
            {
                numberOfItems = allItemLinks.Count;
            }

            var filterLink = allItemLinks.Take(numberOfItems);
            return new List<string>();
        }

        private List<string> GetAllItemLinks()
        {
            string itemsXPath = "//*[@id=\"collection-wrapper\"]/div/div/div/div[1]/div/div[2]/div[1]";
            var allItems = Driver.FindElementsByXPath(itemsXPath)[0].FindElements(By.CssSelector("div.product-img > a"));

            return (ExtractLink(allItems)).ToList();
        }

        public override List<string> GetAllItems(int numberOfItems)
        {   
            return base.GetAllItems(numberOfItems);
        }

        private List<string> GetAnItem()
        {
            return new List<string>();
        }

        private IEnumerable<string> ExtractLink(ReadOnlyCollection<IWebElement> allItems)
        {
            foreach (var item in allItems)
            {
                yield return item.GetAttribute("href");
            }
        }
    }
}
