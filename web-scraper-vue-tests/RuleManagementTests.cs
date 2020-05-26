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
            const string cardList = "#app > div > main > div > div > div.row.row--dense.align-start";

            driver.Manage().Window.Maximize();
            driver.Url = "http://localhost:8080";

            var cards = driver.FindElements(By.CssSelector(cardList + " > div"));

            Assert.AreEqual(2, cards.Count);
                
            var searchInput = driver.FindElement(By.CssSelector("#plus-card > button"));
            searchInput.SendKeys(Keys.Enter);

            var title = driver.FindElement(By.XPath("//*[@id=\"plus-card\"]/form/div/div/div[1]/div/div/div[1]/div/input"));
            title.SendKeys("Test title");

            var description = driver.FindElement(By.XPath("//*[@id=\"plus-card\"]/form/div/div/div[2]/div/div/div[1]/div/input"));
            description.SendKeys("Some description");

            var prefix = driver.FindElement(By.XPath("//*[@id=\"plus-card\"]/form/div/div/div[3]/div/div/div[1]/div/input"));
            prefix.SendKeys("prefix");

            var suffix = driver.FindElement(By.XPath("//*[@id=\"plus-card\"]/form/div/div/div[4]/div/div/div[1]/div/input"));
            suffix.SendKeys("suffix");

            var saveButton = driver.FindElement(By.CssSelector("#plus-card > form > div > div > div:nth-child(5) > button"));
            Assert.AreEqual(true, saveButton.Enabled);

            saveButton.SendKeys(Keys.Enter);


            cards = driver.FindElements(By.CssSelector(cardList + " > div"));
            Assert.AreEqual(3, cards.Count);

            var secondCardTitle = driver.FindElement(By.CssSelector(cardList + " > div:nth-child(2) > div > div > div.v-card__title > span"));
            Assert.AreEqual("Test title", secondCardTitle.Text);

        }

        [TearDown]
        public void End()
        {
            driver.Close();
        }
    }
}