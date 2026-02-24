using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetPatients(CancellationToken cancellationToken = default);
        Task<IQueryable<Patient>> PatientSearchBirthDate(DateTime birthDate, CancellationToken cancellationToken = default);
        Task CreatePatient(Patient patient, CancellationToken cancellationToken = default);
        Task UpdatePatient(Guid id , Patient newPatient, CancellationToken cancellationToken = default);
        Task DeletePatient(Guid id, CancellationToken cancellationToken = default);
    }
}
