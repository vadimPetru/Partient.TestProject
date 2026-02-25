using AutoMapper;
using Partient.TestProject.Application.DTO_s;
using Partient.TestProject.Domain.Interfaces;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task CreatePatient(PatientRequest request,CancellationToken cancellationToken)
        {
            var patient = _mapper.Map<Patient>(request);
            await _repository.CreatePatient(patient, cancellationToken);
        }

        public Task<Patient> GetPatientById(Guid id , CancellationToken cancellationToken)
        {
            var patient = _repository.GetPatientById(id, cancellationToken);
            return patient ?? null;
        }

        public async Task<IEnumerable<Patient>> GetPatientResponses(int pageSize , int pageNumber, CancellationToken cancellation)
        {
            var partients =  await _repository.GetPatients(pageSize,pageNumber,cancellation);

            if (partients is null)
               return Enumerable.Empty<Patient>();

            return partients;
        }

        public async Task RemovePatient(Guid id , CancellationToken cancellationToken)
        {
            await _repository.DeletePatient(id, cancellationToken);
        }

        public async Task UpdatePatient(Guid id , PatientRequest request , CancellationToken cancellationToken)
        {
            var patient = await _repository.GetPatientById(id) ?? throw new NullReferenceException();
            _mapper.Map(request, patient);
           await _repository.UpdatePatient(patient, cancellationToken);
        }
    }
}
