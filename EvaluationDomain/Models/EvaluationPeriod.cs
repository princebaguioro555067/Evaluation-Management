using System.ComponentModel.DataAnnotations;

namespace EvaluationDomain.Models
{
    // Represents a monthly evaluation cycle (e.g., April 2025).
    // All KPM scores, payment records, and recon reports are tied to a period.
    public class EvaluationPeriod : BaseClass
    {
        [Required]
        public int Month { get; set; }   // 1–12

        [Required]
        public int Year { get; set; }

        public virtual ICollection<KpmScore> KpmScores { get; set; } = new List<KpmScore>();
        public virtual ICollection<PaymentRecord> PaymentRecords { get; set; } = new List<PaymentRecord>();
        public virtual ICollection<ReconReport> ReconReports { get; set; } = new List<ReconReport>();

        public string Display => $"{new DateTime(Year, Month, 1):MMMM yyyy}";

        public override string ToString() => Display;
    }
}
