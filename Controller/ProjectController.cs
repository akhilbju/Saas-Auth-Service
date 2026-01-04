using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Saas_Auth_Service.Controller
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProjectController
    {
        /// <summary>
        /// Project Service
        /// </summary>
        private readonly IProjectServices _projectServices;
        private readonly ITaskService _taskService;

        public ProjectController(IProjectServices projectServices, ITaskService taskService)
        {
            _projectServices = projectServices;
            _taskService = taskService;
        }

        /// <summary>
        /// Create Project
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserType.MANAGER},{UserType.ADMIN}")]
        [HttpPost]
        public Response CreateProject(CreateProjectRequest request)
        {
            return _projectServices.CreateProject(request);
        }

        /// <summary>
        /// Get Projects with Pagination and Filtering
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetAllProjects> GetProjectsAsync(GetProjectRequest request)
        {
            return await _projectServices.GetProjectsAsync(request);
        }

        /// <summary>
        /// Get Project Details by Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("{projectId}")]
        public async Task<GetProjectDetails> GetProjectByIdAsync(int projectId)
        {
            return await _projectServices.GetProjectByIdAsync(projectId);
        }

        /// <summary>
        /// Create Task
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserType.MANAGER},{UserType.ADMIN}")]
        [HttpPost]
        public Response CreateTask(CreateTaskRequest request)
        {
            return _taskService.CreateTask(request);
        }

        /// <summary>
        /// Create Project Status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserType.MANAGER},{UserType.ADMIN}")]
        [HttpPost]
        public Response CreateProjectStatus(CreateProjectStatus request)
        {
            return _projectServices.CreateProjectStatus(request);
        }
        /// <summary>
        /// Delete Project Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpDelete("{statusId}")]
        public Response DeleteProjectStatus(int statusId)
        {
            var response = new Response();
            var status = _dbContext.Statuses.FirstOrDefaultAsync(s => s.StatusId == statusId).Result;
            if (status == null)
            {
                response.IsSuccess = false;
                response.Error = "Status " + ErrorMessages.NotFound;
                return response;
            }
            _dbContext.Statuses.Remove(status);
            _dbContext.SaveChanges();
            response.IsSuccess = true;
            response.Message = "Status " + SuccessMessages.DeleteSuccess;
            return response;
        }

        /// <summary>
        /// Edit Project Status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserType.MANAGER},{UserType.ADMIN}")]
        [HttpPatch]
        public Response EditProjectStatus(EditProjectStatus request)
        {
            var response = new Response();
            var status = _dbContext.Statuses.FirstOrDefaultAsync(s => s.StatusId == request.StatusId).Result;
            if (status == null)
            {
                response.IsSuccess = false;
                response.Error = "Status " + ErrorMessages.NotFound;
                return response;
            }
            if (request.Status != null)
            {
                status.Status = request.Status;
            }
            if (request.IsDefault != null)
            {
                status.IsDefault = (bool)request.IsDefault;
            }
            if (request.Position != null)
            {
                status.Position = (int)request.Position;
            }
            _dbContext.Statuses.Update(status);
            _dbContext.SaveChanges();
            response.IsSuccess = true;
            response.Message = "Status " + SuccessMessages.UpdateSuccess;
            return response;
        }

        /// <summary>
        /// To Get the Project Status
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserType.ADMIN},{UserType.MANAGER}")]
        [HttpGet("projectId")]
        public Task<List<GetProjectStatus>> GetProjectStatuses(int projectId)
        {
            var response = _dbContext.Statuses.Where(x => x.ProjectId == projectId)
                            .Select(status => new GetProjectStatus()
                            {
                                IsDefault = status.IsDefault,
                                Position = status.Position,
                                Status = status.Status,
                                StatusId = status.StatusId
                            }).ToListAsync();

            return response;
        }

        /// <summary>
        /// To get the Tasks
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("projectId")]
        public Task<List<GetTask>> GetTasks(int projectId)
        {
            var response = _dbContext.ProjectTasks.Where(x=>x.TaskId == projectId)
                            .Select(task => new GetTask()
                            {
                                Description = task.Description,
                                Name = task.Name,
                                Status = task.Status,
                                TaskId = task.TaskId,
                                Type = task.Type
                            }).ToListAsync();
            return response;
        }

    }
}