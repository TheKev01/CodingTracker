using System.Globalization;

namespace CodingTracker;

public class UserInputController
{
    
    public static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (!closeApp)
        {
            Console.WriteLine("\n\n ### CODING-TRACKER ### \n    -- MAIN MENU --");
            Console.WriteLine("\nWhat would you like to do?\n");
            Console.WriteLine("Type '0' to close Application.");
            Console.WriteLine("Type '1' to View all records.");
            Console.WriteLine("Type '2' to Insert record.");
            Console.WriteLine("Type '3' to Update record.");
            Console.WriteLine("Type '4' to Delete record.");
            Console.WriteLine("Type '5' to use StopWatch.");
            Console.WriteLine("---------------------------------------\n");
            Console.Write("> ");

            string command = Console.ReadLine();
            
            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    CodingSessionController.GetAllRecords();
                    break;
                case "2":
                    CodingSessionController.Insert();
                    break;
                case "3":
                    CodingSessionController.Update();
                    break;
                case "4":
                    CodingSessionController.Delete();
                    break;
                case "5":
                    StopWatchController.Timer();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 5.\n");
                    break;
            }
        }
    }
    
    
    public static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        Console.Write("> ");
        
        string numberInput = Console.ReadLine();
        if (numberInput == "0") 
            GetUserInput();
        
        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }
        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }

    
    public static string GetDateInput()
    {
        Console.WriteLine($"\n\nPlease insert Date: (Format: dd-MM-yy). \nType '0' to return to main menu.");
        Console.Write("> ");
        
        string dateInput = Console.ReadLine();
        if (dateInput == "0") 
            GetUserInput();
        
        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("de-DE"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date (Format: dd-MM-yy). \n\n");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }
    
    public static string GetTimeInput(string message)
    {
        Console.WriteLine($"\n\nPlease insert {message}: (Format: HH:mm). \nType '0' to return to main menu.");
        Console.Write("> ");
        
        string dateInput = Console.ReadLine();
        if (dateInput == "0") 
            GetUserInput();
        
        while (!DateTime.TryParseExact(dateInput, "HH:mm", new CultureInfo("de-DE"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date (Format: HH:mm). \n\n");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }
    
    
    // public static string GetDateInputOld(string message)
    // {
    //     Console.WriteLine($"\n\nPlease insert {message}: (Format: dd-MM-yy HH:mm). \nType '0' to return to main menu.");
    //     Console.Write("> ");
    //     
    //     string dateInput = Console.ReadLine();
    //     if (dateInput == "0") 
    //         GetUserInput();
    //     
    //     while (!DateTime.TryParseExact(dateInput, "dd-MM-yy HH:mm", new CultureInfo("de-DE"), DateTimeStyles.None, out _))
    //     {
    //         Console.WriteLine("\n\nInvalid date (Format: dd-MM-yy HH:mm). \n\n");
    //         dateInput = Console.ReadLine();
    //     }
    //     return dateInput;
    // }
}