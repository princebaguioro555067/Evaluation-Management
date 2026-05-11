using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace EvaluationInfrastructure.Repositories
{
    public class PaymentRecordRepository
    {
        private readonly AppDbContext _context = new AppDbContext();
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public List<PaymentRecord> GetForEmployee(int employeeId, int periodId)
        {
            return _context.PaymentRecords
                .Include(p => p.Employee)
                .Include(p => p.EvaluationPeriod)
                .Where(p => p.EmployeeId == employeeId && p.EvaluationPeriodId == periodId)
                .OrderBy(p => p.Company)
                .ToList();
        }

        public PaymentRecord? GetForEmployeeAndCompany(int employeeId, int periodId, PaymentCompany company)
        {
            return _context.PaymentRecords
                .FirstOrDefault(p =>
                    p.EmployeeId == employeeId &&
                    p.EvaluationPeriodId == periodId &&
                    p.Company == company);
        }

        public async Task UpsertAsync(PaymentRecord record)
        {
            await _semaphore.WaitAsync();
            try
            {
                var existing = await _context.PaymentRecords
                    .FirstOrDefaultAsync(p =>
                        p.EmployeeId == record.EmployeeId &&
                        p.EvaluationPeriodId == record.EvaluationPeriodId &&
                        p.Company == record.Company);

                if (existing == null)
                {
                    _context.PaymentRecords.Add(record);
                }
                else
                {
                    existing.ReqsReceived = record.ReqsReceived;
                    existing.LatePayments = record.LatePayments;
                    existing.DaysCountOfDelay = record.DaysCountOfDelay;
                    existing.AccuracyReqsReceived = record.AccuracyReqsReceived;
                    existing.WrongPaymentOrPayee = record.WrongPaymentOrPayee;
                    existing.DoublePayments = record.DoublePayments;
                    existing.OtherErrors = record.OtherErrors;
                    existing.ReturnedPaymentsDueToError = record.ReturnedPaymentsDueToError;
                    existing.CvReqsReceived = record.CvReqsReceived;
                    existing.CvOtherErrors = record.CvOtherErrors;
                    existing.CancelledCvsOrCheques = record.CancelledCvsOrCheques;
                    existing.ComplaintsFromStore = record.ComplaintsFromStore;
                }

                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _context.PaymentRecords.FindAsync(id);
            if (record == null) return false;

            _context.PaymentRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
