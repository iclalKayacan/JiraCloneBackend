using JiraCloneBackend.Data;
using JiraCloneBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JiraCloneBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,ProjectOwner,User")]
        [HttpGet]
        public IActionResult GetProjects()
        {
            var projects = _context.Projects.Include(p => p.Columns).ThenInclude(c => c.Tasks).ToList();
            return Ok(projects);
        }

        [Authorize(Roles = "Admin,ProjectOwner,User")]
        [HttpGet("{id}")]
        public IActionResult GetProject(int id)
        {
            var project = _context.Projects.Include(p => p.Columns).ThenInclude(c => c.Tasks).FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpGet("my-projects")]
        [Authorize(Roles = "Admin,ProjectOwner,User")]
        public async Task<IActionResult> GetMyProjects()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var myProjects = await _context.Projects
                .Include(p => p.Columns)
                    .ThenInclude(c => c.Tasks)
                .Where(p => p.UserProjects.Any(up => up.UserId == userId))
                .ToListAsync();

            return Ok(myProjects);
        }


        [Authorize(Roles = "Admin,ProjectOwner")]
        [HttpPost]
        public IActionResult CreateProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [Authorize(Roles = "Admin,ProjectOwner")]
        [HttpPost("{projectId}/columns")]
        public IActionResult CreateColumn(int projectId, Column column)
        {
            var project = _context.Projects.Include(p => p.Columns).FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound();
            }

            column.ProjectId = projectId;
            _context.Columns.Add(column);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProject), new { id = projectId }, project);
        }

        [Authorize(Roles = "Admin,ProjectOwner")]
        [HttpPost("{projectId}/columns/{columnId}/tasks")]
        public IActionResult CreateTask(int projectId, int columnId, TaskItem task)
        {
            var column = _context.Columns.Include(c => c.Tasks).FirstOrDefault(c => c.Id == columnId && c.ProjectId == projectId);
            if (column == null)
            {
                return NotFound();
            }

            task.ColumnId = columnId;
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProject), new { id = projectId }, column);
        }

        [HttpPost("{projectId}/assign-user/{userId}")]
        [Authorize(Roles = "Admin,ProjectOwner")]
        public async Task<IActionResult> AssignUserToProject(int projectId, int userId)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == projectId);
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!projectExists || !userExists)
                return NotFound("Project or User not found.");

            var userProject = new UserProject
            {
                ProjectId = projectId,
                UserId = userId
            };

            _context.UserProjects.Add(userProject);
            await _context.SaveChangesAsync();

            return Ok("User assigned to project successfully.");
        }

    }
}
