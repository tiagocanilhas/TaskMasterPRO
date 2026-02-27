using System.IO;

namespace TaskMasterPRO.Data
{
    public static class DatabaseConfig
    {
        public static string GetConnectionString()
        {
            string dbPath = Path.Combine(AppContext.BaseDirectory, "TaskMasterPRO.db");
            return $"Data Source={dbPath}";
        }
    }
}
