using AffiliateCrawlers.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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

            var allItemInfo = GetItemInfo(filterLink);

            return new List<string>();
        }

        private List<string> GetAllItemLinks()
        {
            const string itemsXPath = "//*[@id=\"collection-wrapper\"]/div/div/div/div[1]/div/div[2]/div[1]";
            var allItems = Driver.FindElementsByXPath(itemsXPath)[0].FindElements(By.CssSelector("div.product-img > a"));

            return (ExtractLink(allItems)).ToList();
        }

        public override List<string> GetAllItems(int numberOfItems)
        {
            return base.GetAllItems(numberOfItems);
        }

        private async Task<IEnumerable<ItemInfoModel>> GetItemInfo(IEnumerable<string> links)
        {
            const string titleXPath = "//*[@id=\"product-wrapper\"]/div/div/div/div/div[1]/div/div[2]/div/div[1]/h1";
            const string contentXPath = "//*[@id=\"product-wrapper\"]/div/div/div/div/div[1]/div/div[2]/div/div[3]";
            const string originalPriceXPath = "";
            const string salePriceXPath = "//*[@id=\"product-wrapper\"]/div/div/div/div/div[1]/div/div[2]/div/div[2]/span";
            const string imageLinksXPath = "//*[@id=\"ProductThumbs\"]";

            var itemInfoList = new List<ItemInfoModel>();

            foreach (var link in links)
            {
                // Open link in new tab
                Driver.ExecuteScript($"window.open('{link}', 'new_window')");
                Driver.SwitchTo().Window(Driver.WindowHandles[1]);

                var title = "";
                var url = link;
                var content = "";
                var orginalPrice = "";
                var salePrice = "";
                var imageLinks = new List<string>();

                itemInfoList.Add(new ItemInfoModel
                {
                    Title = title,
                    Url = url,
                    Content = content,
                    OriginalPrice = orginalPrice,
                    SalePrice = salePrice,
                    ImageLinks = imageLinks
                });

                //File.AppendAllText(galleriesPath, GalleryToString(gallery));
                Driver.Close();
                Driver.SwitchTo().Window(Driver.WindowHandles[0]);

                await Task.Delay(3000);
            }
            return itemInfoList;
        }

        private IEnumerable<string> ExtractLink(ReadOnlyCollection<IWebElement> allItems)
        {
            foreach (var item in allItems)
            {
                yield return item.GetAttribute("href");
            }
        }

        private void ExportToFile()
        {
            //if (!File.Exists(galleriesPath))
            //{
            //    using var sw = File.CreateText(galleriesPath);
            //    galleries.ForEach(x => sw.WriteLine(GalleryToString(x)));
            //}

            //using (var sw = File.AppendText(galleriesPath))
            //{
            //    galleries.ForEach(x => sw.WriteLine(GalleryToString(x)));
            //}
        }
    }
}