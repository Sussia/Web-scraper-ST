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

        public string text
        {
            get
            {
                return card.FindElement(By.CssSelector("div.v-card__text")).Text;
            }
        }

        public RuleForm editForm
        {
            get {
                return new RuleForm(card.FindElement(By.CssSelector("form")));
            }
        }

        public IWebElement expandButton
        {
            get {
                return card.FindElement(By.CssSelector("button.expand-button"));
            }
        }

        public IWebElement editButton
        {
            get
            {
                return card.FindElement(By.CssSelector("button.edit-button"));
            }
        }

        public IWebElement deleteButton
        {
            get {
                return card.FindElement(By.CssSelector("button.delete-button"));
            }
        }


    }
}
