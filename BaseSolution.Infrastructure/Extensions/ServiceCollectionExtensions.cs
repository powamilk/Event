using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.Database.AppDbContext;
using BaseSolution.Infrastructure.Implements.Repositories.ReadOnly;
using BaseSolution.Infrastructure.Implements.Repositories.ReadWrite;
using BaseSolution.Infrastructure.Implements.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace BaseSolution.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ExampleReadOnlyDbContext>(options =>
            {
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddDbContextPool<ExampleReadWriteDbContext>(options =>
            {
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddDbContextPool<AppDbReadOnlyContext>(options =>
            {
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddDbContextPool<AppDbReadWriteContext>(options =>
            {
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });
            services.AddScoped<IParticipantReadOnlyRepository, ParticipantReadOnlyRepository>();
            services.AddScoped<IParticipantReadWriteRepository, ParticipantReadWriteRepository>();

            services.AddScoped<IOrganizerReadOnlyRepository, OrganizerReadOnlyRepository>();
            services.AddScoped<IOrganizerReadWriteRepository, OrganizerReadWriteRepository>();

            services.AddScoped<IEventReadOnlyRepository, EventReadOnlyRepository>();
            services.AddScoped<IEventReadWriteRepository, EventReadWriteRepository>();

            services.AddScoped<IRegistrationReadOnlyRepository, RegistrationReadOnlyRepository>();
            services.AddScoped<IRegistrationReadWriteRepository, RegistrationReadWriteRepository>();

            services.AddScoped<IReviewReadOnlyRepository, ReviewReadOnlyRepository>();
            services.AddScoped<IReviewReadWriteRepository, ReviewReadWriteRepository>();

            services.AddTransient<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();

            services.AddTransient<ILocalizationService, LocalizationService>();

            return services;
        }
    }
}