using Partient.TestProject.Application.DTO_s;
using Partient.TestProject.Domain.Interfaces;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public Task CreatePatient()
        {
            throw new NotImplementedException();
        }

        public Task<Patient> GetPatient()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Patient>> GetPatientResponses(CancellationToken cancellation)
        {
            var partients =  await _repository.GetPatients(cancellation);

            if (partients is null)
               return Enumerable.Empty<Patient>();

            return partients;
        }

        public Task RemovePatient()
        {
            throw new NotImplementedException();
        }

        public Task UpdatePatient()
        {
            throw new NotImplementedException();
        }
    }
}
