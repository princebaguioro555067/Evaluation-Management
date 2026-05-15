using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationDomain.Models
{
    public enum EmployeeRole
    {
        AAM,
        Supervisor,
        Staff
    }

    public class Employee : BaseClass
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Designation { get; set; } = string.Empty;

        public EmployeeRole Role { get; set; }

        public int GroupNumber { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public int? SupervisorId { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        public virtual Employee? Supervisor { get; set; }

        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
        public virtual ICollection<EvaluationSubmission> Submissions { get; set; } = new List<EvaluationSubmission>();

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

        [NotMapped]
        public bool IsManager => Role == EmployeeRole.AAM || Role == EmployeeRole.Supervisor;
    }
}