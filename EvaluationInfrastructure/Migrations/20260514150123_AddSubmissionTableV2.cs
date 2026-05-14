using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluationInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionTableV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerNote",
                table: "Submissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerNote",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
