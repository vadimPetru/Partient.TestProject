using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Domain.Interfaces
{
    public interface IPatientRepository
    {
        IQueryable<Patient> GetPatients();
        Task<Patient> GetPatientById(Guid id, CancellationToken cancellationToken = default);


        IQueryable<Patient> PatientSearchPrefixBirthDate(DateTime start, DateTime end, string? prefix, IQueryable<Patient> query);
        IQueryable<Patient> PatientSearchPrefixBirthDateTime(DateTime start, DateTime end, string? prefix, IQueryable<Patient> query);
        IQueryable<Patient> PatientSearchPrefixBirthInstant(DateTime target, string? prefix, IQueryable<Patient> query);
        IQueryable<Patient> PatientSearchPrefixBirthPeriod(DateTime rStart, DateTime rEnd, string? prefix, IQueryable<Patient> query);
        IQueryable<Patient> PatientSearchPrefixBirthDateTiming(DateTime rStart, DateTime rEnd, string prefix, IQueryable<Patient> query);

        Task<Guid> CreatePatient(Patient patient, CancellationToken cancellationToken = default);
        Task<Guid> UpdatePatient(Patient patient, CancellationToken cancellationToken = default);
        Task<Guid> DeletePatient(Patient patient, CancellationToken cancellationToken = default);
    }
}
