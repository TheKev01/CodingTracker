using System.Collections.Immutable;
using CodingTracker.Config;


namespace CodingTracker;

public static class Program
{
    
    public static void Main(string[] args)
    {
        DbConfig.InitDb();
        
        UserInputController.GetUserInput();
    }
    
    
    
    
}
