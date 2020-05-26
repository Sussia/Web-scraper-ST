using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Safari;

namespace web_scraper_vue_tests
{
    public class RuleManagementTests
    {
        IWebDriver driver;
        private static string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

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

        [Test]
        public void UploadRuleTest()
        {
            string testTitle = "Test rule";
            var rules = new ParsingRule[]
            {
                new ParsingRule {
                    title = testTitle,
                    prefix = "<test>",
                    suffix = "</test>"
                }
            };
            string path = Path.Combine(homeDirectory, "Desktop/test.json");
            File.WriteAllText(path, JsonSerializer.Serialize(rules));
            var fileInput = driver.FindElement(By.CssSelector("input[type=\"file\"]"));
            fileInput.SendKeys(path);

            var cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(2, cards.Count);

            var newCard = new RuleCard(cards[1]);
            Assert.AreEqual(testTitle, newCard.title);

            File.Delete(path);
        }

        [Test]
        public void DownloadRuleTest()
        {
            var downloadButton = driver.FindElement(By.CssSelector("a.download-button"));
            downloadButton.SendKeys(Keys.Enter);
            Thread.Sleep(2000);
            string path = Path.Combine(homeDirectory, "Downloads/parsing_rules.json");
            var rules = JsonSerializer.Deserialize<ParsingRule[]>(File.ReadAllText(path));

            Assert.AreEqual("Title", rules[0].title);

            File.Delete(path);
        }

        [TearDown]
        public void End()
        {
            driver.Close();
        }
    }
}