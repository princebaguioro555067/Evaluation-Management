using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluationInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EvaluationDb : Migration
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
                name: "EvaluationPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KpmScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EvaluationPeriodId = table.Column<int>(type: "int", nullable: false),
                    KpmType = table.Column<int>(type: "int", nullable: false),
                    Target = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Actual = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    AchievementRate = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Points = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpmScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KpmScores_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KpmScores_EvaluationPeriods_EvaluationPeriodId",
                        column: x => x.EvaluationPeriodId,
                        principalTable: "EvaluationPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EvaluationPeriodId = table.Column<int>(type: "int", nullable: false),
                    Company = table.Column<int>(type: "int", nullable: false),
                    ReqsReceived = table.Column<int>(type: "int", nullable: false),
                    LatePayments = table.Column<int>(type: "int", nullable: false),
                    DaysCountOfDelay = table.Column<int>(type: "int", nullable: false),
                    AccuracyReqsReceived = table.Column<int>(type: "int", nullable: false),
                    WrongPaymentOrPayee = table.Column<int>(type: "int", nullable: false),
                    DoublePayments = table.Column<int>(type: "int", nullable: false),
                    OtherErrors = table.Column<int>(type: "int", nullable: false),
                    ReturnedPaymentsDueToError = table.Column<int>(type: "int", nullable: false),
                    CvReqsReceived = table.Column<int>(type: "int", nullable: false),
                    CvOtherErrors = table.Column<int>(type: "int", nullable: false),
                    CancelledCvsOrCheques = table.Column<int>(type: "int", nullable: false),
                    ComplaintsFromStore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentRecords_EvaluationPeriods_EvaluationPeriodId",
                        column: x => x.EvaluationPeriodId,
                        principalTable: "EvaluationPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReconReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EvaluationPeriodId = table.Column<int>(type: "int", nullable: false),
                    ReportType = table.Column<int>(type: "int", nullable: false),
                    AssignmentStore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffDeadlineDay = table.Column<int>(type: "int", nullable: false),
                    SupervisorDeadlineDay = table.Column<int>(type: "int", nullable: true),
                    DateOfFirstSubmission = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedBySupervisorDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimelinessActual = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    DateReturned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateResubmitted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccuracyActual = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReconReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReconReports_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReconReports_EvaluationPeriods_EvaluationPeriodId",
                        column: x => x.EvaluationPeriodId,
                        principalTable: "EvaluationPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SupervisorId",
                table: "Employees",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_KpmScores_EmployeeId_EvaluationPeriodId_KpmType",
                table: "KpmScores",
                columns: new[] { "EmployeeId", "EvaluationPeriodId", "KpmType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KpmScores_EvaluationPeriodId",
                table: "KpmScores",
                column: "EvaluationPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecords_EmployeeId",
                table: "PaymentRecords",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecords_EvaluationPeriodId",
                table: "PaymentRecords",
                column: "EvaluationPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_ReconReports_EmployeeId",
                table: "ReconReports",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReconReports_EvaluationPeriodId",
                table: "ReconReports",
                column: "EvaluationPeriodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KpmScores");

            migrationBuilder.DropTable(
                name: "PaymentRecords");

            migrationBuilder.DropTable(
                name: "ReconReports");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "EvaluationPeriods");
        }
    }
}
