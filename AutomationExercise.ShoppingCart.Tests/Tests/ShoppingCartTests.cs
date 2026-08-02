using AutomationExercise.ShoppingCart.Tests.Base;
using AutomationExercise.ShoppingCart.Tests.Models;
using AutomationExercise.ShoppingCart.Tests.Pages;
using System.Text.RegularExpressions;

namespace AutomationExercise.ShoppingCart.Tests.Tests
{
    public class ShoppingCartTests : BaseTest
    {
        [Test]
        public void User_Should_Add_Two_Mens_Jeans_And_Verify_Cart_Positive()
        {
            //Arrange
            HomePage homePage = new HomePage(driver!);
            CookieBanner cookieBanner = new CookieBanner(driver!);
            AdvertisementPopup advertisementPopup = new AdvertisementPopup(driver!);
            ProductsPage productsPage = new ProductsPage(driver!);
            CartPage cartPage = new CartPage(driver!);

            const string firstProductName = "Soft Stretch Jeans";
            const string secondProductName = "Regular Fit Straight Jeans";

            //Act
            homePage.Open();
            cookieBanner.AcceptConsentIfDisplayed();

            homePage.GoToProducts();
            advertisementPopup.CloseIfDisplayed();

            productsPage.ExpandMenCategory();
            advertisementPopup.CloseIfDisplayed();

            productsPage.ScrollToJeansSubcategory();
            advertisementPopup.CloseIfDisplayed();

            productsPage.ClickJeansSubcategory();
            advertisementPopup.CloseIfDisplayed();

            string actualCategoryTitle = Regex.Replace(productsPage.GetCategoryTitle(),
                @"\s+",
                " ")
                .Trim();

            Assert.That(actualCategoryTitle, Is.EqualTo("Men - Jeans Products").IgnoreCase,
                "The Men - Jeans category was not opened.");

            productsPage.AddProductToCart(firstProductName);
            productsPage.ContinueShopping();

            productsPage.AddProductToCart(secondProductName);
            productsPage.ViewCart();

            List<CartItem> cartItems = cartPage.GetCartItems().ToList();

            //Assert
            Assert.That(cartItems, Has.Count.EqualTo(2), "The cart should contain exactly two products!");

            Assert.That(cartItems.Select(item => item.Name), Is.EquivalentTo
                (new[]  { firstProductName, secondProductName }),
                "The cart does not contain the expected products!");

            CartItem firstProduct = cartItems.Single(item => item.Name == firstProductName);

            CartItem secondProduct = cartItems.Single(item => item.Name == secondProductName);

            Assert.Multiple(() =>
            {
                Assert.That(firstProduct.Price, Is.EqualTo("Rs. 799"), "The first product price is incorrect!");
                Assert.That(firstProduct.Quantity, Is.EqualTo(1), "The first product quantity is incorrect!");
                Assert.That(firstProduct.Total, Is.EqualTo("Rs. 799"), "The first product total is incorrect.");

                Assert.That(secondProduct.Price, Is.EqualTo("Rs. 1200"), "The second product price is incorrect.");
                Assert.That(secondProduct.Quantity, Is.EqualTo(1), "The second product quantity is incorrect.");
                Assert.That(secondProduct.Total, Is.EqualTo("Rs. 1200"), "The second product total is incorrect.");
            });
        }
    }
}
