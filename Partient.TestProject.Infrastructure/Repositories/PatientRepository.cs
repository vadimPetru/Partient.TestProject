using Microsoft.EntityFrameworkCore;
using Partient.TestProject.Domain.Interfaces;
using Partient.TestProject.Domain.Models;
using Partient.TestProject.Infrastructure.Expressions;

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

        public async Task DeletePatient(Guid id, CancellationToken cancellationToken = default)
        {
            _context.Remove(id);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Patient> GetPatientById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id ,cancellationToken);
        }

        public async Task<IEnumerable<Patient>> GetPatients(
            int pageSize ,
            int pageNumber,
            CancellationToken cancellationToken = default
            )
        {
            return await _context.Patients
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public IQueryable<Patient> PatientSearchBirthDate(string birthDate, CancellationToken cancellationToken = default)
        {
            return _context.Patients
                .AsNoTracking()
                .WhereFhirDate(birthDate, p => p.BirthDate);
        }

        public async Task UpdatePatient(Patient newPatient, CancellationToken cancellationToken = default)
        {
            _context.Update(newPatient);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
