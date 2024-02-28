#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Sevriukoff.ProjectManager.Infrastructure.Migrations.ProjectDbContext
{
    /// <inheritdoc />
    public partial class EntityProjectTaskEditForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Employees_AssignedToId",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Employees_CreatedById",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Projects_ProjectId",
                table: "ProjectTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTask",
                table: "ProjectTask");

            migrationBuilder.RenameTable(
                name: "ProjectTask",
                newName: "ProjectTasks");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_ProjectId",
                table: "ProjectTasks",
                newName: "IX_ProjectTasks_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_CreatedById",
                table: "ProjectTasks",
                newName: "IX_ProjectTasks_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_AssignedToId",
                table: "ProjectTasks",
                newName: "IX_ProjectTasks_AssignedToId");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedToId",
                table: "ProjectTasks",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTasks",
                table: "ProjectTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Employees_AssignedToId",
                table: "ProjectTasks",
                column: "AssignedToId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Employees_CreatedById",
                table: "ProjectTasks",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Employees_AssignedToId",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Employees_CreatedById",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTasks",
                table: "ProjectTasks");

            migrationBuilder.RenameTable(
                name: "ProjectTasks",
                newName: "ProjectTask");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTasks_CreatedById",
                table: "ProjectTask",
                newName: "IX_ProjectTask_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTasks_AssignedToId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_AssignedToId");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedToId",
                table: "ProjectTask",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTask",
                table: "ProjectTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Employees_AssignedToId",
                table: "ProjectTask",
                column: "AssignedToId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Employees_CreatedById",
                table: "ProjectTask",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Projects_ProjectId",
                table: "ProjectTask",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
