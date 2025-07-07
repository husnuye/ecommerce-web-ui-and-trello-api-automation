using NUnit.Framework;
using WebTests.Core;
using WebTests.Pages;
using WebTests.Utils;
using WebTests.Config;
using System.IO;
using log4net;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace WebTests.Tests
{
    /// <summary>
    /// Complete E2E shopping flow test for Zara.com (login, search, add to cart, quantity change, remove).
    /// </summary>
    public class ZaraFullE2ETests : BaseTest
    {
        // log4net logger instance
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ZaraFullE2ETests));

        [Test]
        public void Zara_FullEndToEndFlow_ShouldSucceed()
        {
            try
            {
                Logger.Info("Test started: Zara_FullEndToEndFlow_ShouldSucceed");


                // Step 1: Open homepage

                //  Open home page and accept cookies if needed
                var homePage = new HomePage(driver);
                homePage.AcceptCookiesIfPresent();
                Logger.Info("Home page loaded and cookies accepted (if any).");


                // Step 2: Login

                // Go to login - open login page with hover if needed
                homePage.ClickLoginButtonWithHover();
                Logger.Info("Login button clicked.");

                // Click "GİRİŞ YAP" button on landing, if present
                var loginLanding = new LoginLandingPage(driver);
                loginLanding.ClickLoginFormButton();
                Logger.Info("'GİRİŞ YAP' button clicked on landing page.");

                // Login with valid credentials
                var loginPage = new LoginPage(driver);
                loginPage.Login(ConfigurationReader.Email, ConfigurationReader.Password);
                Logger.Info("Login form filled and submitted.");

                //  Assert user is logged in (profile icon visible, etc.)
                Assert.That(homePage.IsLoggedIn(), Is.True, "Login failed: User profile is not visible.");
                Logger.Info("Login success verified. User is logged in.");


                // Step 3: Navigate to Men → Tümünü Gör

                //  Navigate to Men's section after login
                Logger.Info("Navigating to 'Men' section from order page...");
                homePage.NavigateToMenSectionFromOrderPage();
                Logger.Info("Successfully navigated to 'Men' section.");

                //  Click "TÜMÜNÜ GÖR" link if it is visible
                Logger.Info("Attempting to click 'TÜMÜNÜ GÖR' link if present...");
                homePage.ClickSeeAllIfPresent();
                Logger.Info("'TÜMÜNÜ GÖR' link clicked if it was available.");





                // Step 4:  The word “şort” from the 1st row, 1st column of the Excel file is typed into the search box.

                // Open search input (header search)
                Logger.Info("Opening search input via header...");
                homePage.OpenSearchBox();
                Logger.Info("Search input successfully opened.");

                //  Read search keywords from Excel

                Logger.Info("Starting keyword search sequence from Excel data...");
                List<string> keywords = null;
                string keyword1 = null;
                string keyword2 = null;

                try
                {
                    // Build absolute path to Excel file
                    var excelPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationReader.ExcelPath);
                    Logger.Info($"Excel path: {excelPath}");
                    Console.WriteLine($"[DEBUG] Excel path: {excelPath}");

                    // Check if Excel file exists
                    if (!File.Exists(excelPath))
                    {
                        var msg = $"Excel file not found at path: {excelPath}";
                        Logger.Error(msg);
                        Console.WriteLine($"[ERROR] {msg}");
                        Assert.Fail(msg);
                    }

                    // Read first row keywords from Excel file
                    keywords = ExcelReader.ReadFirstRow(excelPath);

                    if (keywords == null || keywords.Count < 2)
                        throw new Exception("Excel file does not contain enough keywords!");

                    keyword1 = keywords[0];
                    keyword2 = keywords[1];

                    Logger.Info($"Loaded keywords from Excel: '{keyword1}', '{keyword2}'");
                    Console.WriteLine($"[DEBUG] Loaded from Excel: {keyword1}, {keyword2}");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error loading keywords from Excel: {ex.Message}");
                    Console.WriteLine($"[ERROR] {ex.Message}");
                    Assert.Fail("Excel reading failed. Test aborted.");
                }

                // Step 5 : The word “şort” in the search box is cleared.


                // Search for the first keyword and then clear the input
                var searchPage = new SearchPage(driver);
                searchPage.Search(keyword1);
                Logger.Info($"Searched for keyword: '{keyword1}'");
                searchPage.ClearSearch();
                Logger.Info("Cleared first search input.");
                // Add wait before submitting search
                Thread.Sleep(1500); // give time for suggestion box or search load




                // Step 6: The word “gömlek” from the 1st row, 2nd column of the Excel file is typed into the search box.

                // Search for second keyword
                searchPage.Search(keyword2);
                Logger.Info($"Searched for keyword: '{keyword2}'");
                // Add wait before submitting search
                Thread.Sleep(1500); // give time for suggestion box or search load


                //  Step 7 :The Enter key is pressed (keyboard simulation).

                // Click Enter to submit the search
                searchPage.PressEnterOnSearch();
                Logger.Info($"Searched for keyword: '{keyword2}' and submitted the search.");


                //Step 11: Assert that search results are displayed
                // Assert.That(searchPage.IsSearchResultDisplayed(), Is.True, "Search results are not displayed on the page!");
                //Logger.Info("Search result grid is visible and search succeeded."); 



                // Step 8: The first product from the listed search results is selected.

                // Ardından arama sayfasına yönlendirme


                searchPage.SelectFirstProduct();
                Logger.Info("First product selected from search results.");





                /*  // TEST Block
                   driver.Navigate().GoToUrl("https://www.zara.com/tr/tr/dokulu-orgu-t-shirt-p03003401.html?v1=415381591");
                   Logger.Info("Navigated directly to product detail page.");
                   Logger.Info("Login successful.");

                   // Sayfa tam yüklenene kadar bekle
                   //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                   // wait.Until(drv => ((IJavaScriptExecutor)drv).ExecuteScript("return document.readyState").Equals("complete"));

                */

                // Create ProductPage instance after navigating to product detail
                var productPage = new ProductPage(driver);


                // Step 9: The product name and price information are written to a .txt file.

                // Get product name and price, save to .txt file

                string productName = productPage.GetProductName();
                string productPrice = productPage.GetProductPrice();

                string productInfo = $"Product: {productName}\nPrice: {productPrice}";
                FileHelper.SaveText(ConfigurationReader.OutputPath, productInfo);

                Logger.Info($"[INFO] Product details saved to text file.\n{productInfo}");


                // Step 10: The product is added to the cart.

                // Step 10: The product is added to the cart with detailed steps.

                productPage.AddProductToCartWithSize();


                Logger.Info("[STEP 10.1] 'Ekle' button clicked.");

                Logger.Info("[STEP 10.2] Size selector button clicked.");

                Logger.Info("[STEP 10.3] Smart Size popup handled if present.");

                Logger.Info("[STEP 10.4] First available size selected.");

                Logger.Info("[STEP 10.5] 'Complete Order' button clicked.");

                // Step 10 completed successfully
                Logger.Info("Step 10: Product added to cart successfully.");

                // Step 11: The product price on the product page is compared with the cart price.

                // Go to cart and check product price matches



                //test block




                var cartPage = new CartPage(driver);

                // Get cart price from cart page
                string cartPrice = cartPage.GetCartPrice();

                // Parse product page and cart prices to decimals for comparison
                decimal productPagePriceDecimal = cartPage.ParsePriceStringToDecimal(productPrice);
                decimal cartPriceDecimal = cartPage.ParsePriceStringToDecimal(cartPrice);

                // Log comparison result
                if (cartPriceDecimal == productPagePriceDecimal)
                {
                    Logger.Info($"[PASS] Cart price matches product page price. Product Page: {productPrice}, Cart: {cartPrice}");
                }
                else
                {
                    Logger.Warn($"[WARN] Price mismatch. Product Page: {productPrice}, Cart: {cartPrice}");
                }

                // Step 12: Change the product quantity in the cart to 2 and verify

                // Change quantity to 2
                cartPage.ChangeQuantity("2");

                // Explicit wait to ensure quantity is updated in DOM
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(driver => cartPage.GetSelectedQuantity() == "2");

                // Get selected quantity after change
                string selectedQty = cartPage.GetSelectedQuantity();

                // Assert the quantity is correctly updated
                Assert.That(selectedQty, Is.EqualTo("2"), "Cart quantity does not match selected value");
                Logger.Info("[CHECK] Quantity changed to 2 and verified.");

                // Step 13: The product is removed from the cart and it is verified that the cart is empty.


                // Remove product, verify cart is empt

                cartPage.RemoveProduct();
                Thread.Sleep(3000); // Sadece debug için 3 saniye beklet


                // Step 13: Verify that the cart is empty
                bool isCartEmpty = cartPage.IsCartEmptyAfterRemovingProduct();

                Assert.That(isCartEmpty, Is.True, "Expected empty cart message, but it was not found.");
                Logger.Info("[CHECK] Empty cart message has been verified.");
                /*// Step 14: Log out at the end of the test to ensure clean state for next tests
                var logoutPage = new LogoutPage(driver);
                logoutPage.Logout();
                Logger.Info("Logged out successfully. Test completed.");*/

                Logger.Info("Test completed successfully: Zara_FullEndToEndFlow_ShouldSucceed");
            }
            catch (Exception ex)
            {
                Logger.Error("Test failed with exception: " + ex.Message, ex);
                throw;
            }
        }
    }
}