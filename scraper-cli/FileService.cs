using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace scraper_cli
{
    public static class FileService
    {
        public static void ExportToJson(object parsingRules, string path)
        {
            string jsonString = JsonConvert.SerializeObject(parsingRules);
            File.WriteAllText(path, jsonString);
        }

        public static T ImportFromJson<T>(string path)
        {
            string jsonString = File.ReadAllText(path);
            T Tobject = JsonConvert.DeserializeObject<T>(jsonString);
            return Tobject;
        }

        internal static void ExportRawContent(string content, string path)
        {
            File.WriteAllText(path, content);
        }

        internal static void ExportToCsv(List<Dictionary<string, string>> scrapedValuesList, string path)
        {
            StringBuilder sb = new StringBuilder();
            //Headers
            sb.AppendJoin(';', scrapedValuesList[0].Keys.ToArray());

            foreach (var scrapedValues in scrapedValuesList)
            {
                sb.Append('\n');
                sb.AppendJoin(';', scrapedValues.Values.ToArray());
            }
            File.WriteAllText(path, sb.ToString(),Encoding.UTF8);
        }
    }
}
