

namespace EvaluationDomain.Models
{
    public class Submission : BaseClass
    {
        public int EmployeeId { get; set; } // The ID of the staff member
        public int GroupNumber { get; set; } // The Team Number (1-5)
        public DateTime SubmissionDate { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }
}