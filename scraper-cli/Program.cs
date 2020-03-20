using System;
using System.Collections.Generic;
using System.IO;

namespace scraper_cli
{
    public class Program
    {
        public static string[] MenuOptions =
        {
            "Get raw page content",
            "Parse with active rules",
            "Create parsing rule",
            "Import rules",
            "Export rules",
            "Display rules",
            "Exit"
        };

        public static string[] RuleExportOptions =
        {
            "Export to JSON",
            "Cancel (or any other key)"
        };

        public static string[] RuleImportOptions =
        {
            "Import from JSON",
            "Cancel (or any other key)"
        };

        public static string[] RawContentOptions =
        {
            "Export to file",
            "Write to console",
            "Cancel (or any other key)"
        };

        public static string[] ScrapedValuesOptions =
        {
            "Write to console",
            "Export to csv",
            "Export to JSON",
            "Cancel (or any other key)"
        };

        public static string[] UrlsInputOptions =
        {
            "Import from JSON",
            "Read from console",
            "Cancel (or any other key)"
        };

        public static List<ParsingRule> ParsingRules = new List<ParsingRule>();

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to web scraper!");

            string url, response, path;

            bool isWorking = true;

            while (isWorking)
            {
                ShowOptions(MenuOptions);
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Input URL:");
                        url = Console.ReadLine();
                        response = RequestService.SendRequest(url);
                        if (response == null)
                        {
                            Console.WriteLine("Couldn't get response");
                        }
                        else
                        {
                            ShowOptions(RawContentOptions);
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    Console.Write("Please specify file path: ");
                                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Console.ReadLine());
                                    FileService.ExportRawContent(response, path);
                                    Console.Clear();
                                    Console.WriteLine("===== Successfully saved =====\n");
                                    break;

                                case "2":
                                    Console.WriteLine("----------------Start of the page---------------");
                                    Console.WriteLine(response);
                                    Console.WriteLine("-----------------End of the page----------------");
                                    break;

                                default:
                                    break;
                            }
                        }

                        break;

                    case "2":
                        ShowOptions(UrlsInputOptions);
                        string input = Console.ReadLine();
                        if (input != "1" && input != "2")
                        {
                            break;
                        }
                        List<string> urls;
                        switch (input)
                        {
                            case "1":
                                Console.Write("Please specify file path: ");
                                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Console.ReadLine());
                                urls = new List<string>(FileService.ImportFromJson<string[]>(path));
                                break;
                            default:
                                urls = new List<string>();
                                Console.WriteLine("Input URLs (empty to end):");
                                while ((url = Console.ReadLine()) != "")
                                {
                                    urls.Add(url);
                                }
                                break;
                        }

                        List<Dictionary<string, string>> scrapedValuesList = new List<Dictionary<string, string>>();
                        foreach (string item in urls)
                        {
                            Dictionary<string, string> scrapedValues = ProcessURL(item);
                            if (scrapedValues != null)
                            {
                                scrapedValuesList.Add(scrapedValues);
                            }
                        }

                        ShowOptions(ScrapedValuesOptions);
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.WriteLine("Scraped values:");
                                foreach (var item in ParsingRules)
                                {
                                    Console.Write($"{item.title}                  | ");
                                }
                                Console.WriteLine();
                                foreach (var item in scrapedValuesList)
                                {
                                    foreach (var field in item)
                                    {
                                        Console.Write($"{field.Value} | ");
                                    }
                                    Console.WriteLine();
                                }
                                break;
                            case "2":
                                Console.Write("Please specify file path: ");
                                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Console.ReadLine());
                                FileService.ExportToCsv(scrapedValuesList, path);
                                Console.Clear();
                                Console.WriteLine("===== Successfully saved =====\n");
                                break;
                            case "3":
                                Console.Write("Please specify file path: ");
                                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Console.ReadLine());
                                FileService.ExportToJson(scrapedValuesList, path);
                                Console.Clear();
                                Console.WriteLine("===== Successfully saved =====\n");
                                break;
                            default:
                                break;
                        }

                        Console.WriteLine();
                        break;

                    case "3":
                        Console.Write("Enter title: ");
                        string title = Console.ReadLine();

                        Console.Write("Enter prefix: ");
                        string prefix = Console.ReadLine();

                        Console.Write("Enter suffix: ");
                        string sufffix = Console.ReadLine();

                        Console.Write("Enter description: ");
                        string description = Console.ReadLine();

                        ParsingRules.Add(new ParsingRule(prefix, sufffix, title, description));
                        Console.Clear();
                        Console.WriteLine("===== Rule saved! =====\n");
                        break;

                    case "4":
                        ShowOptions(RuleImportOptions);
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.Write("Please specify file path: ");
                                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Console.ReadLine());
                                ParsingRules = new List<ParsingRule>(FileService.ImportFromJson<ParsingRule[]>(path));
                                Console.Clear();
                                Console.WriteLine("===== Successfully loaded =====\n");
                                break;
                            default:
                                break;
                        }
                        break;

                    case "5":
                        ShowOptions(RuleExportOptions);
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.Write("Please specify file path: ");
                                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Console.ReadLine());
                                FileService.ExportToJson(ParsingRules.ToArray(), path);
                                Console.Clear();
                                Console.WriteLine("===== Successfully saved =====\n");
                                break;
                            default:
                                break;
                        }
                        break;

                    case "6":
                        Console.WriteLine("Active rules:");
                        foreach (var item in ParsingRules)
                        {
                            Console.WriteLine($"Title: {item.title}\nDescription: {item.description}\n");
                        }
                        break;

                    case "7":
                        isWorking = false;
                        break;

                    default:
                        Console.WriteLine("Wrong input");
                        break;
                }
            }
        }

        private static Dictionary<string, string> ProcessURL(string url)
        {
            string response = RequestService.SendRequest(url);
            return response != null ? ParsePage(response, ParsingRules) : null;
        }

        private static Dictionary<string, string> ParsePage(string pageContent, List<ParsingRule> parsingRules)
        {
            Dictionary<string, string> scrapedValues = new Dictionary<string, string>();
            foreach (var item in parsingRules)
            {
                scrapedValues.Add(item.title, ExtractSrtingBetween(pageContent, item.prefix, item.suffix));
            }
            return scrapedValues;
        }

        private static string ExtractSrtingBetween(string sourceString, string prefix, string suffix)
        {
            int pFrom = sourceString.IndexOf(prefix) + prefix.Length;
            string restString = sourceString.Substring(pFrom);
            int pTo = restString.IndexOf(suffix);
            return (pFrom - prefix.Length != -1 && pTo != -1) ? restString.Substring(0, pTo) : "";
        }

        private static void ShowOptions(string[] options)
        {
            Console.WriteLine("Available options:");
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            Console.Write("> ");
        }
    }
}
