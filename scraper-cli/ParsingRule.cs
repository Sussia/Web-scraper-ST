using System;
namespace scraper_cli
{
    public class ParsingRule
    {
        public string prefix;
        public string suffix;
        public string title;
        public string description;

        public ParsingRule(string prefix, string suffix, string title, string description = null)
        {
            this.prefix = prefix;
            this.suffix = suffix;
            this.title = title;
            this.description = description;
        }
    }
}
