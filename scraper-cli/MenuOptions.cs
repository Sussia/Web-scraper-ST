using System;
namespace scraper_cli
{
    public static class MenuOptions
    {
        public static string[] MainMenu =
        {
            "Get raw page content",
            "Parse with active rules",
            "Parsing rules management",
            "Exit"
        };

        public static string[] RuleManagement =
        {
            "Create parsing rule",
            "Import rules",
            "Export rules",
            "Display rules",
            "Display specific rule",
            "Edit rule",
            "Delete rule",
        };

        public static string[] RuleExport =
        {
            "Export to JSON",
            "Cancel (or any other key)"
        };

        public static string[] RuleImport =
        {
            "Import from JSON",
            "Cancel (or any other key)"
        };

        public static string[] RawContent =
        {
            "Export to file",
            "Write to console",
            "Cancel (or any other key)"
        };

        public static string[] ScrapedValues =
        {
            "Write to console",
            "Export to csv",
            "Export to JSON",
            "Cancel (or any other key)"
        };

        public static string[] UrlsInput =
        {
            "Import from JSON",
            "Read from console",
            "Cancel (or any other key)"
        };
    }
}
