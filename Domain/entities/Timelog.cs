public class Timelog
{
    public int TimelogId { get; set; }
    public int CreatedBy { get; set; }
    public int TaskId { get; set; }
    public TimeSpan Time { get; set; }
    public DateOnly Date { get; set; }
    public ProjectTask Task { get; set; }
}