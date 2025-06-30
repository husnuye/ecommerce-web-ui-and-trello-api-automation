using OpenQA.Selenium;
using NUnit.Framework;

namespace WebTests.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        private readonly By EmailInput = By.CssSelector("input[autocomplete='email']");
        private readonly By PasswordInput = By.CssSelector("input[type='password']");
        private readonly By SubmitButton = By.CssSelector("button[data-qa-id='logon-form-submit']");

        public void Login(string email, string password)
        {
            TestContext.WriteLine("🔐 LoginPage.Login STARTED");
            TestContext.WriteLine("Active window title: " + driver.Title);
            TestContext.WriteLine("Active window handles: " + string.Join(",", driver.WindowHandles));

            WaitUntilVisible(EmailInput);
            Type(EmailInput, email);
            Thread.Sleep(1000);

            WaitUntilVisible(PasswordInput);
            Type(PasswordInput, password);

            TestContext.WriteLine("Before submit: email and password fields filled.");
            Thread.Sleep(3000);

            SafeClick(SubmitButton);

            Thread.Sleep(3000);
            TestContext.WriteLine("After submit: submit button clicked.");
            TestContext.WriteLine("✅ Login form submitted.");
        }
    }
}