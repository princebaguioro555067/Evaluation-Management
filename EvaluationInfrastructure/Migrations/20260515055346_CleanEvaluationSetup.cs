using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluationInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CleanEvaluationSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KpmScores_Employees_EmployeeId",
                table: "KpmScores");

            migrationBuilder.DropForeignKey(
                name: "FK_KpmScores_EvaluationPeriods_EvaluationPeriodId",
                table: "KpmScores");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRecords_Employees_EmployeeId",
                table: "PaymentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRecords_EvaluationPeriods_EvaluationPeriodId",
                table: "PaymentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ReconReports_Employees_EmployeeId",
                table: "ReconReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ReconReports_EvaluationPeriods_EvaluationPeriodId",
                table: "ReconReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReconReports",
                table: "ReconReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentRecords",
                table: "PaymentRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KpmScores",
                table: "KpmScores");

            migrationBuilder.DropIndex(
                name: "IX_KpmScores_EmployeeId_EvaluationPeriodId_KpmType",
                table: "KpmScores");

            migrationBuilder.RenameTable(
                name: "ReconReports",
                newName: "ReconReport");

            migrationBuilder.RenameTable(
                name: "PaymentRecords",
                newName: "PaymentRecord");

            migrationBuilder.RenameTable(
                name: "KpmScores",
                newName: "KpmScore");

            migrationBuilder.RenameIndex(
                name: "IX_ReconReports_EvaluationPeriodId",
                table: "ReconReport",
                newName: "IX_ReconReport_EvaluationPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_ReconReports_EmployeeId",
                table: "ReconReport",
                newName: "IX_ReconReport_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRecords_EvaluationPeriodId",
                table: "PaymentRecord",
                newName: "IX_PaymentRecord_EvaluationPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRecords_EmployeeId",
                table: "PaymentRecord",
                newName: "IX_PaymentRecord_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_KpmScores_EvaluationPeriodId",
                table: "KpmScore",
                newName: "IX_KpmScore_EvaluationPeriodId");

            migrationBuilder.AlterColumn<decimal>(
                name: "TimelinessActual",
                table: "ReconReport",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AccuracyActual",
                table: "ReconReport",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "KpmScore",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Target",
                table: "KpmScore",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Points",
                table: "KpmScore",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Actual",
                table: "KpmScore",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AchievementRate",
                table: "KpmScore",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldPrecision: 5,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReconReport",
                table: "ReconReport",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentRecord",
                table: "PaymentRecord",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KpmScore",
                table: "KpmScore",
                column: "Id");

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
                name: "IX_KpmScore_EmployeeId",
                table: "KpmScore",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationSubmissions_EmployeeId_Month_Year",
                table: "EvaluationSubmissions",
                columns: new[] { "EmployeeId", "Month", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationSubmissions_ReviewedByManagerId",
                table: "EvaluationSubmissions",
                column: "ReviewedByManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_KpmScore_Employees_EmployeeId",
                table: "KpmScore",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KpmScore_EvaluationPeriods_EvaluationPeriodId",
                table: "KpmScore",
                column: "EvaluationPeriodId",
                principalTable: "EvaluationPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRecord_Employees_EmployeeId",
                table: "PaymentRecord",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRecord_EvaluationPeriods_EvaluationPeriodId",
                table: "PaymentRecord",
                column: "EvaluationPeriodId",
                principalTable: "EvaluationPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReconReport_Employees_EmployeeId",
                table: "ReconReport",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReconReport_EvaluationPeriods_EvaluationPeriodId",
                table: "ReconReport",
                column: "EvaluationPeriodId",
                principalTable: "EvaluationPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KpmScore_Employees_EmployeeId",
                table: "KpmScore");

            migrationBuilder.DropForeignKey(
                name: "FK_KpmScore_EvaluationPeriods_EvaluationPeriodId",
                table: "KpmScore");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRecord_Employees_EmployeeId",
                table: "PaymentRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRecord_EvaluationPeriods_EvaluationPeriodId",
                table: "PaymentRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_ReconReport_Employees_EmployeeId",
                table: "ReconReport");

            migrationBuilder.DropForeignKey(
                name: "FK_ReconReport_EvaluationPeriods_EvaluationPeriodId",
                table: "ReconReport");

            migrationBuilder.DropTable(
                name: "EvaluationSubmissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReconReport",
                table: "ReconReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentRecord",
                table: "PaymentRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KpmScore",
                table: "KpmScore");

            migrationBuilder.DropIndex(
                name: "IX_KpmScore_EmployeeId",
                table: "KpmScore");

            migrationBuilder.RenameTable(
                name: "ReconReport",
                newName: "ReconReports");

            migrationBuilder.RenameTable(
                name: "PaymentRecord",
                newName: "PaymentRecords");

            migrationBuilder.RenameTable(
                name: "KpmScore",
                newName: "KpmScores");

            migrationBuilder.RenameIndex(
                name: "IX_ReconReport_EvaluationPeriodId",
                table: "ReconReports",
                newName: "IX_ReconReports_EvaluationPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_ReconReport_EmployeeId",
                table: "ReconReports",
                newName: "IX_ReconReports_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRecord_EvaluationPeriodId",
                table: "PaymentRecords",
                newName: "IX_PaymentRecords_EvaluationPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRecord_EmployeeId",
                table: "PaymentRecords",
                newName: "IX_PaymentRecords_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_KpmScore_EvaluationPeriodId",
                table: "KpmScores",
                newName: "IX_KpmScores_EvaluationPeriodId");

            migrationBuilder.AlterColumn<decimal>(
                name: "TimelinessActual",
                table: "ReconReports",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AccuracyActual",
                table: "ReconReports",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "KpmScores",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Target",
                table: "KpmScores",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Points",
                table: "KpmScores",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Actual",
                table: "KpmScores",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AchievementRate",
                table: "KpmScores",
                type: "decimal(5,4)",
                precision: 5,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReconReports",
                table: "ReconReports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentRecords",
                table: "PaymentRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KpmScores",
                table: "KpmScores",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_KpmScores_EmployeeId_EvaluationPeriodId_KpmType",
                table: "KpmScores",
                columns: new[] { "EmployeeId", "EvaluationPeriodId", "KpmType" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KpmScores_Employees_EmployeeId",
                table: "KpmScores",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KpmScores_EvaluationPeriods_EvaluationPeriodId",
                table: "KpmScores",
                column: "EvaluationPeriodId",
                principalTable: "EvaluationPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRecords_Employees_EmployeeId",
                table: "PaymentRecords",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRecords_EvaluationPeriods_EvaluationPeriodId",
                table: "PaymentRecords",
                column: "EvaluationPeriodId",
                principalTable: "EvaluationPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReconReports_Employees_EmployeeId",
                table: "ReconReports",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReconReports_EvaluationPeriods_EvaluationPeriodId",
                table: "ReconReports",
                column: "EvaluationPeriodId",
                principalTable: "EvaluationPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
