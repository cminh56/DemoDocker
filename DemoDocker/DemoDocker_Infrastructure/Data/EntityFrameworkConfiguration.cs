using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoDocker_Infrastructure.Data
{
    public static class EntityFrameworkConfiguration
    {
        public static IServiceCollection AddEntityFrameworkConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure());
            });

            return services;
        }
    }
} 