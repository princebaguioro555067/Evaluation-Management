using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationDomain.Models
{
    public enum KpmType
    {
        TimelinessOfPayment = 1,                        // Weight: 0.20, Target: 90
        AccuracyOfPayment = 2,                          // Weight: 0.20, Target: 100
        AccuracyOfCvBooking = 3,                        // Weight: 0.10, Target: 90
        NoIncidenceOfPenaltiesAndCharges = 4,           // Weight: 0.15, Target: 100
        TimelinessCompletenessAccuracyOfRecon = 5,      // Weight: 0.20, Target: 90
        NoIncidenceOfUnresolvedReconItems = 6           // Weight: 0.15, Target: 100
    }

    public class KpmScore : BaseClass
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
        public KpmType KpmType { get; set; }

        // Target % — pre-defined but stored for flexibility
        public decimal Target { get; set; }

        // Actual % — computed or manually entered from monitoring reports
        public decimal? Actual { get; set; }

        // % achievement = Actual / Target (capped at 1.0)
        public decimal? AchievementRate { get; set; }

        // Weight from template (0.10–0.20)
        public decimal Weight { get; set; }

        // Points = AchievementRate * Weight * 100
        public decimal? Points { get; set; }

        public string? Remarks { get; set; }

        [NotMapped]
        public string KpmLabel => KpmType switch
        {
            KpmType.TimelinessOfPayment => "Timeliness of Payment",
            KpmType.AccuracyOfPayment => "Accuracy of Payment",
            KpmType.AccuracyOfCvBooking => "Accuracy of CV Booking",
            KpmType.NoIncidenceOfPenaltiesAndCharges => "No incidence of Penalties & Charges",
            KpmType.TimelinessCompletenessAccuracyOfRecon => "Timeliness, Completeness, & Accuracy of Recon & Reports",
            KpmType.NoIncidenceOfUnresolvedReconItems => "No incidence of Unresolved Recon Items for 2 months",
            _ => "Unknown"
        };
    }
}
