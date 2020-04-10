using System;
using System.Collections.Generic;
using System.IO;

namespace scraper_cli
{
    public class WebScraper
    {
        private IConsole myConsole;

        public List<ParsingRule> ParsingRules;

        public WebScraper(IConsole myConsole)
        {
            this.myConsole = myConsole;
            ParsingRules = new List<ParsingRule>();
        }

        public WebScraper()
        {
            myConsole = new MyConsole();
            ParsingRules = new List<ParsingRule>();
        }

        public int Start()
        {
            myConsole.WriteLine("Welcome to web scraper!");


            string url, response, path, ruleTitle;

            ParsingRule rule;

            bool isWorking = true;

            while (isWorking)
            {
                ShowOptions(MenuOptions.MainMenu);
                switch (myConsole.ReadLine())
                {
                    case "1":
                        myConsole.WriteLine("Input URL:");
                        url = myConsole.ReadLine();
                        response = RequestService.SendRequest(url);
                        if (response == null)
                        {
                            myConsole.WriteLine("Couldn't get response");
                        }
                        else
                        {
                            ShowOptions(MenuOptions.RawContent);
                            switch (myConsole.ReadLine())
                            {
                                case "1":
                                    myConsole.Write("Please specify file path: ");
                                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), myConsole.ReadLine());
                                    FileService.ExportRawContent(response, path);
                                    myConsole.Clear();
                                    myConsole.WriteLine("===== Successfully saved =====\n");
                                    break;

                                case "2":
                                    myConsole.WriteLine("----------------Start of the page---------------");
                                    myConsole.WriteLine(response);
                                    myConsole.WriteLine("-----------------End of the page----------------");
                                    break;

                                default:
                                    break;
                            }
                        }

                        break;

                    case "2":
                        ShowOptions(MenuOptions.UrlsInput);
                        string input = myConsole.ReadLine();
                        if (input != "1" && input != "2") break;
                        List<string> urls;
                        switch (input)
                        {
                            case "1":
                                myConsole.Write("Please specify file path: ");
                                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), myConsole.ReadLine());
                                urls = new List<string>(FileService.ImportFromJson<string[]>(path));
                                break;
                            default:
                                urls = new List<string>();
                                myConsole.WriteLine("Input URLs (empty to end):");
                                while ((url = myConsole.ReadLine()) != "")
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

