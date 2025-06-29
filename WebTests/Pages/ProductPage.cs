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
        private readonly By ProductName = By.CssSelector("h1.product-name");
        private readonly By ProductPrice = By.CssSelector(".price__amount");
        private readonly By AddToCart = By.CssSelector("button[data-testid='add-to-cart']");

        private readonly Random random = new Random();

        public ProductPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Selects a random product from search results.
        /// </summary>
        public void SelectRandomProduct()
        {
            var products = driver.FindElements(ProductLinks).ToList();
            if (products.Count == 0)
                throw new Exception("No products found");

            var selected = products[random.Next(products.Count)];
            ScrollTo(ProductLinks);
            selected.Click();
        }

        /// <summary>
        /// Gets product name from detail page.
        /// </summary>
        public string GetProductName() => WaitAndFind(ProductName).Text.Trim();

        /// <summary>
        /// Gets product price from detail page.
        /// </summary>
        public string GetProductPrice() => WaitAndFind(ProductPrice).Text.Trim();

        /// <summary>
        /// Adds product to cart.
        /// </summary>
        public void AddProductToCart() => Click(AddToCart);
    }
}