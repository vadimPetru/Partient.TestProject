using Bogus;
using Partient.TestProject.Domain.Enums;
using Partient.TestProject.Domain.Models;


static IEnumerable<Patient> Generate(int count)
{
    var patientFaker = new Faker<Patient>("ru")
        .RuleFor(p => p.Name, f => new Name
        {
            Id = Guid.NewGuid(),
            Family = f.Name.LastName(),
            Use = f.Name.FirstName(),
            Given = new string[2]
            {
                    f.Name.FirstName(),
                    f.Name.FirstName()
            }

        })
        .RuleFor(p => p.Gender, f => f.PickRandom(Gender.male, Gender.female, Gender.other))
        .RuleFor(p => p.BirthDate, f => f.Date.Past(80, DateTime.Now.AddYears(-18)))
        .RuleFor(p => p.Active, f => f.Random.Bool(0.8f));



    for (int i = 0; i < count; i++)
    {
        yield return patientFaker.Generate();
    }
}

var fake = Generate(100);

Task[] tasks = new Task[10];


