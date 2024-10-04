using BaseSolution.Application.DataTransferObjects.Event.Request;
using BaseSolution.Application.DataTransferObjects.Organizer.Request;
using BaseSolution.Application.DataTransferObjects.Participant.Request;
using BaseSolution.Application.DataTransferObjects.Registration.Request;
using BaseSolution.Application.DataTransferObjects.Review.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.Database.AppDbContext;
using BaseSolution.Infrastructure.Extensions.Validator.EventValidator;
using BaseSolution.Infrastructure.Extensions.Validator.Organizer;
using BaseSolution.Infrastructure.Extensions.Validator.ParticipantValidator;
using BaseSolution.Infrastructure.Extensions.Validator.RegistrationsValidator;
using BaseSolution.Infrastructure.Extensions.Validator.ReviewValidator;
using BaseSolution.Infrastructure.Implements.Repositories.ReadOnly;
using BaseSolution.Infrastructure.Implements.Repositories.ReadWrite;
using BaseSolution.Infrastructure.Implements.Services;
using FluentValidation;
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

            services.AddTransient<IValidator<EventCreateRequest>, EventCreateValidator>();
            services.AddTransient<IValidator<EventUpdateRequest>, EventUpdateValidator>();
            services.AddTransient<IValidator<OrganizerCreateRequest>, OrganizerCreateValidator>();
            services.AddTransient<IValidator<OrganizerUpdateRequest>, OrganizerUpdateValidator>();
            services.AddTransient<IValidator<ParticipantCreateRequest>, ParticipantCreateValidator>();
            services.AddTransient<IValidator<ParticipantUpdateRequest>, ParticipantUpdateValidator>();
            services.AddTransient<IValidator<RegistrationCreateRequest>, RegistrationCreateValidator>();
            services.AddTransient<IValidator<RegistrationUpdateRequest>, RegistrationUpdateValidator>();
            services.AddTransient<IValidator<ReviewCreateRequest>, ReviewCreateValidator>();
            services.AddTransient<IValidator<ReviewUpdateRequest>, ReviewUpdateValidator>();

            services.AddTransient<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();

            services.AddTransient<ILocalizationService, LocalizationService>();

            return services;
        }
    }
}