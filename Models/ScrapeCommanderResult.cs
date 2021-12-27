namespace BudgetScraper.Models
{
    public class ScrapeCommanderResult
    {
        public bool ScrapeCommanderExists { get; set; }
        public ScrapeCommander? ScrapeCommanderInstance { get; set; }
        public string? ConnectionString { get; set; }
    }
}