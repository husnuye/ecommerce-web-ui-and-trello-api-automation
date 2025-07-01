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



        /// <summary>
        /// Selects the first product from the search result list.
        /// </summary>
        public void SelectFirstProduct()
        {
            WaitUntilVisible(ProductList);
            var products = WaitAndFindAll(ProductList);

            if (products.Count == 0)
                throw new Exception("No products found in search results.");

            var firstProduct = products.First();
            ScrollToElement(firstProduct);
            firstProduct.Click();

            TestContext.WriteLine("[INFO] First product in search results clicked.");

        
    }
}
 }