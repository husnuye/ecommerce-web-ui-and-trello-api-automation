using OpenQA.Selenium;

namespace WebTests.Pages
{
    /// <summary>
    /// Handles keyword search functionality.
    /// </summary>
    public class SearchPage : BasePage
    {
        private readonly By SearchInput = By.CssSelector("input[type='search']");

        public SearchPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Inputs a keyword and submits the search.
        /// </summary>
        public void Search(string keyword)
        {
            Type(SearchInput, keyword);
            ExecuteScript("arguments[0].dispatchEvent(new KeyboardEvent('keydown', {'key':'Enter'}));", WaitAndFind(SearchInput));
        }

        /// <summary>
        /// Clears the search input field.
        /// </summary>
        public void ClearSearch()
        {
            WaitAndFind(SearchInput).Clear();
        }
    }
}