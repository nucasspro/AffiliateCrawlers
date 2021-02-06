using AffiliateCrawlers.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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

        public override async Task<List<string>> Start(int numberOfItems)
        {
            try
            {
                var url = _listSourceLink[0];
                var web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                var document = doc.DocumentNode;
                var a = document.QuerySelectorAll(".content");

                //Driver.Url = _listSourceLink[0];
                //Driver.Navigate();
                //Utilities.ScrollDown(Driver);

                //var allItemLinks = GetAllItemLinks();

                //if (numberOfItems > allItemLinks.Count)
                //{
                //    numberOfItems = allItemLinks.Count;
                //}

                //var filterLink = allItemLinks.Take(numberOfItems);

                //var allItemInfo = await GetItemInfo(filterLink);

                return new List<string>();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Không thể mở trang");
                return new List<string>();
            }

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
            var itemInfoList = new List<ItemInfoModel>();

            foreach (var link in links)
            {
                // Open link in new tab
                Driver.ExecuteScript($"window.open('{link}', 'new_window')");
                Driver.SwitchTo().Window(Driver.WindowHandles[1]);

                itemInfoList.Add(new ItemInfoModel
                {
                    Title = GetProductTitle(),
                    Url = link,
                    Content = GetProductContent(),
                    OriginalPrice = GetProductSalePrice(),
                    SalePrice = GetProductSalePrice(),
                    ImageLinks = GetProductImages().ToList()
                });

                Driver.Close();
                Driver.SwitchTo().Window(Driver.WindowHandles[0]);

                await Task.Delay(1000);
            }
            return itemInfoList;
        }

        private string GetProductTitle()
        {
            const string titleXPath = "//*[@id=\"product-wrapper\"]/div/div/div/div/div[1]/div/div[2]/div/div[1]/h1";
            return Driver.FindElementByXPath(titleXPath).GetAttribute("innerHTML");
        }

        private string GetProductContent()
        {
            const string contentXPath = "//*[@id=\"product-wrapper\"]/div/div/div/div/div[1]/div/div[2]/div/div[3]";
            return Driver.FindElementByXPath(contentXPath).GetAttribute("innerHTML").Replace("\t", "").Replace(" style=\"font-size: 12pt;\"", "");
        }

        private string GetProductSalePrice()
        {
            const string salePriceXPath = "//*[@id=\"product-wrapper\"]/div/div/div/div/div[1]/div/div[2]/div/div[2]/span";
            return Driver.FindElementByXPath(salePriceXPath).GetAttribute("innerHTML").Replace("₫", "").Replace(",", "");
        }

        private IEnumerable<string> GetProductImages()
        {
            const string imageLinksXPath = "//*[@id=\"ProductThumbs\"]";
            var images = Driver.FindElementByXPath(imageLinksXPath).FindElements(By.ClassName("product-single__thumbnail"));
            foreach (var image in images)
            {
                yield return image.GetAttribute("href");
            }
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