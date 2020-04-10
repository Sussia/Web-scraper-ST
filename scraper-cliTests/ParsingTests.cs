using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scraper_cli;

namespace scraper_cliTests
{
    [TestClass]
    public class ParsingTests
    {
        [TestMethod]
        public void ParsePage()
        {
            WebScraper scraper = new WebScraper();
            string title = "My test title";
            string testPageContent = $"<html><title>{title}</title></html>";
            ParsingRule rule = new ParsingRule("<title>", "</title>", title);
            Dictionary<string, string> scrapedValues =
                scraper.ParsePage(testPageContent, new List<ParsingRule>() { rule });
            string scrapedTitle = scrapedValues[rule.title];

            Assert.AreEqual(title, scrapedTitle);
        }
    }
}
