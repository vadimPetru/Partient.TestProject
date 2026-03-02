using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using Partient.TestProject.Domain.Enums;
using Partient.TestProject.Domain.Interfaces;
using Partient.TestProject.Domain.Models;


namespace Partient.TestProject.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _context;

        public PatientRepository(PatientDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreatePatient(Patient patient, CancellationToken cancellationToken = default)
        {
            _context.Add(patient);
            await _context.SaveChangesAsync(cancellationToken);
            return patient.Id;
        }

        public async Task<Guid> DeletePatient(Patient patient, CancellationToken cancellationToken = default)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync(cancellationToken);
            return patient.Id;
        }

        public async Task<Patient> GetPatientById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public IQueryable<Patient> GetPatients()
        {
            return _context
                .Patients
                .AsNoTracking();
        }

        public IQueryable<Patient> PatientSearchPrefixBirthDate(DateTime start,
            DateTime end,
            string? prefix,
            IQueryable<Patient> query)
        {

            return prefix switch
            {
                nameof(SearchPrefix.ne) => query.Where(p => p.BirthDate < start || p.BirthDate > end),
                nameof(SearchPrefix.eq) => query.Where(p => p.BirthDate >= start && p.BirthDate <= end),
                nameof(SearchPrefix.lt) => query.Where(p => p.BirthDate < end),
                nameof(SearchPrefix.gt) => query.Where(p => p.BirthDate > start),
                nameof(SearchPrefix.ge) => query.Where(p => p.BirthDate >= start),
                nameof(SearchPrefix.le) => query.Where(p => p.BirthDate <= end),
                nameof(SearchPrefix.sa) => query.Where(p => p.BirthDate > end),
                nameof(SearchPrefix.eb) => query.Where(p => p.BirthDate < start),
                nameof(SearchPrefix.ap) => ApplyApproximate(query, start, end),
                _ => query.Where(p => p.BirthDate >= start && p.BirthDate <= end)
            };
        }

        public IQueryable<Patient> PatientSearchPrefixBirthDateTime(DateTime start,
            DateTime end,
            string? prefix,
            IQueryable<Patient> query)
        {
            return prefix switch
            {
                nameof(SearchPrefix.eq) => query.Where(p => p.BirthDate >= start && p.BirthDate < end),
                nameof(SearchPrefix.ne) => query.Where(p => p.BirthDate < start || p.BirthDate >= end),
                nameof(SearchPrefix.lt) => query.Where(p => p.BirthDate < start),
                nameof(SearchPrefix.gt) => query.Where(p => p.BirthDate >= end),
                nameof(SearchPrefix.le) => query.Where(p => p.BirthDate < end),
                nameof(SearchPrefix.ge) => query.Where(p => p.BirthDate >= start),
                nameof(SearchPrefix.sa) => query.Where(p => p.BirthDate >= end),
                nameof(SearchPrefix.eb) => query.Where(p => p.BirthDate < start),   
                _ => query.Where(p => p.BirthDate >= start && p.BirthDate < end)
            };
        }

        public IQueryable<Patient> PatientSearchPrefixBirthInstant(
           DateTime target,
           string? prefix,
           IQueryable<Patient> query)
        {
            return prefix switch
            {
                nameof(SearchPrefix.eq) => query.Where(p => p.BirthDate == target),
                nameof(SearchPrefix.ne) => query.Where(p => p.BirthDate != target),
                nameof(SearchPrefix.lt) => query.Where(p => p.BirthDate < target),
                nameof(SearchPrefix.gt) => query.Where(p => p.BirthDate > target),
                nameof(SearchPrefix.le) => query.Where(p => p.BirthDate <= target),
                nameof(SearchPrefix.ge) => query.Where(p => p.BirthDate >= target),
                nameof(SearchPrefix.sa) => query.Where(p => p.BirthDate > target),
                nameof(SearchPrefix.eb) => query.Where(p => p.BirthDate < target),
                _ => query.Where(p => p.BirthDate == target)
            };
        }

        public IQueryable<Patient> PatientSearchPrefixBirthPeriod(
            DateTime rStart, DateTime rEnd,
            string? prefix,
            IQueryable<Patient> query)
        {

            var searchRange = new NpgsqlRange<DateTime>(rStart, rEnd);

            return prefix switch
            {
                nameof(SearchPrefix.eq) => query.Where(p => new NpgsqlRange<DateTime>(p.BirthDate, p.BirthDate).Overlaps(searchRange)),
                nameof(SearchPrefix.ne) => query.Where(p => !new NpgsqlRange<DateTime>(p.BirthDate, p.BirthDate).Overlaps(searchRange)),
                _ => query
            };
        }

        public IQueryable<Patient> PatientSearchPrefixBirthDateTiming(
            DateTime rStart, DateTime rEnd,
            string prefix,
            IQueryable<Patient> query)
        {

            var searchRange = new NpgsqlRange<DateTime>(rStart, rEnd);

            return  prefix switch
            {
                nameof(SearchPrefix.eq) => query.Where(p => searchRange.Contains(p.BirthDate)),
                nameof(SearchPrefix.ne) => query.Where(p => !searchRange.Contains(p.BirthDate)),
                _ => query
            };
        }

        private IQueryable<Patient> ApplyApproximate(IQueryable<Patient> query, DateTime start, DateTime end)
        {
            var diff = (end - start).Ticks / 10;
            var apStart = start.AddTicks(-diff);
            var apEnd = end.AddTicks(diff);
            return query.Where(p => p.BirthDate >= apStart && p.BirthDate <= apEnd);
        }

        public IQueryable<Patient> FullPeriod(IQueryable<Patient> query, DateTime? periodStart, DateTime? periodEnd)
        {
            return query.Where(p => p.BirthDate >= periodStart.Value &&
                                         p.BirthDate <= periodEnd.Value);
        }

        public async Task<Guid> UpdatePatient(Patient newPatient, CancellationToken cancellationToken = default)
        {
            _context.Patients.Update(newPatient);
            await _context.SaveChangesAsync(cancellationToken);
            return newPatient.Id;
        }
    }
}
