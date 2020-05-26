using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Safari;

namespace web_scraper_vue_tests
{
    public class RuleManagementTests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new SafariDriver();
        }
        
        [Test]
        public void RuleCreationTest()
        {
            driver.Manage().Window.Maximize();
            driver.Url = "http://localhost:8080";
            var searchInput = driver.FindElement(By.CssSelector("#plus-card > button"));
            searchInput.SendKeys(Keys.Enter);
            var input = driver.FindElement(By.XPath("//*[@id=\"plus-card\"]/form/div/div/div[1]/div/div/div[1]/div/input"));
            input.SendKeys("Test text asasasasaasas asasas");

        }

        [TearDown]
        public void End()
        {
            driver.Close();
        }
    }
}