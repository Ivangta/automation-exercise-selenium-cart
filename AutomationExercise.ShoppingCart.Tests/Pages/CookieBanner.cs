using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationExercise.ShoppingCart.Tests.Pages
{
    public class CookieBanner : BasePage
    {
        private readonly By consentDialog = By.CssSelector(".fc-consent-root");

        private readonly By consentButton = By.CssSelector(".fc-consent-root button.fc-cta-consent");

        public CookieBanner(IWebDriver driver) : base(driver)
        {
        }

        public void AcceptConsentIfDisplayed()
        {
            try
            {
                WebDriverWait shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                IWebElement button = shortWait.Until(currentDriver =>
                {
                    IReadOnlyCollection<IWebElement> buttons = currentDriver.FindElements(consentButton);

                    foreach (IWebElement currentButton in buttons)
                    {
                        if (currentButton.Displayed && currentButton.Enabled)
                        {
                            return currentButton;
                        }
                    }

                    return null;
                })!;

                button.Click();

                WaitUntilInvisible(consentDialog);
            }
            catch (WebDriverTimeoutException)
            {
                // Consent banner is not always displayed.
            }
        }
    }
}
