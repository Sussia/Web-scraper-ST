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
            driver.FindElement(By.CssSelector("#scraping-tab")).SendKeys(Keys.Enter);
        }

        [Test]
        public void LinksControlTest()
        {
            var scrapingComponent = driver.FindElement(By.CssSelector("#scraping-component"));
            Assert.AreEqual(true, scrapingComponent.Displayed);

            string firstUrl = "Test url";
            string secondUrl = "Second test url";

            var urlContainers = driver.FindElements(By.CssSelector("div.url-field"));

            urlContainers[0].FindElement(By.CssSelector("input")).SendKeys(firstUrl);

            urlContainers = driver.FindElements(By.CssSelector("div.url-field"));
            Assert.AreEqual(2, urlContainers.Count);

            urlContainers[1].FindElement(By.CssSelector("input")).SendKeys(secondUrl);

            urlContainers = driver.FindElements(By.CssSelector("div.url-field"));
            Assert.AreEqual(3, urlContainers.Count);

            var removeButtons = driver.FindElements(By.CssSelector("button.remove-url-button"));

            removeButtons[0].SendKeys(Keys.Enter);

            urlContainers = driver.FindElements(By.CssSelector("div.url-field"));
            Assert.AreEqual(2, urlContainers.Count);
            Assert.AreEqual(secondUrl, urlContainers[0].FindElement(By.CssSelector("input")).GetAttribute("value"));
        }

        [Test]
        public void ScrapingTest()
        {
            var links = new string[] { "https://example.com" };
            string path = Path.Combine(homeDirectory, "Desktop/test.json");
            File.WriteAllText(path, JsonSerializer.Serialize(links));
            var fileInput = driver.FindElement(By.CssSelector("input[type=\"file\"]"));
            fileInput.SendKeys(path);

            driver.FindElement(By.CssSelector("#scrape-button")).SendKeys(Keys.Enter);
            Thread.Sleep(5000);

            var dataTable = driver.FindElement(By.CssSelector("#data-table"));

            var rows = dataTable.FindElements(By.CssSelector("td"));
            Assert.AreEqual(1, rows.Count);
            Assert.AreEqual("Example Domain", rows[0].Text);

            var downloadButton = driver.FindElement(By.CssSelector("a.download-button"));
            downloadButton.SendKeys(Keys.Enter);
            Thread.Sleep(2000);
            string tablePath = Path.Combine(homeDirectory, "Downloads/scraped_values.csv");
            Assert.AreEqual(true, File.Exists(tablePath));

            driver.FindElement(By.CssSelector("#clear-table-button")).SendKeys(Keys.Enter);
            var dataTables = driver.FindElements(By.CssSelector("#data-table"));
            Assert.AreEqual(0, dataTables.Count);

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
