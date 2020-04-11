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

        public override bool Equals(object obj)
        {
            return obj is ParsingRule rule &&
                   prefix == rule.prefix &&
                   suffix == rule.suffix &&
                   title == rule.title &&
                   description == rule.description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(prefix, suffix, title, description);
        }

        public override string ToString()
        {
            return $"Title: {title}\nDescription: {description}\nPrefix: {prefix}\nSuffix: {suffix}";
        }
    }
}
