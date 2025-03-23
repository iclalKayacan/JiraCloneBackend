using JiraCloneBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JiraCloneBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
