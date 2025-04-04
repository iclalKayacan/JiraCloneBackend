using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JiraCloneBackend.Models;
using JiraCloneBackend.Data;
using JiraCloneBackend.Dto;
using System.IO;

namespace JiraCloneBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskItemController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/TaskItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            return task;
        }

        // POST: api/TaskItem
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItemCreateDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Assignee = dto.Assignee,
                CreatedAt = dto.CreatedAt,
                ColumnId = dto.ColumnId,
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            if (dto.UserIds != null && dto.UserIds.Any())
            {
                foreach (var userId in dto.UserIds)
                {
                    var assignment = new TaskItemAssignment
                    {
                        TaskItemId = task.Id,
                        UserId = userId
                    };
                    _context.TaskItemAssignments.Add(assignment);
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetTaskItem), new { id = task.Id }, task);
        }


        // PUT: api/TaskItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            if (id != task.Id)
                return BadRequest();

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tasks.Any(t => t.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // PATCH: api/TaskItem/5/description
        [HttpPatch("{id}/description")]
        public async Task<IActionResult> UpdateDescription(int id, [FromBody] string newDescription)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.Description = newDescription;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/TaskItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/TaskItem/5/upload
        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadFiles(int id, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("Dosya yüklenemedi: Dosyalar bulunamadı veya boş.");
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            try
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Uploads",
                    "Tasks",
                    id.ToString()
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var newAttachments = new List<TaskAttachment>();

                foreach (var file in files)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var attachment = new TaskAttachment
                    {
                        TaskItemId = id,
                        FilePath = filePath,
                        FileName = file.FileName
                    };

                    newAttachments.Add(attachment);
                }

                _context.TaskAttachments.AddRange(newAttachments);
                await _context.SaveChangesAsync();

                var fileUrls = newAttachments.Select(a =>
                    $"{Request.Scheme}://{Request.Host}/Uploads/Tasks/{id}/{Path.GetFileName(a.FilePath)}"
                ).ToList();

                return Ok(new { filePaths = fileUrls });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
