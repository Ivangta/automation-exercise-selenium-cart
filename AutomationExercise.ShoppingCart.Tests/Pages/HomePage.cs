using OpenQA.Selenium;

namespace AutomationExercise.ShoppingCart.Tests.Pages
{
    public class HomePage : BasePage
    {
        private const string Url = "https://www.automationexercise.com/";

        private readonly By productsLink =
            By.CssSelector("a[href='/products']");

        public HomePage(IWebDriver driver)
            : base(driver)
        {
        }

        public void Open()
        {
            NavigateTo(Url);
        }

        public void GoToProducts()
        {
            Click(productsLink);
        }
    }
}
