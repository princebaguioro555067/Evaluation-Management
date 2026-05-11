using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationDomain.Models
{
    public enum EmployeeRole
    {
        AAM,        // Assist Accounting Manager — top-level reviewer
        Supervisor, // Payable Supervisor — manages a group of staff
        Staff       // Regular staff member under a supervisor
    }

    public class Employee : BaseClass
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Designation { get; set; } = string.Empty;

        public EmployeeRole Role { get; set; }

        // Group number (1–5) 
        public int GroupNumber { get; set; }

        // ForeignKey to the supervisor
        public int? SupervisorId { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        public virtual Employee? Supervisor { get; set; }

        // Staff members under this employee (only populated for Supervisors)
        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();

        // All KPM scores recorded for this employee
        public virtual ICollection<KpmScore> KpmScores { get; set; } = new List<KpmScore>();

        // All payment records for this employee
        public virtual ICollection<PaymentRecord> PaymentRecords { get; set; } = new List<PaymentRecord>();

        // All recon & report submissions for this employee
        public virtual ICollection<ReconReport> ReconReports { get; set; } = new List<ReconReport>();

        [NotMapped]
        public string RoleDisplay => Role switch
        {
            EmployeeRole.AAM => "Assist Accounting Manager",
            EmployeeRole.Supervisor => "Payable Supervisor",
            EmployeeRole.Staff => "Staff",
            _ => "Unknown"
        };

        [NotMapped]
        public string GroupLabel => Role == EmployeeRole.Staff
            ? $"{GroupNumber}|STAFF"
            : $"{GroupNumber}|SUPER";
    }
}
