﻿using JiraCloneBackend.Models;
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
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<TaskItemAssignment> TaskItemAssignments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItemAssignment>()
                .HasKey(t => new { t.TaskItemId, t.UserId });

            modelBuilder.Entity<TaskItemAssignment>()
                .HasOne(t => t.TaskItem)
                .WithMany(t => t.TaskItemAssignments)
                .HasForeignKey(t => t.TaskItemId);

            
        }


    }

}
