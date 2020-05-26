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
            driver.Manage().Window.Maximize();
            driver.Url = "http://localhost:8080";
        }
        
        [Test]
        public void RuleCreationTest()
        {
            var cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(1, cards.Count);
                
            var plusButton = driver.FindElement(By.CssSelector("#plus-card > button"));
            plusButton.SendKeys(Keys.Enter);

            RuleForm creationForm = new RuleForm(driver.FindElement(By.CssSelector("#plus-card > form")));

            string testTitle = "Test title";

            creationForm.title.SendKeys(testTitle);
            creationForm.description.SendKeys("Some description");
            creationForm.perfix.SendKeys("prefix");
            creationForm.suffix.SendKeys("suffix");

            var saveButton = creationForm.saveButton;

            Assert.AreEqual(true, saveButton.Enabled);

            saveButton.SendKeys(Keys.Enter);

            cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(2, cards.Count);

            var secondCardTitle = cards[1].FindElement(By.CssSelector("div.v-card__title > span"));
            Assert.AreEqual(testTitle, secondCardTitle.Text);

        }

        [TearDown]
        public void End()
        {
            driver.Close();
        }
    }
}