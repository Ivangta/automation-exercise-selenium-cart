using OpenQA.Selenium;

namespace AutomationExercise.ShoppingCart.Tests.Pages
{
    public class ProductsPage : BasePage
    {
        private readonly By menCategory =
            By.CssSelector("a[href='#Men']");

        private readonly By jeansSubcategory =
            By.CssSelector("a[href='/category_products/6']");

        private readonly By categoryTitle =
            By.CssSelector("h2.title.text-center");

        private readonly By cartModal =
            By.Id("cartModal");

        private readonly By continueShoppingButton =
            By.CssSelector("button.close-modal");

        private readonly By viewCartLink =
            By.CssSelector("#cartModal a[href='/view_cart']");

        public ProductsPage(IWebDriver driver)
            : base(driver)
        {
        }

        public void SelectMensJeansCategory()
        {
            ScrollToElement(menCategory);
            Click(menCategory);
            Click(jeansSubcategory);
        }

        public string GetCategoryTitle()
        {
            return GetText(categoryTitle);
        }

        public void AddProductToCart(string productName)
        {
            By addToCartButton = By.XPath(
                $"//div[contains(@class,'product-image-wrapper')]" +
                $"[.//div[contains(@class,'productinfo')]//p[normalize-space()='{productName}']]" +
                $"//div[contains(@class,'productinfo')]" +
                $"//a[contains(@class,'add-to-cart')]");

            ScrollToElement(addToCartButton);
            Click(addToCartButton);
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
