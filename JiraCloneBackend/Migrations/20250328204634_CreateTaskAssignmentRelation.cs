using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JiraCloneBackend.Migrations
{
    /// <inheritdoc />
    public partial class CreateTaskAssignmentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItemUser");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "TaskItemUser",
                columns: table => new
                {
                    AssignedTasksId = table.Column<int>(type: "int", nullable: false),
                    AssignedUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemUser", x => new { x.AssignedTasksId, x.AssignedUsersId });
                    table.ForeignKey(
                        name: "FK_TaskItemUser_Tasks_AssignedTasksId",
                        column: x => x.AssignedTasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskItemUser_Users_AssignedUsersId",
                        column: x => x.AssignedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemUser_AssignedUsersId",
                table: "TaskItemUser",
                column: "AssignedUsersId");
        }
    }
}
