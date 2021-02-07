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
using System;

namespace AffiliateCrawlers.Pages
{
    public class Sexyforever : CrawlPageBase
    {
        private readonly List<string> _listSourceLink = new List<string>
        {
        };

        public string sourceLink = "";

        public Sexyforever(RemoteWebDriver driver = null)
        {
            Host = "https://sexyforever.vn";
            FileExtension = "txt";
            FileName = "sexyforever_";
            Driver = driver;
        }

        public override async Task<List<ProductInfoModel>> Start(string url, int numberOfItems)
        {
            try
            {
                var web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                HtmlNode document = doc.DocumentNode;
                var allProductLink = GetAllProductLink(document, numberOfItems);

                return GetAllProductInfo(document, allProductLink).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở trang");
                return null;
            }
        }

        private IEnumerable<string> GetAllProductLink(HtmlNode document, int quantity)
        {
            var links = GetProductLinkOfPage(document);
            if (quantity > links.Count)
            {
                quantity = links.Count;
            }

            for (int i = 0; i < quantity; i++)
            {
                yield return GetProductLink(links[i]);
            }
        }

        private void HasNextPage()
        {
        }

        private List<HtmlNode> GetProductLinkOfPage(HtmlNode document) => document.QuerySelectorAll(".product-img > a").ToList();

        private string GetProductLink(HtmlNode document) => $"{ Host }{ document.GetAttributeValue("href", string.Empty) }";


        private IEnumerable<ProductInfoModel> GetAllProductInfo(HtmlNode document, IEnumerable<string> urls)
        {
            return urls.Select(url => GetProductInfo(document, url));
        }

        private ProductInfoModel GetProductInfo(HtmlNode document, string url)
        {
            var web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            document = doc.DocumentNode;

            string title = document.QuerySelector(".product-content > .pro-content-head > h1").InnerText;
            string content = document.QuerySelector(".product-content > .pro-short-desc").InnerHtml.Replace("\t", "").Replace(" style=\"font-size: 12pt;\"", ""); ;
            string originalPrice = document.QuerySelector(".product-content > .pro-price > .current-price").InnerText.Replace("₫", "").Replace(",", ""); ;
            string salePrice = originalPrice;

            List<string> imageLinks = new();
            foreach (var item in document.QuerySelectorAll("#ProductThumbs > div.inner > li > a").ToList())
            {
                imageLinks.Add($"https:{ item.GetAttributeValue("href", string.Empty) }");
            }

            return new ProductInfoModel()
            {
                Title = title,
                Url = url,
                Content = content,
                OriginalPrice = originalPrice,
                SalePrice = salePrice,
                ImageLinks = imageLinks
            };
        }



        public override List<string> GetAllItems(int numberOfItems)
        {
            return base.GetAllItems(numberOfItems);
        }

        private async Task<IEnumerable<ProductInfoModel>> GetItemInfo(IEnumerable<string> links)
        {
            var itemInfoList = new List<ProductInfoModel>();

            foreach (var link in links)
            {
                // Open link in new tab
                Driver.ExecuteScript($"window.open('{link}', 'new_window')");
                Driver.SwitchTo().Window(Driver.WindowHandles[1]);

                itemInfoList.Add(new ProductInfoModel
                {
                    Title = ""/*GetProductTitle()*/,
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


        //private string GetProductTitle()
        //{
        //    const string titleXPath = "//*[@id=\"product-wrapper\"]/div/div/div/div/div[1]/div/div[2]/div/div[1]/h1";
        //    return Driver.FindElementByXPath(titleXPath).GetAttribute("innerHTML");
        //}

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