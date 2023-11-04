using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using CodingTracker.Config;
using CodingTracker.Models;
using ConsoleTableExt;
using Microsoft.Data.Sqlite;

namespace CodingTracker;

public class CodingSessionController
{
    // CRUD Create Read Update Delete


    #region CRUD
    
    public static void Insert()
    {
        Console.Clear();

        string date = UserInput.GetDateInput();
        string from = UserInput.GetTimeInput("StartTime");
        string to = UserInput.GetTimeInput("EndTime");

        while (DateTime.Compare(DateTime.ParseExact(from, "HH:mm", new CultureInfo("de-DE")), DateTime.ParseExact(to, "HH:mm", new CultureInfo("de-DE"))) > 0)
        {
            Console.WriteLine("\n\n'EndTime' darf nicht vor 'StartTime' liegen.");
            from = UserInput.GetTimeInput("StartTime");
            to = UserInput.GetTimeInput("EndTime");
        }
        
        string duration = CalculateDuration(from, to);

        using (var connection = new SqliteConnection(DbConfig.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO coding_session(Date, StartTime, EndTime, Duration) " +
                                   $"VALUES('{date}', '{from}', '{to}', '{duration}') ";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        Console.WriteLine("\n CodingSession successfully added.");
    }

    public static void GetAllRecords()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(DbConfig.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM coding_session";

            List<CodingSession> codingSessions = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    codingSessions.Add(
                        new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("de-DE")),
                            StartTime = DateTime.ParseExact(reader.GetString(2), "HH:mm", new CultureInfo("de-DE")),
                            EndTime   = DateTime.ParseExact(reader.GetString(3), "HH:mm", new CultureInfo("de-DE")),
                            // Duration  = DateTime.ParseExact(reader.GetString(3), "dd-MM-yy HH:mm", new CultureInfo("de-DE"))
                            Duration  = DateTime.ParseExact(reader.GetString(4), "HH:mm:ss", null, DateTimeStyles.None)
                        }
                    );
                }
            }
            else
                Console.WriteLine("No rows found.");
            
            connection.Close();
            
            // foreach (var cs in codingSessions)
            // {
            //     Console.WriteLine($"{cs.Id} | {cs.StartTime.ToString("dd-MMM yyyy - HH:mm")} | {cs.EndTime.ToString("dd-MMM yyyy - HH:mm")} | {cs.Duration.ToString("HH:mm")} ");
            // }
            
            ConsoleTableBuilder
                .From(codingSessions)
                .WithTitle("CODING-SESSIONS", ConsoleColor.DarkCyan)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
        }
    }

    public static void Update()
    {
        Console.Clear();
        GetAllRecords();

        var codingSessionId = UserInput.GetNumberInput("\n\nPlease type Id of the 'CodingSession' you would like to update. \nType '0' to get back to main menu.\n\n");

        using (var connection = new SqliteConnection(DbConfig.ConnectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_session WHERE Id = {codingSessionId} )";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {codingSessionId} doesn't exist. \n\n");
                connection.Close();
                Update();
            }

            string date = UserInput.GetDateInput();
            string from = UserInput.GetTimeInput("StartTime");
            string to   = UserInput.GetTimeInput("EndTime");

            while (DateTime.Compare(DateTime.ParseExact(from, "HH:mm", new CultureInfo("de-DE")), DateTime.ParseExact(to, "HH:mm", new CultureInfo("de-DE"))) > 0)
            {
                Console.WriteLine("'EndTime' darf nicht vor 'StartTime' liegen.");
                from = UserInput.GetTimeInput("StartTime");
                to   = UserInput.GetTimeInput("EndTime");
            }

            string duration = CalculateDuration(from, to);

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE coding_session " +
                                   $"SET Date = '{date}', StartTime = '{from}', EndTime = '{to}', Duration = '{duration}' " +
                                   $"WHERE Id = {codingSessionId} ";

            tableCmd.ExecuteNonQuery();
            Console.WriteLine($"\n\nCodingSession with Id '{codingSessionId}' successfully updated. \n\n");
            connection.Close();
        }
    }

    public static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        int codingSessionId = UserInput.GetNumberInput("\n\nPlease type Id of the 'codingSession' you would like to update. \nType '0' to get back to main menu.\n\n");

        using (var connection = new SqliteConnection(DbConfig.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM coding_session WHERE Id = {codingSessionId} ";

            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id '{codingSessionId}' doesn't exist. \n\n");
                Delete();
            }
            Console.WriteLine($"\n\nRecord with Id '{codingSessionId}' successfully deleted. \n\n");
            
            connection.Close();
        }
    }
    
    #endregion

    
    #region Methods
    private static string CalculateDuration(string startTime, string endTime)
    {
        // return (DateTime.ParseExact(endTime, "HH:mm", new CultureInfo("de-DE")) -<
        //         DateTime.ParseExact(startTime, "HH:mm", new CultureInfo("de-DE")))
        //         .ToString("HH:mm");
        
        TimeOnly from = TimeOnly.ParseExact(startTime, "HH:mm");
        TimeOnly to = TimeOnly.ParseExact(endTime, "HH:mm");

        TimeSpan result = to - from;
        
        // DateTime from;
        // DateTime to;
        // DateTime.TryParseExact(startTime, "HH:mm", new CultureInfo("de-DE"), DateTimeStyles.None, out from);
        // DateTime.TryParseExact(endTime, "HH:mm", new CultureInfo("de-DE"), DateTimeStyles.None, out to);
        //
        // string result = (to - from).ToString("HH:mm");
        
        return result.ToString();
    }
    
    #endregion
    
    
}