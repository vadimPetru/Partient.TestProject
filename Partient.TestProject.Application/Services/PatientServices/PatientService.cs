using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Partient.TestProject.Application.DTO_s;
using Partient.TestProject.Domain.Enums;
using Partient.TestProject.Domain.Interfaces;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Application.Services.PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> CreatePatient(PatientRequest request, CancellationToken cancellationToken)
        {
            var patient = _mapper.Map<Patient>(request);
            patient.Id = Guid.NewGuid();
            var result = await _repository.CreatePatient(patient, cancellationToken);
            return result;
        }

        public Task<Patient> GetPatientById(Guid id, CancellationToken cancellationToken)
        {
            var patient = _repository.GetPatientById(id, cancellationToken);
            return patient ?? null;
        }

        public async Task<IEnumerable<Patient>> GetPatients(CancellationToken cancellationToken)
        {
            var partients = _repository.GetPatients();

            var collection = await partients
                .ToListAsync(cancellationToken);

            return collection ?? Enumerable.Empty<Patient>();
        }

        public async Task<Guid> RemovePatient(Guid id, CancellationToken cancellationToken)
        {
            var partiant = await _repository.GetPatientById(id, cancellationToken) ?? throw new NullReferenceException(nameof(Patient));

            return await _repository.DeletePatient(partiant, cancellationToken);
        }

        public async Task<Guid> UpdatePatient(Guid id, PatientRequest request, CancellationToken cancellationToken)
        {
            var patient = await _repository.GetPatientById(id, cancellationToken) ?? throw new NullReferenceException();
            _mapper.Map(request, patient);
            return await _repository.UpdatePatient(patient, cancellationToken);
        }

        public IQueryable<Patient> Handler(string[] birthdate, ParserType type)
        {
            var query = _repository.GetPatients();

            foreach (var date in birthdate)
            {
                var (prefix, value) = ParseDate(date);
                var range = new Domain.Models.Range();
                range.CreateRange(value, type);
                query = type switch
                {
                    ParserType.date => _repository.PatientSearchPrefixBirthDate(range.StartDate, range.EndDate, prefix, query),
                    ParserType.datetime => _repository.PatientSearchPrefixBirthDateTime(range.StartDate, range.EndDate, prefix, query),
                    ParserType.instant => _repository.PatientSearchPrefixBirthInstant(range.StartDate, prefix, query),
                    ParserType.period => _repository.PatientSearchPrefixBirthPeriod(range.StartDate, range.EndDate, prefix, query),
                    ParserType.timing => _repository.PatientSearchPrefixBirthDateTiming(range.StartDate, range.EndDate, prefix, query),
                    _ => query
                };
            }

            return query;

        }

        private (string prefix, string date) ParseDate(string value)
        {
            if (!char.IsLetter(value[0]) && !char.IsLetter(value[1])) return (string.Empty, value);

            var prefix = value.Substring(0, 2);
            var dateStr = value.Substring(2);

            return (prefix, dateStr);
        }
    }
}
