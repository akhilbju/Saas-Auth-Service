using Microsoft.EntityFrameworkCore;

public class TimeLogRepository : ITimelogRepository
{
    private readonly AppDbContext _context;
    public TimeLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public void DeleteLog(Timelog Log)
    {
        _context.Timelogs.Remove(Log);
        _context.SaveChanges();
    }

    public void CreateLog(Timelog Log)
    {
        _context.Timelogs.Add(Log);
        _context.SaveChanges();
    }

    public List<Timelog> GetAllLog(int TaskId)
    {
        return _context.Timelogs.Where(x=>x.TaskId == TaskId).ToList();
    }
}