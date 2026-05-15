using EvaluationDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace EvaluationInfrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EvaluationSubmission> EvaluationSubmissions { get; set; }
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=(localdb)\\MSSQLLocalDB;" +
                    "Initial Catalog=EvaluationDb;" +
                    "Integrated Security=True;" +
                    "Connect Timeout=30;Encrypt=True;" +
                    "Trust Server Certificate=False;" +
                    "Application Intent=ReadWrite;" +
                    "Multi Subnet Failover=False;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique username
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Username)
                .IsUnique();

            // Employee self-referencing (supervisor)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Supervisor)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee → EvaluationSubmission
            modelBuilder.Entity<EvaluationSubmission>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Submissions)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Manager who reviewed the submission
            modelBuilder.Entity<EvaluationSubmission>()
                .HasOne(s => s.ReviewedByManager)
                .WithMany()
                .HasForeignKey(s => s.ReviewedByManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // One submission per employee per month/year
            modelBuilder.Entity<EvaluationSubmission>()
                .HasIndex(s => new { s.EmployeeId, s.Month, s.Year })
                .IsUnique();

            // Score precision
            modelBuilder.Entity<EvaluationSubmission>()
                .Property(s => s.Score)
                .HasPrecision(5, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
