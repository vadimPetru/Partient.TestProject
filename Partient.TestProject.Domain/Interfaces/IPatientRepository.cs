using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetPatients(int pageSize, int pageNumber ,CancellationToken cancellationToken = default);
        Task<Patient> GetPatientById(Guid id , CancellationToken cancellationToken = default);  
        IQueryable<Patient> PatientSearchBirthDate(string birthDate, CancellationToken cancellationToken = default);
        Task<Guid> CreatePatient(Patient patient, CancellationToken cancellationToken = default);
        Task UpdatePatient(Patient newPatient, CancellationToken cancellationToken = default);
        Task DeletePatient(Guid id, CancellationToken cancellationToken = default);
    }
}
