using System.Reflection;

namespace BudgetScraper
{
    public class WebScraperFactory
    {
        public static int Start()
        {
            var query = from type in Assembly.GetExecutingAssembly().GetTypes()
                        where type.IsClass && type.Namespace == "BudgetScraper.Utilities"
                        select type;
            foreach (var item in query)
            {
                Activator.CreateInstance(item);
            }
            return query.ToList().Count;
        }

        public static List<string> GetFileNames()
        {
            List<string> files = new();
            var query = from type in Assembly.GetExecutingAssembly().GetTypes()
                        where type.IsClass && type.Namespace == "BudgetScraper.Utilities"
                        select type;
            foreach (var item in query)
            {
                files.Add(item.Name);
            }
            return files;
        }
    }
}
