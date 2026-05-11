using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationDomain.Models
{
    public enum ReconReportType
    {
        CDB = 1,                    // Staff deadline: 5th
        PayableTradNonTrade = 2,    // Staff deadline: 28th
        PrsAudit = 3,               // Staff deadline: 28th
        RentalDeposit = 4,          // Staff deadline: 10th
        PrepaidRent = 5,            // Staff deadline: 10th
        PrepaidInsurance = 6,       // Staff deadline: 10th
        EwtRemittanceCtfsi = 7,     // Staff deadline: 10th
        EwtRemittanceEmcor = 8,     // Staff deadline: 10th
        ApOthers = 9                // Staff deadline: 10th
    }

    public class ReconReport : BaseClass
    {
        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required]
        public int EvaluationPeriodId { get; set; }

        [ForeignKey(nameof(EvaluationPeriodId))]
        public virtual EvaluationPeriod EvaluationPeriod { get; set; } = null!;

        [Required]
        public ReconReportType ReportType { get; set; }

        // The store/branch assignment for this recon row
        public string? AssignmentStore { get; set; }

        // Company (e.g. "EMCOR", "CTFSI", bank name, etc.)
        public string? Company { get; set; }

        // --- TIMELINESS ---
        public int StaffDeadlineDay { get; set; }
        public int? SupervisorDeadlineDay { get; set; }

        public DateTime? DateOfFirstSubmission { get; set; }
        public DateTime? CheckedBySupervisorDate { get; set; }

        // Actual timeliness score (100 = on time, penalized per day late)
        public decimal? TimelinessActual { get; set; }

        // --- ACCURACY ---
        public DateTime? DateReturned { get; set; }
        public DateTime? DateResubmitted { get; set; }

        // Accuracy score (100 = no error, penalized per returned recon)
        public decimal? AccuracyActual { get; set; }

        public string? Remarks { get; set; }

        // --- Computed helpers ---
        [NotMapped]
        public bool IsLate =>
            DateOfFirstSubmission.HasValue &&
            DateOfFirstSubmission.Value.Day > StaffDeadlineDay;

        [NotMapped]
        public bool WasReturned => DateReturned.HasValue;

        [NotMapped]
        public string ReportTypeLabel => ReportType switch
        {
            ReconReportType.CDB => "CDB",
            ReconReportType.PayableTradNonTrade => "Payable (Trade - Nontrade)",
            ReconReportType.PrsAudit => "PRS Audit",
            ReconReportType.RentalDeposit => "Rental Deposit",
            ReconReportType.PrepaidRent => "Prepaid Rent",
            ReconReportType.PrepaidInsurance => "Prepaid Insurance",
            ReconReportType.EwtRemittanceCtfsi => "EWT Remittance - CTFSI",
            ReconReportType.EwtRemittanceEmcor => "EWT Remittance - EMCOR",
            ReconReportType.ApOthers => "AP Others",
            _ => "Unknown"
        };

        [NotMapped]
        public int DefaultStaffDeadline => ReportType switch
        {
            ReconReportType.CDB => 5,
            ReconReportType.PayableTradNonTrade => 28,
            ReconReportType.PrsAudit => 28,
            _ => 10
        };
    }
}
