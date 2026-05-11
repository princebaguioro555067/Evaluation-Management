using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationInfrastructure.Repositories
{
    public class ReconReportRepository
    {
        private readonly AppDbContext _context = new AppDbContext();
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public List<ReconReport> GetForEmployee(int employeeId, int periodId)
        {
            return _context.ReconReports
                .Include(r => r.Employee)
                .Include(r => r.EvaluationPeriod)
                .Where(r => r.EmployeeId == employeeId && r.EvaluationPeriodId == periodId)
                .OrderBy(r => r.ReportType)
                .ToList();
        }

        public List<ReconReport> GetForGroup(int groupNumber, int periodId)
        {
            return _context.ReconReports
                .Include(r => r.Employee)
                .Include(r => r.EvaluationPeriod)
                .Where(r => r.EvaluationPeriodId == periodId
                         && r.Employee.GroupNumber == groupNumber)
                .OrderBy(r => r.Employee.Name)
                .ThenBy(r => r.ReportType)
                .ToList();
        }

        public List<ReconReport> GetAllForPeriod(int periodId)
        {
            return _context.ReconReports
                .Include(r => r.Employee)
                .Include(r => r.EvaluationPeriod)
                .Where(r => r.EvaluationPeriodId == periodId)
                .OrderBy(r => r.Employee.GroupNumber)
                .ThenBy(r => r.Employee.Name)
                .ThenBy(r => r.ReportType)
                .ToList();
        }

        public List<ReconReport> GetLateSubmissions(int periodId)
        {
            return GetAllForPeriod(periodId)
                .Where(r => r.IsLate)
                .ToList();
        }

        public List<ReconReport> GetReturnedRecons(int periodId)
        {
            return GetAllForPeriod(periodId)
                .Where(r => r.WasReturned)
                .ToList();
        }

        public async Task UpsertAsync(ReconReport report)
        {
            await _semaphore.WaitAsync();
            try
            {
                var existing = await _context.ReconReports
                    .FirstOrDefaultAsync(r =>
                        r.EmployeeId == report.EmployeeId &&
                        r.EvaluationPeriodId == report.EvaluationPeriodId &&
                        r.ReportType == report.ReportType &&
                        r.AssignmentStore == report.AssignmentStore);

                if (existing == null)
                {
                    if (report.StaffDeadlineDay == 0)
                        report.StaffDeadlineDay = report.DefaultStaffDeadline;

                    _context.ReconReports.Add(report);
                }
                else
                {
                    existing.AssignmentStore = report.AssignmentStore;
                    existing.Company = report.Company;
                    existing.StaffDeadlineDay = report.StaffDeadlineDay;
                    existing.SupervisorDeadlineDay = report.SupervisorDeadlineDay;
                    existing.DateOfFirstSubmission = report.DateOfFirstSubmission;
                    existing.CheckedBySupervisorDate = report.CheckedBySupervisorDate;
                    existing.TimelinessActual = report.TimelinessActual;
                    existing.DateReturned = report.DateReturned;
                    existing.DateResubmitted = report.DateResubmitted;
                    existing.AccuracyActual = report.AccuracyActual;
                    existing.Remarks = report.Remarks;
                }

                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // Seeds all 9 report type rows for an employee + period if they don't exist yet
        public async Task SeedDefaultsAsync(int employeeId, int periodId)
        {
            var allTypes = Enum.GetValues<ReconReportType>();

            foreach (var reportType in allTypes)
            {
                var exists = await _context.ReconReports.AnyAsync(r =>
                    r.EmployeeId == employeeId &&
                    r.EvaluationPeriodId == periodId &&
                    r.ReportType == reportType);

                if (!exists)
                {
                    var dummy = new ReconReport
                    {
                        EmployeeId = employeeId,
                        EvaluationPeriodId = periodId,
                        ReportType = reportType
                    };
                    dummy.StaffDeadlineDay = dummy.DefaultStaffDeadline;
                    _context.ReconReports.Add(dummy);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var report = await _context.ReconReports.FindAsync(id);
            if (report == null) return false;

            _context.ReconReports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
