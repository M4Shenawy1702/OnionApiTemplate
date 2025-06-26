using OnionApiTemplate.API.Extensions;
using OnionApiTemplate.Application;
using OnionApiTemplate.Domain.IRepositoty;
using OnionApiTemplate.Domain.Setting;
using OnionApiTemplate.Infrastructure;

namespace OnionApiTemplate.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebApplicationServices(builder.Configuration);
            builder.Services.RegisterApplicationServices();

            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await dbInitializer.InitializeDatabaseAsync();
                await dbInitializer.InitializeIdentityAsync();
            }

            app.UseCustomExceptionHandling();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}
