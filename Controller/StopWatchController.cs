using System.Diagnostics;

namespace CodingTracker;

public class StopWatchController
{
    public static void Timer()
    {
        Console.Clear();
        string timeFrom = "";
        string timeTo = "";
        Stopwatch stopwatch = new Stopwatch();

        Console.WriteLine("\n\n ### STOP-WATCH ### ");
        Console.WriteLine("Type '0' to get back to main menu.");
        Console.WriteLine("Type '1' to start timer.");
        Console.WriteLine("Type '2' to stop timer.");
        Console.WriteLine("Type '3' to show timer.");
        Console.WriteLine("Type '4' to insert codingSession.");
        Console.WriteLine("---------------------------------------");
        
        bool endStopWatch = false;
        while (!endStopWatch)
        {
            Console.Write("> ");
            
            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    endStopWatch = true;
                    break;
                case "1":
                    stopwatch.Start();
                    timeFrom = DateTime.Now.ToString("HH:mm");
                    Console.WriteLine($"Timer started at '{timeFrom}'.");
                    break;
                case "2":
                    stopwatch.Stop();
                    timeTo = DateTime.Now.ToString("HH:mm");
                    Console.WriteLine($"Timer stopped at '{timeTo}'.");
                    break;
                case "3":
                    string duration = stopwatch.Elapsed.ToString()[..8];
                    Console.WriteLine($"Current time: '{duration}'.");
                    break;
                case "4":
                    if (stopwatch.IsRunning)
                        stopwatch.Stop();
                    if (timeFrom == "" || timeTo == "")
                        Console.WriteLine("'timeFrom' and 'timeTo' cannot be empty.");
                    CodingSessionController.Insert(timeFrom, timeTo);
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
        
        UserInputController.GetUserInput();
    }
    
    
}