using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EvaluationInfrastructure.Repositories
{
    public class EmployeeRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public List<Employee> GetAll()
        {
            return _context.Employees
                .Include(e => e.Supervisor)
                .Include(e => e.Subordinates)
                .OrderBy(e => e.GroupNumber)
                .ThenBy(e => e.Role)
                .ThenBy(e => e.Name)
                .ToList();
        }

        public Employee? GetById(int id)
        {
            return _context.Employees
                .Include(e => e.Supervisor)
                .Include(e => e.Subordinates)
                .FirstOrDefault(e => e.Id == id);
        }

        public List<Employee> GetByRole(EmployeeRole role)
        {
            return _context.Employees
                .Include(e => e.Supervisor)
                .Where(e => e.Role == role)
                .OrderBy(e => e.GroupNumber)
                .ThenBy(e => e.Name)
                .ToList();
        }

        public List<Employee> GetByGroup(int groupNumber)
        {
            return _context.Employees
                .Include(e => e.Supervisor)
                .Where(e => e.GroupNumber == groupNumber)
                .OrderBy(e => e.Role)
                .ThenBy(e => e.Name)
                .ToList();
        }

        public List<Employee> GetStaffUnderSupervisor(int supervisorId)
        {
            return _context.Employees
                .Where(e => e.SupervisorId == supervisorId && e.Role == EmployeeRole.Staff)
                .OrderBy(e => e.Name)
                .ToList();
        }

        public Employee? GetSupervisorOfGroup(int groupNumber)
        {
            return _context.Employees
                .Include(e => e.Subordinates)
                .FirstOrDefault(e => e.GroupNumber == groupNumber && e.Role == EmployeeRole.Supervisor);
        }

        public Employee? GetAam()
        {
            return _context.Employees
                .FirstOrDefault(e => e.Role == EmployeeRole.AAM);
        }

        public bool NameExists(string name, int? excludeId = null)
        {
            return _context.Employees
                .Any(e => e.Name == name && e.Id != excludeId);
        }

        public void Add(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Name))
                throw new ArgumentException("Employee name is required.");

            if (NameExists(employee.Name))
                throw new ArgumentException($"An employee named '{employee.Name}' already exists.");

            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void Update(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Name))
                throw new ArgumentException("Employee name is required.");

            if (NameExists(employee.Name, employee.Id))
                throw new ArgumentException($"Another employee named '{employee.Name}' already exists.");

            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Subordinates)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return false;

            // Detach subordinates before delete to avoid FK violations
            foreach (var sub in employee.Subordinates)
                sub.SupervisorId = null;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
