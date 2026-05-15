using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;
using EvaluationInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Evaluation_Management
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var db = new AppDbContext())
            {
                db.Database.Migrate();
            }

            SeedAdminAccount();

            ApplicationConfiguration.Initialize();
            Application.Run(new Login_Form());
        }

        private static void SeedAdminAccount()
        {
            var repo = new EmployeeRepository();

            if (!repo.UsernameExists("admin"))
            {
                repo.Add(new Employee
                {
                    Username = "admin",
                    Password = "admin123",
                    Name = "Administrator",
                    Designation = "Assist Accounting Manager",
                    Role = EmployeeRole.AAM,
                    GroupNumber = 1
                });
            }
        }
    }
}