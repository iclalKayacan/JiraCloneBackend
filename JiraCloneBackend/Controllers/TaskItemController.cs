﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JiraCloneBackend.Models;
using JiraCloneBackend.Data;
using JiraCloneBackend.Dto;

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
            var task = await _context.Tasks.FindAsync(id);

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
                ColumnId = dto.ColumnId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

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
    }
}
