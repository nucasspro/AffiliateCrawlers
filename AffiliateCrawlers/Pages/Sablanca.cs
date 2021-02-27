using AffiliateCrawlers.Commons;
using AffiliateCrawlers.Models;
using AffiliateCrawlers.Models.PageModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AffiliateCrawlers.Pages
{
    public class Sablanca : CrawlPageBase
    {
        private readonly log4net.ILog _log;

        /// <summary>
        /// Constructor
        /// </summary>
        public Sablanca(log4net.ILog log)
        {
            Host = Constants.Sablanca.HostName;
            FileName = Constants.Sablanca.FileName;
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
                var products = GetAllProductInfo(url, quantity);

                _log.Info("Get all data successfully");

                return products;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new List<ProductInfoModel>();
            }
        }

        /// <summary>
        /// Get product info
        /// </summary>
        /// <param name="url"></param>
        /// <param name="quantity"></param>
        /// <param name="startPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<ProductInfoModel> GetAllProductInfo(string url, int quantity, int startPageIndex = 0, int pageSize = 12)
        {
            List<ProductInfoModel> allData = new();
            var client = new RestClient(url)
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.POST);

            do
            {
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "ASP.NET_SessionId=kbsiqsu3pvbzq3k4ma1iyjqe; BNIS_x-bni-jas=MtpT/DLkhzTE5WmTnxNLKFFRG3hMSp1UvxePQcQbI3U4l7J+Sz6amsTw/2BnDz7eX5ie8cXiuZJEboTPXE1W44Tr32dPsZkSVjQnRC/g6msu+7fGgk7ZSQ==");
                request.AddParameter("order", "");
                request.AddParameter("pageindex", startPageIndex);
                request.AddParameter("pagesize", pageSize);
                IRestResponse response = client.Execute(request);

                if (response.Content?.Length == 0)
                {
                    return allData;
                }

                foreach (var item in JsonConvert.DeserializeObject<List<SablancaModel>>(response.Content))
                {
                    if (allData.Count >= quantity)
                    {
                        return allData;
                    }

                    allData.Add(GetData(item));
                }

                startPageIndex++;
            } while (allData.Count < quantity);

            return allData;
        }

        /// <summary>
        /// Get data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private ProductInfoModel GetData(SablancaModel item)
        {
            string name = item.ITEMNAME;
            string url = $"{Host}{item.MAINGROUPLINK}/{item.NAMEID}";
            string originalPrice = item.RETAILPRICE.ToString();
            string salePrice = item.SALEPRICE.ToString();
            List<string> imageLinks = item.LISTIMAGES;

            StringBuilder content = new StringBuilder()
                .Append(Constants.Sablanca.MaterialText).Append(':').Append(item.MATERIALTEXT).Append(Environment.NewLine)
                .Append(Constants.Sablanca.StrapTypeText).Append(':').Append(item.STRAPTYPETEXT).Append(Environment.NewLine)
                .Append(Constants.Sablanca.DimensionText).Append(':').Append(item.DIMENSION).Append(Environment.NewLine)
                .Append(Constants.Sablanca.CompartmentText).Append(':').Append(item.COMPARTMENT).Append(Environment.NewLine)
                .Append(Constants.Sablanca.StyleText).Append(':').Append(item.STYLETEXT).Append(Environment.NewLine)
                .Append(Constants.Sablanca.Description).Append(':').Append(item.DESCRIPTION).Append(Environment.NewLine);

            _log.Debug($"Get new product: {name} - {url}");

            return new ProductInfoModel()
            {
                Name = name,
                Url = url,
                Content = content.ToString(),
                OriginalPrice = originalPrice,
                SalePrice = salePrice,
                ImageLinks = imageLinks,
            };
        }
    }
}