using Microsoft.Extensions.DependencyInjection;
using Partient.TestProject.Application.Mappings;

namespace Partient.TestProject.Application.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
