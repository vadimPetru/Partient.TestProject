using Partient.TestProject.Domain.Enums;

namespace Partient.TestProject.Domain.Models
{
    public class Range
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public void CreateRange(string value, ParserType type)
        {
            switch (type)
            {

                case ParserType.date:
                    DateHandler(value);
                    break;
                case ParserType.datetime:
                    DateTimeHandler(value);
                    break;
                case ParserType.instant:
                    InstantHandler(value);
                    break;
                case ParserType.period:
                    PeriodHandler(value);
                    break;
                case ParserType.timing:
                    TimingHandler(value);
                    break;
            }

            StartDate = DateTime.SpecifyKind(StartDate, DateTimeKind.Utc);
            EndDate = DateTime.SpecifyKind(EndDate, DateTimeKind.Utc);
        }

        private void InstantHandler(string value)
        {
            if (value.Contains('T') && value.Contains('.'))
            {
                var dateTimeParts = value.Split('T');
                var dateParts = dateTimeParts[0].Split('-');
                var timeParts = dateTimeParts[1].Split(':');
                var secondParts = timeParts[2].Split('.');

                int year = int.Parse(dateParts[0]);
                int month = int.Parse(dateParts[1]);
                int day = int.Parse(dateParts[2]);
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);
                int second = int.Parse(secondParts[0]);
                int millisecond = int.Parse(secondParts[1]);

                StartDate = new DateTime(year, month, day, hour, minute, second, millisecond);
                EndDate = StartDate;
            }
        }

        private void DateTimeHandler(string value)
        {
            if (value.Contains('T'))
            {
                var dateTimeParts = value.Split('T');
                var dateParts = dateTimeParts[0].Split('-');
                var timeParts = dateTimeParts[1].Split(':');

                int year = int.Parse(dateParts[0]);
                int month = int.Parse(dateParts[1]);
                int day = int.Parse(dateParts[2]);
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);
                int second = timeParts.Length > 2 ? int.Parse(timeParts[2]) : 0;

                StartDate = new DateTime(year, month, day, hour, minute, second);

                if (timeParts.Length > 2 && timeParts[2].Contains('.'))
                {
                    var secondParts = timeParts[2].Split('.');
                    second = int.Parse(secondParts[0]);
                    int millisecond = int.Parse(secondParts[1]);
                    StartDate = new DateTime(year, month, day, hour, minute, second, millisecond);
                    EndDate = StartDate.AddTicks(1);
                }
                else
                {
                    EndDate = StartDate.AddSeconds(1).AddTicks(-1);
                }
            }
        }

        private void DateHandler(string value)
        {
            var date = value.Split(new char[] { '-', ':', '.' });

            if (date.Length == 1)
            {
                StartDate = new DateTime(int.Parse(date[0]), 1, 1, 00, 00, 00);
                EndDate = new DateTime(int.Parse(date[0]), 12, 31, 23, 59, 59);
            }

            else if (date.Length == 2)
            {
                var year = int.Parse(date[0]);
                var month = int.Parse(date[1]);
                StartDate = new DateTime(year, month, 1, 00, 00, 00);
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59, 999);
            }

            else if (date.Length == 3)
            {
                var year = int.Parse(date[0]);
                var month = int.Parse(date[1]);
                var day = int.Parse(date[2]);
                StartDate = new DateTime(year, month, day, 0, 0, 0);
                EndDate = new DateTime(year, month, day, 23, 59, 59, 999);
            }
            else
            {
                throw new Exception("Невалидный ввод");
            }
        }

        private void PeriodHandler(string value)
        {
            var date = value.Split(new char[] { '-', ':', '.' });
            int year = int.Parse(date[0]);

            if (date.Length == 1)
            {
                StartDate = new DateTime(year, 1, 1, 0, 0, 0);
                EndDate = DateTime.MaxValue;
            }
            else if (date.Length == 2)
            {
                int month = int.Parse(date[1]);
                StartDate = new DateTime(year, month, 1, 0, 0, 0);
                EndDate = DateTime.MaxValue;
            }
            else
            {
                int month = int.Parse(date[1]);
                int day = int.Parse(date[2]);
                StartDate = new DateTime(year, month, day, 0, 0, 0);
                EndDate = DateTime.MaxValue;
            }
        }

        private void TimingHandler(string value)
        {
            var date = value.Split(new char[] { '-', ':', '.' });
            int year = int.Parse(date[0]);

            if (date.Length == 1)
            {
                StartDate = new DateTime(year, 1, 1, 0, 0, 0);
                EndDate = new DateTime(year, 12, 31, 23, 59, 59, 999);
            }
            else if (date.Length == 2)
            {
                int month = int.Parse(date[1]);
                StartDate = new DateTime(year, month, 1, 0, 0, 0);

                int lastDay = DateTime.DaysInMonth(year, month);
                EndDate = new DateTime(year, month, lastDay, 23, 59, 59, 999);
            }
            else
            {
                int month = int.Parse(date[1]);
                int day = int.Parse(date[2]);

                StartDate = new DateTime(year, month, 1, 0, 0, 0);
                int lastDay = DateTime.DaysInMonth(year, month);
                EndDate = new DateTime(year, month, lastDay, 23, 59, 59, 999);
            }
        }
    }
}
