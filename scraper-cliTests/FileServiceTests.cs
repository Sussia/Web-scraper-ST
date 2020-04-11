using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scraper_cli;

namespace scraper_cliTests
{
    [TestClass]
    public class FileServiceTests
    {
        public static string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        [TestMethod]
        public void ExportToJsonTests()
        {
            string validPath = Path.Combine(homeDirectory, "Desktop/test.json");
            string status = new FileService().ExportToJson(new ParsingRule("", "", ""), validPath);
            Assert.AreEqual("Successfuly saved!", status);
            File.Delete(validPath);
        }

        [TestMethod]
        public void ExportToJsonIOExTest()
        {
            string validPath = Path.Combine(homeDirectory, "Desktop/test.json");
            FileStream fs = File.OpenWrite(validPath);
            string status = new FileService().ExportToJson(new ParsingRule("", "", ""), validPath);
            Assert.AreEqual("Error: can't access the file", status);
            fs.Close();
            File.Delete(validPath);
        }

        [TestMethod]
        public void ExportToJsonDirNotFoundExTest()
        {
            string validPath = Path.Combine(homeDirectory, "Fesktop/test.json");
            string status = new FileService().ExportToJson(new ParsingRule("", "", ""), validPath);
            Assert.AreEqual("Error: directory not found", status);
        }

        [TestMethod]
        public void ExportToJsonUnexpectedExTest()
        {
            string status = new FileService().ExportToJson(new ParsingRule("", "", ""), null);
            string expectedStatus = "Unexpected error:";
            Assert.AreEqual(expectedStatus, status.Substring(0, expectedStatus.Length));
        }

        [TestMethod]
        public void ExportToCsvSuccessfulTest()
        {
            string validPath = Path.Combine(homeDirectory, "Desktop/test.csv");
            Dictionary<string, string> testDict = new Dictionary<string, string>
            {
                {"Test key", "Test value"}
            };
            var list = new List<Dictionary<string, string>> { testDict };
            string status = new FileService().ExportToCsv(list, validPath);
            Assert.AreEqual("Successfuly saved!", status);
            File.Delete(validPath);
        }

        [TestMethod]
        public void ExportToCsvEmptyListTest()
        {
            string validPath = Path.Combine(homeDirectory, "Desktop/test.csv");
            var list = new List<Dictionary<string, string>>();
            string status = new FileService().ExportToCsv(list, validPath);
            Assert.AreEqual("Error: list is empty", status);
        }

        [TestMethod]
        public void ImportfromJsonTest()
        {
            var fs = new FileService();
            var expectedRule = new ParsingRule("a", "a", "a");
            string validPath = Path.Combine(homeDirectory, "Desktop/import_test.json");
            fs.ExportToJson(expectedRule, validPath);

            var rule = fs.ImportFromJson<ParsingRule>(validPath);

            Assert.AreEqual(expectedRule, rule);

            File.Delete(validPath);
        }

        [TestMethod]
        public void ImportfromJsonExTest()
        {
            var fs = new FileService();
            string validPath = Path.Combine(homeDirectory, "Fesktop/import_test.json");

            var rule = fs.ImportFromJson<ParsingRule>(validPath);

            Assert.AreEqual(default(ParsingRule), rule);

        }
    }
}
