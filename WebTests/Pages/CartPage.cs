using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Globalization;

namespace WebTests.Pages
{
    /// <summary>
    /// Handles all shopping cart related operations.
    /// </summary>
    public class CartPage : BasePage
    {
        private readonly By CartIcon = By.CssSelector("a[data-testid='cart-link']");
        private readonly By CartPrice = By.CssSelector(".price__amount"); // Sepetteki ürün fiyatının selectorü
        private readonly By QuantitySelect = By.CssSelector("select.quantity-selector"); // Adet dropdown
        private readonly By RemoveButton = By.CssSelector("button[data-testid='remove-item']");
        private readonly By EmptyCartMessage = By.XPath("//p[contains(text(), 'Sepetinizde ürün bulunmamaktadır')]");

        private readonly By IncreaseButton = By.CssSelector("div.zds-quantity-selector__increase[data-ga-id='add-order-item-unit']");
        private readonly By QuantityInput = By.CssSelector("input.zds-quantity-selector__units-input");
        private readonly By CartPopup = By.CssSelector("div.add-to-cart-notification-content");



        public CartPage(IWebDriver driver) : base(driver) { }



        /// <summary>
        /// Returns the product price displayed in the cart.
        /// </summary>
        public string GetCartPrice()
        {
            var priceElement = WaitAndFind(CartPrice);
            string priceText = priceElement.Text.Trim();
            TestContext.WriteLine($"[INFO] Cart product price found: {priceText}");
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
            // Split multiline text into individual lines
            var lines = priceText
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Select the last line that contains a TL price (usually the discounted one)
            var priceLine = lines
                .Where(line => line.Contains("TL"))
                .LastOrDefault();

            if (priceLine == null)
                throw new FormatException("Price line could not be found.");

            // Clean the price string: remove thousand separators, replace comma with dot, strip currency
            var cleaned = priceLine
                .Replace(".", "")
                .Replace(",", ".")
                .Replace(" TL", "")
                .Trim();

            // Parse the cleaned string into a decimal
            return decimal.Parse(cleaned, CultureInfo.InvariantCulture);
        }

