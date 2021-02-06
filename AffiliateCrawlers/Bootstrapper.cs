using AffiliateCrawlers.ViewModels;
using Caliburn.Micro;
using System;
using System.Windows;

namespace AffiliateCrawlers
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            // Create singleton instance
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            _container.PerRequest<MainWindowViewModel>();

            ChangeMappingLocator();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        private static void ChangeMappingLocator()
        {
            const string defaultSubNamespaceForViews = "AffiliateCrawlers.Views";
            const string defaultSubNamespaceForViewModels = "AffiliateCrawlers.ViewModels";

            TypeMappingConfiguration config = new()
            {
                DefaultSubNamespaceForViews = defaultSubNamespaceForViews,
                DefaultSubNamespaceForViewModels = defaultSubNamespaceForViewModels
            };

            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.AddSubNamespaceMapping(defaultSubNamespaceForViewModels, defaultSubNamespaceForViews);
        }
    }
}