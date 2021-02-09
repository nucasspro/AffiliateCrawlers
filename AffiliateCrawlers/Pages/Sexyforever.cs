using AffiliateCrawlers.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
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
        public override List<ProductInfoModel> Start(string url, int quantity)
        {
            try
            {
                var web = new HtmlWeb();
                var productUrls = GetAllProductLink2(web, url, quantity);

                return GetAllProductInfo(web, productUrls).ToList();
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
        public override List<string> GetAllProductLink2(HtmlWeb web, string url, int quantity)
        {
            List<string> links = new();
            string nextPageLink = string.Empty;

            HtmlDocument doc;
            HtmlNode document;

            do
            {
                doc = web.Load(url);
                document = doc.DocumentNode;
                foreach (var item in document.QuerySelectorAll(".product-img > a").ToList())
                {
                    links.Add($"{ Host }{ item.GetAttributeValue("href", string.Empty) }");
                }

                if (!HasNextPage(document, ref url))
                {
                    break;
                }

            } while (links.Count < quantity);

            return links;
        }

        /// <summary>
        /// Get all product info
        /// </summary>
        /// <param name="document"></param>
        /// <param name="urls"></param>
        /// <returns></returns>
        private IEnumerable<ProductInfoModel> GetAllProductInfo(HtmlWeb web, IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                yield return GetProductInfo(web, url);
            }
        }

        /// <summary>
        /// Get product info
        /// </summary>
        /// <param name="document"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private ProductInfoModel GetProductInfo(HtmlWeb web, string url)
        {
            HtmlDocument doc = web.Load(url);
            HtmlNode document = doc.DocumentNode;

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
            var content = document.QuerySelector(".product-content > .pro-short-desc")
                .InnerHtml
                .Replace("\t", "");

            Regex regex = new("(<span).*?(\">)");
            content = regex.Replace(content, "<span>");

            return HttpUtility.HtmlDecode(content);
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

        private bool HasNextPage(HtmlNode document, ref string nextPageLink)
        {
            var nextButton = document.QuerySelector(".nextPage > a");
            if (nextButton is not null)
            {
                nextPageLink = nextButton.GetAttributeValue("href", string.Empty);
                if (nextPageLink.Length != 0)
                {
                    nextPageLink = $"{ Host }{ nextPageLink }";
                    return true;
                }
            }
            return false;
        }
    }
}