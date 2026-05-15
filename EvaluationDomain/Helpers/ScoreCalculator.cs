using System.Drawing;

namespace EvaluationDomain.Helpers
{
    /// <summary>
    /// Computes evaluation score based on the 28th deadline rule.
    /// On time = 100. Each day late = -10 points. Minimum score = 50.
    /// </summary>
    public static class ScoreCalculator
    {
        public const int DEADLINE_DAY = 28;
        public const int MAX_SCORE = 100;
        public const int MIN_SCORE = 50;
        public const int PENALTY_PER_DAY = 10;

        public static decimal Compute(DateTime submissionDate, int month, int year)
        {
            var deadline = new DateTime(year, month, DEADLINE_DAY);

            if (submissionDate.Date <= deadline.Date)
                return MAX_SCORE;

            int daysLate = (submissionDate.Date - deadline.Date).Days;
            decimal score = MAX_SCORE - (daysLate * PENALTY_PER_DAY);
            return Math.Max(score, MIN_SCORE);
        }

        public static string GetLevel(decimal score) => score switch
        {
            >= 95 => "Excellent",
            >= 85 => "Great",
            >= 75 => "Good",
            >= 65 => "Fair",
            _ => "Needs Improvement"
        };

        public static Color GetLevelColor(decimal score) => score switch
        {
            >= 95 => Color.FromArgb(0, 150, 80),      // Green
            >= 85 => Color.FromArgb(30, 130, 200),    // Blue
            >= 75 => Color.FromArgb(255, 165, 0),     // Orange
            >= 65 => Color.FromArgb(220, 120, 20),    // Dark Orange
            _ => Color.FromArgb(200, 30, 30)      // Red
        };

        /// <summary>
        /// Returns how many days remain until the 28th of the current month.
        /// Negative means the deadline has passed.
        /// </summary>
        public static int DaysUntilDeadline()
        {
            var today = DateTime.Today;
            var deadline = new DateTime(today.Year, today.Month, DEADLINE_DAY);
            return (deadline - today).Days;
        }
    }
}