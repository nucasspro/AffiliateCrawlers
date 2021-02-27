using log4net;
using System.Windows;

namespace AffiliateCrawlers
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            _log.Info("        =============  Started Logging  =============        ");

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _log.Info("        =============  Ended Logging  =============        ");
            base.OnExit(e);
        }
    }
}