                        ShowOptions(MenuOptions.ScrapedValues);
                        switch (myConsole.ReadLine())
                        {
                            case "1":
                                myConsole.WriteLine("Scraped values:");
                                foreach (var item in ParsingRules)
                                {
                                    myConsole.Write($"{item.title}                  | ");
                                }
                                myConsole.WriteLine();
                                foreach (var item in scrapedValuesList)
                                {
                                    foreach (var field in item)
                                    {
                                        myConsole.Write($"{field.Value} | ");
                                    }
                                    myConsole.WriteLine();
                                }
                                break;
                            case "2":
                                myConsole.Write("Please specify file path: ");
                                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), myConsole.ReadLine());
                                FileService.ExportToCsv(scrapedValuesList, path);
                                myConsole.Clear();
                                myConsole.WriteLine("===== Successfully saved =====\n");
                                break;
                            case "3":
                                myConsole.Write("Please specify file path: ");
                                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), myConsole.ReadLine());
                                string status = FileService.ExportToJson(scrapedValuesList, path);
                                myConsole.Clear();
                                myConsole.WriteLine($"===== {status} =====\n");
                                break;
                            default:
                                break;
                        }

                        myConsole.WriteLine();
                        break;

                    case "3":
                        ShowOptions(MenuOptions.RuleManagement);
                        switch (myConsole.ReadLine())
                        {
                            case "1":
                                myConsole.Write("Enter title: ");
                                string title = myConsole.ReadLine();

                                myConsole.Write("Enter prefix: ");
                                string prefix = myConsole.ReadLine();

                                myConsole.Write("Enter suffix: ");
                                string sufffix = myConsole.ReadLine();

                                myConsole.Write("Enter description: ");
                                string description = myConsole.ReadLine();

                                ParsingRules.Add(new ParsingRule(prefix, sufffix, title, description));
                                myConsole.Clear();
                                myConsole.WriteLine("===== Rule saved! =====\n");
                                break;

                            case "2":
                                ShowOptions(MenuOptions.RuleImport);
                                switch (myConsole.ReadLine())
                                {
                                    case "1":
                                        myConsole.Write("Please specify file path: ");
                                        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), myConsole.ReadLine());
                                        ParsingRules = new List<ParsingRule>(FileService.ImportFromJson<ParsingRule[]>(path));
                                        myConsole.Clear();
                                        myConsole.WriteLine("===== Successfully loaded =====\n");
                                        break;
                                    default:
                                        break;
                                }
                                break;

                            case "3":
                                ShowOptions(MenuOptions.RuleExport);
                                switch (myConsole.ReadLine())
                                {
                                    case "1":
                                        myConsole.Write("Please specify file path: ");
                                        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), myConsole.ReadLine());
                                        string status = FileService.ExportToJson(ParsingRules.ToArray(), path);
                                        myConsole.Clear();
                                        myConsole.WriteLine($"===== {status} =====\n");
                                        break;
                                    default:
                                        break;
                                }
                                break;

                            case "4":
                                myConsole.WriteLine("Active rules:\n");
                                foreach (var item in ParsingRules)
                                {
                                    myConsole.WriteLine($"Title: {item.title}\nDescription: {item.description}\n");
                                }
                                break;

                            case "5":
                                myConsole.Write("Enter rule title: ");
                                ruleTitle = myConsole.ReadLine();
                                rule = ParsingRules.Find(rule => rule.title == ruleTitle);
                                if (rule != null)
                                {
                                    myConsole.WriteLine($"\n{rule}\n");
                                }
                                else
                                {
                                    myConsole.WriteLine("Rule not found\n");
                                }
                                break;

                            case "6":
                                myConsole.Write("Enter rule title: ");
                                ruleTitle = myConsole.ReadLine();
                                rule = ParsingRules.Find(rule => rule.title == ruleTitle);
                                if (rule != null)
                                {
                                    myConsole.Write("Enter title: ");
                                    string titleE = myConsole.ReadLine();
                                    titleE = titleE == "" ? rule.title : titleE;

                                    myConsole.Write("Enter prefix: ");
                                    string prefixE = myConsole.ReadLine();
                                    prefixE = prefixE == "" ? rule.prefix : prefixE;

                                    myConsole.Write("Enter suffix: ");
                                    string sufffixE = myConsole.ReadLine();
                                    sufffixE = sufffixE == "" ? rule.suffix : sufffixE;

                                    myConsole.Write("Enter description: ");
                                    string descriptionE = myConsole.ReadLine();
                                    descriptionE = descriptionE == "" ? rule.description : descriptionE;

                                    ParsingRules.Remove(rule);
                                    ParsingRules.Add(new ParsingRule(prefixE, sufffixE, titleE, descriptionE));
                                    myConsole.Clear();
                                    myConsole.WriteLine("===== Rule saved! =====\n");
                                    break;
                                }
                                else
                                {
                                    myConsole.WriteLine("Rule not found\n");
                                }
                                break;

                            case "7":
                                myConsole.Write("Enter rule title: ");
                                ruleTitle = myConsole.ReadLine();
                                rule = ParsingRules.Find(rule => rule.title == ruleTitle);
                                if (rule != null)
                                {
                                    ParsingRules.Remove(rule);
                                    myConsole.Clear();
                                    myConsole.WriteLine("===== Rule deleted =====\n");
                                }
                                else
                                {
                                    myConsole.WriteLine("Rule not found\n");
                                }
                                break;

                        }
                        break;

                    case "4":
                        isWorking = false;
                        break;

                    default:
                        myConsole.WriteLine("Wrong input");
                        break;
                }
            }
            return 0;
        }

        public Dictionary<string, string> ProcessURL(string url)
        {
            string response = RequestService.SendRequest(url);
            return response != null ? ParsePage(response, ParsingRules) : null;
        }

        public Dictionary<string, string> ParsePage(string pageContent, List<ParsingRule> parsingRules)
        {
            Dictionary<string, string> scrapedValues = new Dictionary<string, string>();
            foreach (var item in parsingRules)
            {
                scrapedValues.Add(item.title, ExtractSrtingBetween(pageContent, item.prefix, item.suffix));
            }
            return scrapedValues;
        }

        public string ExtractSrtingBetween(string sourceString, string prefix, string suffix)
        {
            int pFrom = sourceString.IndexOf(prefix) + prefix.Length;
            string restString = sourceString.Substring(pFrom);
            int pTo = restString.IndexOf(suffix);
            return (pFrom - prefix.Length != -1 && pTo != -1) ? restString.Substring(0, pTo) : "";
        }

        public void ShowOptions(string[] options)
        {
            myConsole.WriteLine("Available options:");
            for (int i = 0; i < options.Length; i++)
            {
                myConsole.WriteLine($"{i + 1}. {options[i]}");
            }
            myConsole.Write("> ");
        }
    }
}
