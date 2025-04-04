﻿// <auto-generated />
using System;
using JiraCloneBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JiraCloneBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250328203306_AddTaskItemAssignmentTable")]
    partial class AddTaskItemAssignmentTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JiraCloneBackend.Models.Column", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Columns");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.TaskAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaskItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TaskItemId");

                    b.ToTable("TaskAttachments");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Assignee")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ColumnId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ColumnId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.TaskItemAssignment", b =>
                {
                    b.Property<int>("TaskItemId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TaskItemId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("TaskItemAssignments");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskItemUser", b =>
                {
                    b.Property<int>("AssignedTasksId")
                        .HasColumnType("int");

                    b.Property<int>("AssignedUsersId")
                        .HasColumnType("int");

                    b.HasKey("AssignedTasksId", "AssignedUsersId");

                    b.HasIndex("AssignedUsersId");

                    b.ToTable("TaskItemUser");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.Column", b =>
                {
                    b.HasOne("JiraCloneBackend.Models.Project", "Project")
                        .WithMany("Columns")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.TaskAttachment", b =>
                {
                    b.HasOne("JiraCloneBackend.Models.TaskItem", "TaskItem")
                        .WithMany("Attachments")
                        .HasForeignKey("TaskItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaskItem");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.TaskItem", b =>
                {
                    b.HasOne("JiraCloneBackend.Models.Column", "Column")
                        .WithMany("Tasks")
                        .HasForeignKey("ColumnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Column");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.TaskItemAssignment", b =>
                {
                    b.HasOne("JiraCloneBackend.Models.TaskItem", "TaskItem")
                        .WithMany("TaskItemAssignments")
                        .HasForeignKey("TaskItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JiraCloneBackend.Models.User", "User")
                        .WithMany("TaskItemAssignments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaskItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskItemUser", b =>
                {
                    b.HasOne("JiraCloneBackend.Models.TaskItem", null)
                        .WithMany()
                        .HasForeignKey("AssignedTasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JiraCloneBackend.Models.User", null)
                        .WithMany()
                        .HasForeignKey("AssignedUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JiraCloneBackend.Models.Column", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.Project", b =>
                {
                    b.Navigation("Columns");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.TaskItem", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("TaskItemAssignments");
                });

            modelBuilder.Entity("JiraCloneBackend.Models.User", b =>
                {
                    b.Navigation("TaskItemAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
