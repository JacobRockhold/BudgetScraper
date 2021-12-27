namespace BudgetScraper.Models
{
    public class CredentialsResult
    {
        public string? ServiceID { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceUsername { get; set; }
        public string? ServicePassword { get; set; }
        public bool Error { get; internal set; }
        public string ErrorMessage { get; internal set; }
        public string? ErrorStackTrace { get; internal set; }
        public bool RemovedCredentials { get; set; }
    }
}
