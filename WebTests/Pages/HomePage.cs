using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace WebTests.Pages
{
    /// <summary>
    /// Represents actions and elements on the ZARA home page (desktop).
    /// </summary>
    public class HomePage : BasePage
    {
        private readonly By MenMenu = By.CssSelector("a[aria-label='Erkek']");
        private readonly By SeeAll = By.XPath("//a[contains(text(), 'TÜMÜNÜ GÖR')]");
        private readonly By CookieAcceptButton = By.Id("onetrust-accept-btn-handler");
        private readonly By LoginButton = By.CssSelector("a[data-qa-id='layout-header-user-logon']");
        private readonly By MenuButton = By.CssSelector("button[aria-label='Menüyü aç']"); // Eğer mobil için gerekirse

        public HomePage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Accepts cookie banner if visible.
        /// </summary>
        public void AcceptCookiesIfPresent()
        {
            if (IsVisible(CookieAcceptButton))
            {
                Click(CookieAcceptButton);
                TestContext.WriteLine("[INFO] Cookie banner accepted.");
            }
        }

        /// <summary>
        /// Clicks the login button from the header, using hover if necessary.
        /// </summary>
        public void ClickLoginButtonWithHover()
        {
            try
            {
                // Optionally hover or scroll, depending on site behavior
                // Hover(MenuButton); // Eğer gerekirse aktif et
                WaitUntilVisible(LoginButton, 10);
                ScrollTo(LoginButton); // Gerekirse
                Click(LoginButton);
                TestContext.WriteLine("✅ Login button clicked with hover/scroll if needed.");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"❌ Failed to click login button: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Checks if the user is logged in by verifying the presence of the profile/account icon in the header.
        /// Update the selector according to the actual element that appears after login.
        /// </summary>
        /// <returns>True if the profile/account icon is visible, otherwise false.</returns>
        public bool IsLoggedIn()
        {
            try
            {
                // Try to find the profile/account icon that appears after a successful login
                By profileIcon = By.CssSelector("a[data-qa-id='layout-header-user-account']");
                // Return true if the profile icon is visible
                return IsVisible(profileIcon);
            }
            catch
            {
                // If not found or any error occurs, assume the user is not logged in
                return false;
            }
        }




        /// <summary>
        /// Navigates to the Men's section from the user/order page after login (for mobile/redirect flow).
        /// </summary>
        public void NavigateToMenSectionFromOrderPage()
        {
            try
            {
                // 1. Click the hamburger menu button (top left corner)
                var hamburgerMenuButton = By.CssSelector("button[aria-label='Menüyü aç']");
                Thread.Sleep(1200);
                SafeClick(hamburgerMenuButton);
                TestContext.WriteLine("[INFO] Hamburger menu opened.");
                Thread.Sleep(1000); // Small wait for menu animation

                // 2. Wait for and click the 'ERKEK' (Men) section link (the <a> parent, not just <span>)
                var menSectionLink = By.XPath("//a[.//span[text()='ERKEK']]");
                WaitUntilVisible(menSectionLink, 10);
                SafeClick(menSectionLink);
                TestContext.WriteLine("[INFO] 'ERKEK' section clicked.");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"[ERROR] Could not navigate to Men's section: {ex.Message}");
                throw;
            }
        }

        public void ClickSeeAllIfPresent()
        {
            try
            {
                var seeAllElement = WaitAndFind(By.XPath("//span[text()='TÜMÜNÜ GÖR']"));
                seeAllElement.Click();
                TestContext.WriteLine("[INFO] Clicked 'TÜMÜNÜ GÖR'");
            }
            catch (WebDriverTimeoutException)
            {
                TestContext.WriteLine("'TÜMÜNÜ GÖR' not found – continuing...");
            }
        }
        public void OpenSearchBox()
        {
            // Use a robust XPath selector for the search icon (looks for the 'Ara' text).
            var searchIconBy = By.XPath("//span[text()='Ara']");

            TestContext.WriteLine("[DEBUG] Attempting to open search box via XPath...");

            try
            {
                // Wait until the element with 'Ara' text is visible in the DOM.
                WaitUntilVisible(searchIconBy);

                // Try to click the element safely.
                SafeClick(searchIconBy);

                TestContext.WriteLine("[INFO] Search box successfully clicked using XPath and SafeClick.");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("[ERROR] Failed to click search box with XPath: " + ex.Message);

                // Optional: Take a screenshot for troubleshooting.
                try
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    var path = $"SearchBoxClickError_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    screenshot.SaveAsFile(path);
                    TestContext.WriteLine("[INFO] Screenshot saved to: " + path);
                }
                catch { /* ignore screenshot errors */ }

                throw;
            }
        }


    }
}