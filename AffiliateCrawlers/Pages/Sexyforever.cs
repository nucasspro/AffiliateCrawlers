using AffiliateCrawlers.Commons;
using AffiliateCrawlers.Models;
using Caliburn.Micro;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
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
        private readonly log4net.ILog _log;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log"></param>
        public Sexyforever(log4net.ILog log)
        {
            Host = Constants.Sexyforever.HostName;
            FileName = Constants.Sexyforever.FileName;
            _log = log;
        }

        /// <summary>
        /// Start crawler
        /// </summary>
        /// <param name="url"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public override List<ProductInfoModel> Start(string url, int quantity)
        {
            try
            {
                var web = new HtmlWeb();
                var productUrls = GetAllProductLink(web, url, quantity);

                _log.Info("Get all data successfully");

                return GetAllProductInfo(web, productUrls).ToList();
            }
            catch (Exception ex)
            {
                _log.Error($"{ex.Message}\r\n{ex.StackTrace}");
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get all product link
        /// </summary>
        /// <param name="web"></param>
        /// <param name="url"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public List<string> GetAllProductLink(HtmlWeb web, string url, int quantity)
        {
            List<string> links = new();

            HtmlDocument doc;
            HtmlNode document;

            do
            {
                doc = web.Load(url);
                document = doc.DocumentNode;
                var urls = document.QuerySelectorAll(".product-img > a").ToList();
                if (urls.Count == 0)
                {
                    break;
                }
                foreach (var item in urls)
                {
                    string link = $"{Host}{item.GetAttributeValue("href", string.Empty)}";
                    links.Add(link);

                    _log.Debug($"Add new link: {link}");

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
        /// <param name="web"></param>
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
        /// <param name="web"></param>
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
            (res.SalePrice, res.OriginalPrice) = GetProductPrice(document);
            res.ImageLinks = GetProductImages(document).ToList();

            _log.Debug($"Get new product: {res.Name} - {res.Url}");
            return res;
        }

        /// <summary>
        /// Get product name
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private string GetProductName(HtmlNode document)
        {
            return document
                .QuerySelector(".product-content > .pro-content-head > h1")
                .InnerText;
        }

        /// <summary>
        /// Get sale price and original price
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private (string, string) GetProductPrice(HtmlNode document)
        {
            var salePrice = GetProductSalePrice(document);
            var originalPrice = GetProductOriginalPrice(document) ?? salePrice;

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
            var content = document
                .QuerySelector(".product-content > .pro-short-desc")
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
                .QuerySelectorAll("#ProductThumbs > div.inner > li > a")
                .ToList()
                .Select(item => $"https:{ item.GetAttributeValue("href", string.Empty) }");
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

                    _log.Info("Can go to next page");
                    return true;
                }
            }

            _log.Info("Can't go to next page");
            return false;
        }
    }
}