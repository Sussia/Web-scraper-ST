using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Safari;

namespace web_scraper_vue_tests
{
    public class ScrapingTests
    {
        IWebDriver driver;
        private static string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        [SetUp]
        public void Setup()
        {
            driver = new SafariDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "http://localhost:8080";
            // 1.	Нажатие на вкладку «Скрейпинг»
            driver.FindElement(By.CssSelector("#scraping-tab")).SendKeys(Keys.Enter);
        }

        [Test]
        public void LinksControlTest()
        {
            // 2.	Проверка отображения вкладки скрейпинга
            var scrapingComponent = driver.FindElement(By.CssSelector("#scraping-component"));
            Assert.AreEqual(true, scrapingComponent.Displayed);

            string firstUrl = "Test url";
            string secondUrl = "Second test url";

            var urlContainers = driver.FindElements(By.CssSelector("div.url-field"));

            // 3.	Заполнение первого адреса
            urlContainers[0].FindElement(By.CssSelector("input")).SendKeys(firstUrl);

            // 4.	Проверка его отображения на странице
            urlContainers = driver.FindElements(By.CssSelector("div.url-field"));
            Assert.AreEqual(2, urlContainers.Count);

            // 5.	Заполнение второго адреса
            urlContainers[1].FindElement(By.CssSelector("input")).SendKeys(secondUrl);

            // 6.	Проверка его отображения на странице
            urlContainers = driver.FindElements(By.CssSelector("div.url-field"));
            Assert.AreEqual(3, urlContainers.Count);

            // 7.	Нажатие на кнопку удаления адреса из списка напротив первого адреса
            var removeButtons = driver.FindElements(By.CssSelector("button.remove-url-button"));
            removeButtons[0].SendKeys(Keys.Enter);

            // 8.	Проверка оставшегося адреса на равенство второму введенному
            urlContainers = driver.FindElements(By.CssSelector("div.url-field"));
            Assert.AreEqual(2, urlContainers.Count);
            Assert.AreEqual(secondUrl, urlContainers[0].FindElement(By.CssSelector("input")).GetAttribute("value"));
        }

        [Test]
        public void ScrapingTest()
        {
            // 2.	Создание файла со списком адресов для скрейпинга
            var links = new string[] { "https://example.com" };
            string path = Path.Combine(homeDirectory, "Desktop/test.json");
            File.WriteAllText(path, JsonSerializer.Serialize(links));

            // 3.	Нажатие на кнопку «Загрузить адреса»
            // 4.	Указание пути к файлу с адресами
            var fileInput = driver.FindElement(By.CssSelector("input[type=\"file\"]"));
            fileInput.SendKeys(path);

            // 5.	Нажатие на кнопку «Получить контент»
            driver.FindElement(By.CssSelector("#scrape-button")).SendKeys(Keys.Enter);
            Thread.Sleep(5000);

            // 6.	Проверка соответствия отображенного контента в таблице предполагаемому
            var dataTable = driver.FindElement(By.CssSelector("#data-table"));
            var rows = dataTable.FindElements(By.CssSelector("td"));
            Assert.AreEqual(1, rows.Count);
            Assert.AreEqual("Example Domain", rows[0].Text);

            // 7.	Нажатие на кнопку «Скачать таблицу»
            var downloadButton = driver.FindElement(By.CssSelector("a.download-button"));
            downloadButton.SendKeys(Keys.Enter);
            Thread.Sleep(2000);

            // 8.	Проверка присутствия файла с таблицей на компьютере
            string tablePath = Path.Combine(homeDirectory, "Downloads/scraped_values.csv");
            Assert.AreEqual(true, File.Exists(tablePath));

            // 9.	Нажатие на кнопку «Очистить таблицу»
            driver.FindElement(By.CssSelector("#clear-table-button")).SendKeys(Keys.Enter);

            // 10.	Проверка отсутствия таблицы на странице
            var dataTables = driver.FindElements(By.CssSelector("#data-table"));
            Assert.AreEqual(0, dataTables.Count);

            // 11.	Удаления файла с адресами и файла с таблицей
            File.Delete(path);
            File.Delete(tablePath);

        }

        [TearDown]
        public void End()
        {
            driver.Close();
        }
    }
}
