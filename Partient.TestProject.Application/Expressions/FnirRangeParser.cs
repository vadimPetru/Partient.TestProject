using Partient.TestProject.Domain.Exceptions;
using System.Globalization;

namespace Partient.TestProject.Application.Expressions
{
    public static class FnirRangeParser
    {
        public static (DateTime Start, DateTime End) ParseToRangeDate(string dateStr)
        {
            return dateStr.Length switch
            {
                4 => (new DateTime(int.Parse(dateStr), 1, 1), new DateTime(int.Parse(dateStr), 12, 31, 23, 59, 59)),
                7 => (DateTime.ParseExact(dateStr, "yyyy-MM", CultureInfo.InvariantCulture),
                      DateTime.ParseExact(dateStr, "yyyy-MM", CultureInfo.InvariantCulture).AddMonths(1).AddTicks(-1)),
                _ => (DateTime.Parse(dateStr).Date, DateTime.Parse(dateStr).Date.AddDays(1).AddTicks(-1))
            };
        }

        public static (DateTime Start, DateTime End) ParseToRangeDateTime(string dateValue)
        {
            if (!DateTime.TryParse(dateValue, out var dt))
                throw new FhirSearchException("Invalid date");

            var start = dt.Date;
            var end = dt.Date.AddDays(1);

            return (start, end);
        }

        public static (DateTime Start, DateTime End) ParseToRange(string value)
        {
            if (!DateTimeOffset.TryParse(value, out var dto))
                throw new FhirSearchException($"Invalid date format: {value}");

            var dt = dto.UtcDateTime;

            return value.Length switch
            {
                4 => (
                    new DateTime(dt.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(dt.Year, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc)
                ),
                7 => (
                    new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59, 999, DateTimeKind.Utc)
                ),
                10 => (
                    dt.Date,
                    dt.Date.AddDays(1).AddTicks(-1)
                ),
                _ => (dt, dt)
            };
        }

        public static DateTime ParseToInstant(string dateValue)
        {
            if (!DateTimeOffset.TryParse(dateValue, out var dto))
                throw new FhirSearchException("Invalid Instant format");

            var instant = dto.UtcDateTime;

            return instant;
        }
    }
}
