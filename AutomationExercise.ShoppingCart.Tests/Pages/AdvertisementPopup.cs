using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationExercise.ShoppingCart.Tests.Pages
{
    public class AdvertisementPopup : BasePage
    {
        private static readonly By CloseButtonLocator = By.CssSelector(
             "#dismiss-button, " +
             "#dismiss-button-element, " +
             "[aria-label='Close ad'], " +
             "[aria-label='Close']");

        public AdvertisementPopup(IWebDriver driver)
            : base(driver)
        {
        }

        public bool CloseIfDisplayed()
        {
            driver.SwitchTo().DefaultContent();

            try
            {
                // Google vignette рекламата в този сайт добавя
                // "google_vignette" към URL адреса.
                bool advertisementExpected = driver.Url.Contains(
                    "google_vignette",
                    StringComparison.OrdinalIgnoreCase);

                // Няма индикация за реклама — не чакаме изобщо.
                if (!advertisementExpected)
                {
                    return false;
                }

                WebDriverWait advertisementWait =
                    new WebDriverWait(
                        driver,
                        TimeSpan.FromSeconds(3));

                advertisementWait.PollingInterval =
                    TimeSpan.FromMilliseconds(200);

                advertisementWait.IgnoreExceptionTypes(
                    typeof(NoSuchElementException),
                    typeof(StaleElementReferenceException),
                    typeof(NoSuchFrameException));

                return advertisementWait.Until(currentDriver =>
                {
                    currentDriver.SwitchTo().DefaultContent();

                    return TryCloseInCurrentContextAndFrames(
                        currentDriver,
                        currentDepth: 0);
                });
            }
            catch (WebDriverTimeoutException)
            {
                // Рекламата е опционална.
                return false;
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

            // Две iframe нива са достатъчни за Google vignette.
            const int maximumFrameDepth = 2;

            if (currentDepth >= maximumFrameDepth)
            {
                return false;
            }

            IReadOnlyCollection<IWebElement> frames =
                currentDriver.FindElements(By.TagName("iframe"));

            foreach (IWebElement frame in frames)
            {
                bool switchedToFrame = false;

                try
                {
                    if (!frame.Displayed)
                    {
                        continue;
                    }

                    currentDriver.SwitchTo().Frame(frame);
                    switchedToFrame = true;

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
                    // Рекламният iframe може да бъде презареден.
                }
                catch (NoSuchFrameException)
                {
                    // iframe-ът може да изчезне преди превключването.
                }
                finally
                {
                    // ParentFrame се извиква само ако действително
                    // сме влезли във frame-а.
                    if (switchedToFrame)
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
            }

            return false;
        }

        private bool TryClickCloseButton(
            IWebDriver currentDriver)
        {
            IReadOnlyCollection<IWebElement> closeButtons =
                currentDriver.FindElements(CloseButtonLocator);

            foreach (IWebElement closeButton in closeButtons)
            {
                try
                {
                    if (!closeButton.Displayed ||
                        !closeButton.Enabled)
                    {
                        continue;
                    }

                    try
                    {
                        closeButton.Click();
                    }
                    catch (ElementClickInterceptedException)
                    {
                        ClickWithJavaScript(
                            currentDriver,
                            closeButton);
                    }
                    catch (ElementNotInteractableException)
                    {
                        ClickWithJavaScript(
                            currentDriver,
                            closeButton);
                    }

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    // Бутонът може да бъде заменен при зареждането.
                }
            }

            return false;
        }

        private static void ClickWithJavaScript(
            IWebDriver currentDriver,
            IWebElement element)
        {
            IJavaScriptExecutor javaScript =
                (IJavaScriptExecutor)currentDriver;

            javaScript.ExecuteScript(
                "arguments[0].click();",
                element);
        }
    }
}
