using NUnit.Framework;
using OpenQA.Selenium;
using WebTests.Config;
using System;
using System.IO;

namespace WebTests.Core
{
    /// <summary>
    /// BaseTest sets up and tears down WebDriver before/after each test.
    /// Includes error logging and optional screenshot capture.
    /// </summary>
    public class BaseTest
    {
        protected IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            TestContext.WriteLine($"[SetUp] Starting test: {TestContext.CurrentContext.Test.Name}");

            driver = DriverFactory.GetDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(ConfigurationReader.BaseUrl);

            TestContext.WriteLine($"Navigated to: {ConfigurationReader.BaseUrl}");
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;

            // If test failed, take screenshot
            if (context.Result.Outcome.Status == TestStatus.Failed)
            {
                string screenshotsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                Directory.CreateDirectory(screenshotsDir);

                string filePath = Path.Combine(screenshotsDir, $"{context.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png");

                try
                {
                    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
                    TestContext.AddTestAttachment(filePath, "Screenshot on failure");
                    TestContext.WriteLine($"[TearDown] Screenshot saved to: {filePath}");
                }
                catch (Exception e)
                {
                    TestContext.WriteLine($"[TearDown] Failed to capture screenshot: {e.Message}");
                }
            }

            // Quit driver regardless of test result
            DriverFactory.QuitDriver();
            TestContext.WriteLine($"[TearDown] Finished test: {context.Test.Name}");
        }
    }
}