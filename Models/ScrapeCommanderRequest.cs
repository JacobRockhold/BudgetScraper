namespace BudgetScraper.Models
{
    public class ScrapeCommanderRequest
    {
        public bool ScrapeCommanderExists { get; set; }
        public ScrapeCommander? ScrapeCommanderInstance { get; set; }
    }
}