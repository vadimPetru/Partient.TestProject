using Bogus;
using Partient.TestProject.Domain.Enums;
using Partient.TestProject.Domain.Models;
using System.Net.Http.Json;
using System.Text.Json;


IEnumerable<PatientRequest> Generate(int count)
{
    var patientFaker = new Faker<PatientRequest>("ru")
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
        .RuleFor(p => p.BirthDate, f =>
        {
            var date = f.Date.Past(80, DateTime.Now.AddYears(-18));
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        })
       .RuleFor(p => p.Active, f => f.Random.Bool(0.8f));

 

    for (int i = 0; i < count; i++)
    {
        yield return patientFaker.Generate();
    }
}

var fake = Generate(100);
HttpClient httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:7001")

};

using var semaphore = new SemaphoreSlim(10);
var tasks = new List<Task>();

foreach (var patient in fake)
{

    await semaphore.WaitAsync();

    tasks.Add(Task.Run(async () =>
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("/api/Patient", patient);

            if (response.IsSuccessStatusCode)
                Console.WriteLine($"Успешно: {patient.Name.Family}");
            else
                Console.WriteLine($"Ошибка {response.StatusCode} для {patient.Name.Family}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: {ex.Message}");
        }
        finally
        {
            semaphore.Release();
        }
    }));
}
await Task.WhenAll(tasks);

public class PatientRequest
{
    public Name Name { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Active { get; set; }
}