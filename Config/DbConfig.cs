using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CodingTracker.Config;

public class DbConfig
{
    // Build a config object, using env vars and JSON providers.
    private static IConfigurationRoot _config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build();

    public static readonly string ConnectionString = _config.GetConnectionString("DefaultConnection");
    
    
    public static void InitDb()
    {
        InitDb(ConnectionString);
    }

    private static void InitDb(string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS coding_session (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration TEXT
                );
            ";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}