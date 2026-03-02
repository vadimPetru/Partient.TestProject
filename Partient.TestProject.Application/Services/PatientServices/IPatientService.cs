using Partient.TestProject.Application.DTO_s;
using Partient.TestProject.Domain.Enums;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Application.Services.PatientServices
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetPatients(CancellationToken cancellationToken);
        Task<Patient> GetPatientById(Guid id , CancellationToken cancellationToken);
        IQueryable<Patient> Handler(string[] birthdate, ParserType type);

        Task<Guid> CreatePatient(PatientRequest request, CancellationToken cancellationToken);
        Task<Guid> UpdatePatient(Guid id,PatientRequest request, CancellationToken cancellationToken);
        Task<Guid> RemovePatient(Guid guid, CancellationToken cancellationToken);
    }
}
