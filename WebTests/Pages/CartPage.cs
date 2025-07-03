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
    var plusButton = By.CssSelector("div.zds-quantity-selector__increase[data-ga-id='add-order-item-unit']");

    try
    {
        WaitUntilPageLoad(); // Sayfa yüklensin
        WaitUntilInvisible(CartPopup, 10); // Sepete eklendi popup kalksın
        WaitUntilVisible(plusButton, 10);  // Artı buton görünene kadar bekle

        int timesToClick = int.Parse(quantity) - 1;
        if (timesToClick <= 0)
        {
            TestContext.WriteLine($"[INFO] No quantity change needed. Desired quantity: {quantity}");
            return;
        }

        for (int i = 0; i < timesToClick; i++)
        {
            var plusButtonElement = WaitAndFind(plusButton);
            ForceClickWithScrollAndHover(plusButtonElement); // Scroll + hover + click
            Thread.Sleep(300);
        }

        TestContext.WriteLine($"[INFO] Quantity successfully increased to {quantity}");
    }
    catch (NoSuchElementException)
    {
        TestContext.WriteLine($"[ERROR] '+' button not found — selector may be invalid or button not rendered.");
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
            wait.Until(drv =>
                ((IJavaScriptExecutor)drv).ExecuteScript("return document.readyState").ToString() == "complete"
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
            var dropdown = new SelectElement(WaitAndFind(QuantitySelect));
            string selected = dropdown.SelectedOption.Text.Trim();
            TestContext.WriteLine($"[INFO] Current selected quantity: {selected}");
            return selected;
        }

        /// <summary>
        /// Removes the product from the cart.
        /// </summary>
        public void RemoveProduct()
        {
            Click(RemoveButton);
            TestContext.WriteLine("[INFO] Product removed from the cart.");
        }

        /// <summary>
        /// Checks if the cart is empty by verifying the empty cart message.
        /// </summary>
        public bool IsCartEmpty()
        {
            bool empty = IsVisible(EmptyCartMessage);
            TestContext.WriteLine($"[INFO] Is cart empty: {empty}");
            return empty;
        }
    }
}