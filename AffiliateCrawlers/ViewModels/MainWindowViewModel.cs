using AffiliateCrawlers.Commons;
using AffiliateCrawlers.Models;
using AffiliateCrawlers.Pages;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Reflection;

namespace AffiliateCrawlers.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        /// <summary>
        /// App name
        /// </summary>
        public string MainScreenTitle { get; } = Constants.AppConstants.AppName;

        /// <summary>
        /// Main combobox
        /// </summary>
        public List<string> CbbMainPageItemsSource { get; } = new()
        {
            Constants.Sablanca.SiteName,
            Constants.Sexyforever.SiteName,
            Constants.Sevenam.SiteName
        };

        /// <summary>
        /// Sub combobox
        /// </summary>
        private readonly List<ObservableCollection<string>> cbbChildPageItemsSource = new()
        {
            new ObservableCollection<string>
            {
                "https://sablanca.vn/pages/tui-xach-moi-nhat",
                "https://sablanca.vn/pages/giay-dep-moi-nhat",
                "https://sablanca.vn/pages/giay-cao-got",
                "https://sablanca.vn/tui-xach/danh-muc/backpack",
                "https://sablanca.vn/vi-cam-tay"
            },
            new ObservableCollection<string>
            {
                "https://sexyforever.vn/collections/sleepwear",
                "https://sexyforever.vn/collections/ao-khoac-ngu",
                "https://sexyforever.vn/collections/lingeries-set",
                "https://sexyforever.vn/collections/body-suit"
            },
            new ObservableCollection<string>
            {
                "https://sevenam.vn/collections/onsale"
            }
        };

        /// <summary>
        /// Crawler instance
        /// </summary>
        private CrawlPageBase _crawlPage;

        /// <summary>
        /// All products
        /// </summary>
        private List<ProductInfoModel> _products = new();

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ObservableCollection<string> _cbbChildPageItemsSource;

        public ObservableCollection<string> CbbChildPageItemsSource
        {
            get => _cbbChildPageItemsSource;
            set { _cbbChildPageItemsSource = value; NotifyOfPropertyChange(() => CbbChildPageItemsSource); }
        }

        private int _cbbMainPageSelectedIndex;

        public int CbbMainPageSelectedIndex
        {
            get => _cbbMainPageSelectedIndex;
            set { _cbbMainPageSelectedIndex = value; NotifyOfPropertyChange(() => CbbMainPageSelectedIndex); }
        }

        private int _cbbChildPageSelectedIndex;

        public int CbbChildPageSelectedIndex
        {
            get => _cbbChildPageSelectedIndex;
            set { _cbbChildPageSelectedIndex = value; NotifyOfPropertyChange(() => CbbChildPageSelectedIndex); }
        }

        private int _txtQuantity = 5;

        public int TxtQuantity
        {
            get => _txtQuantity;
            set { _txtQuantity = value; NotifyOfPropertyChange(() => TxtQuantity); }
        }

        public void CbbMainPageSelectionChanged()
        {
            CbbChildPageItemsSource = new ObservableCollection<string>(cbbChildPageItemsSource[CbbMainPageSelectedIndex]);
            CbbChildPageSelectedIndex = 0;
        }

        private ObservableCollection<ProductInfoViewModel> _productViewData;

        public ObservableCollection<ProductInfoViewModel> ProductViewData
        {
            get => _productViewData;
            set { _productViewData = value; NotifyOfPropertyChange(() => ProductViewData); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            CbbChildPageItemsSource = new ObservableCollection<string>(cbbChildPageItemsSource[0]);
            CbbChildPageSelectedIndex = 0;
        }

        /// <summary>
        /// Start scrawler
        /// </summary>
        public void StartCrawl()
        {
            switch (CbbMainPageItemsSource[CbbMainPageSelectedIndex])
            {
                case Constants.Sablanca.SiteName:
                    _crawlPage = new Sablanca();
                    break;

                case Constants.Sexyforever.SiteName:
                    _crawlPage = new Sexyforever();
                    break;

                case Constants.Sevenam.SiteName:
                    _crawlPage = new Sevenam();
                    break;

                default:
                    return;
            }

            if (_crawlPage is null)
            {
                return;
            }

            string crawlLink = CbbChildPageItemsSource[CbbChildPageSelectedIndex];
            _products = _crawlPage.Start(crawlLink, TxtQuantity);
            UpdateGUI(_products);
        }

        private void UpdateGUI(List<ProductInfoModel> data)
        {
            ProductViewData = new();
            foreach (var item in data)
            {
                ProductViewData.Add(new ProductInfoViewModel
                {
                    Name = item.Name,
                    Url = item.Url,
                    OriginalPrice = item.OriginalPrice,
                    SalePrice = item.SalePrice,
                    Content = item.Content,
                    ImageLinks = string.Join(",\r\n", item.ImageLinks)
                });
            }
        }

        /// <summary>
        /// Export file
        /// </summary>
        public void ExportFile()
        {
            if (_crawlPage is null || _products.Count is 0)
            {
                MessageBox.Show(Constants.AppConstants.DataIsEmpty);
                return;
            }

            try
            {
                // Open dialog for selecting save folder
                string folderPath = string.Empty;

                System.Windows.Forms.FolderBrowserDialog diag = new();
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    folderPath = diag.SelectedPath;
                }

                if (folderPath?.Length == 0)
                {
                    MessageBox.Show(Constants.AppConstants.WrongFolder);
                    return;
                }

                string fullFilePath = Path.Combine(folderPath, $"{_crawlPage.FileName}{DateTime.Now:yyyyMMdd_hhmmss}.{Constants.AppConstants.TextFileExtension}");

                Utilities.ExportToCSVFile(fullFilePath, _products);

                MessageBox.Show(Constants.AppConstants.ExportFileSuccessful);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Constants.AppConstants.ExportFileFailure);
            }
        }
    }
}