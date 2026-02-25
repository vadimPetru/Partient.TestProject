using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Partient.TestProject.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddDbContext<PatientDbContext>(option =>
                option.UseNpgsql("Server=127.0.0.1;Port=5432;Database=partient;Username=admin;Password=admin"));

            return services;
        }
    }
}
