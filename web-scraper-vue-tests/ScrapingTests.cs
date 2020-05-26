using System;
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

        [TearDown]
        public void End()
        {
            driver.Close();
        }
    }
}
