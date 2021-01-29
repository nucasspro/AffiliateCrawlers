using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Windows;
using System;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace AffiliateCrawlers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<string> _listSourceLink = new List<string>
        {
            "https://sablanca.vn/pages/tui-xach-moi-nhat",
            "https://sablanca.vn/pages/giay-dep-moi-nhat",
            "https://sablanca.vn/pages/giay-cao-got",
            "https://sablanca.vn/tui-xach/danh-muc/backpack",
            "https://sablanca.vn/vi-cam-tay",
        };

        private readonly string _host = "https://sablanca.vn/";
        private readonly string _fileExtension = "txt";
        private readonly string _fileName = "sablanca_";
        private readonly string _chromeWebDriverPath = AppDomain.CurrentDomain.BaseDirectory;

        private FirefoxDriver _firefoxDriver;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            var sourceLink = txtInputLink.Text;
            InitSelenium();

            _firefoxDriver.Url = sourceLink;
            _firefoxDriver.Navigate();
            ScrollDown();
        }



        private void InitSelenium()
        {
            _firefoxDriver = new FirefoxDriver();
        }

        private void DisposeSelenium()
        {

        }

        private void GetAllData()
        {
            var itemsXPath = "//*[@id=\"5051HB011317857\"]";
            var itemData = _firefoxDriver.FindElementByXPath(itemsXPath);


            GetAnItems();
        }

        private void ScrollDown()
        {
            IJavaScriptExecutor js = _firefoxDriver;

            var lastHeight = (long)js.ExecuteScript("return document.body.scrollHeight");

            while (true)
            {
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                Task.Delay(20000);
                MessageBox.Show("Test");
                var newHeight = (long)js.ExecuteScript("return document.body.scrollHeight");
                if (newHeight == lastHeight)
                {
                    break;
                }
                lastHeight = newHeight;
            }
        }


        private List<string> GetAnItems()
        {
            return new List<string>();
        }

        private void ExportFile()
        {


            DisposeSelenium();
        }
    }
}
