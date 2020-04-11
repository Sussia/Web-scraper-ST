using System;
using System.Collections.Generic;
using System.IO;

namespace scraper_cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebScraper ws = new WebScraper(new MyConsole(), new RequestService());
            ws.Start();
        }
    }
}
