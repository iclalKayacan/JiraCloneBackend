using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JiraCloneBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachmentToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Tasks");
        }
    }
}
