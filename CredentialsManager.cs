using BudgetScraper.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace BudgetScraper
{
    public class CredentialsManager
    {
        public ScrapeCommander _commander;

        public CredentialsManager()
        {
            _commander = new ScrapeCommander();
            EnsureTableExists();
        }

        public CredentialsResult GetCredentials(CredentialsRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            CredentialsResult result = new();

            using (IDbConnection connection = new SqliteConnection(_commander.ConnectionString))
            {
                connection.Open();
                IDataReader? reader;
                try
                {
                    reader = connection.ExecuteReader("SELECT ServiceID,ServiceName,ServiceUsername,ServicePassword FROM ServiceCredentials WHERE ServiceName=@ServiceName;",
                        new { request.ServiceName });
                }
                catch (SqliteException ex)
                {
                    return new CredentialsResult { Error = true, ErrorMessage = ex.Message, ErrorStackTrace = ex.StackTrace };
                }
                DataTable dt = new();
                dt.Load(reader);
                foreach (DataRow row in dt.Rows)
                {
                    result.ServiceID = row["ServiceID"].ToString();
                    result.ServiceName = row["ServiceName"].ToString();
                    result.ServiceUsername = row["ServiceUsername"].ToString();
                    result.ServicePassword = row["ServicePassword"].ToString();
                }
                connection.Close();
                connection.Dispose();
            }
            return result;
        }

        public CredentialsResult AddCredentials(CredentialsRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            CredentialsResult result;

            using (IDbConnection connection = new SqliteConnection(_commander.ConnectionString))
            {
                connection.Open();
                try
                {
                    var credentials = connection.Execute("INSERT INTO ServiceCredentials (ServiceName,ServiceUsername,ServicePassword) VALUES (@ServiceName, @ServiceUsername, @ServicePassword);",
                        new { request.ServiceName, request.ServiceUsername, request.ServicePassword });
                    result = GetCredentials(request);
                }
                catch (SqliteException ex)
                {
                    return new CredentialsResult { Error = true, ErrorMessage = ex.Message, ErrorStackTrace = ex.StackTrace };
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
                var creation = "CREATE TABLE IF NOT EXISTS \"ServiceCredentials\" (" +
                                    "\"ServiceID\" INTEGER NOT NULL UNIQUE," +
                                    "\"ServiceName\" TEXT NOT NULL UNIQUE," +
                                    "\"ServiceUsername\" TEXT NOT NULL," +
                                    "\"ServicePassword\" TEXT NOT NULL," +
                                    "PRIMARY KEY(\"ServiceID\" AUTOINCREMENT)" +
                                ");";
                connection.Open();
                connection.Execute(creation);
                var defaultExists = connection.Query("SELECT ServiceID,ServiceName,ServiceUsername,ServicePassword FROM ServiceCredentials WHERE ServiceName='default';").ToList();
                if (defaultExists.Count < 1)
                {
                    connection.Execute("INSERT INTO ServiceCredentials (ServiceName,ServiceUsername,ServicePassword) VALUES ('default','defaultuser','defaultpass');");
                }
                connection.Close();
                connection.Dispose();
            }
        }

        public CredentialsResult RemoveCredentials(CredentialsRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            CredentialsResult result = new();

            using (IDbConnection connection = new SqliteConnection(_commander.ConnectionString))
            {
                connection.Open();
                try
                {
                    var credentials = connection.Execute("DELETE FROM ServiceCredentials WHERE ServiceName = @ServiceName;",
                        new { request.ServiceName });
                    result.RemovedCredentials = true;
                }
                catch (SqliteException ex)
                {
                    return new CredentialsResult { Error = true, ErrorMessage = ex.Message, ErrorStackTrace = ex.StackTrace };
                }

                connection.Close();
                connection.Dispose();
            }
            return result;
        }
    }
}
