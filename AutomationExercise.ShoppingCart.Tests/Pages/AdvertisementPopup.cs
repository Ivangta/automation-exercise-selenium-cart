using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationExercise.ShoppingCart.Tests.Pages
{
    public class AdvertisementPopup : BasePage
    {
        private readonly By[] closeButtonLocators =
                {
            By.Id("dismiss-button"),
            By.Id("dismiss-button-element"),
            By.CssSelector("[aria-label='Close ad']"),
            By.CssSelector("[aria-label='Close']"),
            By.CssSelector(".close-button")
        };

        public AdvertisementPopup(IWebDriver driver)
            : base(driver)
        {
        }

        public void CloseIfDisplayed()
        {
            driver.SwitchTo().DefaultContent();

            WebDriverWait shortWait =
                new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            shortWait.PollingInterval =
                TimeSpan.FromMilliseconds(250);

            shortWait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException),
                typeof(NoSuchFrameException));

            try
            {
                shortWait.Until(currentDriver =>
                {
                    currentDriver.SwitchTo().DefaultContent();

                    return TryCloseInCurrentContextAndFrames(
                        currentDriver,
                        currentDepth: 0);
                });
            }
            catch (WebDriverTimeoutException)
            {
                // The advertisement is optional.
            }
            finally
            {
                driver.SwitchTo().DefaultContent();
            }
        }

        private bool TryCloseInCurrentContextAndFrames(
            IWebDriver currentDriver,
            int currentDepth)
        {
            if (TryClickCloseButton(currentDriver))
            {
                return true;
            }

            const int maximumFrameDepth = 5;

            if (currentDepth >= maximumFrameDepth)
            {
                return false;
            }

            List<IWebElement> frames = currentDriver
                .FindElements(By.TagName("iframe"))
                .ToList();

            foreach (IWebElement frame in frames)
            {
                try
                {
                    currentDriver.SwitchTo().Frame(frame);

                    bool wasClosed =
                        TryCloseInCurrentContextAndFrames(
                            currentDriver,
                            currentDepth + 1);

                    if (wasClosed)
                    {
                        return true;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    // The advertisement refreshed while being inspected.
                }
                catch (NoSuchFrameException)
                {
                    // The iframe disappeared before Selenium entered it.
                }
                finally
                {
                    try
                    {
                        currentDriver.SwitchTo().ParentFrame();
                    }
                    catch (WebDriverException)
                    {
                        currentDriver.SwitchTo().DefaultContent();
                    }
                }
            }

            return false;
        }

        private bool TryClickCloseButton(
            IWebDriver currentDriver)
        {
            foreach (By locator in closeButtonLocators)
            {
                IReadOnlyCollection<IWebElement> elements =
                    currentDriver.FindElements(locator);

                foreach (IWebElement element in elements)
                {
                    try
                    {
                        if (!element.Displayed)
                        {
                            continue;
                        }

                        ScrollElementIntoView(
                            currentDriver,
                            element);

                        try
                        {
                            element.Click();
                        }
                        catch (ElementClickInterceptedException)
                        {
                            ClickWithJavaScript(
                                currentDriver,
                                element);
                        }
                        catch (ElementNotInteractableException)
                        {
                            ClickWithJavaScript(
                                currentDriver,
                                element);
                        }

                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        // The close element was replaced.
                    }
                }
            }

            return false;
        }

        private static void ScrollElementIntoView(
            IWebDriver currentDriver,
            IWebElement element)
        {
            IJavaScriptExecutor js =
                (IJavaScriptExecutor)currentDriver;

            js.ExecuteScript(
                "arguments[0].scrollIntoView({block: 'center'});",
                element);
        }

        private static void ClickWithJavaScript(
            IWebDriver currentDriver,
            IWebElement element)
        {
            IJavaScriptExecutor js =
                (IJavaScriptExecutor)currentDriver;

            js.ExecuteScript(
                "arguments[0].click();",
                element);
        }
    }
}
