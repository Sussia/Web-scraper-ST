using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace scraper_cli
{
    public class FileService
    {
        public virtual string ExportToJson(object parsingRules, string path)
        {
            string jsonString = JsonConvert.SerializeObject(parsingRules);
            return SaveToFile(jsonString, path);
        }

        public virtual T ImportFromJson<T>(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                T Tobject = JsonConvert.DeserializeObject<T>(jsonString);
                return Tobject;
            }
            catch (Exception)
            {
                return default(T);
            }

        }

        public virtual void ExportRawContent(string content, string path)
        {
            SaveToFile(content, path);
        }

        public virtual string ExportToCsv(List<Dictionary<string, string>> scrapedValuesList, string path)
        {
            if (scrapedValuesList.Count == 0) return "Error: list is empty";

            StringBuilder sb = new StringBuilder();
            //Headers
            sb.AppendJoin(';', scrapedValuesList[0].Keys.ToArray());

            foreach (var scrapedValues in scrapedValuesList)
            {
                sb.Append('\n');
                sb.AppendJoin(';', scrapedValues.Values.ToArray());
            }
            return SaveToFile(sb.ToString(), path);
        }

        private string SaveToFile(string data, string path)
        {
            try
            {
                File.WriteAllText(path, data, Encoding.UTF8);
                return "Successfuly saved!";
            }
            catch (DirectoryNotFoundException)
            {
                return "Error: directory not found";
            }
            catch (IOException)
            {
                return "Error: can't access the file";
            }
            catch (Exception ex)
            {
                return $"Unexpected error: {ex.Message}";
            }
        }
    }
}
