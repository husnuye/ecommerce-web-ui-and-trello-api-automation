using NUnit.Framework;
using WebTests.Core;
using WebTests.Pages;
using WebTests.Utils;
using WebTests.Config;
using System.IO;

namespace WebTests.Tests
{
    /// <summary>
    /// Tests the complete shopping flow on Zara.com.
    /// </summary>
    public class ShoppingFlowTests : BaseTest
    {
        [Test]
        public void CompleteShoppingFlow()
        {
            var homePage = new HomePage(driver);
            var searchPage = new SearchPage(driver);
            var productPage = new ProductPage(driver);
            var cartPage = new CartPage(driver);

            // Navigate to men's section
            homePage.NavigateToMenSection();

            // Load search keywords from Excel
            var keywords = ExcelReader.ReadFirstRow(ConfigurationReader.ExcelPath);
            string keyword1 = keywords[0];
            string keyword2 = keywords[1];

            // First search (e.g. "şort")
            searchPage.Search(keyword1);
            searchPage.ClearSearch();

            // Second search (e.g. "gömlek")
            searchPage.Search(keyword2);

            // Select a random product
            productPage.SelectRandomProduct();

            // Capture product info
            string name = productPage.GetProductName();
            string price = productPage.GetProductPrice();

            // Save to .txt
            FileHelper.SaveText(ConfigurationReader.OutputPath, $"Ürün: {name}\nFiyat: {price}");

            // Add to cart
            productPage.AddProductToCart();

            // Open cart and verify price
            cartPage.OpenCart();
            string cartPrice = cartPage.GetCartPrice();
            Assert.That(cartPrice, Is.EqualTo(price), "Cart price does not match product price");

            // Change quantity and validate
            cartPage.ChangeQuantity("2");
            Assert.That(cartPage.GetSelectedQuantity(), Is.EqualTo("2"));

            // Remove product and verify empty cart
            cartPage.RemoveProduct();
            Assert.IsTrue(cartPage.IsCartEmpty(), "Cart is not empty after removal");
        }
    }
}