namespace BudgetScraper
{
    public class WebScraperToolsRequest
    {
        public string? ServiceName { get; set; }
        public string? Month { get; set; }
        public string? Day { get; set; }
        public DateTime InsertedTime { get; set; }
    }
}