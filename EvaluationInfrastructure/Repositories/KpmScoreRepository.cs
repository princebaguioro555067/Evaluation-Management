using EvaluationDomain.Models;
using EvaluationDomain.Strategies;
using EvaluationInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EvaluationInfrastructure.Repositories
{
    public class KpmScoreRepository
    {
        private readonly AppDbContext _context = new AppDbContext();
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public List<KpmScore> GetForEmployee(int employeeId, int periodId)
        {
            return _context.KpmScores
                .Include(k => k.Employee)
                .Include(k => k.EvaluationPeriod)
                .Where(k => k.EmployeeId == employeeId && k.EvaluationPeriodId == periodId)
                .OrderBy(k => k.KpmType)
                .ToList();
        }

        public List<KpmScore> GetForGroup(int groupNumber, int periodId)
        {
            return _context.KpmScores
                .Include(k => k.Employee)
                .Include(k => k.EvaluationPeriod)
                .Where(k => k.EvaluationPeriodId == periodId
                         && k.Employee.GroupNumber == groupNumber)
                .OrderBy(k => k.Employee.Role)
                .ThenBy(k => k.Employee.Name)
                .ThenBy(k => k.KpmType)
                .ToList();
        }

        public List<KpmScore> GetAllForPeriod(int periodId)
        {
            return _context.KpmScores
                .Include(k => k.Employee)
                .Include(k => k.EvaluationPeriod)
                .Where(k => k.EvaluationPeriodId == periodId)
                .OrderBy(k => k.Employee.GroupNumber)
                .ThenBy(k => k.Employee.Role)
                .ThenBy(k => k.Employee.Name)
                .ThenBy(k => k.KpmType)
                .ToList();
        }

        public decimal GetTotalPoints(int employeeId, int periodId)
        {
            var scores = GetForEmployee(employeeId, periodId);
            if (!scores.Any()) return 0;

            var employee = _context.Employees.Find(employeeId);
            if (employee == null) return 0;

            var strategy = KpmScoringStrategyFactory.For(employee.Role.ToString());
            var inputs = scores.Select(k => (k.Actual, k.Target, k.Weight));
            return strategy.ComputeTotalPoints(inputs);
        }

        public async Task UpsertAsync(KpmScore score)
        {
            await _semaphore.WaitAsync();
            try
            {
                // Recompute achievement rate and points on every save
                if (score.Actual.HasValue && score.Target > 0)
                {
                    score.AchievementRate = Math.Min(score.Actual.Value / score.Target, 1.0m);
                    score.Points = Math.Round(score.AchievementRate.Value * score.Weight * 100, 2);
                }
                else
                {
                    score.AchievementRate = null;
                    score.Points = null;
                }

                var existing = await _context.KpmScores
                    .FirstOrDefaultAsync(k =>
                        k.EmployeeId == score.EmployeeId &&
                        k.EvaluationPeriodId == score.EvaluationPeriodId &&
                        k.KpmType == score.KpmType);

                if (existing == null)
                {
                    _context.KpmScores.Add(score);
                }
                else
                {
                    existing.Target = score.Target;
                    existing.Actual = score.Actual;
                    existing.AchievementRate = score.AchievementRate;
                    existing.Weight = score.Weight;
                    existing.Points = score.Points;
                    existing.Remarks = score.Remarks;
                }

                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // Seeds all 6 KPM rows with default targets/weights if they don't exist yet
        public async Task SeedDefaultsAsync(int employeeId, int periodId)
        {
            var defaults = new[]
            {
                (KpmType.TimelinessOfPayment,                    90m, 0.20m),
                (KpmType.AccuracyOfPayment,                     100m, 0.20m),
                (KpmType.AccuracyOfCvBooking,                    90m, 0.10m),
                (KpmType.NoIncidenceOfPenaltiesAndCharges,      100m, 0.15m),
                (KpmType.TimelinessCompletenessAccuracyOfRecon,  90m, 0.20m),
                (KpmType.NoIncidenceOfUnresolvedReconItems,     100m, 0.15m),
            };

            foreach (var (kpmType, target, weight) in defaults)
            {
                var exists = await _context.KpmScores.AnyAsync(k =>
                    k.EmployeeId == employeeId &&
                    k.EvaluationPeriodId == periodId &&
                    k.KpmType == kpmType);

                if (!exists)
                {
                    _context.KpmScores.Add(new KpmScore
                    {
                        EmployeeId = employeeId,
                        EvaluationPeriodId = periodId,
                        KpmType = kpmType,
                        Target = target,
                        Weight = weight
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var score = await _context.KpmScores.FindAsync(id);
            if (score == null) return false;

            _context.KpmScores.Remove(score);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
