using AffiliateCrawlers.Pages;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;
using System.Windows;

namespace AffiliateCrawlers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _listPages = new List<string>
        {
            "sablanca.vn",
            "sexyforever.vn",
            "sevenam.vn"
        };

        private FirefoxDriver _firefoxDriver;

        public MainWindow()
        {
            InitializeComponent();
            cbbPageSelected.ItemsSource = _listPages;
            cbbPageSelected.SelectedIndex = 0;
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            InitSelenium();
            CrawlPageBase crawlPage = null;
            switch (cbbPageSelected.SelectedItem.ToString())
            {
                case "sablanca.vn":
                    crawlPage = new Sablanca(_firefoxDriver);
                    break;

                case "sexyforever.vn":
                    crawlPage = new Sexyforever(_firefoxDriver);
                    break;

                case "sevenam.vn":
                    break;

                default:
                    return;
            }

            var data = crawlPage.Start(1);
        }

        private void InitSelenium()
        {
            _firefoxDriver = new FirefoxDriver();
        }

        private void DisposeSelenium()
        {
        }


        private void btnExportFile_Click(object sender, RoutedEventArgs e)
        {
            ExportFile();
        }

        private void ExportFile()
        {
            DisposeSelenium();
        }
    }
}