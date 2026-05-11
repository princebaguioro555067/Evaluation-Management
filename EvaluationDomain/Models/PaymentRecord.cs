using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationDomain.Models
{
    public enum PaymentCompany
    {
        General,  // Used for Supervisors (one consolidated batch)
        EMCOR,    // Staff: EMCOR payment batch
        CTFSI     // Staff: CTFSI payment batch
    }

    public class PaymentRecord : BaseClass
    {
        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required]
        public int EvaluationPeriodId { get; set; }

        [ForeignKey(nameof(EvaluationPeriodId))]
        public virtual EvaluationPeriod EvaluationPeriod { get; set; } = null!;

        public PaymentCompany Company { get; set; } = PaymentCompany.General;

        // --- TIMELINESS OF PAYMENTS ---
        public int ReqsReceived { get; set; }
        public int LatePayments { get; set; }

        [NotMapped]
        public int ProcessedOnTime => ReqsReceived - LatePayments;

        [NotMapped]
        public decimal? TimelinessActual =>
            ReqsReceived > 0
                ? Math.Round((decimal)ProcessedOnTime / ReqsReceived * 100, 2)
                : null;

        public int DaysCountOfDelay { get; set; }

        // --- ACCURACY OF PAYMENTS ---
        public int AccuracyReqsReceived { get; set; }
        public int WrongPaymentOrPayee { get; set; }
        public int DoublePayments { get; set; }
        public int OtherErrors { get; set; }
        public int ReturnedPaymentsDueToError { get; set; }

        [NotMapped]
        public int TotalAccuracyErrors =>
            WrongPaymentOrPayee + DoublePayments + OtherErrors + ReturnedPaymentsDueToError;

        [NotMapped]
        public int AccuratelyProcessed => AccuracyReqsReceived - TotalAccuracyErrors;

        [NotMapped]
        public decimal? AccuracyActual =>
            AccuracyReqsReceived > 0
                ? Math.Round((decimal)AccuratelyProcessed / AccuracyReqsReceived * 100, 2)
                : null;

        // --- ACCURACY OF CV BOOKING ---
        public int CvReqsReceived { get; set; }
        public int CvOtherErrors { get; set; }
        public int CancelledCvsOrCheques { get; set; }
        public int ComplaintsFromStore { get; set; }

        [NotMapped]
        public int TotalCvErrors => CvOtherErrors + CancelledCvsOrCheques + ComplaintsFromStore;

        [NotMapped]
        public int CvAccuratelyProcessed => CvReqsReceived - TotalCvErrors;

        [NotMapped]
        public decimal? CvAccuracyActual =>
            CvReqsReceived > 0
                ? Math.Round((decimal)CvAccuratelyProcessed / CvReqsReceived * 100, 2)
                : null;
    }
}
