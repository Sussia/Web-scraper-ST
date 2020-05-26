using System;
using OpenQA.Selenium;

namespace web_scraper_vue_tests
{
    public class RuleForm
    {
        public IWebElement form;

        public RuleForm(IWebElement form)
        {
            this.form = form;
        }

        public IWebElement title {
            get {
                return form.FindElement(By.CssSelector("div.title-field")).FindElement(By.CssSelector("input"));
            }
        }

        public IWebElement description
        {
            get
            {
                return form.FindElement(By.CssSelector("div.description-field")).FindElement(By.CssSelector("input"));
            }
        }

        public IWebElement perfix
        {
            get
            {
                return form.FindElement(By.CssSelector("div.prefix-field")).FindElement(By.CssSelector("input"));
            }
        }

        public IWebElement suffix
        {
            get
            {
                return form.FindElement(By.CssSelector("div.suffix-field")).FindElement(By.CssSelector("input"));
            }
        }

        public IWebElement saveButton
        {
            get
            {
                return form.FindElement(By.CssSelector("button.save-button"));
            }
        }

        public IWebElement cancelButton
        {
            get
            {
                return form.FindElement(By.CssSelector("div.title-field")).FindElement(By.CssSelector("input"));
            }
        }
    }
}
