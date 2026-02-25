using Partient.TestProject.Application.DTO_s;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Application.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetPatientResponses(int pageSize, int pageNumber, CancellationToken cancellation);
        Task<Patient> GetPatientById(Guid id , CancellationToken cancellationToken);


        Task CreatePatient(PatientRequest request, CancellationToken cancellationToken);
        Task UpdatePatient(Guid id ,PatientRequest newRequest, CancellationToken cancellationToken);
        Task RemovePatient(Guid guid, CancellationToken cancellationToken);
    }
}
