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
        public void RuleControlsTest()
        {
            var cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(1, cards.Count);
                
            var plusButton = driver.FindElement(By.CssSelector("#plus-card > button"));
            plusButton.SendKeys(Keys.Enter);

            RuleForm creationForm = new RuleForm(driver.FindElement(By.CssSelector("#plus-card > form")));

            string testTitle = "Test title";
            string prefix = "<prefix>";
            string suffix = "<suffix>";

            creationForm.title.SendKeys(testTitle);
            creationForm.description.SendKeys("Some description");
            creationForm.perfix.SendKeys(prefix);
            creationForm.suffix.SendKeys(suffix);

            var saveButton = creationForm.saveButton;

            Assert.AreEqual(true, saveButton.Enabled);

            saveButton.SendKeys(Keys.Enter);

            cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(2, cards.Count);

            var newCard = new RuleCard(cards[1]);

            Assert.AreEqual(testTitle, newCard.title);

            newCard.expandButton.SendKeys(Keys.Enter);
            Assert.AreEqual($"Prefix: {prefix}Suffix: {suffix}", newCard.text);
            newCard.expandButton.SendKeys(Keys.Enter);

            newCard.editButton.SendKeys(Keys.Enter);
            var editForm = newCard.editForm;
            string newTitle = "New test title";
            editForm.title.SendKeys(Keys.Command + "a");
            editForm.title.SendKeys(Keys.Backspace);
            editForm.title.SendKeys(newTitle);
            editForm.saveButton.SendKeys(Keys.Enter);
            newCard = new RuleCard(driver.FindElements(By.CssSelector("div.rule-card"))[1]);
            Assert.AreEqual(newTitle, newCard.title);

            newCard.deleteButton.SendKeys(Keys.Enter);
            cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(1, cards.Count);

        }

        [TearDown]
        public void End()
        {
            driver.Close();
        }
    }
}