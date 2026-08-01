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

            options.PageLoadStrategy = PageLoadStrategy.Eager;

            IWebDriver driver = new ChromeDriver(options);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);

            return driver;
        }
    }
}
