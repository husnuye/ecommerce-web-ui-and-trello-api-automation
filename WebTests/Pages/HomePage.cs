using OpenQA.Selenium;

namespace WebTests.Pages
{
    /// <summary>
    /// Represents home page navigation actions.
    /// </summary>
    public class HomePage : BasePage
    {
        private readonly By MenMenu = By.CssSelector("a[aria-label='Erkek']");
        private readonly By SeeAll = By.XPath("//a[contains(text(), 'TÜMÜNÜ GÖR')]");

        public HomePage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Navigates to the men's section by hover and click.
        /// </summary>
        public void NavigateToMenSection()
        {
            Hover(MenMenu);
            Click(SeeAll);
        }
    }
}