using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluationInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FreshStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    GroupNumber = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ManagerComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedByManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationSubmissions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluationSubmissions_Employees_ReviewedByManagerId",
                        column: x => x.ReviewedByManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SupervisorId",
                table: "Employees",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Username",
                table: "Employees",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationSubmissions_EmployeeId_Month_Year",
                table: "EvaluationSubmissions",
                columns: new[] { "EmployeeId", "Month", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationSubmissions_ReviewedByManagerId",
                table: "EvaluationSubmissions",
                column: "ReviewedByManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvaluationSubmissions");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
