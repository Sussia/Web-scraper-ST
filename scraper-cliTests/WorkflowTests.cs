using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using scraper_cli;

namespace scraper_cliTests
{
    [TestClass]
    public class WorkflowTests
    {
        private Dictionary<string, string> files = new Dictionary<string, string>
        {
            {"Rules", "Desktop/test_rules.json"},
            {"Scraped values", "Desktop/test_result.csv"},
            {"Links", "Desktop/test_links.json"},
            {"Raw page", "Desktop/test_raw_page.txt"}
        };

        [TestMethod]
        public void CreateRule_Show_Find_ParseTwoLinks_ExportTocsv_Test()
        {
            var consoleMok = new Mock<MyConsole>();
            consoleMok.SetupSequence(c => c.ReadLine())
                .Returns("3")                           //Option "Rule management"
                .Returns("1")                           //Option "Create rule"
                .Returns("Title")                       //  Title of the rule
                .Returns("<title>")                     //  Prefix
                .Returns("</title>")                   //  Suffix
                .Returns("Title of the page")           //  Description
                .Returns("3")                           //Option "Rule management"
                .Returns("4")                           //Option "Show all"
                .Returns("3")                           //Option "Rule management"
                .Returns("5")                           //Option "Find rule"
                .Returns("Title")                       //  Rule title to find
                .Returns("3")                           //Option "Rule management"
                .Returns("5")                           //Option "Find rule"
                .Returns("Fake rule")                   //Option "Find rule" - Check the program if it handles lack of rule
                .Returns("2")                           //Option "Parse page"
                .Returns("2")                           //Option input from console
                .Returns("http://example.com")          //Url to parse
                .Returns("")                            //End of input
                .Returns("2")                           //Option "Export to csv")
                .Returns(files["Scraped values"])       //File path to save
                .Returns("asd")                         //Check handling wrong input
                .Returns("4");                          //Option "Exit"

            var requestServiceMock = new Mock<RequestService>();
            requestServiceMock.Setup(x => x.SendRequest("http://example.com"))
                .Returns("<html><title>Example Domain</title></html>");
            WebScraper webScraper = new WebScraper(consoleMok.Object, requestServiceMock.Object);

            //Check that program exits correctly
            Assert.AreEqual(0, webScraper.Start());

            //Check that rule was created
            var parsingRule = new ParsingRule("<title>", "</title>", "Title", "Title of the page");
            Assert.AreEqual(1, webScraper.ParsingRules.Count);
            Assert.AreEqual(parsingRule, webScraper.ParsingRules[0]);
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), files["Scraped values"]));
        }
    }
}
