using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebTests.Config;

namespace WebTests.Core
{
    /// <summary>
    /// DriverFactory is responsible for creating and managing WebDriver instances.
    /// It supports singleton pattern to reuse the driver within the same test context.
    /// </summary>
    public static class DriverFactory
    {
        // Thread-safe driver instance (one per test thread)
        [ThreadStatic]
        private static IWebDriver driver;

        /// <summary>
        /// Returns a single WebDriver instance per thread.
        /// </summary>
        public static IWebDriver GetDriver()
        {
            if (driver == null)
            {
                // Currently only Chrome is implemented
                if (ConfigurationReader.Browser.ToLower() == "chrome")
                {
                    driver = new ChromeDriver();
                }
                else
                {
                    throw new NotSupportedException("Only Chrome browser is currently supported.");
                }
            }

            return driver;
        }

        /// <summary>
        /// Properly quits and cleans up the WebDriver instance.
        /// </summary>
        public static void QuitDriver()
        {
            if (driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }
    }
}