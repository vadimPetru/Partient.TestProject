using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Partient.TestProject.Application.Services;
using Partient.TestProject.Domain.Interfaces;
using Partient.TestProject.Infrastructure.Repositories;

namespace Partient.TestProject.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientService, PatientService>();

            services.AddDbContext<PatientDbContext>(option =>
                option.UseNpgsql("Server=127.0.0.1;Port=5432;Database=partient;Username=admin;Password=admin"));

            return services;
        }
    }
}
