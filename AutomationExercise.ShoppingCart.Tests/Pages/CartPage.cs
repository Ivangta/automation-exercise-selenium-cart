using AutomationExercise.ShoppingCart.Tests.Models;
using OpenQA.Selenium;

namespace AutomationExercise.ShoppingCart.Tests.Pages
{
    public class CartPage : BasePage
    {
        private readonly By cartRows = By.CssSelector("#cart_info_table tbody tr");

        public CartPage(IWebDriver driver) : base(driver)
        {
        }

        public IReadOnlyCollection<CartItem> GetCartItems()
        {
            IReadOnlyCollection<IWebElement> rows = FindElements(cartRows);

            List<CartItem> cartItems = new List<CartItem>();

            foreach (IWebElement row in rows)
            {
                string name = row.FindElement(By.CssSelector(".cart_description h4 a")).Text.Trim();

                string price = row.FindElement( By.CssSelector(".cart_price p")).Text.Trim();

                string quantityText = row.FindElement(By.CssSelector(".cart_quantity button")).Text.Trim();

                string total = row.FindElement(By.CssSelector(".cart_total_price")).Text.Trim();

                int quantity = int.Parse(quantityText);

                CartItem cartItem = new CartItem(name, price, quantity, total);

                cartItems.Add(cartItem);
            }

            return cartItems;
        }
    }
}
