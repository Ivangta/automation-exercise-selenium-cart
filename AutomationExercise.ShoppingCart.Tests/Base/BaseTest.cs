using OpenQA.Selenium;

namespace AutomationExercise.ShoppingCart.Tests.Base
{
    public class BaseTest
    {
        protected IWebDriver? driver;

        [SetUp]
        public void Setup()
        {
            driver = DriverFactory.CreateDriver();
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
            driver = null;
        }
    }
}
