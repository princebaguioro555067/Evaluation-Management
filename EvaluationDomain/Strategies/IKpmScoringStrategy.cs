namespace EvaluationDomain.Strategies
{
    public interface IKpmScoringStrategy
    {
        decimal ComputeTotalPoints(IEnumerable<(decimal? Actual, decimal Target, decimal Weight)> kpmInputs);
        decimal ComputeKpmPoints(decimal? actual, decimal target, decimal weight);
    }

    // Standard scoring for Staff
    public class StaffScoringStrategy : IKpmScoringStrategy
    {
        public decimal ComputeKpmPoints(decimal? actual, decimal target, decimal weight)
        {
            if (actual == null || target == 0) return 0;
            var achievement = Math.Min(actual.Value / target, 1.0m);
            return Math.Round(achievement * weight * 100, 2);
        }

        public decimal ComputeTotalPoints(IEnumerable<(decimal? Actual, decimal Target, decimal Weight)> kpmInputs)
            => kpmInputs.Sum(k => ComputeKpmPoints(k.Actual, k.Target, k.Weight));
    }

    // Scoring for Supervisors
    public class SupervisorScoringStrategy : IKpmScoringStrategy
    {
        public decimal ComputeKpmPoints(decimal? actual, decimal target, decimal weight)
        {
            if (actual == null || target == 0) return 0;
            var achievement = Math.Min(actual.Value / target, 1.0m);
            return Math.Round(achievement * weight * 100, 2);
        }

        public decimal ComputeTotalPoints(IEnumerable<(decimal? Actual, decimal Target, decimal Weight)> kpmInputs)
            => kpmInputs.Sum(k => ComputeKpmPoints(k.Actual, k.Target, k.Weight));
    }

    // Scoring for AAM
    public class AamScoringStrategy : IKpmScoringStrategy
    {
        public decimal ComputeKpmPoints(decimal? actual, decimal target, decimal weight)
        {
            if (actual == null || target == 0) return 0;
            var achievement = Math.Min(actual.Value / target, 1.0m);
            return Math.Round(achievement * weight * 100, 2);
        }

        public decimal ComputeTotalPoints(IEnumerable<(decimal? Actual, decimal Target, decimal Weight)> kpmInputs)
            => kpmInputs.Sum(k => ComputeKpmPoints(k.Actual, k.Target, k.Weight));
    }

    // Factory — pick the right strategy by role
    public static class KpmScoringStrategyFactory
    {
        public static IKpmScoringStrategy For(string role) => role.ToLowerInvariant() switch
        {
            "aam" => new AamScoringStrategy(),
            "supervisor" => new SupervisorScoringStrategy(),
            "staff" => new StaffScoringStrategy(),
            _ => throw new ArgumentException($"Unknown role: {role}")
        };
    }
}
