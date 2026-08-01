using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.ShoppingCart.Tests.Base
{
    public static class DriverFactory
    {
        public static IWebDriver CreateDriver()
        {
            ChromeOptions options = new ChromeOptions();

            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");

            return new ChromeDriver(options);
        }
    }
}
