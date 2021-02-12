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
    public class Sevenam : CrawlPageBase
    {
        public Sevenam(RemoteWebDriver driver = null)
        {
            Host = "https://sevenam.vn";
            FileName = "sevenam_";
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
                var productUrls = GetAllProductLink(web, url, quantity);

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
        public List<string> GetAllProductLink(HtmlWeb web, string url, int quantity)
        {
            HtmlDocument doc;
            HtmlNode document;
            List<string> links = new();

            do
            {
                doc = web.Load(url);
                document = doc.DocumentNode;
                var urls = document.QuerySelectorAll(".product-img-pos > a").ToList();
                if (urls.Count == 0)
                {
                    break;
                }

                foreach (var item in urls)
                {
                    links.Add($"{ Host }{ item.GetAttributeValue("href", string.Empty) }");
                    if (links.Count >= quantity)
                    {
                        return links;
                    }
                }

                if (!HasNextPage(document, ref url))
                {
                    return links;
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
            (res.OriginalPrice, res.SalePrice) = GetProductPrice(document);
            res.ImageLinks = GetProductImages(document).ToList();

            return res;
        }

        /// <summary>
        /// Get product content
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private string GetProductName(HtmlNode document)
        {
            return document.QuerySelector(".product-page-ticky > h1").InnerText;
        }

        private (string, string) GetProductPrice(HtmlNode document)
        {
            var salePrice = GetProductSalePrice(document);
            var originalPrice = GetProductOriginalPrice(document);
            if (originalPrice is null)
            {
                originalPrice = salePrice;
            }

            return (salePrice, originalPrice);
        }

        /// <summary>
        /// Get product original price
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private string GetProductOriginalPrice(HtmlNode document)
        {
            return document
                .QuerySelector("#ComparePrice > s")?
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
                .QuerySelector("#ProductPrice")?
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
            var content = document.QuerySelector(".panel1")
                .InnerHtml
                .Replace("\t", "");

            Regex regex = new("(<span).*?(\">)");
            content = regex.Replace(content, "<span>");

            return HttpUtility.HtmlDecode(content);
        }

        /// <summary>
        /// Get product image links
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private IEnumerable<string> GetProductImages(HtmlNode document)
        {
            return document
                .QuerySelectorAll("#sliderproduct1 > .grid__item > .item > img")
                .ToList()
                .Select(item => $"https:{ item.GetAttributeValue("src", string.Empty) }");
        }

        /// <summary>
        /// Can go next page
        /// </summary>
        /// <param name="document"></param>
        /// <param name="nextPageLink"></param>
        /// <returns></returns>
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