using EvaluationDomain.Helpers;
using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EvaluationInfrastructure.Repositories
{
    public class EvaluationSubmissionRepository
    {
        private readonly AppDbContext _context = new AppDbContext();
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        // ---------------------------------------------------------------
        // READ
        // ---------------------------------------------------------------

        public List<EvaluationSubmission> GetForEmployee(int employeeId)
        {
            return _context.EvaluationSubmissions
                .Include(s => s.Employee)
                .Include(s => s.ReviewedByManager)
                .Where(s => s.EmployeeId == employeeId)
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToList();
        }

        public EvaluationSubmission? GetForEmployeeAndPeriod(int employeeId, int month, int year)
        {
            return _context.EvaluationSubmissions
                .Include(s => s.Employee)
                .Include(s => s.ReviewedByManager)
                .FirstOrDefault(s => s.EmployeeId == employeeId
                    && s.Month == month && s.Year == year);
        }

        public List<EvaluationSubmission> GetForGroup(int groupNumber, int month, int year)
        {
            return _context.EvaluationSubmissions
                .Include(s => s.Employee)
                .Include(s => s.ReviewedByManager)
                .Where(s => s.Employee.GroupNumber == groupNumber
                    && s.Month == month && s.Year == year)
                .OrderBy(s => s.Employee.Name)
                .ToList();
        }

        public List<EvaluationSubmission> GetAllForPeriod(int month, int year)
        {
            return _context.EvaluationSubmissions
                .Include(s => s.Employee)
                .Include(s => s.ReviewedByManager)
                .Where(s => s.Month == month && s.Year == year)
                .OrderBy(s => s.Employee.GroupNumber)
                .ThenBy(s => s.Employee.Name)
                .ToList();
        }

        public List<EvaluationSubmission> GetPendingForGroup(int groupNumber, int month, int year)
        {
            return GetForGroup(groupNumber, month, year)
                .Where(s => s.Status == SubmissionStatus.Pending)
                .ToList();
        }

        // ---------------------------------------------------------------
        // WRITE
        // ---------------------------------------------------------------

        public async Task SubmitAsync(int employeeId, int month, int year,
    DateTime submissionDate, string comment)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                var existing = await _context.EvaluationSubmissions
                    .FirstOrDefaultAsync(s => s.EmployeeId == employeeId
                        && s.Month == month && s.Year == year)
                    .ConfigureAwait(false);

                decimal score = ScoreCalculator.Compute(submissionDate, month, year);

                if (existing == null)
                {
                    _context.EvaluationSubmissions.Add(new EvaluationSubmission
                    {
                        EmployeeId = employeeId,
                        Month = month,
                        Year = year,
                        SubmissionDate = submissionDate,
                        Comment = comment,
                        Score = score,
                        Status = SubmissionStatus.Pending
                    });
                }
                else
                {
                    if (existing.Status == SubmissionStatus.Approved)
                        throw new InvalidOperationException(
                            "This month's evaluation has already been approved.");

                    existing.SubmissionDate = submissionDate;
                    existing.Comment = comment;
                    existing.Score = score;
                    existing.Status = SubmissionStatus.Pending;
                    existing.ManagerComment = null;
                    existing.ReviewedDate = null;
                    existing.ReviewedByManagerId = null;
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task ApproveAsync(int submissionId, int managerId, string? managerComment)
        {
            await _semaphore.WaitAsync();
            try
            {
                var submission = await _context.EvaluationSubmissions
                    .FindAsync(submissionId);

                if (submission == null)
                    throw new ArgumentException("Submission not found.");

                submission.Status = SubmissionStatus.Approved;
                submission.ReviewedByManagerId = managerId;
                submission.ReviewedDate = DateTime.Now;
                submission.ManagerComment = managerComment;

                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task RejectAsync(int submissionId, int managerId, string? managerComment)
        {
            await _semaphore.WaitAsync();
            try
            {
                var submission = await _context.EvaluationSubmissions
                    .FindAsync(submissionId);

                if (submission == null)
                    throw new ArgumentException("Submission not found.");

                submission.Status = SubmissionStatus.Rejected;
                submission.ReviewedByManagerId = managerId;
                submission.ReviewedDate = DateTime.Now;
                submission.ManagerComment = managerComment;

                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}