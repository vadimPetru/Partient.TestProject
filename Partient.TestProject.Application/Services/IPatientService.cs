using Partient.TestProject.Application.DTO_s;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Application.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetPatientResponses(CancellationToken cancellation);
        Task<Patient> GetPatientById(CancellationToken cancellationToken);


        Task CreatePatient(PatientRequest request, CancellationToken cancellationToken);
        Task UpdatePatient(PatientRequest newRequest , Guid guid, CancellationToken cancellationToken);
        Task RemovePatient(Guid guid, CancellationToken cancellationToken);
    }
}
