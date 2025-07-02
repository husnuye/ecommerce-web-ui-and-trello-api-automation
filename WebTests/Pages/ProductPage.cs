using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;
using System.Linq;
using NUnit.Framework; // For TestContext
using SeleniumExtras.WaitHelpers;

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

        private readonly By SizeOptionsSelector = By.CssSelector("ul.size-selector-sizes li.size-selector-sizes__size--enabled");
        // Alias for enabled size options (for clarity in method usage)
        // By SizeOptionsEnabledSelector = By.CssSelector("ul.size-selector-sizes li.size-selector-sizes__size--enabled");

        private readonly By SmartSizeNoThanksButton = By.XPath("//button[contains(text(),'Hayır, teşekkürler')]");

        // Selector for available size options (update the selector as needed for your site)
        //private readonly By SizeOptionsSelector = By.CssSelector("button.size-selector-option:not([disabled])");

        private readonly By SizeOptionsEnabledSelector = By.CssSelector("button.size-selector-sizes-size__button[data-qa-action='size-in-stock']");


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
        /// Converts a price string (e.g. "1.690,00 TL") to a decimal number.
        /// It removes currency symbols and handles locale-specific formatting (dot as thousand separator, comma as decimal separator).
        /// </summary>
        /// <param name="priceText">The price string to convert.</param>
        /// <returns>The decimal representation of the price.</returns>
        /// <exception cref="FormatException">Thrown if the price string cannot be parsed into a decimal.</exception>
        public decimal ParsePriceStringToDecimal(string priceText)
        {
            string cleaned = priceText.Replace("TL", "").Trim();
            cleaned = cleaned.Replace(".", "");
            cleaned = cleaned.Replace(",", ".");
            if (decimal.TryParse(cleaned, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"Could not parse price text: {priceText}");
            }
        }

        /// <summary>
        /// Adds a product to the cart by clicking 'Add' button, selecting first available size,
        /// and clicking 'Complete Order' button if present.
        /// </summary>
        public void AddProductToCartWithSize()
        {
            TestContext.WriteLine("[INFO] Starting process to add product to cart.");

            // Step 1: Click 'Add' button if visible
            if (IsVisible(AddButtonSelector))
            {
                TestContext.WriteLine("[INFO] 'Ekle' button found, clicking it.");
                SafeClick(AddButtonSelector);

                // Step 2: Wait for size options to appear
                TestContext.WriteLine("[INFO] Waiting for size selector buttons to appear.");
                bool sizeOptionsVisible = WaitUntilVisibleOrTimeout(SizeOptionsEnabledSelector, TimeSpan.FromSeconds(15));
                if (!sizeOptionsVisible)
                {
                    TestContext.WriteLine("[WARN] Size options did not appear within timeout.");
                    return;
                }

                // Step 3: Select first enabled size
                var sizes = driver.FindElements(SizeOptionsEnabledSelector);
                var firstActiveSize = sizes.FirstOrDefault(s => s.Displayed && s.Enabled);

                if (firstActiveSize != null)
                {
                    TestContext.WriteLine($"[INFO] Selecting first available size: '{firstActiveSize.Text}'.");

                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", firstActiveSize);

                    try
                    {
                        firstActiveSize.Click();
                    }
                    catch (Exception ex)
                    {
                        TestContext.WriteLine($"[WARN] Normal click failed: {ex.Message}, trying JS click.");
                        try
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", firstActiveSize);
                        }
                        catch (Exception jsEx)
                        {
                            TestContext.WriteLine($"[ERROR] JS click also failed: {jsEx.Message}");
                            return;
                        }
                    }
                }
                else
                {
                    TestContext.WriteLine("[WARN] No active size button found, skipping size selection.");
                }

                // Step 4: Wait a bit before checking for 'Complete Order' button
                TestContext.WriteLine("[INFO] Waiting 5 seconds before checking 'Complete Order' button.");
                System.Threading.Thread.Sleep(5000);

                // Step 5: Click 'Complete Order' button if available
                if (IsVisible(CompleteOrderButton))
                {
                    TestContext.WriteLine("[INFO] 'Complete Order' button found, clicking it.");
                    SafeClick(CompleteOrderButton);
                }
                else
                {
                    TestContext.WriteLine("[WARN] 'Complete Order' button not found after adding to cart.");
                }
            }
            else
            {
                TestContext.WriteLine("[WARN] 'Ekle' button not found, skipping add to cart.");
                return;
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

