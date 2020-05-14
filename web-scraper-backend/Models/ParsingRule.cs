using System;
namespace web_scraper_backend.Models
{
    public class ParsingRule
    {
        public string prefix { get; set; }
        public string suffix { get; set; }
        public string title { get; set; }

        public ParsingRule(string prefix, string suffix, string title)
        {
            this.prefix = prefix;
            this.suffix = suffix;
            this.title = title;
        }

        public ParsingRule()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is ParsingRule rule &&
                   prefix == rule.prefix &&
                   suffix == rule.suffix &&
                   title == rule.title;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(prefix, suffix, title);
        }

        public override string ToString()
        {
            return $"Title: {title}\nPrefix: {prefix}\nSuffix: {suffix}";
        }
    }
}
