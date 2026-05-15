using EvaluationDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace EvaluationInfrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EvaluationPeriod> EvaluationPeriods { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;" +
                "Initial Catalog=EvaluationDb;" +
                "Integrated Security=True;" +
                "Connect Timeout=30;Encrypt=True;" +
                "Trust Server Certificate=False;" +
                "Application Intent=ReadWrite;" +
                "Multi Subnet Failover=False;" +
                "Command Timeout=30");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique username
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Username)
                .IsUnique();

            // Employee self-referencing relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Supervisor)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee → KpmScore
            modelBuilder.Entity<KpmScore>()
                .HasOne(k => k.Employee)
                .WithMany(e => e.KpmScores)
                .HasForeignKey(k => k.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // EvaluationPeriod → KpmScore
            modelBuilder.Entity<KpmScore>()
                .HasOne(k => k.EvaluationPeriod)
                .WithMany(ep => ep.KpmScores)
                .HasForeignKey(k => k.EvaluationPeriodId)
                .OnDelete(DeleteBehavior.Cascade);

            // One KpmScore per employee per period per KPM type
            modelBuilder.Entity<KpmScore>()
                .HasIndex(k => new { k.EmployeeId, k.EvaluationPeriodId, k.KpmType })
                .IsUnique();

            // Employee → PaymentRecord
            modelBuilder.Entity<PaymentRecord>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.PaymentRecords)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // EvaluationPeriod → PaymentRecord
            modelBuilder.Entity<PaymentRecord>()
                .HasOne(p => p.EvaluationPeriod)
                .WithMany(ep => ep.PaymentRecords)
                .HasForeignKey(p => p.EvaluationPeriodId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employee → ReconReport
            modelBuilder.Entity<ReconReport>()
                .HasOne(r => r.Employee)
                .WithMany(e => e.ReconReports)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // EvaluationPeriod → ReconReport
            modelBuilder.Entity<ReconReport>()
                .HasOne(r => r.EvaluationPeriod)
                .WithMany(ep => ep.ReconReports)
                .HasForeignKey(r => r.EvaluationPeriodId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal precision
            modelBuilder.Entity<KpmScore>().Property(k => k.Target).HasPrecision(5, 2);
            modelBuilder.Entity<KpmScore>().Property(k => k.Actual).HasPrecision(5, 2);
            modelBuilder.Entity<KpmScore>().Property(k => k.AchievementRate).HasPrecision(5, 4);
            modelBuilder.Entity<KpmScore>().Property(k => k.Weight).HasPrecision(4, 2);
            modelBuilder.Entity<KpmScore>().Property(k => k.Points).HasPrecision(6, 2);
            modelBuilder.Entity<ReconReport>().Property(r => r.TimelinessActual).HasPrecision(5, 2);
            modelBuilder.Entity<ReconReport>().Property(r => r.AccuracyActual).HasPrecision(5, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
