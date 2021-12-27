using BudgetScraper.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace BudgetScraper
{
    public class ScrapeCommander
    {
        private readonly ScrapeCommanderResult result;


        public string? ConnectionString;

        public ScrapeCommander()
        {
            result = new ScrapeCommanderResult() { ScrapeCommanderInstance = this, ScrapeCommanderExists = true };
            AddChromeDriverToAppData();
            CreateConnectionString();
            ConnectionString = result.ConnectionString;
        }

        public static void Run()
        {
            WebScraperFactory.Start();
        }

        private void CreateConnectionString()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Life Buddy\\Data\\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            result.ConnectionString = "Data Source=" + path + "Scraper.db;";
        }

        public static string GetAppDataPathString()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Life Buddy\\";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        public bool DatabaseExists()
        {
            bool connected = false;
            // SQLite will create the database automatically if it does not already exist.
            using (IDbConnection connection = new SqliteConnection(result.ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open) connected = true;
                connection.Close();
                connection.Dispose();
            }
            return connected;
        }

        public static bool AddChromeDriverToAppData()
        {
            try
            {
                File.Copy(Directory.GetCurrentDirectory() + "\\chromedriver.exe", GetAppDataPathString() + "\\chromedriver.exe", true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\r\n\r\n" + ex.StackTrace);
                return false;
            }
        }
    }
}