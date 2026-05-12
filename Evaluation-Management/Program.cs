using EvaluationInfrastructure.Data;
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

            ApplicationConfiguration.Initialize();
            Application.Run(new Login_Form());
        }
    }
}