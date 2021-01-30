using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Threading;

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
                Thread.Sleep(5000);

                var newHeight = (long)js.ExecuteScript("return document.body.scrollHeight");
                if (newHeight == lastHeight)
                {
                    break;
                }
                lastHeight = newHeight;
            }
        }
    }
}