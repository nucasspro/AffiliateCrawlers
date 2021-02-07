using AffiliateCrawlers.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AffiliateCrawlers.Pages
{
    public class Sexyforever : CrawlPageBase
    {
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

        private List<HtmlNode> GetProductLinkOfPage(HtmlNode document) => document.QuerySelectorAll(".product-img > a").ToList();

        private string GetProductLink(HtmlNode document) => $"{ Host }{ document.GetAttributeValue("href", string.Empty) }";

        private IEnumerable<ProductInfoModel> GetAllProductInfo(HtmlNode document, IEnumerable<string> urls) => urls.Select(url => GetProductInfo(document, url));

        private ProductInfoModel GetProductInfo(HtmlNode document, string url)
        {
            var web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            document = doc.DocumentNode;

            string title = document.QuerySelector(".product-content > .pro-content-head > h1")
                .InnerText;

            string content = document.QuerySelector(".product-content > .pro-short-desc")
                .InnerHtml
                .Replace("\t", "")
                .Replace(" style=\"font-size: 12pt;\"", "");

            string originalPrice = document.QuerySelector(".product-content > .pro-price > .current-price")
                .InnerText
                .Replace("₫", "")
                .Replace(",", "");

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

        private void HasNextPage()
        {
        }
    }
}