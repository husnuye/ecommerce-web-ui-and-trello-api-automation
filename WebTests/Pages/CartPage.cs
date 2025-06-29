using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WebTests.Pages
{
    /// <summary>
    /// Handles all shopping cart related operations.
    /// </summary>
    public class CartPage : BasePage
    {
        private readonly By CartIcon = By.CssSelector("a[data-testid='cart-link']");
        private readonly By CartPrice = By.CssSelector(".price__amount");
        private readonly By QuantitySelect = By.CssSelector("select.quantity-selector");
        private readonly By RemoveButton = By.CssSelector("button[data-testid='remove-item']");
        private readonly By EmptyCartMessage = By.XPath("//p[contains(text(), 'Sepetinizde ürün bulunmamaktadır')]");

        public CartPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Opens the shopping cart.
        /// </summary>
        public void OpenCart() => Click(CartIcon);

        /// <summary>
        /// Retrieves the total cart price.
        /// </summary>
        public string GetCartPrice() => WaitAndFind(CartPrice).Text.Trim();

        /// <summary>
        /// Changes product quantity to the specified value.
        /// </summary>
        public void ChangeQuantity(string quantity)
        {
            var dropdown = new SelectElement(WaitAndFind(QuantitySelect));
            dropdown.SelectByText(quantity);
        }

        /// <summary>
        /// Returns the currently selected quantity.
        /// </summary>
        public string GetSelectedQuantity()
        {
            var dropdown = new SelectElement(WaitAndFind(QuantitySelect));
            return dropdown.SelectedOption.Text.Trim();
        }

        /// <summary>
        /// Removes the product from the cart.
        /// </summary>
        public void RemoveProduct() => Click(RemoveButton);

        /// <summary>
        /// Checks if the cart is empty.
        /// </summary>
        public bool IsCartEmpty() => IsVisible(EmptyCartMessage);
    }
}