using AutomationExercise.ShoppingCart.Tests.Base;
using AutomationExercise.ShoppingCart.Tests.Models;
using AutomationExercise.ShoppingCart.Tests.Pages;

namespace AutomationExercise.ShoppingCart.Tests.Tests
{
    public class ShoppingCartTests : BaseTest
    {
        [Test]
        public void User_Should_Add_Two_Mens_Jeans_And_Verify_Cart()
        {
            //Arrange
            HomePage homepage = new HomePage(driver!);
            ProductsPage productsPage = new ProductsPage(driver!);
            CartPage cartPage = new CartPage(driver!);

            const string firstProductName = "Soft Streatch Jeans";
            const string secondProductName = "Regular Fit Straight Jeans";

            //Act
            homepage.Open();
            homepage.GoToProducts();

            productsPage.SelectMensJeansCategory();

            Assert.That(productsPage.GetCategoryTitle(), Is.EqualTo("Men - Jeans Products"),
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

            CartItem firstProduct = cartItems.Single(item => item.Name == secondProductName);

            CartItem secondProduct = cartItems.Single(item => item.Name == secondProductName);

            Assert.Multiple(() =>
            {
                Assert.That(firstProduct.Price, Is.EqualTo("Rs. 799"), "The first product price is incorrect!");
                Assert.That(firstProduct.Quantity, Is.EqualTo(1), "The first product quantity is incorrect!");
                Assert.That(firstProduct.Total,Is.EqualTo("Rs. 799"), "The first product total is incorrect.");

                Assert.That(secondProduct.Price, Is.EqualTo("Rs. 1200"), "The second product price is incorrect.");

                Assert.That(secondProduct.Quantity, Is.EqualTo(1),"The second product quantity is incorrect.");

                Assert.That(secondProduct.Total, Is.EqualTo("Rs. 1200"), "The second product total is incorrect.");
            });
        }
    }
}
