using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationExercise.ShoppingCart.Tests.Pages
{
    public class BasePage
    {
        protected readonly IWebDriver driver;
        protected readonly WebDriverWait wait;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;

            wait = new WebDriverWait(new SystemClock(), driver, TimeSpan.FromSeconds(10),
                TimeSpan.FromMilliseconds(500));

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        protected IWebElement WaitUntilVisible(By locator)
        {
            return wait.Until(driver =>
            {
                IWebElement element = driver.FindElement(locator);

                return element.Displayed ? element : null;
            })!;
        }

        protected IWebElement WaitUntilClickable(By locator)
        {
            return wait.Until(driver =>
            {
                IWebElement element = driver.FindElement(locator);

                return element.Displayed && element.Enabled ? element : null;
            })!;
        }

        protected void Click(By locator)
        {
            WaitUntilClickable(locator).Click();
        }

        protected string GetText(By locator)
        {
            return WaitUntilVisible(locator).Text;
        }

        protected IReadOnlyCollection<IWebElement> FindElements(By locator)
        {
            return wait.Until(driver =>
            {
                IReadOnlyCollection<IWebElement> elements = driver.FindElements(locator);

                return elements.Count > 0 ? elements  : null;
            })!;
        }

        protected void ScrollToElement(By locator)
        {
            IWebElement element = WaitUntilVisible(locator);

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", 
                element);
        }

        protected void NavigateTo(string url)
        {
            driver.Navigate().GoToUrl(url);
        }
    }
}
