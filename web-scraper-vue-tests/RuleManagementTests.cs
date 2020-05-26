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

            var cards = driver.FindElements(By.CssSelector("div.rule-card"));

            Assert.AreEqual(1, cards.Count);
                
            var searchInput = driver.FindElement(By.CssSelector("#plus-card > button"));
            searchInput.SendKeys(Keys.Enter);

            var ruleCreationForm = driver.FindElement(By.CssSelector("#plus-card > form"));

            var title = ruleCreationForm.FindElement(By.CssSelector("div.title-field")).FindElement(By.CssSelector("input"));
            title.SendKeys("Test title");

            var description = ruleCreationForm.FindElement(By.CssSelector("div.description-field")).FindElement(By.CssSelector("input"));
            description.SendKeys("Some description");

            var prefix = ruleCreationForm.FindElement(By.CssSelector("div.prefix-field")).FindElement(By.CssSelector("input"));
            prefix.SendKeys("prefix");

            var suffix = ruleCreationForm.FindElement(By.CssSelector("div.suffix-field")).FindElement(By.CssSelector("input"));
            suffix.SendKeys("suffix");

            var saveButton = ruleCreationForm.FindElement(By.CssSelector("button.save-button"));
            Assert.AreEqual(true, saveButton.Enabled);

            saveButton.SendKeys(Keys.Enter);


            cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(2, cards.Count);

            var secondCardTitle = cards[1].FindElement(By.CssSelector("div.v-card__title > span"));
            Assert.AreEqual("Test title", secondCardTitle.Text);

        }

        [TearDown]
        public void End()
        {
            driver.Close();
        }
    }
}