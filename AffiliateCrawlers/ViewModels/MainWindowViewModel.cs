using AffiliateCrawlers.Models;
using AffiliateCrawlers.Pages;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AffiliateCrawlers.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        public string MainScreenTitle { get; } = "AffiliateCrawlers";

        public List<string> CbbMainPageItemsSource { get; } = new()
        {
            "sablanca.vn",
            "sexyforever.vn",
            "sevenam.vn"
        };

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

        private string _tbxAllData;

        public string TbxAllData
        {
            get => _tbxAllData;
            set { _tbxAllData = value; NotifyOfPropertyChange(() => TbxAllData); }
        }

        public void CbbMainPageSelectionChanged()
        {
            CbbChildPageItemsSource = new ObservableCollection<string>(cbbChildPageItemsSource[CbbMainPageSelectedIndex]);
            CbbChildPageSelectedIndex = 0;
        }

        public MainWindowViewModel()
        {
            CbbChildPageItemsSource = new ObservableCollection<string>(cbbChildPageItemsSource[0]);
            CbbChildPageSelectedIndex = 0;
        }

        private CrawlPageBase _crawlPage;

        public async void StartCrawl()
        {
            string crawlLink = CbbChildPageItemsSource[CbbChildPageSelectedIndex];
            switch (CbbMainPageItemsSource[CbbMainPageSelectedIndex])
            {
                case "sablanca.vn":
                    _crawlPage = new Sablanca(null);
                    break;

                case "sexyforever.vn":
                    _crawlPage = new Sexyforever();
                    break;

                case "sevenam.vn":
                    break;

                default:
                    return;
            }

            var data = await _crawlPage.Start(crawlLink, TxtQuantity);
            UpdateGUI(data);
        }

        private void UpdateGUI(List<ProductInfoModel> data)
        {
            foreach (var item in data)
            {
                TbxAllData += "======================================" +
                    $"ProductName = { item.Title }{ Environment.NewLine }" +
                    $"OriginalPrice = { item.OriginalPrice }{ Environment.NewLine }" +
                    $"SalePrice = { item.SalePrice }{ Environment.NewLine }" +
                    $"Content = { item.Content }{ Environment.NewLine }" +
                    $"ImageLinks = { Environment.NewLine }";

                foreach (var link in item.ImageLinks)
                {
                    TbxAllData += $"\t{ link }{ Environment.NewLine}";
                }
            }
        }

        public void ExportFile()
        {
        }
    }
}