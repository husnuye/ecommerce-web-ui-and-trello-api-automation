using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

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

        public CartPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Opens the shopping cart by clicking the cart icon.
        /// </summary>
        public void OpenCart()
        {
            Click(CartIcon);
            TestContext.WriteLine("[INFO] Shopping cart opened.");
        }

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
        /// Changes product quantity to the specified value.
        /// </summary>
        public void ChangeQuantity(string quantity)
        {
            var dropdown = new SelectElement(WaitAndFind(QuantitySelect));
            dropdown.SelectByText(quantity);
            TestContext.WriteLine($"[INFO] Changed product quantity to: {quantity}");
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