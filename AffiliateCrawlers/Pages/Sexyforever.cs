using AffiliateCrawlers.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AffiliateCrawlers.Pages
{
    public class Sexyforever : CrawlPageBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driver"></param>
        public Sexyforever(RemoteWebDriver driver = null)
        {
            Host = "https://sexyforever.vn";
            FileName = "sexyforever_";
            Driver = driver;
        }

        /// <summary>
        /// Start crawler
        /// </summary>
        /// <param name="url"></param>
        /// <param name="numberOfItems"></param>
        /// <returns></returns>
        public override List<ProductInfoModel> Start(string url, int numberOfItems)
        {
            try
            {
                var web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                HtmlNode document = doc.DocumentNode;
                var allProductLink = GetAllProductLink(document, numberOfItems);

                var products = GetAllProductInfo(document, allProductLink).ToList();

                return products;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get all product link
        /// </summary>
        /// <param name="document"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override IEnumerable<string> GetAllProductLink(HtmlNode document, int quantity)
        {
            var links = document.QuerySelectorAll(".product-img > a").ToList();
            if (quantity > links.Count)
            {
                quantity = links.Count;
            }

            for (int i = 0; i < quantity; i++)
            {
                yield return $"{ Host }{ links[i].GetAttributeValue("href", string.Empty) }";
            }
        }

        /// <summary>
        /// Get all product info
        /// </summary>
        /// <param name="document"></param>
        /// <param name="urls"></param>
        /// <returns></returns>
        private IEnumerable<ProductInfoModel> GetAllProductInfo(HtmlNode document, IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                yield return GetProductInfo(document, url);
            }
        }

        /// <summary>
        /// Get product info
        /// </summary>
        /// <param name="document"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private ProductInfoModel GetProductInfo(HtmlNode document, string url)
        {
            var web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            document = doc.DocumentNode;

            ProductInfoModel res = new();
            res.Name = GetProductName(document);
            res.Url = url;
            res.Content = GetProductContent(document);
            res.OriginalPrice = GetProductOriginalPrice(document);
            res.SalePrice = GetProductSalePrice(document);
            res.ImageLinks = GetProductImages(document).ToList();

            return res;
        }

        /// <summary>
        /// Get product original price
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private string GetProductOriginalPrice(HtmlNode document)
        {
            return document
                .QuerySelector(".product-content > .pro-price > .current-price")
                .InnerText
                .Replace("₫", "").Replace(",", "");
        }

        /// <summary>
        /// Get product sale price
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private string GetProductSalePrice(HtmlNode document)
        {
            return document
                .QuerySelector(".product-content > .pro-price > .current-price")
                .InnerText
                .Replace("₫", "")
                .Replace(",", "");
        }

        /// <summary>
        /// Get product content
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private string GetProductContent(HtmlNode document)
        {
            return document.QuerySelector(".product-content > .pro-short-desc")
                .InnerHtml
                .Replace("\t", "")
                .Replace(" style=\"font-size: 12pt;\"", "");
        }

        /// <summary>
        /// Get product content
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private string GetProductName(HtmlNode document)
        {
            return document.QuerySelector(".product-content > .pro-content-head > h1").InnerText;
        }

        /// <summary>
        /// Get product image links
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private IEnumerable<string> GetProductImages(HtmlNode document)
        {
            return document
                .QuerySelectorAll("#ProductThumbs > div.inner > li > a")
                .ToList()
                .Select(item => $"https:{ item.GetAttributeValue("href", string.Empty) }");
        }

        private void HasNextPage()
        {
        }
    }
}