using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scraper_cli;

namespace scraper_cliTests
{
    [TestClass]
    public class RequestServiceTests
    {
        [TestMethod]
        public void RequestUriFormatExTest()
        {
            string url = "test";
            string response = new RequestService().SendRequest(url);
            Assert.AreEqual($"Error: can't parse url: {url}", response);
        }

        [TestMethod]
        public void RequestUnaccessableResourceExTest()
        {
            string url = "http://a";
            string response = new RequestService().SendRequest(url);
            Assert.AreEqual($"Error: can't access resourse: {url}", response);
        }
    }
}
