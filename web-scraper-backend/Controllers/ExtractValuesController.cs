using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web_scraper_backend.Models;
using web_scraper_backend.Services;

namespace web_scraper_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtractValuesController : ControllerBase
    {
        private RequestService requestService = new RequestService();

        private readonly ILogger<ExtractValuesController> _logger;

        public ExtractValuesController(ILogger<ExtractValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello, World!";
        }

        [HttpPost]
        public Dictionary<string, string> Post([FromHeader(Name ="url-to-request")]string url, List<ParsingRule> rules)
        {
            return ProcessURL(url, rules);
        }

        private Dictionary<string, string> ProcessURL(string url, List<ParsingRule> parsingRules)
        {
            string response = requestService.SendRequest(url);
            return response.StartsWith("Error") ? null : ParsePage(response, parsingRules);
        }

        private Dictionary<string, string> ParsePage(string pageContent, List<ParsingRule> parsingRules)
        {
            Dictionary<string, string> scrapedValues = new Dictionary<string, string>();
            foreach (var item in parsingRules)
            {
                scrapedValues.Add(item.title, ExtractSrtingBetween(pageContent, item.prefix, item.suffix));
            }
            return scrapedValues;
        }

        private string ExtractSrtingBetween(string sourceString, string prefix, string suffix)
        {
            int pFrom = sourceString.IndexOf(prefix) + prefix.Length;
            string restString = sourceString.Substring(pFrom);
            int pTo = restString.IndexOf(suffix);
            return (pFrom - prefix.Length != -1 && pTo != -1) ? restString.Substring(0, pTo) : "";
        }
    }
}
