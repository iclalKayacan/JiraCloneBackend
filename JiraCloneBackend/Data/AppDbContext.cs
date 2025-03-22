using JiraCloneBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JiraCloneBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
    }
}
