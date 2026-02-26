using Partient.TestProject.Domain.Enums;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Application.DTO_s
{
    public record PatientRequest(Name Name, Gender Gender, DateTime BirthDate, bool Active);
    public record DeletePatientRequest(Guid Id);
}
