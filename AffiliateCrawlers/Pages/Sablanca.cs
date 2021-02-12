using AffiliateCrawlers.Models;
using AffiliateCrawlers.Models.PageModels;
using Caliburn.Micro;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AffiliateCrawlers.Pages
{
    public class Sablanca : CrawlPageBase
    {
        public Sablanca(RemoteWebDriver driver)
        {
            Host = "https://sablanca.vn/";
            FileName = "sablanca_";
            Driver = driver;
        }

        public override List<ProductInfoModel> Start(string url, int quantity)
        {
            try
            {
                var allData = GetAllProductLink(null, url, quantity);

                StringBuilder allDataString = new();
                foreach (var item in allData)
                {
                    allDataString.Append(item);
                }

                var allDataObject = JsonConvert.DeserializeObject<List<SablancaModel>>(allDataString.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            //Driver.Url = url;
            //Driver.Navigate();
            //Utilities.ScrollDown(Driver);

            // GetAllItems(1);
            return null;
        }

        public List<string> GetAllProductLink(HtmlWeb web, string url, int quantity)
        {
            List<string> links = new();
            var client = new RestClient(url)
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.POST);
            int pageIndex = 0;
            do
            {
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "ASP.NET_SessionId=kbsiqsu3pvbzq3k4ma1iyjqe; BNIS_x-bni-jas=MtpT/DLkhzTE5WmTnxNLKFFRG3hMSp1UvxePQcQbI3U4l7J+Sz6amsTw/2BnDz7eX5ie8cXiuZJEboTPXE1W44Tr32dPsZkSVjQnRC/g6msu+7fGgk7ZSQ==");
                request.AddParameter("order", "");
                request.AddParameter("pageindex", pageIndex);
                request.AddParameter("pagesize", "12");
                IRestResponse response = client.Execute(request);

                if (response.Content == string.Empty)
                {
                    return links;
                }

                var allData = JsonConvert.DeserializeObject<List<SablancaModel>>(response.Content);
                foreach (var item in allData)
                {
                    links.Add
                }


                pageIndex++;
            } while (links.Count < quantity);
            return new List<string>();
        }
    }
}