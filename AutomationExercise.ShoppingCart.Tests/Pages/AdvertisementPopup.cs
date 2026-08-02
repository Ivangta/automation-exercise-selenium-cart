using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationExercise.ShoppingCart.Tests.Pages
{
    public class AdvertisementPopup : BasePage
    {
        private static readonly By vignetteFrames = By.CssSelector(
           "iframe[name^='aswift_'], iframe[id^='aswift_']");

        private static readonly By closeButton = By.CssSelector(
            "#dismiss-button[aria-label='Close ad'], " +
            "#dismiss-button-element, " +
            "#dismiss-button svg path");

        public AdvertisementPopup(IWebDriver driver)
            : base(driver)
        {
        }

        public bool CloseIfDisplayed()
        {
            driver.SwitchTo().DefaultContent();

            if (!driver.Url.Contains("google_vignette", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200)
            };

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException),
                typeof(NoSuchFrameException));

            try
            {
                bool wasClicked = wait.Until(currentDriver =>
                    TryCloseVignette(currentDriver));

                if (!wasClicked)
                {
                    return false;
                }

                wait.Until(currentDriver =>
                    !IsVignetteStillDisplayed(currentDriver));

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            finally
            {
                driver.SwitchTo().DefaultContent();
            }
        }

        private bool TryCloseVignette(IWebDriver currentDriver)
        {
            currentDriver.SwitchTo().DefaultContent();

            IReadOnlyCollection<IWebElement> frames =
                currentDriver.FindElements(vignetteFrames);

            foreach (IWebElement frame in frames)
            {
                try
                {
                    currentDriver.SwitchTo().Frame(frame);

                    IWebElement? button = currentDriver
                        .FindElements(closeButton)
                        .FirstOrDefault(element =>
                            element.Displayed && element.Enabled);

                    if (button == null)
                    {
                        continue;
                    }

                    try
                    {
                        button.Click();
                    }
                    catch (ElementClickInterceptedException)
                    {
                        ClickWithJavaScript(currentDriver, button);
                    }
                    catch (ElementNotInteractableException)
                    {
                        ClickWithJavaScript(currentDriver, button);
                    }

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    // The advertisement iframe was refreshed.
                }
                catch (NoSuchFrameException)
                {
                    // The advertisement iframe disappeared.
                }
                finally
                {
                    currentDriver.SwitchTo().DefaultContent();
                }
            }

            return false;
        }

        private bool IsVignetteStillDisplayed(IWebDriver currentDriver)
        {
            currentDriver.SwitchTo().DefaultContent();

            IReadOnlyCollection<IWebElement> frames =
                currentDriver.FindElements(vignetteFrames);

            foreach (IWebElement frame in frames)
            {
                try
                {
                    currentDriver.SwitchTo().Frame(frame);

                    bool closeButtonIsVisible = currentDriver
                        .FindElements(closeButton)
                        .Any(element => element.Displayed);

                    if (closeButtonIsVisible)
                    {
                        return true;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    // The iframe was removed while checking it.
                }
                catch (NoSuchFrameException)
                {
                    // The iframe no longer exists.
                }
                finally
                {
                    currentDriver.SwitchTo().DefaultContent();
                }
            }

            return false;
        }

        private static void ClickWithJavaScript(
            IWebDriver currentDriver,
            IWebElement element)
        {
            ((IJavaScriptExecutor)currentDriver)
                .ExecuteScript("arguments[0].click();", element);
        }
    }
}
