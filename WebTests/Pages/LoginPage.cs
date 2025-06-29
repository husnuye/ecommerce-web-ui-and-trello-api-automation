using OpenQA.Selenium;

namespace WebTests.Pages
{
    /// <summary>
    /// Handles login functionality for Zara.
    /// </summary>
    public class LoginPage : BasePage
    {
        private readonly By EmailInput = By.Id("logonId");
        private readonly By PasswordInput = By.Id("logonPassword");
        private readonly By LoginButton = By.CssSelector("button[data-testid='login-button']");

        public LoginPage(IWebDriver driver) : base(driver) { }

        /// <summary>
        /// Performs login using provided credentials.
        /// </summary>
        public void Login(string email, string password)
        {
            Type(EmailInput, email);
            Type(PasswordInput, password);
            Click(LoginButton);
        }
    }
}