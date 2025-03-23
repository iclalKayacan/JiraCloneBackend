using JiraCloneBackend.Data;
using JiraCloneBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public IActionResult GetProjects()
        {
            var projects = _context.Projects.Include(p => p.Columns).ThenInclude(c => c.Tasks).ToList();
            return Ok(projects);
        }

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

        [HttpPost]
        public IActionResult CreateProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

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
    }
}
