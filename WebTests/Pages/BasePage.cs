using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

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
        /// Scrolls element into view using JS.
        /// </summary>
        protected void ScrollTo(By by)
        {
            var element = WaitAndFind(by);
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        /// <summary>
        /// Types into an input field after clearing it.
        /// </summary>
        protected void Type(By by, string text)
        {
            var element = WaitAndFind(by);
            element.Clear();
            element.SendKeys(text);
        }

        /// <summary>
        /// Waits until element is visible.
        /// </summary>
        protected IWebElement WaitAndFind(By by)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        /// <summary>
        /// Waits until element is clickable.
        /// </summary>
        protected IWebElement WaitUntilClickable(By by)
        {
            return wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        /// <summary>
        /// Returns true if element is visible.
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
        /// Executes raw JavaScript on the browser.
        /// </summary>
        protected object ExecuteScript(string script, params object[] args)
        {
            return jsExecutor.ExecuteScript(script, args);
        }

        /// <summary>
        /// Hovers over the given element.
        /// </summary>
        protected void Hover(By by)
        {
            var element = WaitAndFind(by);
            actions.MoveToElement(element).Perform();
        }

        /// <summary>
        /// Waits until the given element contains specific text.
        /// </summary>
        protected void WaitUntilTextPresent(By by, string text)
        {
            wait.Until(driver => WaitAndFind(by).Text.Contains(text));
        }

        /// <summary>
        /// Waits until page readyState is 'complete'.
        /// </summary>
        protected void WaitForPageLoad()
        {
            wait.Until(d => jsExecutor.ExecuteScript("return document.readyState").ToString() == "complete");
        }

        /// <summary>
        /// Waits until a specific attribute on an element equals a given value.
        /// </summary>
        protected void WaitUntilAttributeEquals(By by, string attribute, string value)
        {
            wait.Until(driver =>
            {
                var element = driver.FindElement(by);
                return element.GetAttribute(attribute) == value;
            });
        }
    }
}