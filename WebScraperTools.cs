using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace BudgetScraper
{
    public class WebScraperTools
    {
        public ScrapeCommander _commander;

        public WebScraperTools()
        {
            _commander = new ScrapeCommander();
            EnsureTableExists();
        }

        public WebScraperToolsResult AddToUtilityLogs(WebScraperToolsRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            WebScraperToolsResult result;

            using (IDbConnection connection = new SqliteConnection(_commander.ConnectionString))
            {
                connection.Open();
                try
                {
                    var logs = connection.Execute("INSERT INTO UtilityDueDateLogs (ServiceName,Month,Day,DateTimeGathered) VALUES (@ServiceName,@Month,@Day,@InsertedTime);",
                        new { request.ServiceName, request.Month, request.Day, request.InsertedTime });
                    result = new WebScraperToolsResult { Success = true, ServiceName = request.ServiceName };
                }
                catch (SqliteException ex)
                {
                    return new WebScraperToolsResult { Success = false, Error = true, ErrorMessage = ex.Message, ErrorStackTrace = ex.StackTrace };
                }

                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        private void EnsureTableExists()
        {
            using (IDbConnection connection = new SqliteConnection(_commander.ConnectionString))
            {
                var creation = "CREATE TABLE IF NOT EXISTS \"UtilityDueDateLogs\" (" +
                                    "\"UtilityID\" INTEGER NOT NULL UNIQUE," +
                                    "\"ServiceName\" TEXT NOT NULL," +
                                    "\"Month\" TEXT NOT NULL," +
                                    "\"Day\" TEXT NOT NULL," +
                                    "\"DateTimeGathered\" INTEGER NOT NULL," +
                                    "PRIMARY KEY(\"UtilityID\" AUTOINCREMENT)" +
                                ");";
                connection.Open();
                connection.Execute(creation);
                var defaultExists = connection.Query("SELECT UtilityID,Month,Day,DateTimeGathered FROM UtilityDueDateLogs WHERE ServiceName='default';").ToList();
                if (defaultExists.Count < 1)
                {
                    connection.Execute("INSERT INTO UtilityDueDateLogs (ServiceName,Month,Day,DateTimeGathered) VALUES ('default','Dec','22',202112220732);");
                }
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
