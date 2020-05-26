using System;
using OpenQA.Selenium;

namespace web_scraper_vue_tests
{
    public class RuleCard
    {
        IWebElement card;

        public RuleCard(IWebElement card)
        {
            this.card = card;
        }

        public string title
        {
            get {
                return card.FindElement(By.CssSelector("div.v-card__title > span")).Text;
            }
        }
    }
}
