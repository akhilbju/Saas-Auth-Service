public class ProjectStatusRepository : IProjectStatusRepository
{
    private readonly AppDbContext _context;
    public ProjectStatusRepository(AppDbContext context)
    {
        _context = context;
    }
    public void AddStatus(ProjectStatuses statuses)
    {
        _context.Statuses.Add(statuses);
        _context.SaveChanges();
    }
}