using NUnit.Framework;
using WebTests.Core;
using WebTests.Pages;
using WebTests.Utils;
using WebTests.Config;
using System.IO;
using log4net;

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
  
  /*
                // TEST Block
                driver.Navigate().GoToUrl(ConfigurationReader.BaseUrl + "/tr/tr/s-erkek-indirim-l10847.html?v1=2439352");
                Logger.Info("Navigated directly to men's discount page.");
                */
                
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

                /*
                                // Step 11: Assert that search results are displayed
                                Assert.That(searchPage.IsSearchResultDisplayed(), Is.True, "Search results are not displayed on the page!");
                                Logger.Info("Search result grid is visible and search succeeded."); */


                
                // Step 8: A random product from the listed search results is selected.

                //  Select a random product from search results
                var productPage = new ProductPage(driver);
                productPage.SelectRandomProduct();
                Logger.Info("Random product selected from search results.");
               
               
                // Step 9: The product name and price information are written to a .txt file.

                // Get product name and price, save to .txt file
                string name = productPage.GetProductName();
                string price = productPage.GetProductPrice();
                FileHelper.SaveText(ConfigurationReader.OutputPath, $"Product: {name}\nPrice: {price}");
                Logger.Info($"Product name and price saved to txt. Product: {name}, Price: {price}");


                // Step 10: The product is added to the cart.

                // Add product to cart
                productPage.AddProductToCart();
                Logger.Info("Step 13: Product added to cart.");
                

                // Step 11: The product price on the product page is compared with the cart price.
                
                // Go to cart and check product price matches
                var cartPage = new CartPage(driver);
                cartPage.OpenCart();
                string cartPrice = cartPage.GetCartPrice();
                Assert.That(cartPrice, Is.EqualTo(price), "Cart price does not match product price");
                Logger.Info("Verified product price in cart matches product page.");


                // senaryo 12 : The product quantity in the cart is changed to 2 and verified.

                // Change quantity to 2, verify
                cartPage.ChangeQuantity("2");
                Assert.That(cartPage.GetSelectedQuantity(), Is.EqualTo("2"), "Cart quantity does not match selected value");
                Logger.Info("Changed cart quantity to 2 and verified.");

                
                // Step 13: The product is removed from the cart and it is verified that the cart is empty.
                

                // Remove product, verify cart is empty
                cartPage.RemoveProduct();
                Assert.That(cartPage.IsCartEmpty(), Is.True, "Cart is not empty after removal.");
                Logger.Info("Removed product from cart. Verified cart is empty.");

                 /*// Step 16: Log out at the end of the test to ensure clean state for next tests
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