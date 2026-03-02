using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Partient.TestProject.Application.Services.PatientServices;
using Partient.TestProject.Domain.Interfaces;
using Partient.TestProject.Infrastructure.Repositories;

namespace Partient.TestProject.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionStrig)
        {
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientService, PatientService>();

            services.AddDbContext<PatientDbContext>(option =>
                option.UseNpgsql(connectionStrig));

            return services;
        }
    }
}
 