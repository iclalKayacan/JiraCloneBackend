using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JiraCloneBackend.Models;
using JiraCloneBackend.Data;

namespace JiraCloneBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // Tüm kullanıcıları getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // ID ile kullanıcı getir
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }
    }
}
