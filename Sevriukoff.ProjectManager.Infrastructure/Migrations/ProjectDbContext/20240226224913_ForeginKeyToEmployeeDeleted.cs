using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sevriukoff.ProjectManager.Infrastructure.Migrations.ProjectDbContext
{
    /// <inheritdoc />
    public partial class ForeginKeyToEmployeeDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectEmployees");

            migrationBuilder.DropTable(
                name: "EmployeeReference");

            migrationBuilder.CreateTable(
                name: "ProjectEmployee",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEmployee", x => new { x.ProjectId, x.EmployeeId });
                    table.ForeignKey( // Добавил в ручную
                        name: "FK_ProjectEmployee_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectEmployee");

            migrationBuilder.CreateTable(
                name: "EmployeeReference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeReference", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectEmployees",
                columns: table => new
                {
                    EmployeesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEmployees", x => new { x.EmployeesId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectEmployees_EmployeeReference_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "EmployeeReference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectEmployees_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEmployees_ProjectId",
                table: "ProjectEmployees",
                column: "ProjectId");
        }
    }
}
