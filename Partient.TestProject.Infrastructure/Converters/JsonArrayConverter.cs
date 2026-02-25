using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Partient.TestProject.Infrastructure.Converters
{
    public class JsonArrayConverter : ValueConverter<string[], string>
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public JsonArrayConverter() : base(
          v => JsonSerializer.Serialize(v, _options),
          v => JsonSerializer.Deserialize<string[]>(v, _options) ?? new string[0])
        {}
    }
}
