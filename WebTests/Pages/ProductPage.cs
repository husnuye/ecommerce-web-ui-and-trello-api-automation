using OpenQA.Selenium;
using System;
using System.Linq;

namespace WebTests.Pages
{
    /// <summary>
    /// Represents product listing and detail page interactions.
    /// </summary>
    public class ProductPage : BasePage
    {
        private readonly By ProductLinks = By.CssSelector("a.product-link");

        private readonly By ProductName = By.CssSelector(".product-detail-name");
        private readonly By ProductPrice = By.CssSelector(".product-detail-price");
        private readonly By AddToCartButton = By.CssSelector("button.add-to-cart");


        private readonly Random random = new Random();

        public ProductPage(IWebDriver driver) : base(driver) { }



        /// <summary>
        /// Retrieves the selected product's name.
        /// </summary>
        /// <returns>Product name as string</returns>
        public string GetProductName()
        {
            return WaitAndFind(ProductName).Text;
        }

        /// <summary>
        /// Retrieves the selected product's price.
        /// </summary>
        /// <returns>Product price as string</returns>
        public string GetProductPrice()
        {
            return WaitAndFind(ProductPrice).Text;
        }

        /// <summary>
        /// Clicks the "Add to Cart" button for the selected product.
        /// </summary>
        public void AddProductToCart()
        {
            SafeClick(AddToCartButton);
            TestContext.WriteLine("[INFO] Add to cart button clicked.");
        }
    }
}