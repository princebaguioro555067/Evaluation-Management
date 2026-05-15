using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationDomain.Models
{
    public enum SubmissionStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class EvaluationSubmission : BaseClass
    {
        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        // The month and year this evaluation covers
        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        // When the staff actually submitted
        [Required]
        public DateTime SubmissionDate { get; set; }

        // Staff's performance narrative / comment
        public string Comment { get; set; } = string.Empty;

        // Score computed from submission date vs 28th deadline
        public decimal Score { get; set; }

        public SubmissionStatus Status { get; set; } = SubmissionStatus.Pending;

        // Manager review fields
        public string? ManagerComment { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public int? ReviewedByManagerId { get; set; }

        [ForeignKey(nameof(ReviewedByManagerId))]
        public virtual Employee? ReviewedByManager { get; set; }

        // ---------------------------------------------------------------
        // Computed properties — NOT stored in DB
        // ---------------------------------------------------------------

        // Deadline is always the 28th of the evaluation month
        [NotMapped]
        public DateTime Deadline => new DateTime(Year, Month, 28);

        [NotMapped]
        public bool IsOnTime => SubmissionDate.Date <= Deadline.Date;

        [NotMapped]
        public int DaysLate => IsOnTime ? 0 : (SubmissionDate.Date - Deadline.Date).Days;

        [NotMapped]
        public int DaysEarly => IsOnTime ? (Deadline.Date - SubmissionDate.Date).Days : 0;

        [NotMapped]
        public string PeriodDisplay => $"{new DateTime(Year, Month, 1):MMMM yyyy}";

        [NotMapped]
        public string StatusDisplay => Status switch
        {
            SubmissionStatus.Pending => "Pending",
            SubmissionStatus.Approved => "Approved",
            SubmissionStatus.Rejected => "Rejected",
            _ => "Unknown"
        };

        [NotMapped]
        public string EvaluationLevel => Score switch
        {
            >= 95 => "Excellent",
            >= 85 => "Great",
            >= 75 => "Good",
            >= 65 => "Fair",
            _ => "Needs Improvement"
        };

        [NotMapped]
        public string TimelinessDisplay => IsOnTime
            ? $"On Time ({DaysEarly} day{(DaysEarly == 1 ? "" : "s")} early)"
            : $"Late ({DaysLate} day{(DaysLate == 1 ? "" : "s")} after deadline)";
    }
}