        //
        /// <summary>
        /// Increases the quantity of a product in the cart by clicking the '+' button required number of times.
        /// Scrolls to the button, hovers over it, waits for overlay to disappear, and uses JS fallback if needed.
        /// </summary>
        public void ChangeQuantity(string quantity)
        {
            var plusButtonSelector = By.CssSelector("div.zds-quantity-selector__increase");
            int maxTries = 8; // Maximum scroll attempts
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var actions = new OpenQA.Selenium.Interactions.Actions(driver);

            try
            {
                WaitUntilPageLoad();
                WaitUntilInvisible(CartPopup, 10);

                IWebElement plusButtonElement = null;

                for (int i = 0; i < maxTries; i++)
                {
                    try
                    {
                        // Find the increase button inside the first cart item
                        var cartItems = driver.FindElements(By.CssSelector("div.shop-cart-item_info"));
                        if (cartItems.Count > 0)
                        {
                            plusButtonElement = cartItems[0].FindElement(plusButtonSelector);
                        }
                        else
                        {
                            // If no cart items found, try global search
                            plusButtonElement = driver.FindElement(plusButtonSelector);
                        }

                        // Scroll the element into view using JavaScript
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", plusButtonElement);

                        // Wait until the element is visible and enabled (clickable)
                        wait.Until(d => plusButtonElement.Displayed && plusButtonElement.Enabled);

                        break; // Element found, exit loop
                    }
                    catch (NoSuchElementException)
                    {
                        // Element not found, scroll down a bit and retry
                        actions.ScrollByAmount(0, 50).Perform();
                        Thread.Sleep(200);
                    }
                    catch (WebDriverTimeoutException)
                    {
                        // Element not visible yet, scroll and retry
                        actions.ScrollByAmount(0, 50).Perform();
                        Thread.Sleep(200);
                    }
                }

                if (plusButtonElement == null || !plusButtonElement.Displayed)
                {
                    TestContext.WriteLine($"[ERROR] '+' button still not visible after scrolling.");
                    return;
                }

                // Try to get the current quantity value
                int currentQty = 1;
                try
                {
                    var qtySpan = plusButtonElement.FindElement(By.XPath("../preceding-sibling::span[contains(@class,'zds-quantity-selector__quantity')]"));
                    currentQty = int.Parse(qtySpan.Text.Trim());
                }
                catch
                {
                    // If reading fails, assume default quantity = 1
                }

                int desiredQty = int.Parse(quantity);
                int timesToClick = desiredQty - currentQty;

                if (timesToClick <= 0)
                {
                    TestContext.WriteLine($"[INFO] No quantity change needed. Current: {currentQty}, Desired: {desiredQty}");
                    return;
                }

                for (int i = 0; i < timesToClick; i++)
                {
                    try
                    {
                        plusButtonElement.Click();
                    }
                    catch (Exception)
                    {
                        // If normal click fails, fallback to JS click
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", plusButtonElement);
                    }
                    Thread.Sleep(300);
                }

                TestContext.WriteLine($"[INFO] Quantity successfully increased to {quantity}");
            }
            catch (NoSuchElementException)
            {
                TestContext.WriteLine("[ERROR] '+' button not found — selector may be invalid or button not rendered.");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"[ERROR] Failed to change quantity to {quantity}: {ex.Message}");
            }
        }
        /// <summary>
        /// Waits until the page is fully loaded (document.readyState === 'complete').
        /// </summary>
        private void WaitUntilPageLoad(int timeoutInSeconds = 15)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(d =>
                ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete"
            );
        }

        /// <summary>
        /// Checks if an element is present within the specified timeout (in seconds).
        /// </summary>
        private bool IsElementPresent(By by, int timeoutInSeconds)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(drv => drv.FindElements(by).Count > 0);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Scrolls the element into view to ensure it is visible.
        /// </summary>
        private void EnsureElementVisible(By by)
        {
            var element = driver.FindElement(by);
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
        }

        /// <summary>
        /// Returns the currently selected quantity in the cart.
        /// </summary>
        public string GetSelectedQuantity()
        {
            // Parent div içinde input elementi bulunuyor, miktar value attribute’unda
            var quantityInput = WaitAndFind(By.CssSelector("div.zds-quantity-selector input.zds-quantity-selector__units"));

            // Input elementinin value attribute değerini al
            string selected = quantityInput.GetAttribute("value").Trim();

            TestContext.WriteLine($"[INFO] Current selected quantity: {selected}");

            return selected;
        }
        /// <summary>
        /// Removes the product from the cart by clicking the remove button.
        /// Waits until the remove button is clickable before clicking.
        /// Handles fallback JS click if normal click fails.
        /// </summary>
        public void RemoveProduct()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Wait until the remove button is clickable
            var removeBtn = wait.Until(driver =>
            {
                var element = driver.FindElement(By.CssSelector("button[aria-label='Ürünü sil']"));
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            });

            try
            {
                // Try to click normally
                removeBtn.Click();
            }
            catch (ElementNotInteractableException)
            {
                // Fallback: JavaScript click if normal click fails
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click();", removeBtn);
            }

            TestContext.WriteLine("[INFO] Remove button clicked.");
        }
        /// <summary>
        /// Checks if the cart is empty by verifying the empty cart message.
        /// </summary>

        // Locatorlar
        By emptyCartTitle = By.XPath("//div[contains(@class,'shop-cart-view_empty-state')]//span[contains(@class,'zds-empty-state__title')]");

        private readonly By EmptyCartTitle = By.XPath("//div[contains(@class,'shop-cart-view_empty-state')]//span[contains(@class,'zds-empty-state__title')]");

        // In CartPage.cs
        //private readonly By EmptyCartTitle = By.CssSelector("span.zds-empty-state__title");

        public void ScrollToEmptyCartSection()
        {
            var parentElement = driver.FindElement(By.CssSelector("div.shop-cart-view__empty-state"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", parentElement);
        }
        public bool IsCartEmpty()
        {
            try
            {
                var emptyTitleElement = driver.FindElement(EmptyCartTitle);
                string text = emptyTitleElement.Text.Trim();
                return text.Equals("SEPETİNİZ BOŞ", StringComparison.OrdinalIgnoreCase)
                       || text.Equals("Your cart is empty", StringComparison.OrdinalIgnoreCase);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }


    }
}