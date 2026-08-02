using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.ShoppingCart.Tests.Base
{
    public static class DriverFactory
    {
        public static IWebDriver CreateDriver(bool runHeadless = false)
        {
            ChromeOptions options = new ChromeOptions();

            if (runHeadless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
            }
            else
            {
                options.AddArgument("--start-maximized");
            }
            options.AddArgument("--disable-notifications");

            options.PageLoadStrategy = PageLoadStrategy.Eager;

            IWebDriver driver = new ChromeDriver(options);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);

            return driver;
        }
    }
}
