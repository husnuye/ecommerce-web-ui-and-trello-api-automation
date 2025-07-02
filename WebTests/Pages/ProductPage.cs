using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;
using System.Linq;
using NUnit.Framework; // For TestContext

namespace WebTests.Pages
{
    /// <summary>
    /// Represents product listing and detail page interactions.
    /// </summary>
    public class ProductPage : BasePage
    {
        private readonly By ProductLinks = By.CssSelector("a.product-link");

        // Updated selectors based on current Zara site structure
        private readonly By ProductName = By.CssSelector(".product-detail-info__header-name");
        private readonly By ProductPrice = By.CssSelector(".product-detail-info__price");

    // Button selectors
    private readonly By AddButtonSelector = By.XPath("//button[.//span[contains(text(),'Ekle')]]");
    private readonly By SizeSelectorButton = By.XPath("//button[contains(@aria-label, 'Bir beden seçin')]");
    private readonly By AddToCartSelector = By.CssSelector("button.add-to-cart");
    private readonly By CompleteOrderButton = By.XPath("//a[.//span[contains(text(),'Siparişi Tamamla')]]");

    //private readonly By SizeOptionsSelector = By.CssSelector("ul.size-selector-sizes li.size-selector-sizes__size--enabled");

        private readonly By SmartSizeNoThanksButton = By.XPath("//button[contains(text(),'Hayır, teşekkürler')]");

        // Selector for available size options (update the selector as needed for your site)
        //private readonly By SizeOptionsSelector = By.CssSelector("button.size-selector-option:not([disabled])");

        private readonly By SizeOptionsSelector = By.CssSelector("ul.size-selector-sizes li.size-selector-sizes__size--enabled");

        private readonly Random random = new Random();

        public ProductPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Retrieves the selected product's name.
        /// </summary>
        /// <returns>Product name as string</returns>
        public string GetProductName()
        {
            try
            {
                WaitForPageLoad();
                var element = WaitUntilVisible(ProductName, 30);
                jsExecutor.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
                string text = element.Text;
                TestContext.WriteLine($"[INFO] Product name found: {text}");
                return text;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine("[ERROR] Could not find product name: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the selected product's price.
        /// </summary>
        /// <returns>Product price as string</returns>


        public string GetProductPrice()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            var element = wait.Until(drv =>
            {
                var el = drv.FindElement(ProductPrice);
                return (el != null && el.Displayed) ? el : null;
            });
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            string priceText = element.Text;
            TestContext.WriteLine($"[INFO] Product price found: {priceText}");
            return priceText;
        }

        /// <summary>
/// Clicks the 'Ekle' (Add) button if visible.
/// </summary>
/// <returns>True if button clicked, false otherwise.</returns>
public bool ClickAddButton()
{
    if (IsVisible(AddButtonSelector))
    {
        TestContext.WriteLine("[INFO] 'Ekle' button found, clicking it.");
        SafeClick(AddButtonSelector);
        return true;
    }
    TestContext.WriteLine("[WARN] 'Ekle' button not found.");
    return false;
}

/// <summary>
/// Handles the Smart Size popup by clicking 'No Thanks' if popup appears within timeout.
/// </summary>
public void HandleSmartSizePopup()
{
    if (WaitUntilVisibleOrTimeout(SmartSizeNoThanksButton, TimeSpan.FromSeconds(10)))
    {
        TestContext.WriteLine("[INFO] Smart Size popup detected, clicking 'No Thanks' button.");
        SafeClickWithScroll(SmartSizeNoThanksButton);
    }
    else
    {
        TestContext.WriteLine("[INFO] Smart Size popup not present.");
    }
}

/// <summary>
/// Opens the size selector by clicking the size selector button, if visible within timeout.
/// </summary>
/// <returns>True if size selector opened, false otherwise.</returns>
public bool OpenSizeSelector()
{
    if (WaitUntilVisibleOrTimeout(SizeSelectorButton, TimeSpan.FromSeconds(10)))
    {
        TestContext.WriteLine("[INFO] Size selector button found, clicking it.");
        SafeClick(SizeSelectorButton);
        return true;
    }
    TestContext.WriteLine("[WARN] Size selector button not found.");
    return false;
}

/// <summary>
/// Selects the first available size button that is visible and enabled.
/// </summary>
/// <returns>True if a size was selected, false otherwise.</returns>


public bool SelectFirstAvailableSize()
{
    if (WaitUntilVisibleOrTimeout(SizeOptionsSelector, TimeSpan.FromSeconds(10)))
    {
        var sizes = driver.FindElements(SizeOptionsSelector);
        var firstActiveSize = sizes.FirstOrDefault(s => s.Displayed /* && diğer durum kontrolü gerekmeyebilir çünkü selector zaten aktif bedenleri getiriyor */);
        if (firstActiveSize != null)
        {
            TestContext.WriteLine($"[INFO] Selecting first available size: '{firstActiveSize.Text}'.");
            firstActiveSize.Click();
            return true;
        }
        else
        {
            TestContext.WriteLine("[WARN] No active size button found.");
        }
    }
    else
    {
        TestContext.WriteLine("[WARN] Size options did not appear within timeout.");
    }
    return false;
}
/// <summary>
/// Waits for the 'Complete Order' button to be visible and clicks it.
/// </summary>
public void ClickCompleteOrderButtonWithWait()
{
    if (WaitUntilVisibleOrTimeout(CompleteOrderButton, TimeSpan.FromSeconds(10)))
    {
        TestContext.WriteLine("[INFO] 'Complete Order' button found, clicking it.");
        SafeClick(CompleteOrderButton);
    }
    else
    {
        TestContext.WriteLine("[WARN] 'Complete Order' button not found after waiting. Skipping.");
    }
}

/// <summary>
/// Helper method that waits for an element to be visible within a timeout period.
/// </summary>
/// <param name="selector">Element selector</param>
/// <param name="timeout">Maximum wait time</param>
/// <returns>True if element became visible, false if timeout occurred</returns>
public bool WaitUntilVisibleOrTimeout(By selector, TimeSpan timeout)
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

