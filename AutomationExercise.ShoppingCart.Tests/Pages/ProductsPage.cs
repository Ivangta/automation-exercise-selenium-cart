using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace AutomationExercise.ShoppingCart.Tests.Pages
{
    public class ProductsPage : BasePage
    {
        private readonly By menCategory = By.CssSelector("#accordian a[href='#Men']");

        private readonly By jeansSubcategory = By.CssSelector("#Men a[href='/category_products/6']");

        private readonly By categoryTitle = By.CssSelector("h2.title.text-center");

        private readonly By cartModal = By.Id("cartModal");

        private readonly By continueShoppingButton = By.CssSelector("button.close-modal");

        private readonly By viewCartLink = By.CssSelector("#cartModal a[href='/view_cart']");

        public ProductsPage(IWebDriver driver)
            : base(driver)
        {
        }

        public void ExpandMenCategory()
        {
            ScrollToElement(menCategory);
            Click(menCategory);
        }

        public void ScrollToJeansSubcategory()
        {
            WaitUntilVisible(jeansSubcategory);
            ScrollToElement(jeansSubcategory);
        }

        public void ClickJeansSubcategory()
        {
            try
            {
                Click(jeansSubcategory);
            }
            catch (ElementClickInterceptedException)
            {
                bool advertisementClosed = new AdvertisementPopup(driver)
                    .CloseIfDisplayed(waitForAdvertisement: true);

                if (!advertisementClosed)
                {
                    throw;
                }

                Click(jeansSubcategory);
            }
        }

        public string GetCategoryTitle()
        {
            return GetText(categoryTitle);
        }

        public void AddProductToCart(string productName)
        {
            By productCard = By.XPath(
                $"//div[contains(@class,'product-image-wrapper')]" +
                $"[.//div[contains(@class,'productinfo')]//p[normalize-space()='{productName}']]");

            By addToCartButton = By.XPath(
                $"//div[contains(@class,'product-image-wrapper')]" +
                $"[.//div[contains(@class,'productinfo')]//p[normalize-space()='{productName}']]" +
                $"//div[contains(@class,'product-overlay')]//a[contains(@class,'add-to-cart')]");

            IWebElement product = WaitUntilVisible(productCard);

            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});",
                product);

            new Actions(driver).MoveToElement(product).Perform();

            IWebElement button = WaitUntilClickable(addToCartButton);

            try
            {
                button.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", button);
            }
            catch (ElementNotInteractableException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", button);
            }

            WaitUntilVisible(cartModal);
        }

        public void ContinueShopping()
        {
            Click(continueShoppingButton);
            WaitUntilInvisible(cartModal);
        }

        public void ViewCart()
        {
            Click(viewCartLink);
        }
    }
}
