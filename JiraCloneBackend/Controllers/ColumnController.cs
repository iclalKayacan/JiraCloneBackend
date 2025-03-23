using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JiraCloneBackend.Models;
using JiraCloneBackend.Data;
using JiraCloneBackend.Dto;

namespace JiraCloneBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColumnController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Column
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Column>>> GetColumns()
        {
            return await _context.Columns
                .Include(c => c.Tasks)
                .ToListAsync();
        }

        // GET: api/Column/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Column>> GetColumn(int id)
        {
            var column = await _context.Columns
                .Include(c => c.Tasks)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (column == null)
                return NotFound();

            return column;
        }

        // POST: api/Column
        [HttpPost]
        public async Task<ActionResult<Column>> CreateColumn(CreateColumnDto dto)
        {
            var column = new Column
            {
                Title = dto.Title,
                ProjectId = dto.ProjectId
            };

            _context.Columns.Add(column);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColumn), new { id = column.Id }, column);
        }


        // PUT: api/Column/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColumn(int id, Column column)
        {
            if (id != column.Id)
                return BadRequest();

            _context.Entry(column).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Columns.Any(c => c.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Column/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColumn(int id)
        {
            var column = await _context.Columns.FindAsync(id);
            if (column == null)
                return NotFound();

            _context.Columns.Remove(column);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
