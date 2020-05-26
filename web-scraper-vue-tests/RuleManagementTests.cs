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
            // 1.	Проверка количества правил при первоначальной загрузке страницы
            var cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(1, cards.Count);
            // 2.	Нажатие кнопки добавления правила
            var plusButton = driver.FindElement(By.CssSelector("#plus-card > button"));
            plusButton.SendKeys(Keys.Enter);
            // 3.	Проверка открытия формы для создания правила
            RuleForm creationForm = new RuleForm(driver.FindElement(By.CssSelector("#plus-card > form")));

            string testTitle = "Test title";
            string prefix = "<prefix>";
            string suffix = "<suffix>";
            // 4.	Заполнение необходимых полей
            creationForm.title.SendKeys(testTitle);
            creationForm.description.SendKeys("Some description");
            creationForm.perfix.SendKeys(prefix);
            creationForm.suffix.SendKeys(suffix);

            var saveButton = creationForm.saveButton;
            // 5.	Проверка активности кнопки сохранения
            Assert.AreEqual(true, saveButton.Enabled);
            // 6.	Нажатие на кнопку «Сохранить»
            saveButton.SendKeys(Keys.Enter);

            // 7.	Проверка присутствия нового правила на странице и соответствия его введенным данным
            cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(2, cards.Count);
            var newCard = new RuleCard(cards[1]);
            Assert.AreEqual(testTitle, newCard.title);

            // 8.	Нажатие на кнопку отображения подробностей
            newCard.expandButton.SendKeys(Keys.Enter);
            // 9.	Проверка отображения подробностей
            Assert.AreEqual($"Prefix: {prefix}Suffix: {suffix}", newCard.text);
            // 10.	Нажатие на кнопку скрытия подробностей
            newCard.expandButton.SendKeys(Keys.Enter);

            // 11.	Нажатие на кнопку редактирование правила
            newCard.editButton.SendKeys(Keys.Enter);
            var editForm = newCard.editForm;
            // 12.	Заполнение формы редактирования правила
            string newTitle = "New test title";
            editForm.title.SendKeys(Keys.Command + "a");
            editForm.title.SendKeys(Keys.Backspace);
            editForm.title.SendKeys(newTitle);
            // 13.	Нажатие на кнопку «Сохранить»
            editForm.saveButton.SendKeys(Keys.Enter);
            // 14.	Проверка соответствия контента страницы внесенным изменениям
            newCard = new RuleCard(driver.FindElements(By.CssSelector("div.rule-card"))[1]);
            Assert.AreEqual(newTitle, newCard.title);

            // 15.	Нажатие на кнопку удаления правила
            newCard.deleteButton.SendKeys(Keys.Enter);
            // 16.	Проверка отсутствия правила на странице
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

            // 1.	Создание файла с правилом в формате json
            string path = Path.Combine(homeDirectory, "Desktop/test.json");
            File.WriteAllText(path, JsonSerializer.Serialize(rules));

            // 2.	Нажатие на кнопку «Загрузить правила»
            // 3.	Указание пути к файлу
            var fileInput = driver.FindElement(By.CssSelector("input[type=\"file\"]"));
            fileInput.SendKeys(path);

            // 4.	Проверка соответствия контента страницы загружаемому контенту
            var cards = driver.FindElements(By.CssSelector("div.rule-card"));
            Assert.AreEqual(2, cards.Count);
            var newCard = new RuleCard(cards[1]);
            Assert.AreEqual(testTitle, newCard.title);

            // 5.	Удаление файла
            File.Delete(path);
        }

        [Test]
        public void DownloadRuleTest()
        {
            // 1.	Нажатие на кнопку «Скачать правила»
            // 2.	Сохранение файла
            var downloadButton = driver.FindElement(By.CssSelector("a.download-button"));
            downloadButton.SendKeys(Keys.Enter);
            Thread.Sleep(2000);

            // 3.	Проверка соответствия скачанного контента предполагаемому контенту
            string path = Path.Combine(homeDirectory, "Downloads/parsing_rules.json");
            var rules = JsonSerializer.Deserialize<ParsingRule[]>(File.ReadAllText(path));
            Assert.AreEqual("Title", rules[0].title);

            // 4.	Удаление файла
            File.Delete(path);
        }

        [TearDown]
        public void End()
        {
            driver.Close();
        }
    }
}