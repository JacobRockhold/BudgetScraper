namespace BudgetScraper
{
    public class WebScraperToolsResult
    {
        public string? ServiceName { get; set; }
        public bool? Success { get; set; }
        public bool Error { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorStackTrace { get; set; }
        public string? UtilityID { get; internal set; }
    }
}