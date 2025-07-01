using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using NUnit.Framework; // For TestContext

namespace WebTests.Pages
{
    /// <summary>
    /// BasePage class includes common reusable actions and waits for all page classes.
    /// It enhances stability and performance on dynamic JS-based sites like Zara.
    /// </summary>
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected Actions actions;
        protected IJavaScriptExecutor jsExecutor;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            actions = new Actions(driver);
            jsExecutor = (IJavaScriptExecutor)driver;
        }

        /// <summary>
        /// Waits for element and clicks it. Uses JS fallback if needed.
        /// </summary>
        protected void Click(By by)
        {
            try
            {
                WaitUntilClickable(by).Click();
            }
            catch (Exception)
            {
                var element = WaitAndFind(by);
                jsExecutor.ExecuteScript("arguments[0].click();", element);
            }
        }

        /// <summary>
        /// Scrolls element into view using JavaScript.
        /// </summary>
        protected void ScrollTo(By by)
        {
            var element = WaitAndFind(by);
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        /// <summary>
        /// Clears the input field and types the provided text.
        /// </summary>
        protected void Type(By by, string text)
        {
            var element = WaitAndFind(by);
            element.Clear();
            element.SendKeys(text);
        }

        /// <summary>
        /// Waits until the element is visible in the DOM.
        /// </summary>
        protected IWebElement WaitAndFind(By by)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }


        /// <summary>
        /// Waits until the element is clickable (with optional timeout)
        /// </summary>
        protected IWebElement WaitUntilClickable(By by, int timeoutInSeconds = 10)
        {
            var customWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return customWait.Until(ExpectedConditions.ElementToBeClickable(by));
        }


        /// <summary>
        /// Returns true if the element is visible.
        /// </summary>
        protected bool IsVisible(By by)
        {
            try
            {
                return WaitAndFind(by).Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes a custom JavaScript command.
        /// </summary>
        protected object ExecuteScript(string script, params object[] args)
        {
            return jsExecutor.ExecuteScript(script, args);
        }

        /// <summary>
        /// Hovers the mouse over the specified element.
        /// </summary>
        protected void Hover(By by)
        {
            var element = WaitAndFind(by);
            actions.MoveToElement(element).Perform();
        }

        /// <summary>
        /// Waits until the element contains the specified text.
        /// </summary>
        protected void WaitUntilTextPresent(By by, string text)
        {
            wait.Until(driver => WaitAndFind(by).Text.Contains(text));
        }

        /// <summary>
        /// Waits until the page is fully loaded (document.readyState = 'complete').
        /// </summary>
        protected void WaitForPageLoad()
        {
            wait.Until(d => jsExecutor.ExecuteScript("return document.readyState").ToString() == "complete");
        }

        /// <summary>
        /// Waits until a specific attribute on an element equals the given value.
        /// </summary>
        protected void WaitUntilAttributeEquals(By by, string attribute, string value)
        {
            wait.Until(driver =>
            {
                var element = driver.FindElement(by);
                return element.GetAttribute(attribute) == value;
            });
        }


        /// <summary>
        /// Waits until the specified element is visible and returns it.
        /// </summary>
        protected IWebElement WaitUntilVisible(By by, int timeoutInSeconds = 10)
        {
            var customWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return customWait.Until(driver =>
            {
                var el = driver.FindElement(by);
                return el.Displayed ? el : null;
            });
        }

        /// <summary>
        /// Safely scrolls to the element, moves mouse to it, and tries both normal and JS click.
        /// </summary>

        protected void SafeClick(By by)
        {
            Exception lastEx = null;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    var element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
                    actions.MoveToElement(element).Perform();
                    element.Click();
                    TestContext.WriteLine("[INFO] Clicked with mouse actions.");
                    return;
                }
                catch (Exception ex)
                {
                    lastEx = ex;
                    TestContext.WriteLine($"[WARN] Click attempt {i + 1} failed: {ex.Message}");
                    Thread.Sleep(500);
                }
            }
            // Son çare olarak JS click uygula
            var fallbackElement = driver.FindElement(by);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", fallbackElement);
            TestContext.WriteLine("[INFO] Clicked with JS as fallback after retries: " + lastEx?.Message);
        }


        /// <summary>
        /// Accepts the Zara cookie consent popup if it is displayed.
        /// Uses XPath to locate and click the "Accept All Cookies" button.
        /// </summary>
        public void AcceptCookiesIfPresent()
        {
            try
            {
                var cookieButton = driver.FindElement(By.XPath("//button[contains(text(), 'TÜM ÇEREZLERİ KABUL ET')]"));
                if (cookieButton.Displayed)
                {
                    cookieButton.Click();
                    TestContext.WriteLine("[INFO] Cookie banner closed.");
                }
            }
            catch (NoSuchElementException)
            {
                TestContext.WriteLine("[INFO] Cookie banner not found.");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"[WARN] Failed to close cookie banner: {ex.Message}");
            }
        }

        /// <summary>
        /// Scrolls the page to bring the specified element into view.
        /// </summary>
        /// <param name="element">The web element to scroll to.</param>
        protected void ScrollToElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }


        /// <summary>
        /// Waits for and finds all elements matching the given selector.
        /// </summary>
        /// <param name="by">The selector to find elements.</param>
        /// <returns>List of IWebElement</returns>
        protected IList<IWebElement> WaitAndFindAll(By by)
        {
            // You may want to use WebDriverWait for better reliability
            return driver.FindElements(by);
        }


        /// <summary>
        /// Scroll to element and click safely using Actions class.
        /// </summary>
        /// <param name="by">Element locator</param>
        public void SafeClickWithScroll(By by)
        {
            var element = WaitUntilVisible(by, 10);
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);

            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Click().Perform();

            TestContext.WriteLine($"[INFO] Safe clicked element: {by.ToString()}");
        }



        /// <summary>
/// Helper method that waits for an element to be visible within a timeout period.
/// </summary>
/// <param name="selector">Element selector</param>
/// <param name="timeout">Maximum wait time</param>
/// <returns>True if element became visible, false if timeout occurred</returns>
private bool WaitUntilVisibleOrTimeout(By selector, TimeSpan timeout)
{
    try
    {
        var wait = new WebDriverWait(driver, timeout);
        wait.Until(drv =>
        {
            var element = drv.FindElement(selector);
            return (element != null && element.Displayed) ? element : null;
        });
        return true;
    }
    catch (WebDriverTimeoutException)
    {
        TestContext.WriteLine($"[WARN] Timeout waiting for element: {selector}");
        return false;
    }
}
    }
}