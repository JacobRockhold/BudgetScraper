using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BudgetScraper
{
    public class UtilityTemplate : IWebScraper
    {
        private string name = string.Empty;
        private string appDataPath = string.Empty;
        private readonly WebScraperTools tools = new();
        private WebScraperToolsRequest? request;
        public IWebDriver? Driver { get; set; }
        public string ServiceName { get { return name; } set { name = "Utility"; } }
        public string DataPath { get { return appDataPath; } set { appDataPath = ScrapeCommander.GetAppDataPathString(); } }

        public UtilityTemplate()
        {
            Run();
        }

        public void Run()
        {
            BuildDriver();
            GetWebPage();
            TearDownDriver();
            InsertIntoTable();
        }

        public void BuildDriver()
        {
            Driver = new ChromeDriver(DataPath + "\\chromedriver.exe");
        }

        public void TearDownDriver()
        {
            if (Driver != null)
            {
                Driver.Close();
                Driver.Quit();
                Driver.Dispose();
            }
        }

        public void InsertIntoTable()
        {
            if (request != null)
            {
                tools.AddToUtilityLogs(request);
            }
        }

        // This is the only method that should require edits.
        public void GetWebPage()
        {
            request = new() { };
            throw new NotImplementedException();
        }
    }
}
