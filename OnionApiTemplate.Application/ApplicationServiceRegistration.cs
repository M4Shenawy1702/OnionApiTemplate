using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OnionApiTemplate.Application.IServices;
using OnionApiTemplate.Application.MappingProfile;
using OnionApiTemplate.Application.Services;
using OnionApiTemplate.Application.Validations.Auth;

namespace OnionApiTemplate.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            // Register all validators in the assembly
            services.AddValidatorsFromAssembly(typeof(LoginRequestValidation).Assembly);
            services.AddAutoMapper(typeof(UserProfile).Assembly);


            return services;
        }
    }
}
