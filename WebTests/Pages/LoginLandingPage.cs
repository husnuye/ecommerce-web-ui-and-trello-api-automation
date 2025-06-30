using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;

namespace WebTests.Pages
{
    /// <summary>
    /// Represents the login landing page with the main "GİRİŞ YAP" button.
    /// </summary>
    public class LoginLandingPage : BasePage
    {
      
        private readonly By LoginFormOpenButton = By.CssSelector("button[data-qa-id='oauth-logon-button']");

        public LoginLandingPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Moves mouse to the login button and clicks it when clickable.
        /// Uses JavaScript click as fallback.
        /// </summary>
        public void ClickLoginFormButton()
        {
            try
            {
                var button = WaitUntilClickable(LoginFormOpenButton);
                // Move mouse to the button for better reliability (in case of overlays)
                actions.MoveToElement(button).Perform();

                // Try normal click
                button.Click();
                TestContext.WriteLine("➡️ Clicked login form button via mouse move and click.");
            }
            catch (Exception)
            {
                // Fallback: Use JS click
                var button = WaitAndFind(LoginFormOpenButton);
                jsExecutor.ExecuteScript("arguments[0].click();", button);
                TestContext.WriteLine("⚠️ Clicked login form button via JS fallback.");
            }
        }
    }
}