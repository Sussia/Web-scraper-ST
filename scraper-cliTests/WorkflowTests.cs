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
            {"Scraped values json", "Desktop/test_result.json"},
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

            var fileServiceMock = new Mock<FileService>();
            fileServiceMock.Setup(x => x.ExportToCsv(It.IsAny<List<Dictionary<string, string>>>(), files["Scraped values"]))
                .Returns("Successfuly saved!");

            WebScraper webScraper = new WebScraper(consoleMok.Object, requestServiceMock.Object, fileServiceMock.Object);

            //Check that program exits correctly
            Assert.AreEqual(0, webScraper.Start());

            //Check that rule was created
            var parsingRule = new ParsingRule("<title>", "</title>", "Title", "Title of the page");
            Assert.AreEqual(1, webScraper.ParsingRules.Count);
            Assert.AreEqual(parsingRule, webScraper.ParsingRules[0]);
        }

        [TestMethod]
        public void ImportRules_ExportRules_DeleteRule_ImportLinks_SaveToJson_Test()
        {
            var consoleMok = new Mock<MyConsole>();
            consoleMok.SetupSequence(c => c.ReadLine())
                .Returns("3")                           //Option "Rule management"
                .Returns("2")                           //Option "Import rule"
                .Returns("1")                           //Option "Import from json"
                .Returns(files["Rules"])                //  File path to load rules
                .Returns("3")                           //Option "Rule management
                .Returns("3")                           //Option "Export Rules"
                .Returns("1")                           //Option "Export to json"
                .Returns(files["Rules"])                // File path to save rules
                .Returns("3")                           //Option "Rule management"
                .Returns("7")                           //Option "Delete rule"
                .Returns("Title")                       // Name of the rule to delete
                .Returns("2")                           //Option "Parse page"
                .Returns("1")                           //Option "Import link from file"
                .Returns(files["Links"])                //  File path to load links
                .Returns("3")                           //Option "Export to json"
                .Returns(files["Scraped values json"])  // File path to save results
                .Returns("4");                          //Option "Exit"

            var requestServiceMock = new Mock<RequestService>();
            requestServiceMock.Setup(x => x.SendRequest("http://example.com"))
                .Returns("<html><title>Example Domain</title></html>");

            var rulesList = new List<ParsingRule>
            {
                new ParsingRule("t", "t", "Test"),
                new ParsingRule("t2", "t2", "Title")
            };

            var urlsList = new List<string>
            {
                "http://example.com"
            };

            var fileServiceMock = new Mock<FileService>();
            fileServiceMock.Setup(x => x.ExportToJson(It.IsAny<List<Dictionary<string, string>>>(), files["Scraped values json"]))
                .Returns("Successfuly saved!");
            fileServiceMock.Setup(x => x.ExportToJson(It.IsAny<ParsingRule[]>(), files["Rules"]))
                .Returns("Successfuly saved!");
            fileServiceMock.Setup(x => x.ImportFromJson<ParsingRule[]>(files["Rules"]))
                .Returns(rulesList.ToArray());
            fileServiceMock.Setup(x => x.ImportFromJson<string[]>(files["Links"]))
                .Returns(urlsList.ToArray());

            WebScraper webScraper = new WebScraper(consoleMok.Object, requestServiceMock.Object, fileServiceMock.Object);

            Assert.AreEqual(0, webScraper.Start());
        }
    }
}
