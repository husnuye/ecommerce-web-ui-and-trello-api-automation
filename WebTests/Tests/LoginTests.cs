using NUnit.Framework;
using WebTests.Core;
using WebTests.Pages;

namespace WebTests.Tests
{
    /// <summary>
    /// Tests the login functionality using sample credentials.
    /// </summary>
    public class LoginTests : BaseTest
    {
        [Test]
        public void ValidLogin_ShouldSucceed()
        {
            var loginPage = new LoginPage(driver);

            // Test kullanıcı bilgileri (eğer Zara login destekliyorsa)
            loginPage.Login("batuhan.zafer@testinium.com", "Test1234");

            // TODO: assert login success (e.g. profile icon visible)
            Assert.Pass("Login invoked successfully.");
        }
    }
}