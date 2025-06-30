using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.IO;
using NUnit.Framework.Interfaces;
using WebTests.Pages;
using WebTests.Utils;
using WebTests.Config;
using OfficeOpenXml;
using Microsoft.Extensions.Configuration;

namespace WebTests.Core
{
    /// <summary>
    /// BaseTest manages the WebDriver setup/teardown, logging configuration, and failure handling for all UI tests.
    /// </summary>
    public class BaseTest
    {
        protected IWebDriver driver;
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(BaseTest));

        /// <summary>
        /// Runs once before any tests in the assembly. Loads log4net or any global config.
        /// </summary>

        [OneTimeSetUp]
        public void GlobalSetUp()
        {
            var logConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(logConfigPath));

        
        }
        /// <summary>
        /// Runs before each test. Instantiates driver and navigates to the base URL.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            TestContext.WriteLine($"[SetUp] Starting test: {TestContext.CurrentContext.Test.Name}");

            driver = DriverFactory.GetDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(ConfigurationReader.BaseUrl);

            // Accept cookies if present on the home page
            new HomePage(driver).AcceptCookiesIfPresent();
            Logger.Info($"Navigated to: {ConfigurationReader.BaseUrl}");
        }

        /// <summary>
        /// Runs after each test. Takes a screenshot if the test failed and quits the driver.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;

            // Take screenshot on failure
            if (context.Result.Outcome.Status == TestStatus.Failed)
            {
                string screenshotsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                Directory.CreateDirectory(screenshotsDir);
                string filePath = Path.Combine(screenshotsDir, $"{context.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png");

                try
                {
                    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    File.WriteAllBytes(filePath, screenshot.AsByteArray);
                    TestContext.AddTestAttachment(filePath, "Screenshot on failure");
                    Logger.Info($"Screenshot saved to: {filePath}");
                }
                catch (Exception e)
                {
                    Logger.Error($"Failed to capture screenshot: {e.Message}");
                }
            }

            // Quit and dispose WebDriver instance
            if (driver != null)
            {
                try { driver.Quit(); } catch { }
                try { driver.Dispose(); } catch { }
            }
            DriverFactory.QuitDriver();

            Logger.Info($"Test finished: {context.Test.Name}");
        }
    }
}