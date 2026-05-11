using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;

namespace EvaluationInfrastructure.Repositories
{
    public class EvaluationPeriodRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public List<EvaluationPeriod> GetAll()
        {
            return _context.EvaluationPeriods
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToList();
        }

        public EvaluationPeriod? GetById(int id)
        {
            return _context.EvaluationPeriods.Find(id);
        }

        public EvaluationPeriod? GetByMonthYear(int month, int year)
        {
            return _context.EvaluationPeriods
                .FirstOrDefault(p => p.Month == month && p.Year == year);
        }

        public EvaluationPeriod GetOrCreate(int month, int year)
        {
            var existing = GetByMonthYear(month, year);
            if (existing != null) return existing;

            var period = new EvaluationPeriod { Month = month, Year = year };
            _context.EvaluationPeriods.Add(period);
            _context.SaveChanges();
            return period;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var period = await _context.EvaluationPeriods.FindAsync(id);
            if (period == null) return false;

            _context.EvaluationPeriods.Remove(period);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
