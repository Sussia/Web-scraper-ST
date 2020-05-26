using System;
namespace web_scraper_vue_tests
{
    public class ParsingRule
    {
        public string title { get; set; }
        public string description { get; set; }
        public string prefix { get; set; }
        public string suffix { get; set; }
        public bool isEditFormOpen { get; set; } = false;
        public bool details { get; set; } = false;
    }
}
