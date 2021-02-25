using AffiliateCrawlers.Models;
using CsvHelper;
using CsvHelper.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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

        public static void ExportToCSVFile(string fullFilePath, List<ProductInfoModel> data)
        {
            try
            {
                string path = fullFilePath;
                List<string> header = new() { "No", "Name", "Url", "OriginalPrice", "SalePrice", "Content", "ImageLinks" };

                using var writer = new StreamWriter(path);
                using var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                });

                header.ForEach(x => csvWriter.WriteField(x));
                csvWriter.NextRecord();

                for (int i = 0; i < data.Count; i++)
                {
                    csvWriter.WriteField(i + 1);
                    csvWriter.WriteField(data[i].Name);
                    csvWriter.WriteField(data[i].Url);
                    csvWriter.WriteField(data[i].OriginalPrice);
                    csvWriter.WriteField(data[i].SalePrice);
                    csvWriter.WriteField(data[i].Content);

                    StringBuilder imageLinkText = new();
                    data[i].ImageLinks.ForEach(x => imageLinkText.Append(x).Append(Environment.NewLine));
                    csvWriter.WriteField(imageLinkText);

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