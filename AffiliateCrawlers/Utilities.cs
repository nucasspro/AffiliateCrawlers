using AffiliateCrawlers.Models;
using CsvHelper;
using CsvHelper.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace AffiliateCrawlers
{
    public static class Utilities
    {
        public static void ScrollDown(RemoteWebDriver driver)
        {
            IJavaScriptExecutor js = driver;

            var lastHeight = (long)js.ExecuteScript("return document.body.scrollHeight");

            while (true)
            {
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                Task.Delay(2000);

                var newHeight = (long)js.ExecuteScript("return document.body.scrollHeight");
                if (newHeight == lastHeight)
                {
                    break;
                }
                lastHeight = newHeight;
            }
        }

        public static void ExportToCSVFile(string fileName, List<ProductInfoModel> data)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}{DateTime.Now:yyyyMMdd}.csv");

                using var writer = new StreamWriter(path);
                using var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                });

                csvWriter.WriteHeader<ProductInfoModel>();
                csvWriter.NextRecord();

                foreach (var item in data)
                {
                    csvWriter.WriteField(item.Name);
                    csvWriter.WriteField(item.Url);
                    csvWriter.WriteField(item.OriginalPrice);
                    csvWriter.WriteField(item.SalePrice);
                    csvWriter.WriteField(item.Content);
                    csvWriter.WriteField(item.ImageLinks);
                    csvWriter.NextRecord();
                }

                writer.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}