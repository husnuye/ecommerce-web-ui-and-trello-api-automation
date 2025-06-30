using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;  // TestContext için ekledim

namespace WebTests.Pages
{
    /// <summary>
    /// Handles keyword search functionality on Zara's search page.
    /// </summary>
    public class SearchPage : BasePage
    {
        // Use unique ID selector for the search input for maximum reliability
        private readonly By SearchInput = By.Id("search-home-form-combo-input");

        // Use CSS selectors for product-related actions
        private readonly By ProductList = By.CssSelector("ul.product-grid_product-list > li");

        private readonly By ProductName = By.CssSelector(".product-detail-name");
        private readonly By ProductPrice = By.CssSelector(".product-detail-price");
        private readonly By AddToCartButton = By.CssSelector("button.add-to-cart");

        public SearchPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Inputs a keyword into the search box and submits the search with Enter.
        /// </summary>
        /// <param name="keyword">The keyword to search for.</param>
        public void Search(string keyword)
        {
            var input = WaitAndFind(SearchInput);
            input.Clear();
            input.SendKeys(keyword);

            TestContext.WriteLine($"[INFO] Search input: '{keyword}' entered .");
        }
        /// <summary>
        /// Clears the search input field using JavaScript to ensure full reset.
        /// This handles cases where standard .Clear() does not properly trigger input events.
        /// </summary>
        public void ClearSearch()
        {
            var input = WaitAndFind(SearchInput);

            // Clear the input value directly using JavaScript
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = '';", input);

            // Trigger the 'input' event to notify any JS listeners on the page
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));", input);

            // Brief pause to allow the UI to update
            Thread.Sleep(2000);

            TestContext.WriteLine("[INFO] Search input cleared via JavaScript.");
        }

        /// <summary>
        /// Submits the search input by pressing the Enter key.
        /// </summary>
        public void PressEnterOnSearch()
        {
            var input = WaitAndFind(SearchInput);
            input.SendKeys(Keys.Enter);
            TestContext.WriteLine("[INFO] Enter key pressed in search input (search submitted).");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(driver => driver.Url.Contains("section=MAN"));

            // Assertion ile kontrol et
            Assert.That(driver.Url.Contains("section=MAN"), "Search did not stay in MAN section.");

        }



        // <summary>
        /// Selects a random product from the product list.
        /// </summary>
        public void SelectRandomProduct()
        {
            // Wait until the product list is visible
            WaitUntilVisible(ProductList);

            // Find the container of product list again to avoid stale elements
            var products = WaitAndFindAll(ProductList);

            if (products.Count == 0)
                throw new Exception("No products found on search results.");

            // Randomly pick a product index
            var rnd = new Random();
            var chosenIndex = rnd.Next(products.Count);

            // Re-fetch the product before clicking to avoid stale references
            var refreshedProducts = WaitAndFindAll(ProductList);
            var chosenProduct = refreshedProducts[chosenIndex];

            ScrollToElement(chosenProduct);  // Optional: to ensure visibility
            chosenProduct.Click();

            TestContext.WriteLine($"[INFO] Random product clicked. Index: {chosenIndex}");
        }

        /// <summary>
        /// Scrolls the page to bring the specified element into view.
        /// </summary>
        /// <param name="element">The web element to scroll to.</param>
        private void ScrollToElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }


        /// <summary>
        /// Waits for and finds all elements matching the given selector.
        /// </summary>
        /// <param name="by">The selector to find elements.</param>
        /// <returns>List of IWebElement</returns>
        private IList<IWebElement> WaitAndFindAll(By by)
        {
            // You may want to use WebDriverWait for better reliability
            return driver.FindElements(by);
        }

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
