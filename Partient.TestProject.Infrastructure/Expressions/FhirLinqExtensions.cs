using System.Linq.Expressions;

namespace Partient.TestProject.Infrastructure.Expressions
{
    public static class FhirLinqExtensions
    {
        public static IQueryable<T> WhereFhirDate<T>(
            this IQueryable<T> query,
            string fhirParam,
            Expression<Func<T, DateTimeOffset?>> startPath,
            Expression<Func<T, DateTimeOffset?>> endPath = null)
        {
            if (string.IsNullOrWhiteSpace(fhirParam)) return query;

            var (prefix, searchRange) = FhirPathParser.Parse(fhirParam);
            var parameter = startPath.Parameters[0];
            var resourceStart = startPath.Body;

            var resourceEnd = endPath != null
                ? Expression.Invoke(endPath, parameter)
                : startPath.Body;

            var sStart = Expression.Constant(searchRange.Start);
            var sEnd = Expression.Constant(searchRange.End);

            Expression comparison = prefix switch
            {
                "eq" => And(Ge(resourceEnd, sStart), Le(resourceStart, sEnd)),
                "ne" => Not(And(Ge(resourceEnd, sStart), Le(resourceStart, sEnd))),
                "lt" => Lt(resourceStart, sEnd),
                "gt" => Gt(resourceEnd, sStart),
                "ge" => Ge(resourceEnd, sStart),
                "le" => Le(resourceStart, sEnd),
                "sa" => Gt(resourceStart, sEnd),
                "eb" => Lt(resourceEnd, sStart),
                "ap" => BuildApproximate(resourceStart, resourceEnd, searchRange),
                _ => throw new NotSupportedException($"Prefix {prefix} not supported")
            };

            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
            return query.Where(lambda);
        }

        private static Expression Ge(Expression left, Expression right) => Expression.GreaterThanOrEqual(left, right);
        private static Expression Le(Expression left, Expression right) => Expression.LessThanOrEqual(left, right);
        private static Expression Gt(Expression left, Expression right) => Expression.GreaterThan(left, right);
        private static Expression Lt(Expression left, Expression right) => Expression.LessThan(left, right);
        private static Expression And(Expression left, Expression right) => Expression.AndAlso(left, right);
        private static Expression Not(Expression inner) => Expression.Not(inner);

        private static Expression BuildApproximate(Expression start, Expression end, DateRange search)
        {
            var margin = TimeSpan.FromDays(7);
            return And(Ge(end, Expression.Constant(search.Start - margin)),
                       Le(start, Expression.Constant(search.End + margin)));
        }

        public record DateRange(DateTimeOffset Start, DateTimeOffset End);

        public static class FhirPathParser
        {
            public static (string Prefix, DateRange Range) Parse(string input)
            {
                string prefix = "eq";
                string rawDate = input;

                if (char.IsLetter(input[0]) && char.IsLetter(input[1]))
                {
                    prefix = input.Substring(0, 2).ToLower();
                    rawDate = input.Substring(2);
                }

                DateTimeOffset start;
                DateTimeOffset end;

                if (rawDate.Length == 4)
                {
                    start = new DateTimeOffset(int.Parse(rawDate), 1, 1, 0, 0, 0, TimeSpan.Zero);
                    end = start.AddYears(1).AddTicks(-1);
                }
                else if (rawDate.Length == 7)
                {
                    start = DateTimeOffset.Parse(rawDate + "-01");
                    end = start.AddMonths(1).AddTicks(-1);
                }
                else if (!rawDate.Contains("T"))
                {
                    start = DateTimeOffset.Parse(rawDate);
                    end = start.AddDays(1).AddTicks(-1);
                }
                else
                {
                    start = DateTimeOffset.Parse(rawDate);
                    end = start;
                }

                return (prefix, new DateRange(start, end));
            }
        }
    }
}
