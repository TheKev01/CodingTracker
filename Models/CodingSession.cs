namespace CodingTracker.Models;

public class CodingSession
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public TimeOnly Duration { get; set; }
}