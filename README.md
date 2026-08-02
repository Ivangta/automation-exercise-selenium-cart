# Automation Exercise Shopping Cart Test

Automated end-to-end UI test for  
[Automation Exercise](https://www.automationexercise.com/).

The project uses C#, Selenium WebDriver, NUnit, and the Page Object Model design pattern.

## Test Scenario

The test:

1. Opens the website.
2. Accepts the cookie consent form when displayed.
3. Navigates to Products → Men → Jeans.
4. Adds two products to the cart.
5. Verifies the category title.
6. Verifies the product names, prices, quantities, and totals.

## Technologies

- C#
- .NET
- Selenium WebDriver
- NUnit
- Page Object Model
- Google Chrome
- Mozilla Firefox

## Project Structure

```text
AutomationExercise.ShoppingCart.Tests
│
├── Base
│   ├── BaseTest.cs
│   └── DriverFactory.cs
│
├── Models
│   └── CartItem.cs
│
├── Pages
│   ├── AdvertisementPopup.cs
│   ├── BasePage.cs
│   ├── CartPage.cs
│   ├── CookieBanner.cs
│   ├── HomePage.cs
│   └── ProductsPage.cs
│
└── Tests
    └── ShoppingCartTests.cs
```

## Running the Test

Restore the packages and run the test:

```bash
dotnet restore
dotnet test
```

The test can also be executed from Visual Studio Test Explorer.

## Browser Validation

The test was validated with more than 20 consecutive successful runs in:

- Google Chrome
- Mozilla Firefox

## Notes

- Explicit waits are used instead of `Thread.Sleep`.
- Cookie consent and Google vignette advertisements are handled when displayed.
- Browser configuration is located in `DriverFactory.cs`.
