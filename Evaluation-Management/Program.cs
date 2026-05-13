using EvaluationDomain.Models;
using EvaluationInfrastructure.Data;
using EvaluationInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Evaluation_Management
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
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

            // Only creates the AAM account if it doesn't exist yet
            if (!repo.UsernameExists("admin"))
            {
                var admin = new Employee
                {
                    Username = "admin",
                    Password = "admin123",   // will be hashed by repo.Add()
                    Name = "Administrator",
                    Designation = "Assist Accounting Manager",
                    Role = EmployeeRole.AAM,
                    GroupNumber = 0
                };

                repo.Add(admin);
            }
        }
    }
}