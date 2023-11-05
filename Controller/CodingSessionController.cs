using System.Data.Common;
using System.Diagnostics;
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
    #region CREATE
    public static void Insert()
    {
        Console.Clear();

        string date = UserInputController.GetDateInput();
        string from = UserInputController.GetTimeInput("StartTime");
        string to = UserInputController.GetTimeInput("EndTime");

        while (DateTime.Compare(DateTime.ParseExact(from, "HH:mm", new CultureInfo("de-DE")), DateTime.ParseExact(to, "HH:mm", new CultureInfo("de-DE"))) > 0)
        {
            Console.WriteLine("\n\n'EndTime' darf nicht vor 'StartTime' liegen.");
            from = UserInputController.GetTimeInput("StartTime");
            to = UserInputController.GetTimeInput("EndTime");
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

    public static void Insert(string timeFrom, string timeTo)
    {
        string date = DateTime.Now.ToString("dd-MM-yy");
        string duration = CalculateDuration(timeFrom, timeTo);

        using (var connection = new SqliteConnection(DbConfig.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO coding_session(Date, StartTime, EndTime, Duration) " +
                                   $"VALUES('{date}', '{timeFrom}', '{timeTo}', '{duration}') ";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        Console.WriteLine("\n CodingSession successfully added.");
    }
    #endregion CREATE

    #region READ
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
                            Date = DateOnly.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("de-DE")),
                            StartTime = TimeOnly.ParseExact(reader.GetString(2), "HH:mm", new CultureInfo("de-DE")),
                            EndTime   = TimeOnly.ParseExact(reader.GetString(3), "HH:mm", new CultureInfo("de-DE")),
                            Duration  = TimeOnly.ParseExact(reader.GetString(4), "HH:mm", new CultureInfo("de-DE"))
                        }
                    );
                }
            }
            else
                Console.WriteLine("No rows found.");
            
            connection.Close();
            
            ConsoleTableBuilder
                .From(codingSessions)
                .WithTitle("CODING-SESSIONS", ConsoleColor.DarkCyan)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine(TableAligntment.Center);
        }
    }
    #endregion READ

    #region UPDATE
    public static void Update()
    {
        Console.Clear();
        GetAllRecords();

        var codingSessionId = UserInputController.GetNumberInput("\n\nPlease type Id of the 'CodingSession' you would like to update. \nType '0' to get back to main menu.\n\n");

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

            string date = UserInputController.GetDateInput();
            string from = UserInputController.GetTimeInput("StartTime");
            string to   = UserInputController.GetTimeInput("EndTime");

            while (DateTime.Compare(DateTime.ParseExact(from, "HH:mm", new CultureInfo("de-DE")), DateTime.ParseExact(to, "HH:mm", new CultureInfo("de-DE"))) > 0)
            {
                Console.WriteLine("'EndTime' darf nicht vor 'StartTime' liegen.");
                from = UserInputController.GetTimeInput("StartTime");
                to   = UserInputController.GetTimeInput("EndTime");
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
    #endregion UPDATE

    #region DELETE
    public static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        int codingSessionId = UserInputController.GetNumberInput("\n\nPlease type Id of the 'codingSession' you would like to update. \nType '0' to get back to main menu.\n\n");

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
    
    #endregion DELETE
    
    #region METHODS
    private static string CalculateDuration(string startTime, string endTime)
    {
        TimeOnly from = TimeOnly.ParseExact(startTime, "HH:mm");
        TimeOnly to = TimeOnly.ParseExact(endTime, "HH:mm");
        TimeSpan result = to - from;
        
        return result.ToString()[..5];
    }
    
    #endregion


    
}