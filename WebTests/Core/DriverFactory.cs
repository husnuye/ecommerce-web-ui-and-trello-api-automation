using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebTests.Core
{
    /// <summary>
    /// DriverFactory is responsible for creating and managing WebDriver instances.
    /// Implements anti-bot best practices based on BrowserStack and real user fingerprinting.
    /// </summary>
    public static class DriverFactory
    {
        [ThreadStatic]
        private static IWebDriver driver;

        public static IWebDriver GetDriver()
        {
            if (driver == null)
            {
                var options = new ChromeOptions();

                // Always use incognito to avoid caching and cross-test tracking
                options.AddArgument("--incognito");

                // Create a unique, fresh user data directory for every test run
                options.AddArgument("--user-data-dir=/tmp/chromeprofile_" + Guid.NewGuid());

                // Use Turkish locale - adjust if needed
                options.AddArgument("--lang=tr-TR");

                // Set a modern and common user-agent string
                options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

                // Remove "enable-automation" and disable automation extension to minimize detection
                options.AddExcludedArgument("enable-automation");
                options.AddAdditionalOption("useAutomationExtension", false);

                // Disable automation features in Blink engine
                options.AddArgument("--disable-blink-features=AutomationControlled");

                // Maximize window, disable infobars and extensions for cleaner session
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-infobars");
                options.AddArgument("--disable-extensions");

                // Improve stability in Docker/CI environments
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--no-sandbox");

                // Optionally, set referer if required (sometimes helps with certain sites)
                // options.AddArgument("--referer=https://www.zara.com/");

                // Optionally, set homepage if you want browser to always start at Zara
                // options.AddArgument("--homepage=https://www.zara.com/");

                // Final: create driver
                driver = new ChromeDriver(options);
            }
            return driver;
        }

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
