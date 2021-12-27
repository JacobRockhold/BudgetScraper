using OpenQA.Selenium;

namespace BudgetScraper
{
    public interface IWebScraper
    {
        string ServiceName { get; set; }
        string DataPath { get; set; }
        public IWebDriver? Driver { get; set; }
        public void Run();
        public void BuildDriver();
        public void TearDownDriver();
        public void GetWebPage();
        public void InsertIntoTable();
    }
}
