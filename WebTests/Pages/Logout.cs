using OpenQA.Selenium;
using NUnit.Framework;

namespace WebTests.Pages
{
    /// <summary>
    /// Provides actions related to the logout flow on Zara.com.
    /// </summary>
    public class LogoutPage : BasePage
    {
        public LogoutPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Logs out the currently logged-in user.
        /// Steps: Click the user/profile name in the header, click the 'PROFİL' menu item,
        /// then click the 'Oturumu sonlandır' (Log out) button on the profile page.
        /// </summary>
        public void Logout()
        {
            try
            {
                // Step 1: Click the user profile name (e.g., "QA", "Hüsniye") in the site header
                var profileHeaderButton = By.CssSelector("a[data-qa-id='layout-header-account']");
                SafeClick(profileHeaderButton);
                TestContext.WriteLine("[INFO] Clicked the profile name in the header.");

                // Step 2: In the side menu, click the "PROFİL" menu item
                var profileMenuItem = By.XPath("//span[normalize-space()='PROFİL']");
                WaitUntilVisible(profileMenuItem, 8);
                SafeClick(profileMenuItem);
                TestContext.WriteLine("[INFO] Clicked the 'PROFİL' menu item.");

                // Step 3: On the profile page, click the "Oturumu sonlandır" button
                var logoutButton = By.XPath("//span[normalize-space()='Oturumu sonlandır']");
                WaitUntilVisible(logoutButton, 8);
                SafeClick(logoutButton);
                TestContext.WriteLine("[INFO] Clicked the 'Log out' button. User is now logged out.");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"[ERROR] Logout failed: {ex.Message}");
                throw;
            }
        }
    }
}