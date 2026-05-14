using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace EvaluationInfrastructure.Repositories
{
    public class SubmissionRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public void Add(Submission submission)
        {
            _context.Submissions.Add(submission);
            _context.SaveChanges();
        }

        public void Update(Submission submission)
        {
            _context.Submissions.Update(submission);
            _context.SaveChanges();
        }

        // Gets submissions for the Staff member's history
        public List<Submission> GetByEmployee(int employeeId)
        {
            return _context.Submissions.Where(s => s.EmployeeId == employeeId).ToList();
        }

        // Gets submissions for the Manager's approval list
        public List<Submission> GetByGroup(int groupNumber)
        {
            return _context.Submissions.Where(s => s.GroupNumber == groupNumber).ToList();
        }
    }
}