using Partient.TestProject.Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Patient API",
        Version = "v1",
        Description = "API для управления пациентами",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Developer",
            Email = "6ddd14@gmail.com"
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient API V1");
        c.RoutePrefix = "swagger"; 
        c.DocumentTitle = "Patient API Documentation";
    });
}
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
