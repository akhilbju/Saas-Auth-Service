using System.Security.Claims;

public class TaskService : ITaskService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

    public TaskService(IProjectRepository projectRepository,
                       ITaskRepository taskRepository,IHttpContextAccessor httpContextAccessor)
    {
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public Response CreateTask(CreateTaskRequest request)
    {
        var response = new Response();
        var project = _projectRepository.GetByIdAsyncIncludeStatus(request.ProjectId).Result;
        if (project == null)
        {
            response.IsSuccess = false;
            response.Error = ErrorMessages.ProjectError;
            return response;
        }

        ProjectTask newTask = new()
        {
            AssignedTo = request.AssignedTo,
            Name = request.Name,
            Description = request.Description,
            Type = request.Type,
            Duration = request.Duration,
            ProjectId = request.ProjectId,
            CreatedAt = DateTime.UtcNow,
            Project = project,
            CreatedById = Convert.ToInt32(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)),
            Status = project.Status.Where(s => s.IsDefault).FirstOrDefault()!.StatusId,
        };
        _taskRepository.AddTask(newTask);
        response.IsSuccess = true;
        response.Message = "Task" + SuccessMessages.CreationSuccess;
        return response;
    }
}