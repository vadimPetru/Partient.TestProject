namespace Partient.TestProject.Domain.Exceptions
{
    public class FhirSearchException : Exception
    {
        public string StatusCode { get; } = "400";
        public FhirSearchException(string message) : base(message) { }
    }
}
