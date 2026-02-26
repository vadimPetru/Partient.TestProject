using Partient.TestProject.Application.DTO_s;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Application.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetPatientResponses(int pageSize, int pageNumber, CancellationToken cancellation);
        Task<Patient> GetPatientById(Guid id , CancellationToken cancellationToken);


        Task<Guid> CreatePatient(PatientRequest request, CancellationToken cancellationToken);
        Task UpdatePatient(Guid id,PatientRequest request, CancellationToken cancellationToken);
        Task RemovePatient(Guid guid, CancellationToken cancellationToken);
        Task<IQueryable<Patient>> Search(string[] birthDate, CancellationToken cancellationToken);
    }
}
